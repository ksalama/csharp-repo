using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;
using System.Threading.Tasks;



namespace HCUK.EmotionDetector
{
    public class EventProcessor : IEventProcessor
    {

        const string outboundEventHubConnectionString = "Endpoint=sb://ksmsdnservicebus-ns.servicebus.windows.net/;SharedAccessKeyName=defaultPolicy;SharedAccessKey=tQ6GCEg7w7z5YSYW15mz6/OQtbp+917RCpSqYOhwxCQ=;EntityPath=vision-eventhub-outbound";

        Stopwatch checkpointStopWatch;

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine("SimpleEventProcessor initialized.  Partition: '{0}', Offset: '{1}'",
                context.Lease.PartitionId, context.Lease.Offset);
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("Processor Shutting Down. Partition '{0}', Reason: '{1}'.",
                context.Lease.PartitionId, reason);
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        public static void SendToOutboundhub(string partition, string emotion)
        {
            var eventHubClient = EventHubClient.
                CreateFromConnectionString(outboundEventHubConnectionString);


            var emotionMessage = new EmotionMessage() { Emotion = emotion };

            string jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(emotionMessage);
            EventData eventData = new EventData(Encoding.UTF8.GetBytes(jsonMessage));


            var sender = eventHubClient.CreatePartitionedSender(partition);
            sender.Send(eventData);

            Console.WriteLine("{0} > Sending message to partition '{1}': {2}",
                DateTime.Now, partition, jsonMessage);


        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {

            foreach (EventData eventData in messages)
            {
                #region Processing logic

                var imageBytes = eventData.GetBytes();

                try
                {


                    Console.WriteLine();

                    string emotion = EmotionAPIWrapper.GetEmotions(imageBytes);

                    System.Threading.Thread.Sleep(3000);

                    Console.WriteLine("Emotion:" + emotion);

                    Console.WriteLine();


                    SendToOutboundhub("0", emotion);




                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion
            }

            //Call checkpoint every 5 minutes, so that worker can resume processing from 5 minutes back if it restarts.
            if (this.checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(1))
            {
                await context.CheckpointAsync();
                this.checkpointStopWatch.Restart();
            }
        }


    }

    class EmotionMessage
    {
        public string Emotion
        {
            get;
            set;
        }
    }


}
