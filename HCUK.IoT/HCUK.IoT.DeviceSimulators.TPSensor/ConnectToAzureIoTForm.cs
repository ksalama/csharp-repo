using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCUK.IoT.DeviceSimulators.TPSensor
{
    public partial class ConnectToAzureIoTForm : Form
    {
        public ConnectToAzureIoTForm()
        {
            InitializeComponent();
        }

        public string connectionString;

        private void ConnectToAzureIoTForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.connectionString = textBox1.Text;
        }
    }
}
