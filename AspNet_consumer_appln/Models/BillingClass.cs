using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace AspNet_consumer_appln.Models
{
    public class BillingClass
    {
        [Key]
        public string billing_no { get; set; }
        public int? pat_id { get; set; }
        public string pat_first_name { get; set; }

    }
}
