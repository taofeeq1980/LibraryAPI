using ApplicationServices.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApplicationServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}