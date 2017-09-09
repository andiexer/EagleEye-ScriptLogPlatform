using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;


namespace Logging.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, builder) => {
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.WithMachineName()
                        .Enrich.WithProperty("EESLPApiName","EESLP.Logging.Api")
                        .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://192.168.112.100:9200")) 
                        {

                        })
                        .CreateLogger();

                    builder.AddSerilog();
                })
                .UseStartup<Startup>()
                .Build();
    }
}
