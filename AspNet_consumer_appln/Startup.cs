using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AspNet_consumer_appln.Models;
using Swashbuckle.AspNetCore.Swagger;
using AspNet_consumer_appln.IntegrationEvents;

namespace AspNet_consumer_appln
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
            String constr = Configuration.GetConnectionString("Connectionstr");
            services.AddDbContext<DBcontextClass>(opt => opt.UseSqlServer(constr));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //added to code for swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "My Demo API", Version = "1.0" });
            });
            //till  here
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // app.UseHttpsRedirection();
            //added to code for swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Patient API (V 1.0)");
            });
            //till here
            app.UseMvc();
            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            //var eventBus = app.ApplicationServices.GetRequiredService<EventBusRabbitMQ>();
          //  eventBus.Subscribe<PatientUpdatedIntegrationEvent, patientUpdatedIntegrationEventHandler>();
            EventBusRabbitMQ eventbusrabbitmq_obj = new EventBusRabbitMQ();
            eventbusrabbitmq_obj.Subscribe<PatientUpdatedIntegrationEvent, patientUpdatedIntegrationEventHandler>();
        }
    }
}
