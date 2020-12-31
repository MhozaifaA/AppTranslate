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

        //public string ThesaurusPath { get; set; } = string.Empty;

        //private string _ThesaurusPath= string.Empty;
        //public  string ThesaurusPath 
        //{
        //    get => _ThesaurusPath;
        //    set => new Task(async () =>{ await Thesaurus(_ThesaurusPath = value);}).Start(); 
        //}


        public  void Thesaurus(params (string lang1, string lang2)[] lang)
        {
                foreach (var item in lang)
                    Translate.Add(item.lang1, item.lang2);
        }

        public async void Thesaurus(string thesaurusPath)
        {
            if (wwwroot is not null && !string.IsNullOrEmpty(thesaurusPath))
            {
                var _lang =await wwwroot.GetFromJsonAsync<Dictionary<string, string>>(thesaurusPath).ConfigureAwait(false);
                foreach (var item in _lang)
                    Translate.Add(item.Key, item.Value);
                _lang.Clear();

            }
        }


        public HttpClient wwwroot {get;set;}
    }
}
