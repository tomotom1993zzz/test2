using EpiscanUtil;
using UserScriptHost;
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
using System.Reflection;

namespace DelaySweepDemo
{
    public partial class Form1 : Form
    {
        private MyEpiscan episcan = null;

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

            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));

            episcan = new MyEpiscan();
            episcan.ImageRoi = Roi;

            UpdateCameraList();
            InitUISyncboard();
            InitUIScreen();
            InitUIScript();

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

                this._syncDelayNud.Value = episcan.Sensor.Delay;
                this._syncExpNud.Value = (decimal)episcan.Sensor.Exposure;

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
                episcan.Sensor.OnSensorParameterChanged = UpdateUIfromCamera;

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


        void UpdateDelayExposure()
        {
            if (this._directModeRb.Checked)
            {
                float exposure = 0.5f; // 0.5[ms]
                episcan.Sensor.ShutterMode = EpiscanUtil.MySensor.ShutterModeList.Rolling;
                episcan.Sensor.Delay = (int)(-0.5f * exposure * 1e3f);
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
        public int[] Roi { set; get; } = new[] { 50, 308, 938, 540 };
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

        dynamic currentScript = null;
        string[] scriptFiles = null;
        private void InitUIScript()
        {
            var scriptDir = @"scripts";
            scriptFiles = Directory.GetFiles(scriptDir, "*.cs");
            ScriptListBox.Items.Clear();
            foreach (var scriptFile in scriptFiles)
            {
                ScriptListBox.Items.Add(scriptFile.Replace($@"{scriptDir}\", ""));
            }
        }
        
        private void ScriptListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ScriptListBox.SelectedIndex == -1) return;

            ScriptRunButton.Enabled = false;

            currentScript = ScriptLoader.Load(scriptFiles[ScriptListBox.SelectedIndex]);
            currentScript.Configure(episcan);

            PropertyInfo[] props = currentScript.GetType().GetProperties();
            int count = 0;
            ScriptOptionContainer.Controls.Clear();
            foreach (var prop in props)
            {
                if (prop.PropertyType == typeof(int) ||
                    prop.PropertyType == typeof(float) ||
                    prop.PropertyType == typeof(double))
                {
                    ScriptOptionContainer.Controls.Add(new Label
                    {
                        Size = new Size(77, 12),
                        Text = prop.Name,
                        Location = new Point(6, 20 + 26 * count),
                    });
                    var numeric = new NumericUpDown
                    {
                        Size = new Size(114, 19),
                        Name = prop.Name,
                        Location = new Point(86, 18 + 26 * count),
                        Maximum = Decimal.MaxValue,
                        Minimum = Decimal.MinValue,
                        Value = Decimal.Parse(prop.GetValue(currentScript).ToString()),
                        DecimalPlaces = prop.PropertyType == typeof(int) ? 0 : 3
                    };
                    numeric.ValueChanged += (object _sender, EventArgs _e) =>
                    {
                        if (prop.PropertyType == typeof(int))
                            prop.SetValue(currentScript, (int)numeric.Value);
                        else if (prop.PropertyType == typeof(float))
                            prop.SetValue(currentScript, (float)numeric.Value);
                        else if (prop.PropertyType == typeof(double))
                            prop.SetValue(currentScript, (double)numeric.Value);
                    };
                    ScriptOptionContainer.Controls.Add(numeric);
                }
                count++;
            }

            ScriptRunButton.Enabled = true;
        }

        private async void ScriptRunButton_Click(object sender, EventArgs e)
        {
            await currentScript?.Run(episcan);
        }

        private void _screenCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            episcan.Screen.ScreenIndex = _screenCb.SelectedIndex;
        }
    }
}
