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

namespace HCUK.IoT.DeviceSimulators.TPSensor
{
    public partial class AddSensorForm : Form
    {
        static public string sensorName;
        static public SensorType sensorType;
 

        public AddSensorForm()
        {
            InitializeComponent();
        }

        private void AddSensorForm_Load(object sender, EventArgs e)
        {
            this.comboBox1.DataSource = Enum.GetNames(typeof(SensorType));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sensorName = textBox1.Text;
            sensorType = (SensorType)Enum.Parse(typeof(SensorType), comboBox1.SelectedItem.ToString());
        }
    }
}
