using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace HCUK.EmotionDetector
{
    class Program
    {

        const string eventHubName = "vision-eventhub-inbound";
        const string eventHubConnectionString = "Endpoint=sb://ksmsdnservicebus-ns.servicebus.windows.net/;SharedAccessKeyName=defaultPolicy;SharedAccessKey=82/CWCjpbJtd+mozOSgMTDJ6UrIbAw7o/VgUMleQjUw=";

        const string storageAccountName = "ksmsdnhdistorage";
        const string storageAccountKey = "FvUJJQFo63DF1iQY8auBepxpUyy3PeNN+VzksA45dTKVOJizFLCYS7hw7giiVj3cnBEIdbbrvCX92ZP4wTRtMQ==";


        static void Main(string[] args)
        {
            RunEventProcessor();
        }
        static public void RunEventProcessor()
        {
            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);

            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
            Console.WriteLine("Registering EventProcessor...");
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            eventProcessorHost.RegisterEventProcessorAsync<EventProcessor>(options).Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }


        static private void SingleTest()
        {
            string imageFile = @"D:\Images\random2.png";
            var bytes = System.IO.File.ReadAllBytes(imageFile);

            var response = EmotionAPIWrapper.GetEmotions(bytes);

            Console.WriteLine(response);

            Console.ReadKey();
        }
    }
}
