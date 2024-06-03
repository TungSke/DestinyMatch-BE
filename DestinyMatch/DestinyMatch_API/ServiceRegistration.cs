using Microsoft.EntityFrameworkCore;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Repository;
using Repository.Interfaces;
using Repository.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DestinyMatch_API
{
    public static class ServiceRegistration
    {
        public static IServiceCollection InjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add JWT service
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            // Read ConnectionString from appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Inject DbContext
            services.AddDbContext<DestinyMatchContext>(options =>
                options.UseSqlServer(connectionString));

            // Inject Service Classes
            services.AddScoped<IAccountService, AccountService>();

            // Inject Repository Classess
            services.AddScoped<IAccountRepository, AccountRepository>();

            // Other services
            return services;
        }
    }
}
