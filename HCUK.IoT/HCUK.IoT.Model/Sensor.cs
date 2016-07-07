using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCUK.IoT.Model
{
    public delegate void EmitMessageHandler(string senderId, SensorDataPoint dataPoint);

    public class Sensor
    {
        
        public static Random random = new Random();

        public string Id
        { get; set; }

        public string Name
        { get; set; }

        public SensorType Type
        { get; set; }

        public int EmissionFrequency
        {
            get { return this._emmissionFrequency; }
        }

        private int _emmissionFrequency = 20;

        private SensorStatus _status;

        object lockFlag = new object();

        public SensorStatus Status
        {
            get { return this._status; }

        }

        private void Run()
        {
            while (true)
            {
                lock (lockFlag)
                {
                    if (this._status == SensorStatus.Off)
                        break;
                }

                this.EmitMessage();

                System.Threading.Thread.Sleep(10000/this._emmissionFrequency);


            }
        }

        public int Temprature
        {
            get {return this._meanTemprature; }
            
            
        }

        public int Pressure
        {
            get { return this._meanPressure; }

        }

        private int _meanTemprature = 12;

        private int _meanPressure = 5;

        public event EmitMessageHandler OnSensorEmitMessage;

        public void Start()
        {
            lock (lockFlag)
            {

                if (this._status != SensorStatus.On)
                {
                    this._status = SensorStatus.On;


                    Task.Run(() =>
                    this.Run()
                    );

                }

            }
        }

        public void Stop()
        {
            lock (lockFlag)
            {

                if (this._status != SensorStatus.Off)
                {
                    this._status = SensorStatus.Off;
                }

            }
        }

        public void SetMeanTemprature(int meanTemprature)
        {
            lock (lockFlag)
            {
                this._meanTemprature = meanTemprature;
            }
        }

        public void SetMeanPreasure(int meanPressure)
        {
            lock (lockFlag)
            {
                this._meanPressure = meanPressure;
            }
        }

        public void SetEmittionFequency(int fequceny)
        {
            lock (lockFlag)
            {
                this._emmissionFrequency = fequceny;
            }
        }
        public void EmitMessage()
        {
            SensorDataPoint dataPoint = new SensorDataPoint()
            {
                Sender = this,
                Temprature = GetNextDouble(this._meanTemprature,3),
                Pressure = GetNextDouble(this._meanPressure,3),
                Time = DateTime.Now
            };

            if (this.OnSensorEmitMessage != null)
                this.OnSensorEmitMessage(this.Name,dataPoint);
        }

        public static double GetNextDouble(double value, double margin)
        {
            double max = value + margin;
            double min = value - margin;

            double range = max - min;
            return min + (range * random.NextDouble());
        }
    }


    public enum SensorStatus
    {
        Off,
        On

    }

    public enum SensorType
    {
        Type1,
        Type2,
        Type3,
        Type4,
        Type5
    }
}
