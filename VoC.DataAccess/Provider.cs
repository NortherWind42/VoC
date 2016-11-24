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
        const string key = "trnsl.1.1.20161124T130153Z.2beb53fe9af9b0e0.68e641a241ea4a230747a8c160f2b294bf1f3f58";
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
            context.SaveChanges();
        }

        public Word CheckWord(string word, Guid userId)
        {
            var values = context.Words.Include("Languages").Where(m => m.WordValue == word).FirstOrDefault();

            var user = context.UserHistory.Where(m => m.UserId == userId).First();
            user.RequestCounter++;
            var average = (user.AverageTime + (DateTime.Now - user.LastRequest));
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

                if (value == item.Code)
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


                var request = (HttpWebRequest)WebRequest.Create("https://translate.yandex.net/api/v1.5/tr/detect");

                var postData = "key="+ key;
                postData += "&text="+ word;
                postData += "&hint="+ language;
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                return new StreamReader(response.GetResponseStream()).ReadToEnd();

                //var content = new FormUrlEncodedContent(values);

                //var responses = await client.PostAsync("https://translate.yandex.net/api/v1.5/tr/detect", content);

                //return await responses.Content.ReadAsStringAsync();
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
