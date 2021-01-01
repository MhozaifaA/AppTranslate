using AppTranslate.Translate.Option;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AppTranslate.Translate.Interop
{
    public class LocalStorage
    {
        private readonly IJSRuntime jsRuntime;
        private readonly IJSInProcessRuntime jSInProcessRuntime;

        private const string js_localStorage_setItem = "localStorage.setItem";
        private const string js_localStorage_getItem = "localStorage.getItem";
        private const string default_localStorage_Key = "Translate";

        private bool IsInjected=true;// default client side true

        public LocalStorage(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
            jSInProcessRuntime = jsRuntime as IJSInProcessRuntime;
        }

        public LocalStorage(IJSRuntime jsRuntime, IOptions<LocalStorageOptions> options) : this(jsRuntime)
        => IsInjected = !options.Value.IsServerSide;


        #region -   SetItem   -

        private string SerializeValue<T>(T value)
        {
            if (value is string str)
                return str;
            else
                return JsonSerializer.Serialize(value);
        }

        #region Sync
        public void SetItem(string value)
        {
            if (ThrowUnRenderInject()) return;
            SetItem(default_localStorage_Key, value);
        }
        public void SetItem(string key, string value)
        {
            if (ThrowUnRenderInject()) return;
            SetItem<string>(key, value);
        }
        public void SetItem<T>(string key, T value)
        {
            if (ThrowUnRenderInject()) return;

            string _value = SerializeValue(value);

            if (jSInProcessRuntime is not null)
                jSInProcessRuntime.InvokeVoid(js_localStorage_setItem, key, _value);
            else
                jsRuntime.InvokeVoidAsync(js_localStorage_setItem, key, _value);
        }
        #endregion

        #region Async
        public async ValueTask SetItemAsync(string value)
        {
            if (ThrowUnRenderInject()) return;
            await SetItemAsync(default_localStorage_Key, value);
        }
        public async ValueTask SetItemAsync(string key, string value)
        {
            if (ThrowUnRenderInject()) return;
            await SetItemAsync<string>(key, value);
        }
        public async ValueTask SetItemAsync<T>(string key, T value)
        {
            if (ThrowUnRenderInject()) return;
            string _value = SerializeValue(value);
            await jsRuntime.InvokeVoidAsync(js_localStorage_setItem, key, _value).ConfigureAwait(false);
        }
        #endregion

        #endregion

        #region -   GetItem   -

        #region Sync
        public string GetItem()
        {
            if (ThrowUnRenderInject()) return default;
            return GetItem(default_localStorage_Key);
        }
        public string GetItem(string key)
        {
            if (ThrowUnRenderInject()) return default;
            return GetItem<string>(key);
        }
        public T GetItem<T>(string key)
        {
            if (ThrowUnRenderInject()) return default;

            if (jSInProcessRuntime is not null)
                return jSInProcessRuntime.Invoke<T>(js_localStorage_getItem, key);
            else
                return default(T);
            //return Task.Factory.StartNew(async ()=>await jsRuntime.InvokeAsync<T>(js_localStorage_getItem, default_localStorage_Key)).Result.Result;
        }
        #endregion

        #region Async
        public async ValueTask<string> GetItemAsync()
        {
            if (ThrowUnRenderInject()) return default;
            return await GetItemAsync(default_localStorage_Key);
        }
        public async ValueTask<string> GetItemAsync(string key)
        {
            if (ThrowUnRenderInject()) return default;
            return await GetItemAsync<string>(key);
        }
        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            if (ThrowUnRenderInject()) return default;
            return await jsRuntime.InvokeAsync<T>(js_localStorage_getItem, key).ConfigureAwait(false);
        }
        #endregion

        #endregion

        #region -   Util   -

        public ValueTask ConsoleLog(params string[] messages)
        {
            if (ThrowUnRenderInject()) return default;
            return jsRuntime.InvokeVoidAsync("console.log", messages);
        }
        private bool ThrowUnRenderInject()
          => !IsInjected; // throw new ArgumentException(nameof(ThrowUnRenderInject) + " AfterRender Translate.Inject()");
        public void Inject()
           => IsInjected = true;

        #endregion


    }

}
