using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using BookShop.Infrastructure.EntityFramework;
using System.Text.Json.Serialization;
using BookShop.ServiceCollectionExtensions;
using BookShop.Services;
using BookShop.Services.Interfaces.Services;
using Serilog;
using MassTransit;
using BookShop.Producer;
using BookShop.Producer.Interface;

namespace BookShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    var enumConverter = new JsonStringEnumConverter();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });
            services.AddHttpClient();
            
            services.AddScoped<IShopService, ShopService>();

            services.AddScoped(isp =>
                new BookShopContextDbContextFactory(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMassTransitPublisherAndConsumer(Configuration);
            services.AddMassTransitHostedService();
            services.AddScoped<IRequestProduser, RequestProduser>();

            services.AddControllers();
            services.AddBackgroundJobs();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookShop", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookShop v1"));
            }
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
