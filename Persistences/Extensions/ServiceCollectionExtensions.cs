using Domain.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Persistence.DbContexts;

namespace Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryDbContext>(options =>

                options.UseSqlServer(configuration.GetConnectionString(DbConnectionStrings.LibraryDbConnection),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(LibraryDbContext).Assembly.FullName);
                        b.EnableRetryOnFailure(5, TimeSpan.FromMinutes(3), null);
                        b.CommandTimeout(3 * 60);

                    })
                    .UseLoggerFactory(MyLoggerFactory)
                    .EnableDetailedErrors());

            services.TryAddScoped<ILibraryDbContext>(provider => provider.GetService<LibraryDbContext>());

            return services;
        }
    }
}
