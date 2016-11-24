using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VoC.DataAccess
{
    public class Provider : IDisposable
    {
        const string key = "trnsl.1.1.20161115T185645Z.4a3f4036470054b5.69e2a32d168f8676c34b4446846234ce829f7dd8";
        const string unknown = "unknown";
        private MainContext context;
        private object contextDisposeLocker = new object();
        public Provider()
        {
            context = new MainContext();
        }

        public void AddNewUser(Guid userId, string username)
        {
            context.UserHistory.Add(new UserHistory()
            {
                AverageTime = TimeSpan.Zero,
                Username = username,
                LastRequest = DateTime.Now,
                RequestCounter = 0,
                UserId = userId
            });
        }

        public Word CheckWord(string word, Guid userId)
        {
            var values = context.Words.Include("Languages").Where(m => m.WordValue == word).First();

            var user = context.UserHistory.Where(m => m.UserId == userId).First();
            user.RequestCounter++;
            var average = (user.AverageTime + (user.LastRequest - DateTime.Now));
            user.AverageTime = new TimeSpan(average.Ticks / 2);
            user.LastRequest = DateTime.Now;
            context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return values;
        }

        public Word AddWords(string word)
        {
            Word wordModel = new Word()
            {
                WordValue = word
            };

            var languageList = context.Languages.ToList();
            foreach (var item in languageList)
            {
                string value = Regex.Match(LanguageCode(word, item.Code).Result, "lang=\"(..)\"").Value.Replace("lang=\"", string.Empty).Replace("\"", string.Empty);

                if(value == item.Code)
                {
                    wordModel.Languages.Add(item);
                }
            }
            context.Words.Add(wordModel);
            context.SaveChanges();
            return wordModel;
        }

        public List<UserHistory> GetTopTen()
        {
            var users = context.UserHistory.OrderByDescending(m => m.RequestCounter).Skip(0).Take(10).ToList();
            return users;
        }

        private async Task<string> LanguageCode(string word, string language)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                   { "key", key },
                   { "text", word },
                   { "hint", language}
                };

                var content = new FormUrlEncodedContent(values);

                var responses = await client.PostAsync("https://translate.yandex.net/api/v1.5/tr/detect", content);

                return await responses.Content.ReadAsStringAsync();
            }
        }

        public void Dispose()
        {
            if (context != null)
            {
                lock (contextDisposeLocker)
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }
            }
        }
    }
}
