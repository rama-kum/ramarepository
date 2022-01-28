using System;
using System.Collections.Generic;
using System.Text;

namespace AspNet_consumer_appln.Models
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri = "rabbitmq://localhost/pathost/";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string PatientUpdatedIntegrationEvent = "PatientUpdatedEvent.service";
    }
}
