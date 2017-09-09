using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using EESLP.BuildingBlocks.Resilence.Http;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using EESLP.Frontend.Gateway.API.Infrastructure.Options;
using EESLP.Frontend.Gateway.API.Infrastructure.Extensions;

namespace EESLP.Frontend.Gateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
           Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Depencency Injection
            services.AddSingleton<IHttpApiClient, StandardHttpClient>();

            // Configure Options
            services.Configure<ApiOptions>(options => {
                options.ScriptsApiUrl = Configuration.GetSection("Services:scripts.api").Value;
                options.LoggingApiUrl = Configuration.GetSection("Services:logging.api").Value;
            });

            // Register Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Frontend.Gateway.API", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "EESLP.Frontend.Gateway.API.xml");
                c.IncludeXmlComments(xmlPath);
            });

            // Add framework services.
            services.AddMvc();
            services.AddAutoMapper();

            // Add Cors support
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Pagination");
            }));

            // Add distributed cache
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["Services:redis.cache"];
                options.ResolveDns();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // use Cors
            app.UseCors("CorsPolicy");

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Enable Swagger Middleware
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Frontend.Gateway.API v1");
            });

            app.UseMvc();
        }
    }
}
