using EpiscanUtil;
using HelperCameraUtil;
using StructuredLightUtil;
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
using System.IO;
using System.Diagnostics;
using OpenCvSharp;

namespace DelaySweepDemo
{
    public partial class Form1 : Form
    {
        private MyEpiscan episcan = null;
        private SubCam subcam = null;
       
        private int count = 0;
        private int bsShowcount = 0;
        public List<Mat> frames_for_depth;
        ///// <summary>
        ///// Episcan Sensor
        ///// </summary>
        //private EpiscanUtil.MySensor episcan.Sensor = null;

        ///// <summary>
        ///// screen to show on projector
        ///// </summary>
        //Form episcan.Screen = null;

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

            public HelperCameraUtil.SubSensor.ShutterModeList_h ShutterMode_h = HelperCameraUtil.SubSensor.ShutterModeList_h.Global;
            public HelperCameraUtil.SubSensor.TriggerModeList_h TriggerMode_h = HelperCameraUtil.SubSensor.TriggerModeList_h.RisingEdge;

            public int Width = 1280;
            public int Height = 1024;
            //public int Width = 1600;
            //public int Height = 1200;
            public bool EnableGainBoost = false;
            public int MasterGain = 20;
            public int Scaler = 100;
            public EpiscanUtil.MySensor.PixelFormatList PixelFormat = EpiscanUtil.MySensor.PixelFormatList.Mono8;
            public HelperCameraUtil.SubSensor.PixelFormatList_h PixelFormat_h = HelperCameraUtil.SubSensor.PixelFormatList_h.Mono8;

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
        public CameraParameter _preset_h = new CameraParameter();

        public Form1()
        {
            InitializeComponent();

            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));

            episcan = new MyEpiscan();
            subcam = new SubCam();

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

            DetectSyncBoard();
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
            episcan.Screen.Create(_screenCb.SelectedIndex);
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
            foreach (var pc in episcan.Sensor.AvailablePixelClock) this._pixelClockCb.Items.Add(pc.ToString());
            this._pixelClockCb.EndUpdate();

            // Registar available shutter mode to dropdown list
            this._shutterModeCb.Items.Clear();
            this._shutterModeCb.BeginUpdate();
            foreach (var ms in episcan.Sensor.AvailableShutterModes) this._shutterModeCb.Items.Add(ms);
            this._shutterModeCb.EndUpdate();

