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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                    };
                });

            // Add Bearer Service
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

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
