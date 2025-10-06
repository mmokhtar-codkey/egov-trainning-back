using Asp.Versioning;
using Training.Api.Extensions.Swagger.Headers;
using Training.Api.Extensions.Swagger.Options;
using Training.Application.Helper;
using Training.Application.Mapping;
using Training.Application.Services.Base;
using Training.Application.Services.Lookups.Action;
using Training.Application.Services.Validators.Base;
using Training.Common.DTO.Identity.Sts;
using Training.Common.Extensions;
using Training.Common.Helpers.JsonHelper;
using Training.Common.Helpers.MailKitHelper;
using Training.Common.Infrastructure.UnitOfWork;
using Training.Infrastructure.Context;
using Training.Infrastructure.DataInitializer;
using Training.Infrastructure.UnitOfWork;
using Training.Integration.CacheRepository;
using Training.Integration.FileRepository;
using Training.Integration.TokenRepository;
using Training.Integration.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
namespace Training.Api.Extensions
{
    /// <summary>
    /// Dependency Extensions
    /// </summary>
    public static class ConfigureDependencyExtension
    {
        private const string ConnectionStringName = "Default";
        /// <summary>
        /// Register Extensions
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration);
            services.RegisterCores();
            services.RegisterLocalization();
            services.RegisterIntegrationRepositories();
            services.RegisterCustomRepositories();
            services.RegisterAutoMapper();
            services.RegisterSecurityOptions(configuration);
            services.RegisterNotificationMetadata(configuration);
            services.RegisterCommonServices(configuration);
            services.ConfigureAuthentication(configuration);
            services.AddSecurityOptions(configuration);
            services.AddHttpClientsOptions(configuration);
            services.AddHttpClients(configuration);
            services.RegisterValidators();
            services.RegisterApiMonitoring();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Converters.Add(new TrimmingConverter());
            });
            services.RegisterApiVersioning();
            services.RegisterSwaggerConfig();
            services.RegisterLowerCaseUrls();
            return services;


        }

        /// <summary>
        /// Add DbContext
        /// </summary>
        private static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TrainingDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString(ConnectionStringName),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,                 // number of retries
            maxRetryDelay: TimeSpan.FromSeconds(10), // delay between retries
            errorNumbersToAdd: null           // additional SQL error codes to consider transient
        )
                    );
            });
            services.AddScoped<DbContext, TrainingDbContext>();
            services.AddSingleton<IDataInitializer, DataInitializer>();
        }

        /// <summary>
        /// Add DbContext
        /// </summary>
        private static void RegisterApiMonitoring(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<TrainingDbContext>();
        }
        /// <summary>
        /// Configure Authentication With Identity Server
        /// </summary>
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer("Bearer", config =>
            {
                config.Authority = configuration["StsConfig:Authority"];
                config.Audience = configuration["StsConfig:Audience"];
                config.RequireHttpsMetadata = false;

            });
        }

        /// <summary>
        /// register auto-mapper
        /// </summary>
        private static void RegisterLocalization(this IServiceCollection services)
        {
            services.AddLocalization();
        }

        /// <summary>
        /// register auto-mapper
        /// </summary>
        private static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingService));
        }

        /// <summary>
        /// Register Business Validators
        /// </summary>
        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddTransient(typeof(IValidator<>), typeof(Validator<>));
        }

        /// <summary>
        /// register Integration Repositories
        /// </summary>
        private static void RegisterIntegrationRepositories(this IServiceCollection services)
        {
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<ICacheRepository, CacheRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
        }

        /// <summary>
        /// register Custom Repositories
        /// </summary>
        /// <param name="services"></param>
        private static void RegisterCustomRepositories(this IServiceCollection services)
        {
        }

        /// <summary>
        /// Register Notification Meta Data
        /// </summary>
        private static void RegisterNotificationMetadata(this IServiceCollection services, IConfiguration configuration)
        {
            var notificationMetadata = configuration.GetSection("EmailMetadata").Get<EmailMetadata>();
            if (notificationMetadata != null)
            {
                services.AddSingleton(notificationMetadata);
            }
        }

        /// <summary>
        /// Register STS  (Secure Token Service) settings 
        /// </summary>
        private static void RegisterSecurityOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var stsConfig = configuration.GetSection("StsConfig").Get<StsConfig>();
            if (stsConfig != null)
            {
                services.AddSingleton(stsConfig);
            }

        }

        /// <summary>
        /// Register Api Versioning
        /// </summary>
        private static void RegisterApiVersioning(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            })
            .AddApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;
            });
        }

        /// <summary>
        /// Lower Case Urls
        /// </summary>
        private static void RegisterLowerCaseUrls(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
        }

        /// <summary>
        /// Swagger Config
        /// </summary>

        private static void RegisterSwaggerConfig(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
            services.AddSwaggerGen(options =>
            {
                var security = new OpenApiSecurityRequirement
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
                                []

                            }
                        };
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(security);
                options.OperationFilter<LanguageHeader>();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        /// <summary>
        /// Register Core Dependencies
        /// </summary>
        private static void RegisterCores(this IServiceCollection services)
        {
            services.AddSingleton<AppHelper>();
            services.AddTransient(typeof(IBaseService<,,,,,>), typeof(BaseService<,,,,,>));
            services.AddTransient(typeof(IServiceBaseParameter<>), typeof(ServiceBaseParameter<>));
            services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            var servicesToScan = Assembly.GetAssembly(typeof(ActionService)); //..or whatever assembly you need
            services.RegisterAssemblyPublicNonGenericClasses(servicesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();
            services.RegisterAssemblyPublicNonGenericClasses(servicesToScan)
                .Where(c => c.Name.EndsWith("Validator"))
                .AsPublicImplementedInterfaces();
        }
    }
}
