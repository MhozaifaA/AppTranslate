using AppTranslate.Translate.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AppTranslate.Translate.Option;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Net;

namespace AppTranslate.Translate
{
    public class AppTranslate : IAppTranslate
    {

        public IReadOnlyDictionary<string, string> Translate { get; set; }

        public const string defaultLanguange = "default";
        public const string undefaultLanguange = "undefault";

        private string Language { get; set; }
        public string ThesaurusPath { get; set; }

        public string this[string index]
        {
            get
            {
                if (Translate.Count == 0)
                    return "...";
                return Language == undefaultLanguange ? (Translate.GetValueOrDefault(index) ?? index) : index;
            }
        }

        private readonly HttpClient httpClient;

        private readonly LocalStorage localStorage;

        public AppTranslate(LocalStorage localStorage, IOptions<AppTranslateOptions> Options)
        {
            if(!Options.Value.IsServerSide)
            Console.WriteLine(" :::::::::::: AppTranslate Injected :::::::::::: ");

            this.localStorage = localStorage;
            this.httpClient = Options.Value.httpClient;
            this.ThesaurusPath = Options.Value.ThesaurusPath;

            WriteCookie();

            Translate = new ReadOnlyDictionary<string, string>(Options.Value.Translate);
        }



        public async ValueTask UpdateThesaurus(string thesaurusPath)
        {
            ThesaurusPath = thesaurusPath;
            if (httpClient is not null && !string.IsNullOrEmpty(thesaurusPath))
            {
                var _lang =await httpClient.GetFromJsonAsync<Dictionary<string, string>>(thesaurusPath).ConfigureAwait(false);
                    Translate = new ReadOnlyDictionary<string, string>(_lang);
              //  _lang.Clear();
            }
            Language = undefaultLanguange;
            NotifyStateChanged();
        }




        private void WriteCookie()
        {
            Language =  localStorage.GetItem();

            if (string.IsNullOrEmpty(Language))
            {
                 Language = defaultLanguange;
                 localStorage.SetItem(defaultLanguange);
            }
        }

        private void WriteCookie(string @default)
        {
            Language = localStorage.GetItem(@default);

            if (string.IsNullOrEmpty(Language))
            {
                Language = defaultLanguange;
                localStorage.SetItem(defaultLanguange);
            }
        }


        private async ValueTask WriteCookieAsync()
        {
            Language = await localStorage.GetItemAsync();

            if (string.IsNullOrEmpty(Language))
            {
                Language = defaultLanguange;
                await localStorage.SetItemAsync(defaultLanguange);
            }
        }


        public void Inject()
        {
            this.localStorage.ConsoleLog(" :::::::::::: AppTranslate Injected :::::::::::: ");
            this.localStorage.Inject();
            WriteCookie();
            NotifyStateChanged();
        }

        public void Inject(string @default)
        {
            this.localStorage.ConsoleLog(" :::::::::::: AppTranslate Injected :::::::::::: ");
            this.localStorage.Inject();
            WriteCookie(@default);
            NotifyStateChanged();
        }

        public async ValueTask InjectAsync()
        {
            await this.localStorage.ConsoleLog(" :::::::::::: AppTranslate Injected :::::::::::: ");
            this.localStorage.Inject();
            await WriteCookieAsync();
            NotifyStateChanged();
        }

        

        public void Refresh()
        {
            Language =  localStorage.GetItem();
        }

        public string Switch()
        {
            Language = Language == defaultLanguange ? undefaultLanguange : defaultLanguange;
            localStorage.SetItem(Language);
            NotifyStateChanged();
            return Language;
        }

        public string Switch(string language)
        {
            Language = language;
            localStorage.SetItem(Language);
            NotifyStateChanged();

            return Language;
        }



        public async Task RefreshAsync()
        {
            Language = await localStorage.GetItemAsync();
        }

        public async Task<string> SwitchAsync()
        {
            Language = Language == defaultLanguange ? undefaultLanguange : defaultLanguange;
            await localStorage.SetItemAsync(Language);
            NotifyStateChanged();
            return Language;
        }

        public async Task<string> SwitchAsync(string language)
        {
            Language = language;
            await localStorage.SetItemAsync(Language);
            NotifyStateChanged();
            return Language;
        }


        public bool IsDefault()
        => Language == defaultLanguange;
        


        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

      
    }



}
