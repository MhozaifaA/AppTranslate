﻿using AppTranslate.Translate.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AppTranslate.Translate.Option;
using System.Net.Http;
using System.Net.Http.Json;
using AppTranslate.Translate.Enums;
using AppTranslate.Translate.Classes;
using System.IO;
using System.Text.Json;

namespace AppTranslate.Translate
{
    public class AppTranslate : IAppTranslate
    {

        public IReadOnlyDictionary<string, string> Translate { get; set; }

        public const string loadingLanguange = "...";
        public const string ConsoleLog = " :::::::::::: AppTranslate Injected :::::::::::: ";

        public string this[string index]
             => Translate.Count == 0 ? loadingLanguange : (Storage.Kinds == LanguageKinds.UnDefault ?
         (Translate.GetValueOrDefault(index) ?? index) : index);

     
        private readonly HttpClient httpClient;
        private readonly LocalStorage localStorage;
        private TranslateStorage Storage = new (LanguageKinds.Default,default(string));
        private bool IsServerSide { get; set; }


        #region -   Constructure    -
        public AppTranslate(LocalStorage localStorage, IOptions<AppTranslateOptions> Options)
        {
            this.httpClient = Options.Value.httpClient;
            this.localStorage = localStorage;
            Translate = new ReadOnlyDictionary<string, string>(Options.Value.Translate);
            
            InitProperties(Options);
            if(!IsServerSide)
            WriteStorage();
            AppLog();
        }

        private void InitProperties(IOptions<AppTranslateOptions> Options)
        {
            this.IsServerSide = Options.Value.IsServerSide;
            this.Storage.Path = Options.Value.ThesaurusPath;
            this.Storage.Code = Options.Value.Code;
        }

        public async ValueTask ChangeThesaurus(string thesaurusPath, string code = null)
        {
            this.Storage.Path = thesaurusPath;
            await GetThesaurus();
            if (IsServerSide)
            await SwitchToUnDefaultAsync(code);
            else
                SwitchToUnDefault(code);

        }

        private async ValueTask GetThesaurus(bool notify=false)
        {
            if (string.IsNullOrEmpty(this.Storage.Path))
                return;

            if (IsServerSide)
            {
                var file =await File.ReadAllTextAsync($"{Directory.GetCurrentDirectory()}{$@"\wwwroot\{this.Storage.Path}"}");
                var _lang = JsonSerializer.Deserialize<Dictionary<string, string>>(file);
                Translate = new ReadOnlyDictionary<string, string>(_lang);
            }else
            {
                var _lang = await httpClient.GetFromJsonAsync<Dictionary<string, string>>(this.Storage.Path).ConfigureAwait(false);
                Translate = new ReadOnlyDictionary<string, string>(_lang);
            }

            if (notify)
                NotifyStateChanged();
        }

        #endregion


        #region -   Storage   -
        private void WriteKindStorage(string key = null)
        {
            string kind = (key is null) ? localStorage.GetItem() :
                                        localStorage.GetItem(key);
            if (string.IsNullOrEmpty(kind))
                localStorage.SetItem((this.Storage.Kinds = LanguageKinds.Default).ToString());
            else
                this.Storage.Kinds = kind.ToEnum<LanguageKinds>();
        }
        private async ValueTask WriteKindStorageAsync(string key = null)
        {
            string kind = (key is null) ? await localStorage.GetItemAsync() :
                                          await localStorage.GetItemAsync(key);
            if (string.IsNullOrEmpty(kind))
                await localStorage.SetItemAsync((this.Storage.Kinds = LanguageKinds.Default).ToString());
            else
                this.Storage.Kinds = kind.ToEnum<LanguageKinds>();
        }

        private void WriteStorage(string key = null)
        {
            #nullable enable
            TranslateStorage? Storage = (key is null) ? localStorage.GetItem<TranslateStorage>() :
                                       localStorage.GetItem<TranslateStorage>(key);
            #nullable disable
            if (Storage is null || String.IsNullOrEmpty(Storage?.Path))
                localStorage.SetItem<TranslateStorage>(this.Storage = new TranslateStorage(LanguageKinds.Default, this.Storage.Path,this.Storage?.Code , Storage?.SupportRTL??false));
            else
            {
                if (!this.Storage.Path.Equals(Storage.Path) || this.Translate.Count == 0)
                {
                    this.Storage = Storage;
                   _ = GetThesaurus(true).ConfigureAwait(false);
                    NotifyStateChanged();
                }
                this.Storage = Storage;
            }
        }

