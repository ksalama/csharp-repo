using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HCUK.CameraSimulator
{
    public partial class MainFrom : Form
    {
        public MainFrom()
        {
            InitializeComponent();
        }

        bool isRunning = false;
        string folderPath = @"D:\Images";
        Random random = new Random();
        EventHubSender eventSender = new EventHubSender();

        private void button1_Click(object sender, EventArgs e)
        {

            if (isRunning)
            {
                this.button1.Text = "Start";
                this.isRunning = false;
            }
            else
            {
                this.button1.Text = "Stop";
                this.isRunning = true;
            }

        }



        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var imageBytes = this.GetImageByteRandomly();

            var stream = new System.IO.MemoryStream(imageBytes);

            try
            {
                this.pictureBox1.Image = Image.FromStream(stream);

                eventSender.Send(imageBytes);
            }
            catch { }
        }

        private byte[] GetImageByteRandomly()
        {
            return System.IO.File.ReadAllBytes(GetImageFilePathRandomly());
        }

        private string GetImageFilePathRandomly()
        {
           

            var files = System.IO.Directory.GetFiles(this.folderPath);

            int index = random.Next(0, files.Length);

            return files[index];
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (isRunning)
                {
                    backgroundWorker1.ReportProgress(0);                    
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }
    }
}
