using Microsoft.Extensions.DependencyInjection;
using ServiceCenter.Models;

namespace ServiceCenter
{
    public static class ServiceCenterExtension
    {
        public static IServiceCollection AddServiceCenter(this IServiceCollection services)
        {
            services.AddTransient<IADToolsService, ADToolsService>();

            return services;
        }
    }
}
