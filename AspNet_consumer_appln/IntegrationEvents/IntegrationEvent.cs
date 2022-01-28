using AspNet_consumer_appln.Models;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet_consumer_appln.IntegrationEvents
{
    public class PatientUpdatedIntegrationEvent : IPatientUpdatedIntegrationEvent
    {
        public int patId { get; set; }
        public string patname { get; set; }

        public PatientUpdatedIntegrationEvent(int pat_Id, string pat_name)
        {
            patId = pat_Id;
            patname = pat_name;
        }
    }

   
    public interface IPatientUpdatedIntegrationEvent
    {
        int patId { get; }
        string patname { get; }

    }



    public class PatientUpdateCommandConsumer : IConsumer<IPatientUpdatedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<IPatientUpdatedIntegrationEvent> context)
        {
            var command = context.Message;
            var patid=  command.patId;
            await Task.Delay(20);

         

            
            //notify subscribers that a order is registered
            // var orderRegisteredEvent = new OrderRegisteredEvent(command, id);
            //publish event
            //await context.Publish<IPatientUpdatedIntegrationEvent1>(orderRegisteredEvent);
        }
    }
    //---------------------Plain rabbitMQ code-------------------------
    public interface IIntegrationEventHandler<T>
    {
        Task Handle(PatientUpdatedIntegrationEvent @event);
       
    }

    public class patientUpdatedIntegrationEventHandler : IIntegrationEventHandler<PatientUpdatedIntegrationEvent>
    {
        public async Task Handle(PatientUpdatedIntegrationEvent @event)
        {
            //do database operations of consumer microservice
            int pid = @event.patId;
            var pname = @event.patname;

        }
 
    }

    public interface IEventBus
    {
        void Subscribe<T, TH>()
          where T : PatientUpdatedIntegrationEvent
          where TH : IIntegrationEventHandler<T>;

    }
    public class EventBusRabbitMQ : IEventBus//, IDisposable
    {

        public Dictionary<string, List<Type>> _handlers;
        public List<Type> _eventTypes;
        public IServiceScopeFactory _serviceScopeFactory;

        public void Subscribe<T, TH>()
            where T : PatientUpdatedIntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {

            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();


            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);


        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body);

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                // using (var scope = _serviceScopeFactory.CreateScope())
                // {
                var subscriptions = _handlers[eventName];
                foreach (var subscription in subscriptions)
                {
                    var handler = Activator.CreateInstance(subscription);//  scope.ServiceProvider.GetService(subscription);
                    if (handler == null) continue;
                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var conreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                  }
                // }
            }
        }

        //-------------till here rabbitMq code---------------------------------
    }
}