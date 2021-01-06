using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppTranslate.Translate.Interop;
using AppTranslate.Translate.Option;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            services.TryAddSingleton<LocalStorage>();
            services.TryAddSingleton<IAppTranslate, AppTranslate>();
            services.Configure(configure);
        }


        public static void AddAppTranslateServerSide(this IServiceCollection services, Action<AppTranslateOptions> configure)
        {
            services.TryAddScoped<LocalStorage>();
            services.Configure<LocalStorageOptions>(configureOptions =>
            new Action<LocalStorageOptions>(a => a.IsServerSide = true).Invoke(configureOptions));
            services.TryAddScoped<IAppTranslate, AppTranslate>(); 
            services.Configure<AppTranslateOptions>(configureOptions =>
                {
                    configureOptions.IsServerSide = true;
                    configure?.Invoke(configureOptions);
                });
        }

        public static void AddAppTranslateServerSide(this IServiceCollection services, string ThesaurusPath, string code = null)
        {
            services.AddAppTranslateServerSide(config => {
                config.ThesaurusPath = ThesaurusPath; config.Code = code;
            });
        }

    }
}
