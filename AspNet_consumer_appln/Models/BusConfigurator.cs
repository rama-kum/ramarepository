using MassTransit;
using MassTransit.RabbitMqTransport;
using System;

namespace AspNet_consumer_appln.Models
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> patientAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(RabbitMqConstants.RabbitMqUri), hst =>
                {
                    hst.Username(RabbitMqConstants.UserName);
                    hst.Password(RabbitMqConstants.Password);
                });

                patientAction?.Invoke(cfg, host);
            });
        }
    }
}
