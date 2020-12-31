using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace AppTranslate.Translate.Option
{
    public class AppTranslateOptions
    {

    
        public Dictionary<string, string> Translate { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public bool IsServerSide{ get; set; }

        private string _ThesaurusPath ;
        public string ThesaurusPath  => _ThesaurusPath;

    
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

        //public void Thesaurus(string thesaurusPath)
        //{
        //    ThesaurusPath = thesaurusPath;
        //}
      
        //public async void Thesaurus(string thesaurusPath)
        //{
        //    _ThesaurusPath = thesaurusPath;
        //    if (httpClient is not null && !string.IsNullOrEmpty(thesaurusPath))
        //    {
        //        var _lang =await httpClient.GetFromJsonAsync<Dictionary<string, string>>(thesaurusPath).ConfigureAwait(false);
        //        foreach (var item in _lang)
        //            Translate.Add(item.Key, item.Value);
        //        _lang.Clear();

        //    }
        //}
        //public HttpClient httpClient { get; set; }


    }
}
