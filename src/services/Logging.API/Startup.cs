using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EESLP.Services.Logging.API.Services;
using EESLP.Services.Logging.API.Infrastructure.Options;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using EESLP.BuilidingBlocks.EventBus.Events;
using EESLP.BuilidingBlocks.EventBus.Options;
using EESLP.Services.Logging.API.Handlers;
using RawRabbit;
using RawRabbit.vNext;
using EESLP.Services.Logging.API.Infrastructure;

namespace Logging.API
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
            services.AddTransient<IScriptInstanceService, ScriptInstanceService>();
            services.AddTransient<ILogService, LogService>();

            // Configure Options
            services.Configure<DatabaseOptions>(Configuration.GetSection("Database"));

            // Register Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Logging.API", Version = "v1" });
            });

            // Add RawRabbit
            ConfigureRabbitMQServices(services);

            // Add framework services.
            services.AddMvc();
            services.AddAutoMapper();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Logging.API v1");
            });

            // Configure RabbitMQ Subscriptions
            ConfigureRabbitMqSubscriptions(app);

            app.UseMvc();
        }

        private void ConfigureRabbitMQServices(IServiceCollection services)
        {
            var rabbitMqOptions = new RabbitMqOptions();
            var rabbitMqOptionsSection = Configuration.GetSection("rabbitmq");
            rabbitMqOptionsSection.Bind(rabbitMqOptions);

            // create client
            var rabbitMqClient = BusClientFactory.CreateDefault(rabbitMqOptions);
            services.Configure<RabbitMqOptions>(rabbitMqOptionsSection);
            services.AddSingleton<IBusClient>(_ => rabbitMqClient);
            services.AddScoped<IEventHandler<ScriptDeleted>, ScriptDeletedHandler>();
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder app)
        {
            var rabbitMqClient = app.ApplicationServices.GetService<IBusClient>();

            // scriptinstancecreated
            var scriptDeletedHandler =
                app.ApplicationServices.GetService<IEventHandler<ScriptDeleted>>();
            rabbitMqClient.SubscribeAsync<ScriptDeleted>(async (msg, context) =>
            {
                await scriptDeletedHandler.HandleAsync(msg);
            });
        }
    }
}
