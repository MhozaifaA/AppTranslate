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
using AppTranslate.Translate.Enums;

namespace AppTranslate.Translate
{
    public class AppTranslate : IAppTranslate
    {

        public IReadOnlyDictionary<string, string> Translate { get; set; }

        public string LanguageCode { get; set; }
        public const string loadingLanguange = "...";
        public const string ConsoleLog = " :::::::::::: AppTranslate Injected :::::::::::: ";

        public string this[string index]
             => Translate.Count == 0 ? loadingLanguange : (Language == LanguageKinds.UnDefault ?
         (Translate.GetValueOrDefault(index) ?? index) : index);


        private string ThesaurusPath { get; set; }
        private bool SupportRTL { get; set; } = false;

        private readonly HttpClient httpClient;

        private readonly LocalStorage localStorage;

        private LanguageKinds Language { get; set; } = LanguageKinds.Default;
        private bool IsServerSide { get; set; }


        #region -   Constructure    -
        public AppTranslate(LocalStorage localStorage, IOptions<AppTranslateOptions> Options)
        {
            this.httpClient = Options.Value.httpClient;
            this.localStorage = localStorage;

            InitOption(Options);

            WriteStorage();

            Translate = new ReadOnlyDictionary<string, string>(Options.Value.Translate);

            AppLog();
        }

        private void InitOption(IOptions<AppTranslateOptions> Options)
        {
            this.IsServerSide = Options.Value.IsServerSide;
            this.ThesaurusPath = Options.Value.ThesaurusPath;
            this.LanguageCode = Options.Value.Code;
        }

        public async ValueTask ChangeThesaurus(string thesaurusPath, string code = null)
        {
            ThesaurusPath = thesaurusPath;
            if (httpClient is not null && !string.IsNullOrEmpty(thesaurusPath))
            {
                var _lang = await httpClient.GetFromJsonAsync<Dictionary<string, string>>(thesaurusPath).ConfigureAwait(false);
                Translate = new ReadOnlyDictionary<string, string>(_lang);
                //  _lang.Clear();
            }
            Switch(code, true);
        }

        #endregion


        #region -   Storage   -
        private void WriteStorage(string key = null)
        {
            string kind = (key is null) ? localStorage.GetItem() :
                                        localStorage.GetItem(key);
            if (string.IsNullOrEmpty(kind))
                localStorage.SetItem((Language = LanguageKinds.Default).ToString());
            else
                Language = kind.ToEnum<LanguageKinds>();
        }
        private async ValueTask WriteStorageAsync(string key = null)
        {

            string kind = (key is null) ? await localStorage.GetItemAsync() :
                                          await localStorage.GetItemAsync(key);
            if (string.IsNullOrEmpty(kind))
                await localStorage.SetItemAsync((Language = LanguageKinds.Default).ToString());
            else
                Language = kind.ToEnum<LanguageKinds>();
        }
        #endregion


        #region -   Inject   -
        public void Inject(string key = null)
        {
            localStorage.ConsoleLog(ConsoleLog);
            localStorage.Inject();
            if (key is null) WriteStorage(); else WriteStorage(key);
            NotifyStateChanged();
        }
        public async ValueTask InjectAsync(string key = null)
        {
            await localStorage.ConsoleLog(ConsoleLog);
            localStorage.Inject();
            if (key is null) await WriteStorageAsync(); else await WriteStorageAsync(key);
            NotifyStateChanged();
        }
        #endregion


        #region -   Switch   -
        public LanguageKinds Switch(string code = null, bool toUndefault = false)
        {
            Language=Language.Switch(toUndefault);
            LanguageCode = code ?? LanguageCode;
            localStorage.SetItem(Language.ToString());
            NotifyStateChanged();
            return Language;
        }
        public async Task<LanguageKinds> SwitchAsync(string code = null, bool toUndefault = false)
        {
            Language = Language.Switch(toUndefault);
            LanguageCode = code ?? LanguageCode;
            await localStorage.SetItemAsync(Language.ToString());
            NotifyStateChanged();
            return Language;
        }
        #endregion


        #region -   Util   -

        private void AppLog()
        {
            if (!IsServerSide)
                Console.WriteLine(ConsoleLog);
        }
        public bool IsDefault => Language == LanguageKinds.Default;
        public string Path => ThesaurusPath;
        public void OnceSupportRTL() => SupportRTL = true;
        public void OnceSupportLTR() => SupportRTL = false;
        public bool IsSupportRTL => SupportRTL;
        #region Notify
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        #endregion

        #endregion

    }
}