        private async ValueTask WriteStorageAsync(string key = null)
        {
            #nullable enable
            TranslateStorage? Storage = (key is null) ? await localStorage.GetItemAsync<TranslateStorage>() :
                                      await  localStorage.GetItemAsync<TranslateStorage>(key);
            #nullable disable

            if (Storage is null)
               await localStorage.SetItemAsync<TranslateStorage>(this.Storage = new TranslateStorage(LanguageKinds.Default, this.Storage.Path, this.Storage.Code, Storage.SupportRTL));
            else
            {
                if (!this.Storage.Path.Equals(Storage.Path) || this.Translate.Count==0)
                {
                    this.Storage = Storage;
                    await GetThesaurus();
                    NotifyStateChanged();
                }
                this.Storage = Storage;
            }
        }
        #endregion


        #region -   Inject   -
        public async ValueTask Inject(string key = null)
        {
            localStorage.Inject();
            await localStorage.ConsoleLog(ConsoleLog);
            if (key is null) await WriteStorageAsync(); else await WriteStorageAsync(key);
            NotifyStateChanged();
        }
        #endregion


        #region -   Switch   -

        private void SwitchBase(string code = null)
        {
            this.Storage.Code = code ?? this.Storage.Code;
            localStorage.SetItem<TranslateStorage>(this.Storage);
            NotifyStateChanged();
        }
        public LanguageKinds Switch(string code = null)
        {
            this.Storage.Kinds = this.Storage.Kinds.Switch();
            SwitchBase(code);
            return this.Storage.Kinds;
        }
        public LanguageKinds SwitchToDefault(string code = null)
        {
            this.Storage.Kinds = LanguageKinds.Default;
            SwitchBase(code);
            return this.Storage.Kinds;
        }
        public LanguageKinds SwitchToUnDefault(string code = null)
        {
            this.Storage.Kinds = LanguageKinds.UnDefault;
            SwitchBase(code);
            return this.Storage.Kinds;
        }


        private async Task SwitchBaseAsync(string code = null)
        {
            this.Storage.Code = code ?? this.Storage.Code;
            await localStorage.SetItemAsync<TranslateStorage>(this.Storage);
            NotifyStateChanged();
        }
        public async Task<LanguageKinds> SwitchAsync(string code = null)
        {
            this.Storage.Kinds = this.Storage.Kinds.Switch();
            await SwitchBaseAsync(code);
            return this.Storage.Kinds;
        }
        public async Task<LanguageKinds> SwitchToDefaultAsync(string code = null)
        {
            this.Storage.Kinds = LanguageKinds.Default;
            await SwitchBaseAsync(code);
            return this.Storage.Kinds;
        }
        public async Task<LanguageKinds> SwitchToUnDefaultAsync(string code = null)
        {
            this.Storage.Kinds = LanguageKinds.UnDefault;
            await SwitchBaseAsync(code);
            return this.Storage.Kinds;
        }


        public async ValueTask<LanguageKinds> Switch(string thesaurusPath, string code = null)
        {
            if (this.Storage.Path.Equals(thesaurusPath)) {
                await SwitchAsync(code); return this.Storage.Kinds;}

            this.Storage.Path = thesaurusPath;
            await GetThesaurus();
            await SwitchToUnDefaultAsync(code);
            return this.Storage.Kinds;
        }

        #endregion


        #region -   Util   -

        public bool IsDefault => this.Storage.Kinds == LanguageKinds.Default;
        public string Path => this.Storage.Path;
        public string Code => this.Storage.Code;
        public bool IsSupportRTL => this.Storage.SupportRTL;


        public void OnceSupportLTR() {
            this.Storage.SupportRTL = false;
            localStorage.SetItem<TranslateStorage>(this.Storage);
            NotifyStateChanged();
        }
        public void OnceSupportRTL() {
            this.Storage.SupportRTL = true;
            localStorage.SetItem<TranslateStorage>(this.Storage);
            NotifyStateChanged();
        }


        public async ValueTask OnceSupportLTRAsync() {
            this.Storage.SupportRTL = false;
            await localStorage.SetItemAsync<TranslateStorage>(this.Storage);
            NotifyStateChanged();
        }

        public async ValueTask OnceSupportRTLAsync() {
            this.Storage.SupportRTL = true;
            await localStorage.SetItemAsync<TranslateStorage>(this.Storage);
            NotifyStateChanged();
        }

        #region Notify
        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
        private async Task NotifyStateChangedAsync() => await Task.Run(()=>OnChange?.Invoke());

        //public event Func<Task> OnChangeAsync;
        //private async Task NotifyStateChangedAsync() =>  await OnChangeAsync.Invoke();

        #endregion

        private void AppLog()
        {
            if (!IsServerSide)
                Console.WriteLine(ConsoleLog);
        }

        #endregion

    }
}
