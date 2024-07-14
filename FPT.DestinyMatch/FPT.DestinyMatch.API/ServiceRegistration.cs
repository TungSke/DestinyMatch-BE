using Microsoft.EntityFrameworkCore;
using FPT.DestinyMatch.Service.Services;
using FPT.DestinyMatch.Service.Interfaces;
using FPT.DestinyMatch.Repository;
using FPT.DestinyMatch.Repository.Interfaces;
using FPT.DestinyMatch.Repository.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json;
using FPT.DestinyMatch.Repository.Models;
using FPT.DestinyMatch.Service.Models.Response;
using Mapster;

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
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMemberPackageService, MemberPackageService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IUniversitityService, UniversityService>();
            services.AddScoped<IHobbyService, HobbyService>();
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<IMatchingService, MatchingService>();


            // Inject Repository Classess
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMemberPackageRepository, MemberPackageRepository>();
            services.AddScoped<IMessageReposirory, MessageReposirory>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<IUniversityRepository, UniversityRepository>();
            services.AddScoped<IHobbyReposiroty, HobbyRepository>();
            services.AddScoped<IMajorRepository, MajorRepository>();
            services.AddScoped<IMatchingRepository, MatchingRepository>();

            services.AddScoped<ChatHub>();
            //
            // =========================[ Other services]=========================
            //

            // Add JWT service
            services.AddJwtService(configuration);

            // Add Authorize On Swagger
            services.AddAuthorizeOnSwagger();

            // Cors
            services.CorsConfig();

            AddKebab(services);

            addMapper();

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

        private static IServiceCollection AddKebab(IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.Conventions.Add(
                    new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            })
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower;
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

        private static IServiceCollection CorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .SetIsOriginAllowed((host) => true);
                    });
            });
            return services;
        }

        private static void addMapper()
        {
            TypeAdapterConfig<Member, MemberResponse>
                .NewConfig()
                .Map(dest => dest.UrlPath, src => src.Pictures.Select(p => p.UrlPath).ToList());
        }
    }
}
