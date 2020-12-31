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
            //      ("Hello", "�����"),
            //      ("Counter", "����"),
            //      ("Current count", "����� ������"),
            //      ("Click me", "������"),
            //      ("BlazorApp", "����� �����"),
            //      ("About", "��"),
            //      ("Fetch data", "��� ��������"),
            //      ("Home", "������"),
            //      ("Welcome to your new app", "����� �� ������� ������"),
            //      ("Hello, world!", "������, �������!"),
            //      ("Change language", "���� �����")
            //      )
            //);

            await builder.Build().RunAsync();
        }
    }
}
