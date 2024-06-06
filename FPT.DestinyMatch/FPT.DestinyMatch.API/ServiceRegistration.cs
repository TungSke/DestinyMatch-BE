using Microsoft.EntityFrameworkCore;
using FPT.DestinyMatch.Service.Services;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Repository;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Repositories;
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

namespace FPT.DestinyMatch.API
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

            // Inject Repository Classess
            services.AddScoped<IAccountRepository, AccountRepository>();

            //
            // =========================[ Other services]=========================
            //

            // Add JWT service
            services.AddJwtService(configuration);

            // Add Authorize On Swagger
            services.AddAuthorizeOnSwagger();

            // Add Google Service
            services.AddGoogleService();
            return services;
        }

        private static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                });
            return services;
        }

        private static IServiceCollection AddAuthorizeOnSwagger(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.AuthenticationSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };
                    policy.RequireAuthenticatedUser();
                });
            });

            // Add Authorize to Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DestinyMatch.API", Version = "v1" });

                // Add JWT Bearer security definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                // Add JWT Bearer authentication to operations
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                     new string[] { }
                    }
                });
            });
            return services;
        }

        private static IServiceCollection AddGoogleService(this IServiceCollection services)
        {
            //services.AddAuthentication().AddGoogle(options =>
            //{
            //    IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");

            //    options.ClientId = "268713324794-6op71f4fodke41ftkgc70r76so334dqn.apps.googleusercontent.com";
            //    options.ClientSecret = "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";
            //});
            return services;
        }
    }
}
