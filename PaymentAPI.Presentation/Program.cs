using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PaymentAPI.Application.Utilities;
using PaymentAPI.Infrastructure.Data;
using PaymentAPI.Presentation.Extention;
using PaymentAPI.Presentation.Middleware;
using Serilog;
using static System.Net.Mime.MediaTypeNames;
using System;
using Coravel;
using Coravel.Scheduling.Schedule;
using Coravel.Scheduling.Schedule.Interfaces;

namespace PaymentAPI.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var env = builder.Environment;
            var endpoint = new AppSettings();
            builder.Configuration.GetSection("AppSettings").Bind(endpoint);


            builder.Host.UseSerilog((ctx, lc) => lc
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                .WriteTo.File($@"{env.WebRootPath /*+ endpoint.LogPath*/}\\Logs\\PaymentAPI_{DateTime.Now:ddMMyyyy}.txt",
             fileSizeLimitBytes: 15_000_000,
             rollOnFileSizeLimit: true,
             shared: true,
             flushToDiskInterval: TimeSpan.FromSeconds(1)));


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddJwtservices(builder.Configuration);
           
         

            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            bool prod = !string.IsNullOrEmpty(endpoint.Swagger) && endpoint.Swagger.ToLower().StartsWith("n");
            if (!prod)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            using (var scope = app.Services.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<PaymentdbContext>();
                dataContext.Database.Migrate();
            }
            
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors(x =>
            {
                x.WithOrigins(builder.Configuration["AllowedHosts"]
                  .Split(",", StringSplitOptions.RemoveEmptyEntries)
                  .Select(o => o.RemovePostFix("/"))
                  .ToArray())
             .AllowAnyMethod()
             .AllowAnyHeader();
            });

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
           

        }
       
     
    }




    class MyService
    {
        public void Run()
        {
            Console.WriteLine("My service is running...");
        }
    }
}

