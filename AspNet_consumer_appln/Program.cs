using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet_consumer_appln.IntegrationEvents;
using AspNet_consumer_appln.Models;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AspNet_consumer_appln
{
    public class Program
    {
       // [Obsolete]
        public static void Main(string[] args)
        {
            //var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            //{
            //    cfg.ReceiveEndpoint(host,
            //       RabbitMqConstants.PatientUpdatedIntegrationEvent, e =>
            //       {
            //           //e.UseRetry(Retry.Immediate(5));
            //           e.Consumer<PatientUpdateCommandConsumer>();

            //       });
            //});

            //bus.Start();

            CreateWebHostBuilder(args).Build().Run();               
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
