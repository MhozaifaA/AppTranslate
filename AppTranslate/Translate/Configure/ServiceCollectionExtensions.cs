using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppTranslate.Translate.Interop;
using AppTranslate.Translate.Option;

namespace AppTranslate.Translate.Configure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppTranslateClientSide(this IServiceCollection services, Action<AppTranslateOptions> configure)
        {
            services.AddSingleton<LocalStorage>().
                AddSingleton<IAppTranslate, AppTranslate>().Configure<AppTranslateOptions>(configureOptions =>
                configure?.Invoke(configureOptions));
        }

        public static void AddAppTranslateServerSide(this IServiceCollection services, Action<AppTranslateOptions> configure)
        {
            services.AddScoped<LocalStorage>().Configure<LocalStorageOptions>(configureOptions =>
            new Action<LocalStorageOptions>(a => a.IsServerSide = true).Invoke(configureOptions)).
                AddScoped<IAppTranslate, AppTranslate>().Configure<AppTranslateOptions>(configureOptions =>
                    configure?.Invoke(configureOptions));
        }
    }
}
