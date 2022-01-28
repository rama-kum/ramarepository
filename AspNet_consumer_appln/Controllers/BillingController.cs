using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNet_consumer_appln.Models;
using AspNet_consumer_appln.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace AspNet_consumer_appln.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private DBcontextClass dbcontext_obj;
     //   private readonly IEventBus _eventBus;
     //   EventBusRabbitMQ eventbusrabbitmq_obj = new EventBusRabbitMQ();

        public BillingController(DBcontextClass context)
        {
            dbcontext_obj = context;
        }

        // GET: api/Billing
        [HttpGet]
        public IEnumerable<string> Get()
        {  
            return new string[] { "value1" };
        }

        // GET: api/Billing/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Billing
        [HttpPost]
        public void Post([FromBody] string value)
        {


        }

        // PUT: api/Billing/5
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string value)
        {
             return "m";
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
