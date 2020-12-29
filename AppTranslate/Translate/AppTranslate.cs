using AppTranslate.Translate.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AppTranslate.Translate.Option;

namespace AppTranslate.Translate
{
    public class AppTranslate : IAppTranslate
    {

        public IReadOnlyDictionary<string, string> Translate { get; }

        public string Language { get; set; }

        public string this[string index] => Language == "ar" ? (Translate.GetValueOrDefault(index) ?? index) : index;

        private readonly LocalStorage localStorage;

        public AppTranslate(LocalStorage localStorage, IOptions<AppTranslateOptions> Options)
        {
            Console.WriteLine(" :::::::::::: AppTranslate Injected :::::::::::: ");

            this.localStorage = localStorage;

            WriteCookie();

            Translate = new ReadOnlyDictionary<string, string>(Options.Value.Translate);
        }


        private void WriteCookie()
        {

            Language =  localStorage.GetItem();

            if (string.IsNullOrEmpty(Language))
            {
                Language = "en";
                 localStorage.SetItem("en");
            }
        }

        private void WriteCookie(string @default)
        {
            Language = localStorage.GetItem(@default);

            if (string.IsNullOrEmpty(Language))
            {
                Language = "en";
                localStorage.SetItem("en");
            }
        }


        private async ValueTask WriteCookieAsync()
        {
            Language = await localStorage.GetItemAsync();

            if (string.IsNullOrEmpty(Language))
            {
                Language = "en";
               await localStorage.SetItemAsync("en");
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
            Language = Language == "en" ? "ar" : "en";
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
            Language = Language == "en" ? "ar" : "en";
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



        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

      
    }



}
