using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelaySweepDemo
{
    public partial class CamView_h : Form
    {
        public HelperCameraUtil.SubSensor SubSensor = null;

        public CamView_h()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            this.SubSensor.ExitCamera();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            this.SubSensor.ExitCamera();
        }
    }
}
