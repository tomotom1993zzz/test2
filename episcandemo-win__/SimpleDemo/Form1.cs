using EpiscanUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EpiscanDemoCS
{


    public partial class Form1 : Form
    {
        /// <summary>
        /// Episcan Sensor
        /// </summary>
        private EpiscanUtil.MySensor _sensor = null;

        /// <summary>
        /// screen to show on projector
        /// </summary>
        Form _screen = null;

        /// <summary>
        /// Camera parameter
        /// </summary>
        public class CameraParameter
        {
            //public int Delay = 0;
            // gray
            // public int DelayOffset = 10975 + 250;
            // color
            //public int DelayOffset = 6450;

            // sony-gray
            public int DelayOffset = 7000;
            public float Exposure = 0.5f;
            // celluon color, gray
            public int PixelClock = 60;
            

            public EpiscanUtil.MySensor.ShutterModeList ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Global;
            public EpiscanUtil.MySensor.TriggerModeList TriggerMode = EpiscanUtil.MySensor.TriggerModeList.RisingEdge;
            public int Width = 1280;
            public int Height = 1024;
            //public int Width = 1600;
            //public int Height = 1200;
            public bool EnableGainBoost = false;
            public int MasterGain = 20;
            public int Scaler = 100;
            public EpiscanUtil.MySensor.PixelFormatList PixelFormat = EpiscanUtil.MySensor.PixelFormatList.Mono8;

            public void SaveAsXml(string path)
            {
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(CameraParameter));
                System.IO.StreamWriter sw = new System.IO.StreamWriter(path, false, new System.Text.UTF8Encoding(false));
                serializer.Serialize(sw, this);
                sw.Close();
            }
            public static CameraParameter FromXml(string path)
            {
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(CameraParameter));
                System.IO.StreamReader sr = new System.IO.StreamReader(path, new System.Text.UTF8Encoding(false));
                CameraParameter cp = (CameraParameter)serializer.Deserialize(sr);
                //ファイルを閉じる
                sr.Close();

                return cp;
            }        
        };

        public CameraParameter _preset = new CameraParameter();


        public Form1()
        {
            InitializeComponent();


            UpdateCameraList();
            InitUISyncboard();
            InitUIScreen();

            // camera groupbox disable
            this._cameraGb.Enabled = false;

            // episcan groupbox disable
            this._episcanGb.Enabled = false;

            //
            this._displayColorCb.SelectedIndex = 0;

            // Set event to receive event notification when display settings change.
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingChanged);

        }

        private void SystemEvents_DisplaySettingChanged(object sender, EventArgs e)
        {
            InitUIScreen();
        }

        void InitUISyncboard()
        {
            // Register available com port to dropdown list
            this.serialsCb.Items.Clear();
            this.serialsCb.BeginUpdate();
            foreach (var port in SyncBoard.AvailablePortNames) this.serialsCb.Items.Add(port);
            if(this.serialsCb.Items.Count >0) this.serialsCb.SelectedIndex = 0;
            this.serialsCb.EndUpdate();

        }
        void InitUIScreen()
        {
            // celluon pico projector width supposed to be 1280
            int supposedWidth = 1280;

            int projIndex = -1;

            // Register Screens
            this._screenCb.Items.Clear();
            this._screenCb.BeginUpdate();
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                Screen scr = Screen.AllScreens[i];
                this._screenCb.Items.Add("[" + i.ToString() + "]" + scr.Bounds.Width.ToString() + "x" + scr.Bounds.Height.ToString());

                if(scr.Bounds.Width == supposedWidth)
                {
                    projIndex = i;
                }
            }
            this._screenCb.EndUpdate();

            // set default
            this._screenCb.SelectedIndex = projIndex != -1 ? projIndex : Screen.AllScreens.Length - 1;
        }
        private void UpdateCameraList()
        {
            string[] models;
            long[] devids;
            EpiscanUtil.MySensor.GetCameraList(out models, out devids);

            // Register available cameras
            this._cameraCb.Items.Clear();
            this._cameraCb.BeginUpdate();
            for(int i = 0; i < models.Length; i++)
            {
                this._cameraCb.Items.Add(models[i] + "(" + devids[i].ToString() + ")");
            }
            if (this._cameraCb.Items.Count > 0) this._cameraCb.SelectedIndex = 0;
            this._cameraCb.EndUpdate();

        }
        void InitUI()
        {
            // Register Color Mode
            this._cmodeCb.Items.Clear();
            this._cmodeCb.BeginUpdate();
            foreach (EpiscanUtil.MySensor.PixelFormatList p in Enum.GetValues(typeof(EpiscanUtil.MySensor.PixelFormatList)))
                this._cmodeCb.Items.Add(p.ToString());
            this._cmodeCb.EndUpdate();
        }
        void InitUIfromCamera()
        {
            // Register available pixel clocks to dropdown list
            this._pixelClockCb.Items.Clear();
            this._pixelClockCb.BeginUpdate();
            foreach (var pc in this._sensor.AvailablePixelClock) this._pixelClockCb.Items.Add(pc.ToString());
            this._pixelClockCb.EndUpdate();

            // Registar available shutter mode to dropdown list
            this._shutterModeCb.Items.Clear();
            this._shutterModeCb.BeginUpdate();
            foreach (var ms in this._sensor.AvailableShutterModes) this._shutterModeCb.Items.Add(ms);
            this._shutterModeCb.EndUpdate();

            // Registar available trigger mode to dropdown list
            this._triggerModeCb.Items.Clear();
            this._triggerModeCb.BeginUpdate();
            foreach (var tm in this._sensor.AvailableTriggerModes) this._triggerModeCb.Items.Add(tm);
            this._triggerModeCb.EndUpdate();

        }
        void UpdateUIfromCamera()
        {
            try
            {
                this._gainBoostCb.Checked = this._sensor.EnableGainBoost;
                this._masterGainNud.Value = this._sensor.MasterGain;

                //this._syncDelayNud.Value = this._sensor.Delay;
                //this._syncExpNud.Value = (decimal)this._sensor.Exposure;

                this._pixelClockCb.SelectedIndex = this._pixelClockCb.FindString(this._sensor.PixelClock.ToString());
                this._shutterModeCb.SelectedIndex = this._shutterModeCb.FindString(this._sensor.ShutterMode.ToString());
                this._triggerModeCb.SelectedIndex = this._triggerModeCb.FindString(this._sensor.TrigerMode.ToString());

                this._camWidthNud.Value = this._sensor.Width;
                this._camHeightNud.Value = this._sensor.Height;

                this._cmodeCb.SelectedIndex = this._cmodeCb.FindString(this._sensor.PixelFormat.ToString());

                this._delayOffsetNud.Value = this._sensor.DelayOffset;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ExitBtnOnClick(object sender, EventArgs e)
        {
            if(this._sensor != null) this._sensor.Close();
            Close();
        }

        private void GainBoostCbOnCheckedChanged(object sender, EventArgs e)
        {
            this._sensor.EnableGainBoost = ((CheckBox)sender).Checked;
        }

        private void MasterGainOnValueChanged(object sender, EventArgs e)
        {
            this._sensor.MasterGain = (int)((NumericUpDown)sender).Value;
        }


        private void PixelClockCbOnSelectedIndexChanged(object sender, EventArgs e)
        {
            this._sensor.PixelClock = int.Parse(((ComboBox)sender).SelectedItem.ToString());
        }
        

        private void ToggleProjectorPower(string port)
        {
            try
            {
                SyncBoard sb = new SyncBoard(port);

                sb.Open();
                sb.TogglePower();

                sb.ToggleProjectorMode();
                sb.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private async void ProjectorPowerToggleBtnOnClick(object sender, EventArgs e)
        {
            try
            {
                string port = this.serialsCb.SelectedItem.ToString();
                await Task.Run(() => ToggleProjectorPower(port));
            }
            catch(Exception ex)
            {
                this._toolStripStatusLabel1.Text = ex.Message;
            }
        }

        private void CamWidthNudOnValueChanged(object sender, EventArgs e)
        {
            this._sensor.Width = (int)((NumericUpDown)sender).Value;
        }

        private void CamHeightNud_ValueChanged(object sender, EventArgs e)
        {
            this._sensor.Height = (int)((NumericUpDown)sender).Value;
        }


        private void SingleCapBtnOnClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png file (*.png)|*.png|All files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int[] buff = this._sensor.CaptureFrame();

                MySensor.PixelFormatList format = this._sensor.PixelFormat;
                if (format == MySensor.PixelFormatList.Mono8 || format == MySensor.PixelFormatList.Bayer8)
                {
                    // to uint8 array
                    byte[] arr = Array.ConvertAll(buff, b => (byte)Math.Min(255, b));
                    ImageFileUtil.Png.SaveGray8(sfd.FileName, this._sensor.Width, this._sensor.Height, arr);
                }
                else if (format == MySensor.PixelFormatList.Mono12 || format == MySensor.PixelFormatList.Bayer12)
                {
                    // to uint16 array
                    UInt16[] arr = Array.ConvertAll(buff, b => (UInt16)Math.Min(65535, 16 * b));
                    ImageFileUtil.Png.SaveGray16(sfd.FileName, this._sensor.Width, this._sensor.Height, arr);
                }
                else if (format == MySensor.PixelFormatList.BGR24)
                {
                    // to uint8 array
                    byte[] arr = Array.ConvertAll(buff, b => (byte)Math.Min(255, b));
                    ImageFileUtil.Png.SaveBgr24(sfd.FileName, this._sensor.Width, this._sensor.Height, arr);
                }
                else if (format == MySensor.PixelFormatList.BGR36)
                {
                    // to uint16 array
                    UInt16[] arr = Array.ConvertAll(buff, b => (UInt16)Math.Min(65535, 16 * b));
                    // convert bgr->rgb
                    for (int j = 0; j < arr.Length; j += 3)
                    {
                        UInt16 tmp = arr[j];
                        arr[j] = arr[j + 2];
                        arr[j + 2] = arr[j];
                    }
                    ImageFileUtil.Png.SaveRgb48(sfd.FileName, this._sensor.Width, this._sensor.Height, arr);
                }
            }
        }



        private void ScalerNudOnValueChanged(object sender, EventArgs e)
        {
            this._sensor.Scaler = (float)((NumericUpDown)sender).Value;

        }


        private void TriggerModeCbOnSelectedIndexChanged(object sender, EventArgs e)
        {
            this._sensor.TrigerMode = (EpiscanUtil.MySensor.TriggerModeList)Enum.Parse(typeof(EpiscanUtil.MySensor.TriggerModeList), ((ComboBox)sender).SelectedItem.ToString());
        }

        private void RestartBtnOnClick(object sender, EventArgs e)
        {
            this._sensor.StartCapture();
        }

        private void UpdateDispString()
        {

            if (this._regularModeRb.Checked)
            {
                _sensor.DispText = "Regular";
            }
            else if (this._directModeRb.Checked)
            {
                _sensor.DispText = "Direct";
            }
            else if (this._indirectModeRb.Checked)
            {
                _sensor.DispText = "Indirect";
            }
            else
            {
                _sensor.DispText = "delay =   " + ((float)this._syncDelayNud.Value).ToString("0") + "us\n" + "exposure = " + (((float)this._syncExpNud.Value) * 1e3f).ToString("0") + "us";
            }

        }

        private void ConnectBtnOnClick(object sender, EventArgs e)
        {
            try
            {
                int device_id = int.Parse(this._cameraCb.SelectedItem.ToString().Split(new char[] { '(', ')' })[1]);

                this._preset = CameraParameter.FromXml("cp.xml");

                CamView f = new CamView();

                _sensor = new EpiscanUtil.MySensor(f.Handle, device_id);

                f.Sensor = this._sensor;


                f.Show();

                f.Width = (int)(0.5 * this._sensor.Width);
                f.Height = (int)(0.5 * this._sensor.Height);

                InitUIfromCamera();
                InitUI();

                //UpdateUIfromCamera();

                this._cameraGb.Enabled = true;
                this._episcanGb.Enabled = true;

                // apply camera parameters
                try
                {
                    ApplyCameraParameters(this._preset);
                }
                catch(Exception)
                {

                }

                // update UI
                UpdateUIfromCamera();

                UpdateDelayExposure();

                UpdateDispString();

                this._sensor.StartCapture();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        private void ApplyCameraParameters(CameraParameter param)
        {
            //this._sensor.Delay = param.Delay;
            this._sensor.DelayOffset = param.DelayOffset;
            this._sensor.PixelClock = param.PixelClock;
            this._sensor.Exposure = param.Exposure;
            this._sensor.ShutterMode = param.ShutterMode;
            this._sensor.TrigerMode = param.TriggerMode;
            this._sensor.Width = param.Width;
            this._sensor.Height = param.Height;
            this._sensor.EnableGainBoost = param.EnableGainBoost;
            this._sensor.MasterGain = param.MasterGain;
            this._sensor.Scaler = param.Scaler;
            this._sensor.PixelFormat = param.PixelFormat;
        }


        void UpdateDelayExposure()
        {
            if (this._directModeRb.Checked)
            {
                float exposure = 0.5f; // 0.5[ms]
                this._sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;
                this._sensor.Delay = (int)(-0.5f * exposure * 1e3f);
                this._sensor.Exposure = exposure;

                //
                this._syncDelayNud.Value = (decimal)0.0;
                this._syncExpNud.Value = (decimal)exposure;
                this._syncDelayNud.Enabled = false;
                this._syncExpNud.Enabled = false;
            }
            else if (this._indirectModeRb.Checked)
            {
                float exposure = 0.5f; // 0.5[ms]
                float gap = 0.3f;

                this._sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;
                this._sensor.Delay = (int)((2.0f * gap) * 1e3f * exposure);
                this._sensor.Exposure = 16.67f - exposure - 2.0f * gap;

                //
                this._syncDelayNud.Value = (decimal)0.0;
                this._syncExpNud.Value = (decimal)this._sensor.Exposure;
                this._syncDelayNud.Enabled = false;
                this._syncExpNud.Enabled = false;
            }
            else if (this._regularModeRb.Checked)
            {
                float gap = 0.3f;
                this._sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Global;
                this._sensor.Delay = -(int)this._delayOffsetNud.Value;
                this._sensor.Exposure = 16.67f - gap;

                //
                this._syncDelayNud.Value = (decimal)0.0;
                this._syncExpNud.Value = (decimal)this._sensor.Exposure;
                this._syncDelayNud.Enabled = false;
                this._syncExpNud.Enabled = false;
            }
            else if (this._veinRb.Checked)
            {
                this._sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;

                float delay = 600f;
                float exposure = 0.5f;

                this._sensor.Delay = (int)(delay - 0.5f * exposure * 1e3f);
                this._sensor.Exposure = exposure;

                this._syncDelayNud.Value = (decimal)delay;
                this._syncExpNud.Value = (decimal)exposure;

                //this._masterGainNud.Value = 100;
            }
            else if (this._manualModeRb.Checked)
            {
                this._sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;
                float exposure = (float)this._syncExpNud.Value;

                try
                {
                    this._sensor.Delay = (int)((float)this._syncDelayNud.Value - 0.5f * exposure * 1e3f);
                    this._sensor.Exposure = exposure;
                }
                catch (Exception)
                {
                    this._toolStripStatusLabel1.Text = "failed to set delay/exposure";

                    this._syncDelayNud.Value = 0;
                    this._syncExpNud.Value = (decimal)0.5;
                    UpdateDelayExposure();
                }


                //
                this._syncDelayNud.Enabled = true;
                this._syncExpNud.Enabled = true;
            }

        }

        private void ImagingModeRbOnCheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Focused)
            {
                UpdateDelayExposure();

                UpdateDispString();

                UpdateUIfromCamera();
            }


        }

        private void SyncDelayExpNudOnValueChanged(object sender, EventArgs e)
        {
            if (this._syncDelayNud.Focused || this._syncExpNud.Focused)
            {
                float syncDelay = (float)this._syncDelayNud.Value;
                float syncExposure = (float)this._syncExpNud.Value;

                this._sensor.Delay = (int)(syncDelay - 0.5f * syncExposure * 1e3f);
                this._sensor.Exposure = syncExposure;

                this._manualModeRb.Checked = true;

                UpdateDispString();
                UpdateUIfromCamera();
            }
        }


        private void DelayOffsetNudOnValueChanged(object sender, EventArgs e)
        {
            this._sensor.DelayOffset = (int)this._delayOffsetNud.Value;
        }

        private void ShowProjectorScreenBtnOnClick(object sender, EventArgs e)
        {
            if (_screen != null) _screen.Dispose();

            int index = this._screenCb.SelectedIndex;
            if (index == -1) return;

            _screen = new Form();

            _screen.Show();

            Screen scr = Screen.AllScreens[index];

            _screen.Location = new Point(scr.WorkingArea.X, scr.WorkingArea.Y);
            _screen.FormBorderStyle = FormBorderStyle.None;
            _screen.WindowState = FormWindowState.Maximized;


            switch(this._displayColorCb.SelectedItem.ToString())
            {
                case "White":
                    _screen.BackColor = Color.White;
                    break;
                case "Black":
                    _screen.BackColor = Color.Black;
                    break;
                case "Red":
                    _screen.BackColor = Color.FromArgb(255, 0, 0);
                    break;
                case "Green":
                    _screen.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case "Blue":
                    _screen.BackColor = Color.FromArgb(0, 0, 255);
                    break;
            }


        }

        private void CloseProjectorScreenBtnOnClick(object sender, EventArgs e)
        {
            if (_screen != null) _screen.Close();
        }

        private void RefreshScreenBtnOnClick(object sender, EventArgs e)
        {
            InitUIScreen();
        }
        private void ResetBtnOnClick(object sender, EventArgs e)
        {
            this._syncDelayNud.Value = 0;
            this._syncExpNud.Value = (decimal)0.5;

            UpdateDelayExposure();
            UpdateDispString();
            UpdateUIfromCamera();
            UpdateDispString();
        }

        private void ColorModeCbOnSelectedIndexCommitted(object sender, EventArgs e)
        {
            this._sensor.PixelFormat = (EpiscanUtil.MySensor.PixelFormatList)Enum.Parse(typeof(EpiscanUtil.MySensor.PixelFormatList), ((ComboBox)sender).SelectedItem.ToString());
        }


        private void ShutterModeCbOnSelectionChangeCommitted(object sender, EventArgs e)
        {
            this._sensor.ShutterMode = (EpiscanUtil.MySensor.ShutterModeList)Enum.Parse(typeof(EpiscanUtil.MySensor.ShutterModeList), ((ComboBox)sender).SelectedItem.ToString());
        }

        private void AutoWbBtnOnClick(object sender, EventArgs e)
        {
            this._sensor.AutoWhitebalanceOn();
        }




    }
}