            // Registar available trigger mode to dropdown list
            this._triggerModeCb.Items.Clear();
            this._triggerModeCb.BeginUpdate();
            foreach (var tm in episcan.Sensor.AvailableTriggerModes) this._triggerModeCb.Items.Add(tm);
            this._triggerModeCb.EndUpdate();

        }
        void UpdateUIfromCamera()
        {
            try
            {
                this._gainBoostCb.Checked = episcan.Sensor.EnableGainBoost;
                this._masterGainNud.Value = episcan.Sensor.MasterGain;

                //this._syncDelayNud.Value = episcan.Sensor.Delay;
                //this._syncExpNud.Value = (decimal)episcan.Sensor.Exposure;

                this._pixelClockCb.SelectedIndex = this._pixelClockCb.FindString(episcan.Sensor.PixelClock.ToString());
                this._shutterModeCb.SelectedIndex = this._shutterModeCb.FindString(episcan.Sensor.ShutterMode.ToString());
                this._triggerModeCb.SelectedIndex = this._triggerModeCb.FindString(episcan.Sensor.TrigerMode.ToString());

                this._camWidthNud.Value = episcan.Sensor.Width;
                this._camHeightNud.Value = episcan.Sensor.Height;

                this._cmodeCb.SelectedIndex = this._cmodeCb.FindString(episcan.Sensor.PixelFormat.ToString());

                this._delayOffsetNud.Value = episcan.Sensor.DelayOffset;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ExitBtnOnClick(object sender, EventArgs e)
        {
            if(episcan.Sensor != null) episcan.Sensor.Close();
            Close();
        }

        private void GainBoostCbOnCheckedChanged(object sender, EventArgs e)
        {
            episcan.Sensor.EnableGainBoost = ((CheckBox)sender).Checked;
        }

        private void MasterGainOnValueChanged(object sender, EventArgs e)
        {
            episcan.Sensor.MasterGain = (int)((NumericUpDown)sender).Value;
        }


        private void PixelClockCbOnSelectedIndexChanged(object sender, EventArgs e)
        {
            episcan.Sensor.PixelClock = int.Parse(((ComboBox)sender).SelectedItem.ToString());
        }
        

        private void ToggleProjectorPower(string port)
        {
            _projectorPowerBtn.Invoke(new Action(() => _projectorPowerBtn.Enabled = false));
            
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
            finally
            {
                _projectorPowerBtn.Invoke(new Action(() => _projectorPowerBtn.Enabled = true));
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
            episcan.Sensor.Width = (int)((NumericUpDown)sender).Value;
        }

        private void CamHeightNud_ValueChanged(object sender, EventArgs e)
        {
            episcan.Sensor.Height = (int)((NumericUpDown)sender).Value;
        }


        private void SingleCapBtnOnClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png file (*.png)|*.png|All files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int[] buff = episcan.Sensor.CaptureFrame();

                MySensor.PixelFormatList format = episcan.Sensor.PixelFormat;
                if (format == MySensor.PixelFormatList.Mono8 || format == MySensor.PixelFormatList.Bayer8)
                {
                    // to uint8 array
                    byte[] arr = Array.ConvertAll(buff, b => (byte)Math.Min(255, b));
                    ImageFileUtil.Png.SaveGray8(sfd.FileName, episcan.Sensor.Width, episcan.Sensor.Height, arr);
                }
                else if (format == MySensor.PixelFormatList.Mono12 || format == MySensor.PixelFormatList.Bayer12)
                {
                    // to uint16 array
                    UInt16[] arr = Array.ConvertAll(buff, b => (UInt16)Math.Min(65535, 16 * b));
                    ImageFileUtil.Png.SaveGray16(sfd.FileName, episcan.Sensor.Width, episcan.Sensor.Height, arr);
                }
                else if (format == MySensor.PixelFormatList.BGR24)
                {
                    // to uint8 array
                    byte[] arr = Array.ConvertAll(buff, b => (byte)Math.Min(255, b));
                    ImageFileUtil.Png.SaveBgr24(sfd.FileName, episcan.Sensor.Width, episcan.Sensor.Height, arr);
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
                    ImageFileUtil.Png.SaveRgb48(sfd.FileName, episcan.Sensor.Width, episcan.Sensor.Height, arr);
                }
            }
        }



        private void ScalerNudOnValueChanged(object sender, EventArgs e)
        {
            episcan.Sensor.Scaler = (float)((NumericUpDown)sender).Value;

        }


        private void TriggerModeCbOnSelectedIndexChanged(object sender, EventArgs e)
        {
            episcan.Sensor.TrigerMode = (EpiscanUtil.MySensor.TriggerModeList)Enum.Parse(typeof(EpiscanUtil.MySensor.TriggerModeList), ((ComboBox)sender).SelectedItem.ToString());
        }

        private void RestartBtnOnClick(object sender, EventArgs e)
        {
            episcan.Sensor.StartCapture();
        }

        private void UpdateDispString()
        {

            if (this._regularModeRb.Checked)
            {
                episcan.Sensor.DispText = "Regular";
            }
            else if (this._directModeRb.Checked)
            {
                episcan.Sensor.DispText = "Direct";
            }
            else if (this._indirectModeRb.Checked)
            {
                episcan.Sensor.DispText = "Indirect";
            }
            else
            {
                episcan.Sensor.DispText = "delay =   " + ((float)this._syncDelayNud.Value).ToString("0") + "us\n" + "exposure = " + (((float)this._syncExpNud.Value) * 1e3f).ToString("0") + "us";
            }

        }

        private void ConnectBtnOnClick(object sender, EventArgs e)
        {
            try
            {
                int device_id = int.Parse(this._cameraCb.SelectedItem.ToString().Split(new char[] { '(', ')' })[1]);

                this._preset = CameraParameter.FromXml("cp.xml");

                CamView f = new CamView();

                episcan.InitializeSensor(f.Handle, device_id);
                episcan.ImageRoi = Roi;
                //Roi = episcan.ImageRoi;
                SetUIImageRoi();

                f.Sensor = episcan.Sensor;


                f.Show();

                f.Width = (int)(0.5 * episcan.Sensor.Width);
                f.Height = (int)(0.5 * episcan.Sensor.Height);

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

                episcan.Sensor.StartCapture();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private void  HelperBtnOnClick(object sender, EventArgs e)
        {
            try
            {
                int device_id_sub = int.Parse(this._cameraCb.SelectedItem.ToString().Split(new char[] { '(', ')' })[1]);

                if (device_id_sub == 1) { device_id_sub = 2; }
                else if (device_id_sub == 2) { device_id_sub = 1; }


                this._preset_h = CameraParameter.FromXml("cp_sub.xml");

                CamView_h f_h = new CamView_h();

                subcam.InitializeSensor(f_h.Handle, device_id_sub);
                subcam.ImageRoi = Roi;
                //Roi = episcan.ImageRoi;
                //SetUIImageRoi();

                f_h.SubSensor = subcam.Sensor_h;


                f_h.Show();

                f_h.Width = (int)(0.5 * subcam.Sensor_h.Width);
                f_h.Height = (int)(0.5 * subcam.Sensor_h.Height);

               // InitUIfromCamera();
               // InitUI();

                //UpdateUIfromCamera();

                this._cameraGb.Enabled = true;
                this._episcanGb.Enabled = true;

                // apply camera parameters
                //try
                //{
                //    ApplyCameraParameters_h(this._preset_h);
                //}
                //catch (Exception)
                //{

                //}

                // update UI
                //UpdateUIfromCamera();

                //UpdateDelayExposure();

                //UpdateDispString();

                subcam.Sensor_h.StartCapture();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void ApplyCameraParameters(CameraParameter param)
        {
            //episcan.Sensor.Delay = param.Delay;
            episcan.Sensor.DelayOffset = param.DelayOffset;
            episcan.Sensor.PixelClock = param.PixelClock;
            episcan.Sensor.Exposure = param.Exposure;
            episcan.Sensor.ShutterMode = param.ShutterMode;
            episcan.Sensor.TrigerMode = param.TriggerMode;
            episcan.Sensor.Width = param.Width;
            episcan.Sensor.Height = param.Height;
            episcan.Sensor.EnableGainBoost = param.EnableGainBoost;
            episcan.Sensor.MasterGain = param.MasterGain;
            episcan.Sensor.Scaler = param.Scaler;
            episcan.Sensor.PixelFormat = param.PixelFormat;
        }

        private void ApplyCameraParameters_h(CameraParameter param)
        {
            //episcan.Sensor.Delay = param.Delay;
            subcam.Sensor_h.DelayOffset = param.DelayOffset;
            subcam.Sensor_h.PixelClock = param.PixelClock;
            subcam.Sensor_h.Exposure = param.Exposure;
            subcam.Sensor_h.ShutterMode = param.ShutterMode_h;
            subcam.Sensor_h.TrigerMode = param.TriggerMode_h;
            subcam.Sensor_h.Width = param.Width;
            subcam.Sensor_h.Height = param.Height;
            subcam.Sensor_h.EnableGainBoost = param.EnableGainBoost;
            subcam.Sensor_h.MasterGain = param.MasterGain;
            subcam.Sensor_h.Scaler = param.Scaler;
            subcam.Sensor_h.PixelFormat = param.PixelFormat_h;
        }


        void UpdateDelayExposure()
        {
            if (this._directModeRb.Checked)
            {
                float exposure = 0.5f; // 0.5[ms]
                episcan.Sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;
                float d_delay = -0.5f * exposure * 1e3f;
                episcan.Sensor.Delay = (int)(d_delay);
                episcan.Sensor.Exposure = exposure;

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

                episcan.Sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;
                episcan.Sensor.Delay = (int)((2.0f * gap) * 1e3f * exposure);
                episcan.Sensor.Exposure = 16.67f - exposure - 2.0f * gap;

                //
                this._syncDelayNud.Value = (decimal)0.0;
                this._syncExpNud.Value = (decimal)episcan.Sensor.Exposure;
                this._syncDelayNud.Enabled = false;
                this._syncExpNud.Enabled = false;
            }
            else if (this._regularModeRb.Checked)
            {
                float gap = 0.3f;
                episcan.Sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Global;
                episcan.Sensor.Delay = -(int)this._delayOffsetNud.Value;
                episcan.Sensor.Exposure = 16.67f - gap;
                //episcan.Sensor.MasterGain = 0;
                //
                this._syncDelayNud.Value = (decimal)0.0;
                this._syncExpNud.Value = (decimal)episcan.Sensor.Exposure;
                this._syncDelayNud.Enabled = false;
                this._syncExpNud.Enabled = false;
            }
            else if (this._veinRb.Checked)
            {
                episcan.Sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;

                float delay = 600f;
                float exposure = 0.5f;

                episcan.Sensor.Delay = (int)(delay - 0.5f * exposure * 1e3f);
                episcan.Sensor.Exposure = exposure;

                this._syncDelayNud.Value = (decimal)delay;
                this._syncExpNud.Value = (decimal)exposure;

                //this._masterGainNud.Value = 100;
            }
            else if (this._manualModeRb.Checked)
            {
                episcan.Sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;
                float exposure = (float)this._syncExpNud.Value;
                //float delay = (float)this.episcan.Sensor.Delay;
                try
                {
                    episcan.Sensor.Delay = (int)((float)this._syncDelayNud.Value - 0.5f * exposure * 1e3f);
                    episcan.Sensor.Exposure = exposure;
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

                episcan.Sensor.Delay = (int)(syncDelay - 0.5f * syncExposure * 1e3f);
                episcan.Sensor.Exposure = syncExposure;

                this._manualModeRb.Checked = true;

                UpdateDispString();
                UpdateUIfromCamera();
            }
        }


        private void DelayOffsetNudOnValueChanged(object sender, EventArgs e)
        {
            episcan.Sensor.DelayOffset = (int)this._delayOffsetNud.Value;
        }

        private void ShowProjectorScreenBtnOnClick(object sender, EventArgs e)
        {
            //if (episcan.Screen != null) episcan.Screen.Dispose();

            //int index = this._screenCb.SelectedIndex;
            //if (index == -1) return;

            //episcan.Screen = new Form();

            //episcan.Screen.Show();

            //Screen scr = Screen.AllScreens[index];

            //episcan.Screen.Location = new Point(scr.WorkingArea.X, scr.WorkingArea.Y);
            //episcan.Screen.FormBorderStyle = FormBorderStyle.None;
            //episcan.Screen.WindowState = FormWindowState.Maximized;


            switch(this._displayColorCb.SelectedItem.ToString())
            {
                case "White":
                    episcan.Screen.BackColor = Color.White;
                    break;
                case "Black":
                    episcan.Screen.BackColor = Color.Black;
                    break;
                case "Red":
                    episcan.Screen.BackColor = Color.FromArgb(255, 0, 0);
                    break;
                case "Green":
                    episcan.Screen.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case "Blue":
                    episcan.Screen.BackColor = Color.FromArgb(0, 0, 255);
                    break;
            }


        }

        private void CloseProjectorScreenBtnOnClick(object sender, EventArgs e)
        {
            if (episcan.Screen != null) episcan.Screen.Close();
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
            episcan.Sensor.PixelFormat = (EpiscanUtil.MySensor.PixelFormatList)Enum.Parse(typeof(EpiscanUtil.MySensor.PixelFormatList), ((ComboBox)sender).SelectedItem.ToString());
        }


        private void ShutterModeCbOnSelectionChangeCommitted(object sender, EventArgs e)
        {
            episcan.Sensor.ShutterMode = (EpiscanUtil.MySensor.ShutterModeList)Enum.Parse(typeof(EpiscanUtil.MySensor.ShutterModeList), ((ComboBox)sender).SelectedItem.ToString());
        }

        private void AutoWbBtnOnClick(object sender, EventArgs e)
        {
            episcan.Sensor.AutoWhitebalanceOn();
        }

        //
        // Modified by Iwaguchi
        //
        private StructuredLight structuredLight;
        private CalibrationForm calibrationForm;
        public int[] Roi { set; get; } = new[] { 227, 200, 1053, 580 };
        //public int[] Roi { set; get; } = new[] { 0, 0, 1600, 1200 };
        //public int[] Roi { set; get; } = new[] { 0, 231, 900, 591 };

        private bool CheckConnection()
        {
            if (episcan.Sensor != null) return true;
            MessageBox.Show("Camera is not connected");
            return false;
        }


        //
        // for 8-bit image
        //
        async Task<byte[]> CaptureAverage8Async(int averageNum = 4)
        {
            float[] arr = null;
            for (int i = 0; i < averageNum; i++)
            {
                await Task.Delay(1);
                var frame = episcan.Sensor.CaptureFrame();
                float[] farr = Array.ConvertAll(frame, b => (float)b);
                arr = arr ?? new float[farr.Length];
                for (int j = 0; j < farr.Length; j++) arr[j] += farr[j];
            }

            return Array.ConvertAll(arr, b => (byte)Math.Min(255, b / averageNum));
        }
        
        byte[] CropImage(byte[] src, int w0, int h0, int[] roi)
        {
            var ch = episcan.Sensor.PixelFormat == MySensor.PixelFormatList.Mono8 ? 1 : 3;
            int x = roi[0], y = roi[1], w = roi[2], h = roi[3];
            var dst = new byte[w * h * ch];
            int count = 0;
            for (int i = y; i < y + h; i++)
                for (int j = x; j < x + w; j++)
                    for (int k = 0; k < ch; k++, count++)
                        dst[count] = src[(i * w0 + j) * ch + k];
            return dst;
        }

        //
        // for 16-bit image
        //
        async Task<UInt16[]> CaptureAverage12Async(int averageNum = 4)
        {
            float[] arr = null;
            for (int i = 0; i < averageNum; i++)
            {
                await Task.Delay(1);
                var frame = episcan.Sensor.CaptureFrame();
                float[] farr = Array.ConvertAll(frame, b => (float)b);
                arr = arr ?? new float[farr.Length];
                for (int j = 0; j < farr.Length; j++) arr[j] += farr[j];
            }
            return Array.ConvertAll(arr, b => (UInt16)Math.Min(65535, b * 16 / averageNum));
        }

        UInt16[] CropImage(UInt16[] src, int w0, int h0, int[] roi)
        {
            var ch = episcan.Sensor.PixelFormat == MySensor.PixelFormatList.Mono12 ? 1 : 3;
            int x = roi[0], y = roi[1], w = roi[2], h = roi[3];
            var dst = new UInt16[w * h];
            int count = 0;
            for (int i = y; i < y + h; i++)
                for (int j = x; j < x + w; j++)
                    for (int k = 0; k < ch; k++, count++)
                        dst[count] = src[(i * w0 + j) * ch + k];
            return dst;
        }

        public async Task<Mat[]> ProjectPatternsAndCaptureAsync()
        {
            var originalPixelFormat = episcan.Sensor.PixelFormat;
            episcan.Sensor.PixelFormat = MySensor.PixelFormatList.Mono8;
            var frames = new List<Mat>();
            for (int i = 0; i < structuredLight.PatternImages.Length; i++)
            {
                var pattern = structuredLight.PatternImages[i];
                episcan.Screen.BackgroundImage = pattern;
                //Application.DoEvents();
                await Task.Delay(100);

                if (episcan.Sensor == null) throw new NullReferenceException();

                var frame = await episcan.CaptureAverage(4);
                frames.Add(frame);
                Cv2.ImWrite($"temp_{i}.png", frame);
                //ImageFileUtil.Png.SaveGray8($"temp_{i}.png", Roi[2], Roi[3], arr);
            }

            episcan.Sensor.PixelFormat = originalPixelFormat;
            return frames.ToArray();
        }

        private async void _structuredLightSingleCaptureButton_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            //Show screen
            ShowProjectorScreenBtnOnClick(null, null);

            // Initialize structured light
            structuredLight = new StructuredLight(episcan.Screen.Size);
            if (!structuredLight.LoadCalibrationData())
                MessageBox.Show("No calibration data loaded");

            var frames = await ProjectPatternsAndCaptureAsync();
            var disparity = structuredLight.CalculateDisparity(frames);

            var sfd = new SaveFileDialog
            {
                Filter = "Txt file (*.txt)|*.txt|All files(*.*)|*.*"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(sfd.FileName, disparity.Select(s => String.Join(" ", s)));
            }
        }

        public async Task CalibrateStructuredLightAsync(double planeDistance, double baseline, double focalLength)
        {
            if (!CheckConnection()) return;

            // Show screen
            ShowProjectorScreenBtnOnClick(null, null);

            // Initialize structured light
            structuredLight = StructuredLight.CalibrationSetup(episcan.Screen.Size);

            // Project patterns and capture
            var frames = await ProjectPatternsAndCaptureAsync();

            structuredLight.Calibrate(planeDistance, baseline, focalLength, frames);
            structuredLight.Calib.Save();
        }

        private void structuredLightCalibrationButton_Click(object sender, EventArgs e)
        {
            calibrationForm = calibrationForm ?? new CalibrationForm();
            calibrationForm.MainForm = this;
            calibrationForm.Show();
        }

        private async void delaySweepStartButton_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;
            
            _manualModeRb.Checked = true;
            UpdateDelayExposure();

            // Show screen
            ShowProjectorScreenBtnOnClick(null, null);
            
            var sfd = new SaveFileDialog()
            {
                Filter = "PNG file (*.png)|*.png|All files(*.*)|*.*",
                FileName = "capture.png"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            var basename = Path.GetDirectoryName(sfd.FileName) + @"/"
                + Path.GetFileNameWithoutExtension(sfd.FileName);

            // color mode
            var isEightBit = episcan.Sensor.PixelFormat == MySensor.PixelFormatList.Mono8 | episcan.Sensor.PixelFormat == MySensor.PixelFormatList.BGR24;
            var isMono = episcan.Sensor.PixelFormat == MySensor.PixelFormatList.Mono8 | episcan.Sensor.PixelFormat == MySensor.PixelFormatList.Mono12;

            // capture black

            //episcan.Screen.BackColor = Color.Black;
            _syncDelayNud.Value = 0;
            UpdateDelayExposure();
            await Task.Delay(50);

            var averageNum = (int)sweepAverageInput.Value;
            var blackFilename = basename + "_black.png";
            if (isEightBit)
            {
                var black = await CaptureAverage8Async(averageNum);
                black = CropImage(black, episcan.Sensor.Width, episcan.Sensor.Height, Roi);
                if (isMono) ImageFileUtil.Png.SaveGray8(blackFilename, Roi[2], Roi[3], black);
                else ImageFileUtil.Png.SaveRgb24(blackFilename, Roi[2], Roi[3], black);
            }
            else
            {
                var black = await CaptureAverage12Async(averageNum);
                black = CropImage(black, episcan.Sensor.Width, episcan.Sensor.Height, Roi);
                if (isMono) ImageFileUtil.Png.SaveGray16(blackFilename, Roi[2], Roi[3], black);
               // if (isMono) ImageFileUtil.Png.SaveBgr48(blackFilename, Roi[2], Roi[3], black);
            }

            // capture delay sweep
            var delayMin = sweepFromInput.Value;
            var delayMax = sweepToInput.Value;
            var delayStep = sweepStepInput.Value;
            var pcMin = PC_From.Value;
            var pcMax = PC_To.Value;
            var pcStep = PC_Step.Value;


            episcan.Screen.BackColor = Color.White;

            var captureNum = (delayMax - delayMin) / delayStep + 1;
            var pccapNum = (pcMax - pcMin) / pcStep + 1;
            for (int pc = 0; pc < pccapNum; pc++)
            {
                var pcvalue = (int)(pcMin + pc * pcStep);
                var foname = basename + $"_{pcvalue:D2}" + @"/" + Path.GetFileNameWithoutExtension(sfd.FileName);
                Directory.CreateDirectory(foname);
                episcan.Sensor.PixelClock = pcvalue;
                //_pixelClockCb.ValueMember = pcvalue.ToString();

                for (int i = 0; i < captureNum; i++)
                {
                    var delay = (int)(delayMin + i * delayStep);
                    _syncDelayNud.Value = delay;
                    UpdateDelayExposure();
                    //await Task.Delay(50);

                    var exposure = (int)Math.Round(episcan.Sensor.Exposure * 1000);
                    var suffix = $"_{i:D4}_{exposure:D5}_{delay:D5}";
                    var sweepFilename = foname + suffix + ".png";

                    if (isEightBit)
                    {
                        var arr = await CaptureAverage8Async(averageNum);
                        arr = CropImage(arr, episcan.Sensor.Width, episcan.Sensor.Height, Roi);
                        if (isMono) ImageFileUtil.Png.SaveGray8(sweepFilename, Roi[2], Roi[3], arr);
                        else ImageFileUtil.Png.SaveBgr24(sweepFilename, Roi[2], Roi[3], arr);
                    }
                    else
                    {
                        var arr = await CaptureAverage12Async(averageNum);
                        arr = CropImage(arr, episcan.Sensor.Width, episcan.Sensor.Height, Roi);
                        if (isMono) ImageFileUtil.Png.SaveGray16(sweepFilename, Roi[2], Roi[3], arr);
                        else ImageFileUtil.Png.SaveBgr48(sweepFilename, Roi[2], Roi[3], arr);
                    }

                }
            }

            // measure disparity
            structuredLight = new StructuredLight(episcan.Screen.Size);
            if (!structuredLight.LoadCalibrationData())
                MessageBox.Show("No calibration data loaded");

            _syncDelayNud.Value = 0;
            UpdateDelayExposure();
            await Task.Delay(50);

            var frames = await ProjectPatternsAndCaptureAsync();
            var disparity = structuredLight.CalculateDisparity(frames);
            File.WriteAllLines(basename + "_disp.txt", disparity.Select(s => String.Join(" ", s)));

            // restart capture
            UpdateDelayExposure();
            if (delaySweepTurnOffProjector.Checked) ProjectorPowerToggleBtnOnClick(null, null);
        }
        
        public void DetectSyncBoard()
        {
            var portFound = SyncBoard.FindSyncboard();
            if (portFound == "NOT_FOUND")
            {
                _toolStripStatusLabel1.Text = "Sync board not found";
                serialsCb.Enabled = true;
                return;
            }
            else
            {
                _toolStripStatusLabel1.Text = $"Sync board found at {portFound}";
            }

            var cbIdx = serialsCb.Items.IndexOf(portFound);
            if (cbIdx != -1)
            {
                serialsCb.SelectedIndex = cbIdx;
                serialsCb.Enabled = false;
            }
            else
            {
                serialsCb.Enabled = true;
            }
        }

        private void _projectorDetectBtn_Click(object sender, EventArgs e)
        {
            DetectSyncBoard();
        }

        bool cropTextBoxChangeEnabled = true;
        private void CropTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!cropTextBoxChangeEnabled) return;
            var x = int.Parse(CropXTextBox.Lines[0]);
            var y = int.Parse(CropYTextBox.Lines[0]);
            var w = int.Parse(CropWTextBox.Lines[0]);
            var h = int.Parse(CropHTextBox.Lines[0]);
            
            if (episcan.Sensor != null)
            {
                x = Math.Max(Math.Min(x, episcan.Sensor.Width), 0);
                y = Math.Max(Math.Min(y, episcan.Sensor.Height), 0);
                w = Math.Max(Math.Min(w, episcan.Sensor.Width - x), 0);
                h = Math.Max(Math.Min(h, episcan.Sensor.Height - y), 0);
            }

            var roi = new[] { x, y, w, h };
            episcan.ImageRoi = roi;
            SetUIImageRoi();
        }

        private void SetUIImageRoi()
        {
            cropTextBoxChangeEnabled = false;
            CropXTextBox.Lines = new[] { episcan.ImageRoi[0].ToString() };
            CropYTextBox.Lines = new[] { episcan.ImageRoi[1].ToString() };
            CropWTextBox.Lines = new[] { episcan.ImageRoi[2].ToString() };
            CropHTextBox.Lines = new[] { episcan.ImageRoi[3].ToString() };
            CropTextBox_TextChanged(null, null);
            cropTextBoxChangeEnabled = true;
        }



        private void _dsweepstartbtn_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            count = 0;
            System.Diagnostics.Trace.WriteLine("テストメッセージ");


            episcan.Sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;

            float exposure = 0.5f; // 0.5[ms]

            episcan.Sensor.Delay = (int)(-0.5f * exposure * 1e3f);
            //episcan.Sensor.Exposure = exposure;


            this._syncExpNud.Value = (decimal)exposure;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            
            //float syncDelay =
            float exposure = 0.5f; // 0.5[ms]
            float s_rate = 30;
            float delay = (float)count * s_rate;
            float[] arr = null;


            if (_dsRangeSwitch.Checked == true)
            {
                delay = delay%((float)dsweepToValue.Value - (float)dsweepFromValue.Value) + (float)dsweepFromValue.Value;

            }
            if (_dsExclusionSwitch.Checked == true)
            {
                if((float)dsExcFromValue.Value <=delay && delay <= (float)dsExcToValue.Value)
                {
                    count += (int)(((float)dsExcToValue.Value - (float)dsExcFromValue.Value) / s_rate);
                }

            }

            this._syncDelayNud.Value = (decimal)delay;
            episcan.Sensor.Delay = (int)(delay - 0.5f * exposure * 1e3f);
            if (delay == (float)dsweepFromValue.Value)
            {
                frames_for_depth = new List<Mat>();
            }

            if (bsShowcount % 2 == 1)
            {
                var frame = episcan.Sensor.CaptureFrame();
                float[] farr = Array.ConvertAll(frame, b => (float)b);
                arr = arr ?? new float[farr.Length];
                Mat image = new Mat(Height, Width, MatType.CV_16UC3, arr);
                frames_for_depth.Add(image);
                if (delay == (float)dsweepToValue.Value)
                {

                }
            }

            UpdateUIfromCamera();
            UpdateDispString();
            count++;
        }


        private void _dsweepstopbtn_Click(object sender, EventArgs e)
        {
            if(timer1.Enabled==true) timer1.Enabled = false;
            else timer1.Enabled = true;
        }

        private void dsRangeSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if(_dsRangeSwitch.Checked==true)
            {
                dsweepFromValue.Enabled = true;
                dsweepToValue.Enabled = true;
            }
            else
            {
                dsweepFromValue.Enabled = false;
                dsweepToValue.Enabled = false;
            }
            
            
        }

        private void dsExclusionSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (_dsExclusionSwitch.Checked == true)
            {
                dsExcFromValue.Enabled = true;
                dsExcToValue.Enabled = true;
            }
            else
            {
                dsExcFromValue.Enabled = false;
                dsExcToValue.Enabled = false;
            }


        }

        public async Task<byte[]> add_delay_images(int averageNum = 4)
        {
            float[] arr = null;
            for (int i = 0; i < averageNum; i++)
            {
                await Task.Delay(1);
                var frame = episcan.Sensor.CaptureFrame();
                float[] farr = Array.ConvertAll(frame, b => (float)b);
                arr = arr ?? new float[farr.Length];
                for (int j = 0; j < farr.Length; j++) arr[j] += farr[j];
            }

            return Array.ConvertAll(arr, b => (byte)Math.Min(255, b / averageNum));
        }


    }
}
