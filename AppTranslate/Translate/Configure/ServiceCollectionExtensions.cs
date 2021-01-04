using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppTranslate.Translate.Interop;
using AppTranslate.Translate.Option;
using System.Net.Http;
using System.Net.Http.Json;

namespace AppTranslate.Translate.Configure
{
    public static class ServiceCollectionExtensions
    {
        private static HttpClient GetHttpClientService(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
           return serviceProvider.GetRequiredService<HttpClient>();
        }

      
        public static async Task AddAppTranslateClientSide(this IServiceCollection services, string ThesaurusPath,string code=null)
        {
            var http = services.GetHttpClientService();
            var data = await http.GetFromJsonAsync<Dictionary<string, string>>(ThesaurusPath);
            services.AddAppTranslateClientSide(config => { 
                config.httpClient = http; config.ThesaurusPath = ThesaurusPath; config.Code = code; config.Thesaurus(data); });
        }

        public static void AddAppTranslateClientSide(this IServiceCollection services, Action<AppTranslateOptions> configure)
        {
            services.AddSingleton<LocalStorage>().
                AddSingleton<IAppTranslate, AppTranslate>().Configure(configure);
        }



        public static void AddAppTranslateServerSide(this IServiceCollection services, Action<AppTranslateOptions> configure)
        {
            services.AddHttpClient();
            services.AddScoped<LocalStorage>().Configure<LocalStorageOptions>(configureOptions =>
            new Action<LocalStorageOptions>(a => a.IsServerSide = true).Invoke(configureOptions)).
                AddScoped<IAppTranslate, AppTranslate>().Configure<AppTranslateOptions>(configureOptions =>
                {
                    configureOptions.IsServerSide = true;
                    configureOptions.httpClient = services.GetHttpClientService();
                    configure?.Invoke(configureOptions);
                });
        }

    }
}
