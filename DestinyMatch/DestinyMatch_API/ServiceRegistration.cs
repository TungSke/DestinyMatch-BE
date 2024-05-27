using Microsoft.EntityFrameworkCore;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Repository;
using Repository.Interfaces;
using Repository.Repositories;

namespace DestinyMatch_API
{
    public static class ServiceRegistration
    {
        public static IServiceCollection InjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Read ConnectionString from appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Inject DbContext
            services.AddDbContext<DestinyMatchContext>(options =>
                options.UseSqlServer(connectionString));

            // Inject Service Classes
            services.AddScoped<IAccountService, AccountService>();

            //Inject Repository Classess
            services.AddScoped<IAccountRepository, AccountRepository>();

            // Other services
            return services;
        }
    }
}
