using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCUK.IoT.Model;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;



namespace HCUK.IoT.AzureServices
{
    public class AzureIoTHubServices
    {

        private RegistryManager _registryManager;
        private string _connectionString;
        private string iotHubUri;

        private Dictionary<string, DeviceClient> _devices =  new Dictionary<string, DeviceClient>();

        public string LastError;

        public string ConnectionString
        {
            get { return this._connectionString; }
        }

        public object Result { get; private set; }

        public bool ConnectToHub(string connectionString)
        {
            bool succeed = false;
            this._connectionString = connectionString;
            try
            {
                _registryManager = RegistryManager.CreateFromConnectionString(this._connectionString);
                succeed = true;
                this.iotHubUri =  connectionString.Split(';')[0].Replace("HostName=", "");


            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
            }
                        
            return succeed;
        }
               

        public async  Task Register(string deviceName)
        {
            string Id = null;

            Device device;
            try
            {
               
                device = await _registryManager.AddDeviceAsync(new Device(deviceName));
                this._devices.Add(deviceName,
                   DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceName, device.Authentication.SymmetricKey.PrimaryKey)));
                Id = device.Authentication.SymmetricKey.PrimaryKey;
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await _registryManager.GetDeviceAsync(deviceName);
                Id = device.Authentication.SymmetricKey.PrimaryKey;

                if (!this._devices.ContainsKey(deviceName))
                {
                    this._devices.Add(deviceName,
                        DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceName, device.Authentication.SymmetricKey.PrimaryKey)));
                }
            }
            catch (Exception ex)
            {
                this.LastError = ex.Message;
            }


            this.Result = Id;
        }

        public async Task Unregister(string deviceName)
        {
            bool succeed = false; 

            var device = _devices[deviceName];
            if (device != null)
            {
                try
                {
                    await _registryManager.RemoveDeviceAsync(deviceName);
                    succeed = true;
                }
                catch (Exception ex)
                {
                    this.LastError = ex.Message;

                }
               
            }

            this.Result = succeed;
        }

        public async void SendMessage(string deviceName, SensorDataPoint dataPoint)
        {

            var messageString = JsonConvert.SerializeObject(dataPoint);
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));            

            var deviceClient = this._devices[deviceName];
            await deviceClient.SendEventAsync(message);
        }
    }
}
