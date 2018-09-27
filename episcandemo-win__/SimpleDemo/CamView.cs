using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EpiscanDemoCS
{
    public partial class CamView : Form
    {
        public EpiscanUtil.MySensor Sensor = null;

        public CamView()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Sensor.ExitCamera();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            this.Sensor.ExitCamera();
        }
    }
}
