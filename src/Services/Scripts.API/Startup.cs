using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EESLP.BuilidingBlocks.EventBus.Events;
using EESLP.BuilidingBlocks.EventBus.Options;
using EESLP.Services.Scripts.API.Infrastructure.Extensions;
using EESLP.Services.Scripts.API.Infrastructure.Options;
using EESLP.Services.Scripts.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Polly;
using RabbitMQ.Client.Exceptions;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;
using Swashbuckle.AspNetCore.Swagger;

namespace EESLP.Services.Scripts.API
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
            // Depencency Injection
            services.AddTransient<IHostService, HostService>();
            services.AddTransient<IScriptService, ScriptService>();

            // Configure Options
            services.Configure<DatabaseOptions>(Configuration.GetSection("Database"));

            // Register Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Scripts.API", Version = "v1" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "EESLP.Services.Scripts.API.xml");
                c.IncludeXmlComments(xmlPath);
            });

            // Add RawRabbit
            //ConfigureRabbitMQServices(services);

            // Add framework services.
            services.AddMvc();

            // Add AutoMapper
            services.AddAutoMapper(typeof(Startup));

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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Enable Swagger Middleware
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scripts.API v1");
            });

            // Configure RabbitMQ Subscriptions
            //ConfigureRabbitMqSubscriptions(app);

            // Add MVC 
            app.UseMvc();
        }

        private void ConfigureRabbitMQServices(IServiceCollection services)
        {
            var rabbitMqOptions = new RabbitMqOptions();
            var rabbitMqOptionsSection = Configuration.GetSection("RabbitMq");
            rabbitMqOptionsSection.Bind(rabbitMqOptions);
            rabbitMqOptions.Hostnames.Clear();
            rabbitMqOptions.Hostnames.Add(rabbitMqOptions.Hostname);

            var rabbitMqClient = BusClientFactory.CreateDefault(rabbitMqOptions);
            services.Configure<RabbitMqOptions>(rabbitMqOptionsSection);
            services.AddSingleton<IBusClient>(_ => rabbitMqClient);
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder app)
        {
            var rabbitMqClient = app.ApplicationServices.GetService<IBusClient>();

        }
    }
}
