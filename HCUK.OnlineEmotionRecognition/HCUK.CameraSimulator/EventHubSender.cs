using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace HCUK.CameraSimulator
{
   
    public class EventHubSender
    {
        const string eventHubName = "vision-eventhub-inbound";
        const string eventHubConnectionString = "Endpoint=sb://ksmsdnservicebus-ns.servicebus.windows.net/;SharedAccessKeyName=defaultPolicy;SharedAccessKey=82/CWCjpbJtd+mozOSgMTDJ6UrIbAw7o/VgUMleQjUw=;EntityPath=vision-eventhub-inbound";

        

        private EventHubClient eventHubClient;

        public EventHubSender()
        {
          this.eventHubClient = EventHubClient.
          CreateFromConnectionString(eventHubConnectionString);
        }

        public void Send(byte[] imageBytes)
        {
            //string jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(imageBytes);
            //EventData eventData = new EventData(Encoding.UTF8.GetBytes(jsonMessage));

            EventData eventData = new EventData(imageBytes);

            var sender = eventHubClient.CreatePartitionedSender("0");
            sender.Send(eventData);
        }

    }
}
