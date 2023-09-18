using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
