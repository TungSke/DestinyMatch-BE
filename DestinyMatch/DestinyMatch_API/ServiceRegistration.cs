using Microsoft.EntityFrameworkCore;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Repository;
using Repository.Interfaces;
using Repository.Repositories;
using System.Text;
using Mapster;
using Repository.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
            var connectionString = configuration.GetConnectionString("OnlineConnection");

            // Inject DbContext
            services.AddDbContext<DestinyMatchContext>(options =>
                options.UseSqlServer(connectionString));


            // Inject Service Classes
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMemberService, MemberService>();

            // Inject Repository Classess
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPackageService, PackageService>();


            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<IPictureService, PictureService>();

            services.AddScoped<IUniversityRepository, UniversityRepository>();
            services.AddScoped<IUniversitityService, UniversityService>();

            services.AddScoped<IHobbyReposiroty, HobbyRepository>();
            services.AddScoped<IHobbyService, HobbyService>();

            services.AddScoped<IMajorRepository, MajorRepository>();
            services.AddScoped<IMajorService, MajorService>();

            services.AddScoped<IMatchRequestService, MatchRequestService>();
            services.AddScoped<IMatchRequestRepository, MatchRequestRepository>();
            // Other services
            SwaggerConfig(services);

            CorsConfig(services);

            // End Inject Services
            return services;
        }

        private static void SwaggerConfig(IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Destiny Match", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });
        }

        private static void CorsConfig(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }
    }
}
