using BookShop.Infrastructure.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using System;
using System.IO;
using System.Threading.Tasks;
using BookShop.Infrastructure;

namespace BookShop
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .Enrich.FromLogContext()
              .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                var host = CreateHostBuilder(args).Build();
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<BookShopContextDbContextFactory>();
                await context.GetContext().Migrate();
                await host.RunAsync();
                Log.Information("Web host started");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");

            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
