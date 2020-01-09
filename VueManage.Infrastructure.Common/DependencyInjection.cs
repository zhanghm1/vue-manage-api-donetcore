using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Infrastructure.Common
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureCommon(this IServiceCollection services)
        {
            services.AddScoped<LanguageManager>();
        }
    }
}
