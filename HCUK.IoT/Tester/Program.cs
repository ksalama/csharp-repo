using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace Tester
{
    class Program
    {
        static RegistryManager registryManager;

        static string connectionString = "HostName=samplehub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=2qxG1uiJuqUjLkftntEmi07ysTaDrIQi9VxHhIW5IoY=";
        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            //AddDeviceAsync().Wait();

            //Console.WriteLine(AddDeviceAsync().Result);
            //Console.ReadLine();

            List<string> devicesToRemove = new List<string>()
            {
               "snsr-01",
               "snsr-02"//,
               //"snsr-03",
               //"snsr-04",
               //"snsr-05"


            };

            foreach (string id in devicesToRemove)
                RemoveDeviceAsync(id).Wait();

            Console.WriteLine("Done");
        }

        private static async Task<string> AddDeviceAsync()
        {
            string deviceId = "myFirstDevice2";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);

            return device.Authentication.SymmetricKey.PrimaryKey;
        }


        public static async Task RemoveDeviceAsync(string deviceId)
        {
            await registryManager.RemoveDeviceAsync(deviceId);
        }
    }
}
