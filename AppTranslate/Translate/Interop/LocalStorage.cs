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
        private bool IsInjected=true;

        public LocalStorage(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
            jSInProcessRuntime = jsRuntime as IJSInProcessRuntime;
        }

        public LocalStorage(IJSRuntime jsRuntime, IOptions<LocalStorageOptions> options) : this(jsRuntime)
        {   IsInjected = !options.Value.IsServerSide;
            ConsoleLog(options.Value.IsServerSide.ToString());
        }
        


        public  void SetItem(string value)
        {
            if (ThrowUnRenderInject()) return;

            if (jSInProcessRuntime is not null)
                jSInProcessRuntime.InvokeVoid("localStorage.setItem", "Translate", value);
            else
                jsRuntime.InvokeVoidAsync("localStorage.setItem", "Translate", value);
        }

        public string GetItem(string @default=null)
        {
            if (ThrowUnRenderInject()) return default;

            if (jSInProcessRuntime is not null)
                return jSInProcessRuntime.Invoke<string>("localStorage.getItem", "Translate");
            else
                return @default;
                return (string) Task.Factory.StartNew(async ()=>await jsRuntime.InvokeAsync<object>("localStorage.getItem", "Translate")).Result.Result;
        }


        public async ValueTask SetItemAsync(string value)
        {
            if (ThrowUnRenderInject()) return;

            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "Translate", value).ConfigureAwait(false);
        }

        public async ValueTask<string> GetItemAsync()
        {
            if (ThrowUnRenderInject()) return default;

            return (await jsRuntime.InvokeAsync<string>("localStorage.getItem", "Translate").ConfigureAwait(false));
        }
     


        public async ValueTask SetItemAsync<T>(string key, T value)
        {
            if (ThrowUnRenderInject()) return;

            string _value = JsonSerializer.Serialize(value); 
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, _value).ConfigureAwait(false);
        }


        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            if (ThrowUnRenderInject()) return default;

            var value= await jsRuntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(value);
        }


        public ValueTask ConsoleLog(string message)
        {
            if (ThrowUnRenderInject())  return default;

            return jsRuntime.InvokeVoidAsync("console.log", message);
        }


        private bool ThrowUnRenderInject()
        {
            if (!IsInjected)
                return true;
             return false;
               // throw new ArgumentException(nameof(ThrowUnRenderInject) + " AfterRender Translate.Inject()");
        }

        public void Inject()
        {
            IsInjected = true;
        }

    }

}
