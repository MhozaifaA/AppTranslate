using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static void AddAppTranslateClientSide(this IServiceCollection services)
        {
            services.AddSingleton<LocalStorage>().
                   AddSingleton<IAppTranslate, AppTranslate>();
        }

        public static async Task AddAppTranslateClientSide(this IServiceCollection services,string ThesaurusPath)
        {
            var http = services.GetHttpClientService();
            var data = await http.GetFromJsonAsync<Dictionary<string, string>>(ThesaurusPath);
            services.AddAppTranslateClientSide(  config =>  config.Thesaurus(data));
        }

        public static void AddAppTranslateClientSide(this IServiceCollection services, Action<AppTranslateOptions> configure)
        {
            services.Configure(configure).Configure<AppTranslateOptions>(configureOptions =>configureOptions.httpClient = services.GetHttpClientService());
            services.AddSingleton<LocalStorage>().
                AddSingleton<IAppTranslate, AppTranslate>();
        }

        public static void AddAppTranslateServerSide(this IServiceCollection services, Action<AppTranslateOptions> configure)
        {
            services.AddScoped<LocalStorage>().Configure<LocalStorageOptions>(configureOptions =>
            new Action<LocalStorageOptions>(a => a.IsServerSide = true).Invoke(configureOptions)).
                AddScoped<IAppTranslate, AppTranslate>().Configure<AppTranslateOptions>(configureOptions =>
                {
                    configureOptions.IsServerSide = true;
                    configure?.Invoke(configureOptions);
                });
        }
    }
}
