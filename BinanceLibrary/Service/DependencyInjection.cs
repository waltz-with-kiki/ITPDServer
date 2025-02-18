using BinanceLibrary.Service.Interface;
using BinanceLibrary.Service.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceLibrary.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBinanceLibrary(this IServiceCollection services)
        {
            services.AddSingleton<MongoDbRepository>();
            services.AddScoped<BinanceApiClient>();
            services.AddScoped<IBinanceService, BinanceService>();

            return services;
        }
    }
}
