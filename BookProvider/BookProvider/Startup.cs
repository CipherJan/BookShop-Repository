using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using BookProvider.Bootstrap;
using BookProvider.Core.ExternalAPI;
using BookProvider.Infrastructure.BookService;
using BookProvider.Infrastructure.BookService.Interface;
using BookProvider.Infrastructure.MassTransit.Interface;
using BookProvider.Infrastructure.MassTransit;
using BookProvider.Infrastructure.ProxyService;
using BookProvider.Infrastructure.ProxyService.Interface;
using System;

namespace BookProvider
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
            services.AddControllers();
            services.AddHttpClient();

            services.AddScoped(isp => 
            {
                var externalApiConfig = new ExternalAPIConfiguration();
                Configuration.GetSection("ExternalAPIUrl").Bind(externalApiConfig);

                string apiUrl = Environment.GetEnvironmentVariable("BOOK_EXTERNAL_API_URL");
                if (!String.IsNullOrEmpty(apiUrl))
                {
                    externalApiConfig.ExternalAPIAddress = apiUrl;
                }


                return externalApiConfig;
            });

            services.AddMassTransitPublisherAndConsumer(Configuration);
            services.AddMassTransitHostedService();

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IResponseProducer, ResponseProducer>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookProvider", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookProvider v1"));
            }

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
