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
    public partial class CalibrationForm : Form
    {
        public Form1 MainForm { set; get; }
        public CalibrationForm()
        {
            InitializeComponent();
        }

        private async void calibrationStartButton_Click(object sender, EventArgs e)
        {
            await MainForm.CalibrateStructuredLightAsync((double)planeDistanceInput.Value,
                (double)baselineInput.Value, (double)focalLengthInput.Value);
            Hide();
        }
    }
}
