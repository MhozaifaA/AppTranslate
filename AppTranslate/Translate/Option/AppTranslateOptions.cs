using System;
using System.Collections.Generic;
using System.Net.Http;

namespace AppTranslate.Translate.Option
{
    public class AppTranslateOptions
    {

        public Dictionary<string, string> Translate { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public bool IsServerSide{ get; set; }
        public string ThesaurusPath { get; set; }
        public string Code { get; set; }

        public void Thesaurus(Dictionary<string, string> lang)
        {
            foreach (var item in lang)
                Translate.Add(item.Key, item.Value);
        }

        public  void Thesaurus(params (string lang1, string lang2)[] lang)
        {
                foreach (var item in lang)
                    Translate.Add(item.lang1, item.lang2);
        }

        public HttpClient httpClient { get; set; }

      


    }
}
