using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EpiscanUtil
{
    public class MySensor
    {
        public int val = 0;
        /// <summary>
        /// Camera Device
        /// </summary>
        private uEye.Camera _camera = null;

        /// <summary>
        /// Window handle
        /// </summary>
        IntPtr _displayHandle = IntPtr.Zero;

        private bool OnLive = false;

        public float Scaler = 100.0f;

        public int _delayOffset = 0;
        public int _delay = 0;

        private double _minExp = 0, _maxExp = 0, _incExp = 0;

        public string DispText = null;

        public delegate void SensorParameterChangedDelegate();
        public SensorParameterChangedDelegate OnSensorParameterChanged = () => { };

        public enum ShutterModeList
        {
            Global,
            Rolling,
        };

        public enum TriggerModeList
        {
            Software,
            RisingEdge,
            Continuous,
        }

        public enum PixelFormatList
        {
            Mono8,
            Mono12,
            Raw8,
            Raw12,
            Bayer8,
            Bayer12,
            BGR24,
            BGR36,
        }
        public MySensor(int deviceId = -1)
        {
            InitCamera(deviceId);
        }
        public MySensor(IntPtr handle, int deviceId = -1)
        {
            this._displayHandle = handle;

            InitCamera(deviceId);


        }


        /// <summary>
        /// Delay[us]
        /// </summary>
        public Int32 Delay
        {
            set
            {
                this._delay = value;

                uEye.Defines.Status stat = this._camera.Trigger.Delay.Set(value + this._delayOffset);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set delay.");
                OnSensorParameterChanged();
            }
            get
            {
                Int32 value;
                uEye.Defines.Status stat = this._camera.Trigger.Delay.Get(out value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get delay.");
                return value - this._delayOffset;
            }
        }
        public int DelayOffset
        {
            set
            {
                this._delayOffset = value;
                uEye.Defines.Status stat = this._camera.Trigger.Delay.Set(this._delay + this._delayOffset);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set delay.");
                OnSensorParameterChanged();
            }
            get
            {
                return this._delayOffset;
            }
        }


        public ShutterModeList ShutterMode
        {
            set
            {
                //bool started = false;
                //this._camera.Acquisition.HasStarted(out started);
                //if (started) this._camera.Acquisition.Stop();

                // if new shutter mode is same as old mode, then return
                if (value == this.ShutterMode) return;

                switch (value)
                {
                    case ShutterModeList.Global:
                        {
                            uEye.Defines.Status stat = this._camera.Device.Feature.ShutterMode.Set(uEye.Defines.Shuttermode.Global);
                            if (stat != uEye.Defines.Status.Success) throw new Exception("Failed to set global shutter mode.");
                        }
                        break;
                    case ShutterModeList.Rolling:
                        {
                            uEye.Defines.Status stat = this._camera.Device.Feature.ShutterMode.Set(uEye.Defines.Shuttermode.Rolling);
                            if (stat != uEye.Defines.Status.Success) throw new Exception("Failed to set rolling shutter mode.");
                        }
                        break;
                    default:
                        throw new Exception("Unknown shutter mode.");
                }
                OnSensorParameterChanged();

                //if (started) this._camera.Acquisition.Capture();

            }
            get
            {
                uEye.Defines.Shuttermode mode;
                uEye.Defines.Status stat = this._camera.Device.Feature.ShutterMode.Get(out mode);
                switch (mode)
                {
                    case uEye.Defines.Shuttermode.Global:
                        return ShutterModeList.Global;
                    case uEye.Defines.Shuttermode.Rolling:
                        return ShutterModeList.Rolling;
                    default:
                        throw new Exception("Unknown shutter mode.");
                }
            }
        }
        public TriggerModeList TrigerMode
        {
            set
            {
                uEye.Defines.Status stat = uEye.Defines.Status.NoSuccess;

                if (this.OnLive)
                {
                    this._camera.Acquisition.Stop();
                }

                switch (value)
                {
                    case TriggerModeList.Software:
                        stat = this._camera.Trigger.Set(uEye.Defines.TriggerMode.Software);               
                        break;
                    case TriggerModeList.RisingEdge:
                        stat = this._camera.Trigger.Set(uEye.Defines.TriggerMode.Lo_Hi);
                        break;
                    case TriggerModeList.Continuous:
                        stat = this._camera.Trigger.Set(uEye.Defines.TriggerMode.Continuous);
                        break;
                    default:
                        throw new Exception("Unknown shutter mode.");
                }

                if (stat != uEye.Defines.Status.Success) throw new Exception("Failed to set trigger mode.");

                if (this.OnLive)
                {
                    this._camera.Acquisition.Capture();
                }
                OnSensorParameterChanged();
            }
            get
            {
                uEye.Defines.TriggerMode mode;
                uEye.Defines.Status stat = this._camera.Trigger.Get(out mode);
                switch (mode)
                {
                    case uEye.Defines.TriggerMode.Software:
                        return  TriggerModeList.Software;
                    case uEye.Defines.TriggerMode.Lo_Hi:
                        return TriggerModeList.RisingEdge;
                    case uEye.Defines.TriggerMode.Continuous:
                        return TriggerModeList.Continuous;
                    default:
                        throw new Exception("Unknown trigger mode.");
                }


            }
        }


        public void SetRGBGain(Int32 r, Int32 g, Int32 b)
        {
            {
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Factor.SetRed(r);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set red gain.");
            }
            {
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Factor.SetGreen(g);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set green gain.");
            }
            {
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Factor.SetBlue(b);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set blue gain.");
            }
            OnSensorParameterChanged();
        }
        public void StartCapture()
        {
            uEye.Defines.Status stat = _camera.Acquisition.Capture();
            if (stat != uEye.Defines.Status.Success) throw new Exception("Start Live Video failed");
        }
        private void InitCamera(int deviceId = -1)
        {
            _camera = new uEye.Camera();

            // Open Camera
            uEye.Defines.Status stat = uEye.Defines.Status.NoSuccess;

            if (deviceId != -1) stat = _camera.Init(deviceId | (Int32)uEye.Defines.DeviceEnumeration.UseDeviceID);
            else
            {
                stat = _camera.Init();
            }
            if (stat != uEye.Defines.Status.Success) throw new Exception("Camera initializing failed");

            // max-min exposure
            this._camera.Timing.Exposure.GetRange(out _minExp, out _maxExp, out _incExp);

            // Default
            this.MasterGain = 100;

            // Trigger Mode
            this._camera.Trigger.Set(uEye.Defines.TriggerMode.Software);

            this._camera.PixelFormat.Set(uEye.Defines.ColorMode.Mono8);

            // Allocate Memory
            stat = _camera.Memory.Allocate();
            if (stat != uEye.Defines.Status.Success) throw new Exception("Allocate Memory failed");


            // Connect Event
            if (this._displayHandle != IntPtr.Zero) _camera.EventFrame += onFrameEvent;
        }
        public void ExitCamera()
        {
            if (_camera != null) _camera.Exit();
            _camera = null;
        }
        public Int32[] CaptureFrame()
        {
            int width = this.Width;
            int height = this.Height;

            int s32MemID;
            uEye.Defines.Status stat = _camera.Memory.GetActive(out s32MemID);

            uEye.Defines.ColorMode cm;
            stat = this._camera.PixelFormat.Get(out cm);

            int timeout = 50; // [ms]
            // freeze capture
            stat = this._camera.Acquisition.Freeze(timeout);

            // grab
            int[] int32buff = null;
            stat = this._camera.Memory.CopyToArray(s32MemID, cm, out int32buff);

            // restart capture
            while (stat != uEye.Defines.Status.Success)
            {
                stat = _camera.Acquisition.Capture();
                //if (stat == uEye.Defines.Status.Success) break;
                Task.Delay(1);
            }
            if (stat != uEye.Defines.Status.Success) throw new Exception("Start Live Video failed");


            return int32buff;
        }

        public void depthsweep(int val)
        {

        }

        public void onFrameEvent(object sender, EventArgs e)
        {



            uEye.Defines.Status stat;

            uEye.Camera camera = sender as uEye.Camera;

            Int32 s32MemID;
            stat = camera.Memory.GetActive(out s32MemID);
            if (stat != uEye.Defines.Status.Success) throw new Exception("failed.");

            uEye.Defines.ColorMode cm;
            stat = camera.PixelFormat.Get(out cm);

            int width = this.Width;
            int height = this.Height;


            int[] int32buff = null;

            while (stat == uEye.Defines.Status.NoSuccess || int32buff == null)
                stat = camera.Memory.CopyToArray(s32MemID, cm, out int32buff);

            float scale = this.Scaler / 100f;
            if (cm == uEye.Defines.ColorMode.Mono12) scale *= 255f / 4095f;

            byte[] buff = Array.ConvertAll(int32buff, i => (byte)Math.Min(255, (float)i * scale));

            System.Drawing.Imaging.PixelFormat oPixformat = System.Drawing.Imaging.PixelFormat.Undefined;

            if (cm == uEye.Defines.ColorMode.BGR8Packed)
            {
                oPixformat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            }
            else if (cm == uEye.Defines.ColorMode.Mono8 || cm == uEye.Defines.ColorMode.Mono12) oPixformat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;

            Bitmap bmp = new Bitmap(width, height, oPixformat);
            
            {
                // Lock Bitmap
                System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(
                    new Rectangle(0, 0, width, height),
                    System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    oPixformat);


                // Get pixel data as byte array
                IntPtr ptr = bmpData.Scan0;
                //byte[] pixels = new byte[bmpData.Stride * height];
                //System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);

                // Restore pixel data
                System.Runtime.InteropServices.Marshal.Copy(buff, 0, ptr, bmpData.Stride * height);

                // Grayscale palette
                if (oPixformat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                {
                    System.Drawing.Imaging.ColorPalette palette = bmp.Palette;
                    for (int i = 0; i < palette.Entries.Length; ++i) palette.Entries[i] = Color.FromArgb(i, i, i);
                    bmp.Palette = palette;
                }

                // Unlock
                bmp.UnlockBits(bmpData);

            }
            //if(oPixformat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            //{
            //    bmp = bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            //    Graphics g = Graphics.FromImage(bmp);

            //    //フォントオブジェクトの作成
            //    Font fnt = new Font("Consolas", 60);
            //    {
            //        g.DrawString(this.DispText, fnt, Brushes.White, 0, 0);
            //    }
            //    g.Dispose();
            //}

            {
                // Get handle to form.
                IntPtr hwnd = this._displayHandle;

                Control c = Form.FromHandle(this._displayHandle);

                // Create new graphics object using handle to window.
                Graphics newGraphics = Graphics.FromHwnd(hwnd);

                // Draw rectangle to screen.
                newGraphics.DrawImage(bmp, new Rectangle(0, 0, c.Width, c.Height));

                // Dispose of new graphics.
                newGraphics.Dispose();

                bmp.Dispose();

                return;
            }
        }

        private void onAutoShutterFinished(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("AutoShutter finished...");
        }
        public void Close()
        {
            this._camera.Exit();
        }

        public bool EnableGainBoost
        {
            get { return this._camera.Gain.Hardware.Boost.Enabled; }
            set
            {
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Boost.SetEnable(value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to enable/disable gain boost.");
                OnSensorParameterChanged();
            }
        }

        /// <summary>
        /// Exposure [ms]
        /// </summary>
        public double Exposure
        {
            set
            {
                double actualExp = value;
                if (value < this._minExp) actualExp = this._minExp;
                else if (value > this._maxExp) actualExp = this._maxExp;
                
                uEye.Defines.Status stat = _camera.Timing.Exposure.Set(actualExp);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set exposure.");
                OnSensorParameterChanged();
            }
            get
            {
                double value;
                uEye.Defines.Status stat = _camera.Timing.Exposure.Get(out value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get exposure.");
                return value;
            }
        }
        public int MasterGain
        {
            set
            {
                bool opened = this._camera.Gain.IsOpened;
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Scaled.SetMaster(value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set master gain.");
                OnSensorParameterChanged();
            }
            get
            {
                int mastergain;
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Scaled.GetMaster(out mastergain);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get master gain.");
                return mastergain;
            }
        }
        public int GainRed
        {
            set
            {
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Scaled.SetRed(value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set red gain.");
                OnSensorParameterChanged();
            }
            get
            {
                int redgain;
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Scaled.GetRed(out redgain);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get red gain.");
                return redgain;
            }
        }
        public int GainGreen
        {
            set
            {
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Scaled.SetGreen(value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set green gain.");
                OnSensorParameterChanged();
            }
            get
            {
                int greengain;
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Scaled.GetGreen(out greengain);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get green gain.");
                return greengain;
            }
        }
        public int GainBlue
        {
            set
            {
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Scaled.SetBlue(value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set blue gain.");
                OnSensorParameterChanged();
            }
            get
            {
                int bluegain;
                uEye.Defines.Status stat = this._camera.Gain.Hardware.Scaled.GetBlue(out bluegain);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get blue gain.");
                return bluegain;
            }
        }
        public int PixelClock
        {
            set
            {
                int[] list;
                this._camera.Timing.PixelClock.GetList(out list);

                if (!list.Contains(value)) throw new Exception("failed to set pixel clock.");

                uEye.Defines.Status stat = this._camera.Timing.PixelClock.Set(value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set pixel clock.");
                OnSensorParameterChanged();
            }
            get
            {
                int pc;
                uEye.Defines.Status stat = _camera.Timing.PixelClock.Get(out pc);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get pixel clock.");
                return pc;
            }
        }
        public int[] AvailablePixelClock
        {
            get
            {
                int[] list;
                this._camera.Timing.PixelClock.GetList(out list);
                return list;
            }
        }
        public string[] AvailableShutterModes
        {
            get
            {
                List<string> list = new List<string>();
                foreach (uEye.Defines.Shuttermode m in Enum.GetValues(typeof(uEye.Defines.Shuttermode)))
                {
                    if (this._camera.Device.Feature.ShutterMode.IsSupported(m))
                    {
                        switch (m)
                            {
                            case uEye.Defines.Shuttermode.Rolling:
                                list.Add(ShutterModeList.Rolling.ToString());
                                break;
                            case uEye.Defines.Shuttermode.Global:
                                list.Add(ShutterModeList.Global.ToString());
                                break;
                            case uEye.Defines.Shuttermode.RollingGlobalStart:
                                break;
                            case uEye.Defines.Shuttermode.GlobalAlternativeTiming:
                                break;
                            default:
                                break;

                        }
                    }
                }
                return list.ToArray();
            }
        }
        public string[] AvailableTriggerModes
        {
            get
            {
                uEye.Defines.TriggerMode supported_modes;
                this._camera.Trigger.GetSupported(out supported_modes);

                List<string> list = new List<string>();
                foreach (uEye.Defines.TriggerMode m in Enum.GetValues(typeof(uEye.Defines.TriggerMode)))
                {
                    if((m& supported_modes) == m)
                    {
                        switch (m)
                        {
                            case uEye.Defines.TriggerMode.Software:
                                list.Add(TriggerModeList.Software.ToString());
                                break;
                            case uEye.Defines.TriggerMode.Lo_Hi:
                                list.Add(TriggerModeList.RisingEdge.ToString());
                                break;
                            case uEye.Defines.TriggerMode.Continuous:
                                list.Add(TriggerModeList.Continuous.ToString());
                                break;
                            default:
                                break;

                        }
                    }
                }
                return list.ToArray();
            }
        }
        public Rectangle AOI
        {
            get
            {
                Rectangle rect;
                uEye.Defines.Status stat = this._camera.Size.AOI.Get(out rect);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get AOI.");
                return rect;
            }
            set
            {
                uEye.Defines.Status stat = this._camera.Size.AOI.Set(value);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set AOI.");
            }
        }
        public int Width
        {
            get
            {
                Rectangle rect;
                uEye.Defines.Status stat = this._camera.Size.AOI.Get(out rect);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get AOI.");
                return rect.Width;
            }
            set
            {
                Rectangle rect = this.AOI;
                rect.Width = value;
                this.AOI = rect;
            }
        }
        public int Height
        {
            get
            {
                Rectangle rect;
                uEye.Defines.Status stat = this._camera.Size.AOI.Get(out rect);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get AOI.");
                return rect.Height;
            }
            set
            {
                Rectangle rect = this.AOI;
                rect.Height = value;
                this.AOI = rect;
            }
        }
        public PixelFormatList PixelFormat
        {
            get
            {
                uEye.Defines.ColorMode cm;
                uEye.Defines.Status stat = this._camera.PixelFormat.Get(out cm);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get color mode.");

                switch (cm)
                {
                    case uEye.Defines.ColorMode.Mono8:
                        return PixelFormatList.Mono8;
                    case uEye.Defines.ColorMode.Mono12:
                        return PixelFormatList.Mono12;
                    case uEye.Defines.ColorMode.SensorRaw8:
                        return PixelFormatList.Raw8;
                    case uEye.Defines.ColorMode.SensorRaw12:
                        return PixelFormatList.Raw12;
                    case uEye.Defines.ColorMode.BGR8Packed:
                        return PixelFormatList.BGR24;
                    default:
                        throw new Exception("faild to get pixel format (colors not supported, yet.");
                        //case uEye.Defines.ColorMode.BGR8Packed:
                        //    return PixelFormatList.BGR24;
                        //case uEye.Defines.ColorMode.BGRA12Unpacked:
                        //    return PixelFormatList.BGR36;
                }
            }
            set
            {
                bool hasStarted = false;
                this._camera.Acquisition.HasStarted(out hasStarted);

                if (hasStarted)
                {
                    if (_camera.Acquisition.Stop() != uEye.Defines.Status.Success) throw new Exception("Stop camera failed");
                }

                // store current color mode
                uEye.Defines.ColorMode cm;
                uEye.Defines.Status stat = this._camera.PixelFormat.Get(out cm);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to get color mode.");
                switch (value)
                {
                    case PixelFormatList.Mono8: cm = uEye.Defines.ColorMode.Mono8; break;
                    case PixelFormatList.Mono12: cm = uEye.Defines.ColorMode.Mono12; break;
                    case PixelFormatList.Raw8: cm = uEye.Defines.ColorMode.SensorRaw8; break;
                    case PixelFormatList.Raw12: cm = uEye.Defines.ColorMode.SensorRaw12; break;
                    case PixelFormatList.BGR24: cm = uEye.Defines.ColorMode.BGR8Packed; break;
                    default:
                        throw new Exception("faild to set pixel format (colors not supported, yet.");
                }
                stat = this._camera.PixelFormat.Set(cm);
                if (stat != uEye.Defines.Status.Success) throw new Exception("failed to set color mode.");

                if (hasStarted)
                {
                    if (_camera.Acquisition.Capture() != uEye.Defines.Status.Success) throw new Exception("Start camera failed");
                }

                Int32[] memList;
                stat = _camera.Memory.GetList(out memList);
                stat = _camera.Memory.Free(memList);

                this._camera.Memory.Allocate();

                OnSensorParameterChanged();
            }

        }

        public static void GetCameraList(out string[] models, out long[]devids)
        {
            uEye.Types.CameraInformation[] list;
            uEye.Info.Camera.GetCameraList(out list);

            models = null;
            devids = null;

            if (list == null) return;
            else
            {
                models = new string[list.Length];
                devids = new long[list.Length];

                for(int i = 0; i < list.Length;i++)
                {
                    models[i] = list[i].Model;
                    devids[i] = list[i].DeviceID;
                }
            }
        }

        public void AutoWhitebalanceOn()
        {
            uEye.Defines.Status stat = this._camera.AutoFeatures.Software.WhiteBalance.SetType(uEye.Defines.Whitebalance.Type.GreyWorld);
            if (stat != uEye.Defines.Status.Success) throw new Exception("failed.");


            stat = this._camera.AutoFeatures.Software.WhiteBalance.SetEnable(uEye.Defines.ActivateMode.Once);
            if (stat != uEye.Defines.Status.Success) throw new Exception("failed.");
        }
    }
}
