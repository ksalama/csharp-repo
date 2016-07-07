using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCUK.IoT.Model
{
    public class SensorDataPoint
    {
        public Sensor Sender
        {
            get;
            set;
        }

        public double Temprature
        {
            get;
            set;
        }

        public double Pressure
        {
            get;
            set;
        }

        public DateTime Time
        {
            get;
            set;
        }
    }
}
