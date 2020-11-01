using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoutingAssistant.Core.Configuration;
using RoutingAssistant.DataLayer.Contracts;
using RoutingAssistant.DataLayer.Implementations;
using RoutingAssistant.Infrastructure;
using RoutingAssistant.Root;

namespace RoutingAssistant
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            services.Configure<TravelServiceOptions>(Configuration.GetSection(TravelServiceOptions.SectionName));
            services.PostConfigure<TravelServiceOptions>(opt => 
            {
                if (string.IsNullOrEmpty(opt.RouterDBPath))
                {
                    opt.RouterDBPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }
            });

            services.RegisterServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, IOsmRouter osmRouter, RoutingAssistantDbContext ctx)
        {
            ctx.Database.EnsureCreated();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
            else 
            {
                var isApiKeyConfigured = !string.IsNullOrEmpty(configuration.GetValue<string>(Constants.ApiKey));
                if (!isApiKeyConfigured) throw new Exception($"You need to set Environment Variable {Constants.ApiKey}");
                app.UseMiddleware<ApiKeyMiddleware>();
            }
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
