using AppTranslate.Translate.Configure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            //builder.Services.AddAppTranslateClientSide();

            await builder.Services.AddAppTranslateClientSide(  "thesaurus.json");

            // builder.Services.AddAppTranslateClientSide(config=>  config.Thesaurus(
            //      ("Hello", "„—Õ»«"),
            //      ("Counter", "⁄œ«œ"),
            //      ("Current count", "«·⁄œœ «·Õ«·Ì"),
            //      ("Click me", "≈‰ﬁ—‰Ì"),
            //      ("BlazorApp", " ÿ»Ìﬁ »·“Ê—"),
            //      ("About", "⁄‰"),
            //      ("Fetch data", "Ã·» «·»Ì«‰« "),
            //      ("Home", "«·„‰“·"),
            //      ("Welcome to your new app", "„—Õ»« »ﬂ » ÿ»Ìﬁﬂ «·ÃœÌœ"),
            //      ("Hello, world!", "„—Õ»«, »«·⁄«·„!"),
            //      ("Change language", " €Ì— «··€…")
            //      )
            //);

            await builder.Build().RunAsync();
        }
    }
}
