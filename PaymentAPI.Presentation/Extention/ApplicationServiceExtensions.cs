using Quartz;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentAPI.Application.Interface;
using PaymentAPI.Application.Models;
using PaymentAPI.Application.Repositories;
using PaymentAPI.Application.Services;
using PaymentAPI.Application.Utilities;
using PaymentAPI.Application.Validations;
using PaymentAPI.Infrastructure.Data;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace PaymentAPI.Presentation.Extention
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            PaymentdbContext.ConnectionString = config.GetConnectionString("PaymentDb");
            services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IMarchantProfile, MarchantProfile>();
            services.AddScoped<ITransaction, TransactionRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
           services.AddScoped<PaymentdbContext>();
            // services.AddDbContext<PaymentdbContext>();
            //services.AddDbContextPool<PaymentdbContext>(options =>
            //{
            //    options.UseSqlServer(config.GetConnectionString("PaymentDb"));
            //});
           
            services.AddTransient<BalanceSheetWorker>(); // Example task
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                q.AddJobAndTrigger<BalanceSheetWorker>(config);
            });
            // Add the Quartz.NET hosted service
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            services.AddSingleton<HtmlEncoder>(
            HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                               UnicodeRanges.CjkUnifiedIdeographs }));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });

            services.AddScoped<IValidator<PaymentProfileRequest>, PaymentProfileRequestValidator>();
            services.AddScoped<IValidator<UpdatePaymentProfileRequest>, UpdatePaymentProfileRequestValidator>();
            services.AddScoped<IValidator<AverageTransactionRequest>, AverageTransactionRequestValidator>();
            services.AddScoped<IValidator<IntraBankTransferRequest>, IntraBankTransferRequestValidator>();
            services.AddScoped<IValidator<NIPTransactionRequest>, NIPTransactionRequestValidator>();
            services.AddScoped<IValidator<CreateCustomerRequest>, CreateCustomerRequestValidator>();

            services.AddScoped<EncryptionActionFilter>();
            services.AddHttpContextAccessor();
            return services;
        }

        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config) where T : IJob
        {
            // Use the name of the IJob as the appsettings.json key
            string jobName = typeof(T).Name;

            // Try and load the schedule from configuration
            var configKey = $"Quartz:{jobName}";
            var cronSchedule = config[configKey];

            // Some minor validation
            if (string.IsNullOrEmpty(cronSchedule))
            {
                //throw new CollectionException($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
                return;
            }

            // register the job as before
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule)); // use the schedule from configuration
        }
        public static void AddJwtservices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(Convert.ToDouble(configuration["HttpSecurity:expiryTime"]));
                options.ExcludedHosts.Add(configuration["HttpSecurity:Url"]);
                options.ExcludedHosts.Add(configuration["HttpSecurity:Url"]);
            });
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });
            services.AddDistributedMemoryCache();
            services.AddSession();
            // services.AddScoped<JwtHandler>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Service", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}

                    }
                });
            });
        }
    }
   
}
