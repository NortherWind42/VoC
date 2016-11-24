using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using VoC.DataAccess;

namespace VoC.WebApp.Controllers
{
    [RoutePrefix("api/Main")]
    [Authorize]
    public class MainController : ApiController
    {
        private Guid _userId = Guid.Empty;
        private ApplicationUserManager _userManager;

        [HttpGet]
        [Route("GetTranslation")]
        public IHttpActionResult GetTranslation(string word)
        {
            if(!string.IsNullOrWhiteSpace(word) && word.Length > 4)
            {
                Provider provider = new Provider();
                var selectedWord = provider.CheckWord(word, UserId);

                if (selectedWord == null)
                {
                    selectedWord = provider.AddWords(word);
                }
                var st = Newtonsoft.Json.JsonConvert.SerializeObject(selectedWord);
                return Ok(st);
            }
            else
            {
                return BadRequest();
            }
        }

        public IHttpActionResult GetTop()
        {
            return Ok();
        }
        private Guid UserId
        {
            get
            {
                if (_userId != Guid.Empty)
                {
                    return _userId;
                }

                if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
                {
                    Guid userId;
                    if (Guid.TryParse(User.Identity.GetUserId(), out userId))
                    {
                        _userId = userId;
                        return this._userId;
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect UserId");
                    }
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}
