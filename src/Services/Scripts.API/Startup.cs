﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EESLP.BuilidingBlocks.EventBus.Events;
using EESLP.BuilidingBlocks.EventBus.Options;
using EESLP.Services.Scripts.API.Handlers;
using EESLP.Services.Scripts.API.Infrastructure.Options;
using EESLP.Services.Scripts.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;
using Swashbuckle.AspNetCore.Swagger;

namespace EESLP.Services.Scripts.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

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
            });


            // Add RawRabbit
            ConfigureRabbitMQServices(services);

            // Add framework services.
            services.AddMvc();
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
            ConfigureRabbitMqSubscriptions(app);

            // Add MVC 
            app.UseMvc();
        }

        private void ConfigureRabbitMQServices(IServiceCollection services)
        {
            var rabbitMqOptions = new RabbitMqOptions();
            var rabbitMqOptionsSection = Configuration.GetSection("rabbitmq");
            rabbitMqOptionsSection.Bind(rabbitMqOptions);

            // create clieit
            var rabbitMqClient = BusClientFactory.CreateDefault(rabbitMqOptions);
            services.Configure<RabbitMqOptions>(rabbitMqOptionsSection);
            services.AddSingleton<IBusClient>(_ => rabbitMqClient);
            services.AddScoped<IEventHandler<ScriptInstanceCreated>, ScriptInstanceCreatedHandler>();
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder app)
        {
            var rabbitMqClient = app.ApplicationServices.GetService<IBusClient>();

            // scriptinstancecreated
            var scriptInstanceCreatedHandler =
                app.ApplicationServices.GetService<IEventHandler<ScriptInstanceCreated>>();
            rabbitMqClient.SubscribeAsync<ScriptInstanceCreated>(async (msg, context) =>
            {
                await scriptInstanceCreatedHandler.HandleAsync(msg);
            });
        }
    }
}
