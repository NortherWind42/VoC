using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoC.WebApp.Models
{
    public class WordModel
    {
        public int Id { get; set; }

        public int Word { get; set; }

        public List<LanguageModel> Languages { get; set; }
    }
}