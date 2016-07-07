using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HCUK.IoT.Model;
using HCUK.IoT.AzureServices;

namespace HCUK.IoT.DeviceSimulators.TPSensor
{
   
    public partial class MainForm : Form
    {
        List<Sensor> _sensors = new List<Sensor>();
        AzureIoTHubServices _iotHubServices = new AzureIoTHubServices();

        public MainForm()
        {
            InitializeComponent();
        }

        private void connectToEventHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectToAzureIoTForm form = new ConnectToAzureIoTForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                string connectionString = form.connectionString;
                if (this._iotHubServices.ConnectToHub(connectionString))
                {
                    this.isConnected = true;
                    this.UpdateFormView();
                    this.toolStripStatusLabel1.Text = "Connected to Azure IoT Hub: " + _iotHubServices.ConnectionString.Split(';')[0].Replace("HostName=","");
                    this.toolStripStatusLabel1.ForeColor = Color.Green;
                }
                else
                {
                    this.UpdateFormView();
                    this.toolStripStatusLabel1.Text = "Fialed to connect: " + _iotHubServices.LastError;
                    this.toolStripStatusLabel1.ForeColor = Color.Red;
                }
            }
        }


        private async void addSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (new AddSensorForm().ShowDialog() == DialogResult.OK)
            {
                await this._iotHubServices.Register(AddSensorForm.sensorName);

                if (this._iotHubServices.Result != null)
                {
                    string Id = this._iotHubServices.Result.ToString();
               

                    Sensor sensor = new Sensor()
                    {
                        Id = Id,
                        Name = AddSensorForm.sensorName,
                        Type = AddSensorForm.sensorType

                    };

                    sensor.OnSensorEmitMessage += sensor_OnSensorEmitMessage;             
                    this._sensors.Add(sensor);

                    this.toolStripStatusLabel1.Text = "Sensor "+sensor.Name+" was added to the IoT Hub";
                    this.toolStripStatusLabel1.ForeColor = Color.Green;

                    this.DataBindSensorListView();
                }
                else
                {

                    this.toolStripStatusLabel1.Text = "failed to add sensor IoT hub: "+_iotHubServices.LastError;
                    this.toolStripStatusLabel1.ForeColor = Color.Red;
                }

            }
        }

    

        void sensor_OnSensorEmitMessage(string senderName, SensorDataPoint dataPoint)
        {
            this._iotHubServices.SendMessage(senderName, dataPoint);
        }




        bool flash = false;
        int frequency = 100;

        private void button1_Click_1(object sender, EventArgs e)
        {
            int index = this.lvSensors.SelectedItems[0].Index;
            var sensor = this._sensors[index];

            sensor.Start();
            this.DataBindSensorListView(index);
            

            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;

            if (backgroundWorker1.IsBusy != true)
            {
                flash = true;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;

            int index = this.lvSensors.SelectedItems[0].Index;
            var sensor = this._sensors[index];


            sensor.Stop();
            this.DataBindSensorListView(index);


            flash = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (true)
            {
                // Perform a time consuming operation and report progress.

                if (flash)
                {
                    System.Threading.Thread.Sleep(10000 / frequency);
                    worker.ReportProgress(flash ? 1 : 0);
                   
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                
                }

            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {


            if (e.ProgressPercentage == 0)
            {
                this.picGrey.Visible = true;
                this.picGreen.Visible = false;
            }
            else
            {
                this.picGreen.Visible = !this.picGreen.Visible;
                this.picGrey.Visible = !this.picGreen.Visible;
            }
        }

   



        private void MainForm_Load(object sender, EventArgs e)
        {
            this.UpdateFormView();
        }


        bool isConnected = false;

        public void UpdateFormView()
        {
            if (isConnected)
            {
                this.connectToEventHubToolStripMenuItem.Enabled = false;
                this.addSensorToolStripMenuItem.Enabled = true;
                this.removeSensorToolStripMenuItem.Enabled = false;

                if (lvSensors.Items.Count > 0)
                {
                    if (lvSensors.SelectedItems.Count > 0)
                    {
                        this.removeSensorToolStripMenuItem.Enabled = true;
                        this.splitContainer1.Panel2.Show();
                        this.UpateSensorControlPannel();
                    }
                }
                else
                {
                    this.splitContainer1.Panel2.Hide();
                }
            }
            else
            {
                this.connectToEventHubToolStripMenuItem.Enabled = true;
                this.addSensorToolStripMenuItem.Enabled = false;
                this.removeSensorToolStripMenuItem.Enabled = false;
                this.splitContainer1.Panel2.Hide();

                this.toolStripStatusLabel1.Text = "Your mahcine is disconnected. Connect to an Azure IoT Hub to start registering sensors and emiting data.";
            }
        }

        private void UpateSensorControlPannel()
        {
            updateSensor = false;

            int index = this.lvSensors.SelectedItems[0].Index;
            var sensor = this._sensors[index];

            this.flash = sensor.Status == SensorStatus.On;
            this.btnStart.Enabled = sensor.Status != SensorStatus.On;
            this.btnStop.Enabled = sensor.Status == SensorStatus.On;

            this.barTemp.Value = sensor.Temprature;
            this.grpTemp.Text = "Temprature: "+this.barTemp.Value.ToString();

            this.barPre.Value = sensor.Pressure;           
            this.grpPre.Text = "Pressure: "+this.barPre.Value.ToString();


            this.barFreq.Value = sensor.EmissionFrequency;
            frequency = barFreq.Value;
            this.grpFreq.Text = "Emission Frequency: " + frequency + " / 10 Sec";



        }

        private void DataBindSensorListView()
        {
            this.lvSensors.Items.Clear();

            foreach (var sensor in this._sensors)
            {
                var item = new ListViewItem(new[] { sensor.Id.ToString(), sensor.Name, sensor.Type.ToString(), sensor.Status.ToString() });
                item.Tag = sensor;
                this.lvSensors.Items.Add(item);
            }

            if (lvSensors.Items.Count > 0)
                this.lvSensors.Items[lvSensors.Items.Count - 1].Selected = true;
            else
                this.UpdateFormView();
        }


        private void DataBindSensorListView(int index)
        {
            this.lvSensors.Items.Clear();

            foreach (var sensor in this._sensors)
            {
                var item = new ListViewItem(new[] { sensor.Id.ToString(), sensor.Name, sensor.Type.ToString(), sensor.Status.ToString() });
                item.Tag = sensor;
                this.lvSensors.Items.Add(item);
            }
            
            this.lvSensors.Items[index].Selected = true;

        }

        private async void removeSensorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvSensors.Items.Count > 0)
            {
                if (lvSensors.SelectedItems.Count > 0)
                {
                    int sensorIndex = this.lvSensors.SelectedItems[0].Index;
                    var sensor = this._sensors[sensorIndex];
                    this._sensors.RemoveAt(sensorIndex);
                    sensor.Stop();
                    await _iotHubServices.Unregister(sensor.Name);
                    if ((bool)_iotHubServices.Result)
                    {
                        this.toolStripStatusLabel1.Text = "Sensor " + sensor.Name + " has been removed";
                        this.toolStripStatusLabel1.ForeColor = Color.Green;
                    }

                }
            }

            DataBindSensorListView();

        }

    

        private void lvSensors_SelectedIndexChanged(object sender, EventArgs e)
        {            
            this.UpdateFormView();
        }

        bool updateSensor = true;

        private void barTemp_Scroll(object sender, EventArgs e)
        {
            this.grpTemp.Text = "Temprature: " + this.barTemp.Value.ToString();

            if (updateSensor)
                this.UpdateSensor();
            

            updateSensor = true;
        }

        private void UpdateSensor()
        {
            int index = this.lvSensors.SelectedItems[0].Index;
            var sensor = this._sensors[index];

            sensor.SetMeanTemprature(this.barTemp.Value);
            sensor.SetMeanPreasure(this.barPre.Value);
            sensor.SetEmittionFequency(this.frequency);

            
        }

        private void barPre_Scroll_1(object sender, EventArgs e)
        {
            this.grpPre.Text = "Pressure: " + this.barPre.Value.ToString();

            if (updateSensor)           
                this.UpdateSensor();
            

            updateSensor = true;
        }

        private void barFreq_Scroll(object sender, EventArgs e)
        {
            frequency = barFreq.Value;
            this.grpFreq.Text = "Emission Frequency: " + frequency + " / 10 Sec";

            if (updateSensor)
                this.UpdateSensor();
            
            updateSensor = true;
        }



    }
}
