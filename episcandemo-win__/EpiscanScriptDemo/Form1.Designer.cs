namespace DelaySweepDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.camerasCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._gainBoostCb = new System.Windows.Forms.CheckBox();
            this._masterGainNud = new System.Windows.Forms.NumericUpDown();
            this._pixelclockLb = new System.Windows.Forms.Label();
            this._pixelClockCb = new System.Windows.Forms.ComboBox();
            this._shutterModeCb = new System.Windows.Forms.ComboBox();
            this._shuttermodeLb = new System.Windows.Forms.Label();
            this._triggerModeCb = new System.Windows.Forms.ComboBox();
            this._triggerLb = new System.Windows.Forms.Label();
            this.serialsCb = new System.Windows.Forms.ComboBox();
            this._portLb = new System.Windows.Forms.Label();
            this._projectorPowerBtn = new System.Windows.Forms.Button();
            this._camWidthNud = new System.Windows.Forms.NumericUpDown();
            this._camHeightNud = new System.Windows.Forms.NumericUpDown();
            this._sizeLb = new System.Windows.Forms.Label();
            this._mastergainLb = new System.Windows.Forms.Label();
            this._singleCapBtn = new System.Windows.Forms.Button();
            this._cameraGb = new System.Windows.Forms.GroupBox();
            this._autoWbBtn = new System.Windows.Forms.Button();
            this._delayOffsetLb = new System.Windows.Forms.Label();
            this._delayOffsetNud = new System.Windows.Forms.NumericUpDown();
            this._restartBtn = new System.Windows.Forms.Button();
            this._colorModeLb = new System.Windows.Forms.Label();
            this._cmodeCb = new System.Windows.Forms.ComboBox();
            this._scaleLb = new System.Windows.Forms.Label();
            this._scalerNud = new System.Windows.Forms.NumericUpDown();
            this._connectBtn = new System.Windows.Forms.Button();
            this._cameraCb = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._projectorDetectBtn = new System.Windows.Forms.Button();
            this._displayColorCb = new System.Windows.Forms.ComboBox();
            this._refreshScreenBtn = new System.Windows.Forms.Button();
            this._screenCb = new System.Windows.Forms.ComboBox();
            this._closeProjectorScreenBtn = new System.Windows.Forms.Button();
            this._showProjectorScreenBtn = new System.Windows.Forms.Button();
            this._syncDelayNud = new System.Windows.Forms.NumericUpDown();
            this._episcanGb = new System.Windows.Forms.GroupBox();
            this._veinRb = new System.Windows.Forms.RadioButton();
            this._resetBtn = new System.Windows.Forms.Button();
            this._manualModeRb = new System.Windows.Forms.RadioButton();
            this._indirectModeRb = new System.Windows.Forms.RadioButton();
            this._directModeRb = new System.Windows.Forms.RadioButton();
            this._regularModeRb = new System.Windows.Forms.RadioButton();
            this._syncExpNud = new System.Windows.Forms.NumericUpDown();
            this._expLb = new System.Windows.Forms.Label();
            this._delayLb = new System.Windows.Forms.Label();
            this._exitBtn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ScriptOptionContainer = new System.Windows.Forms.GroupBox();
            this.ScriptRunButton = new System.Windows.Forms.Button();
            this.ScriptListBox = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._masterGainNud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._camWidthNud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._camHeightNud)).BeginInit();
            this._cameraGb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._delayOffsetNud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._scalerNud)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._syncDelayNud)).BeginInit();
            this._episcanGb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._syncExpNud)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem,
            this.toolStripMenuItem1,
            this.camerasCToolStripMenuItem,
            this.helpHToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(503, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitXToolStripMenuItem});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            // 
            // exitXToolStripMenuItem
            // 
            this.exitXToolStripMenuItem.Name = "exitXToolStripMenuItem";
            this.exitXToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.exitXToolStripMenuItem.Text = "Exit(&X)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(82, 20);
            this.toolStripMenuItem1.Text = "Projector(&P)";
            // 
            // camerasCToolStripMenuItem
            // 
            this.camerasCToolStripMenuItem.Name = "camerasCToolStripMenuItem";
            this.camerasCToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.camerasCToolStripMenuItem.Text = "Cameras(&C)";
            // 
            // helpHToolStripMenuItem
            // 
            this.helpHToolStripMenuItem.Name = "helpHToolStripMenuItem";
            this.helpHToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.helpHToolStripMenuItem.Text = "Help(&H)";
            // 
            // _gainBoostCb
            // 
            this._gainBoostCb.AutoSize = true;
            this._gainBoostCb.Location = new System.Drawing.Point(116, 120);
            this._gainBoostCb.Name = "_gainBoostCb";
            this._gainBoostCb.Size = new System.Drawing.Size(77, 16);
            this._gainBoostCb.TabIndex = 4;
            this._gainBoostCb.Text = "GainBoost";
            this._gainBoostCb.UseVisualStyleBackColor = true;
            this._gainBoostCb.CheckedChanged += new System.EventHandler(this.GainBoostCbOnCheckedChanged);
            // 
            // _masterGainNud
            // 
            this._masterGainNud.Location = new System.Drawing.Point(115, 142);
            this._masterGainNud.Name = "_masterGainNud";
            this._masterGainNud.Size = new System.Drawing.Size(120, 19);
            this._masterGainNud.TabIndex = 9;
            this._masterGainNud.ValueChanged += new System.EventHandler(this.MasterGainOnValueChanged);
            // 
            // _pixelclockLb
            // 
            this._pixelclockLb.Location = new System.Drawing.Point(45, 20);
            this._pixelclockLb.Name = "_pixelclockLb";
            this._pixelclockLb.Size = new System.Drawing.Size(67, 12);
            this._pixelclockLb.TabIndex = 12;
            this._pixelclockLb.Text = "Pixel Clock";
            this._pixelclockLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _pixelClockCb
            // 
            this._pixelClockCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._pixelClockCb.FormattingEnabled = true;
            this._pixelClockCb.Location = new System.Drawing.Point(115, 17);
            this._pixelClockCb.Name = "_pixelClockCb";
            this._pixelClockCb.Size = new System.Drawing.Size(120, 20);
            this._pixelClockCb.TabIndex = 13;
            this._pixelClockCb.SelectedIndexChanged += new System.EventHandler(this.PixelClockCbOnSelectedIndexChanged);
            // 
            // _shutterModeCb
            // 
            this._shutterModeCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._shutterModeCb.FormattingEnabled = true;
            this._shutterModeCb.Location = new System.Drawing.Point(115, 43);
            this._shutterModeCb.Name = "_shutterModeCb";
            this._shutterModeCb.Size = new System.Drawing.Size(120, 20);
            this._shutterModeCb.TabIndex = 14;
            this._shutterModeCb.SelectionChangeCommitted += new System.EventHandler(this.ShutterModeCbOnSelectionChangeCommitted);
            // 
            // _shuttermodeLb
            // 
            this._shuttermodeLb.Location = new System.Drawing.Point(45, 46);
            this._shuttermodeLb.Name = "_shuttermodeLb";
            this._shuttermodeLb.Size = new System.Drawing.Size(63, 12);
            this._shuttermodeLb.TabIndex = 15;
            this._shuttermodeLb.Text = "Shutter";
            this._shuttermodeLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _triggerModeCb
            // 
            this._triggerModeCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._triggerModeCb.FormattingEnabled = true;
            this._triggerModeCb.Location = new System.Drawing.Point(115, 69);
            this._triggerModeCb.Name = "_triggerModeCb";
            this._triggerModeCb.Size = new System.Drawing.Size(120, 20);
            this._triggerModeCb.TabIndex = 16;
            this._triggerModeCb.SelectedIndexChanged += new System.EventHandler(this.TriggerModeCbOnSelectedIndexChanged);
            // 
            // _triggerLb
            // 
            this._triggerLb.Location = new System.Drawing.Point(47, 72);
            this._triggerLb.Name = "_triggerLb";
            this._triggerLb.Size = new System.Drawing.Size(63, 12);
            this._triggerLb.TabIndex = 17;
            this._triggerLb.Text = "Trigger";
            this._triggerLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serialsCb
            // 
            this.serialsCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serialsCb.FormattingEnabled = true;
            this.serialsCb.Location = new System.Drawing.Point(97, 24);
            this.serialsCb.Name = "serialsCb";
            this.serialsCb.Size = new System.Drawing.Size(67, 20);
            this.serialsCb.TabIndex = 18;
            // 
            // _portLb
            // 
            this._portLb.Location = new System.Drawing.Point(9, 27);
            this._portLb.Name = "_portLb";
            this._portLb.Size = new System.Drawing.Size(80, 12);
            this._portLb.TabIndex = 19;
            this._portLb.Text = "Serial Port";
            this._portLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _projectorPowerBtn
            // 
            this._projectorPowerBtn.Location = new System.Drawing.Point(97, 50);
            this._projectorPowerBtn.Name = "_projectorPowerBtn";
            this._projectorPowerBtn.Size = new System.Drawing.Size(120, 20);
            this._projectorPowerBtn.TabIndex = 20;
            this._projectorPowerBtn.Text = "Toggle Power";
            this._projectorPowerBtn.UseVisualStyleBackColor = true;
            this._projectorPowerBtn.Click += new System.EventHandler(this.ProjectorPowerToggleBtnOnClick);
            // 
            // _camWidthNud
            // 
            this._camWidthNud.Location = new System.Drawing.Point(115, 95);
            this._camWidthNud.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this._camWidthNud.Name = "_camWidthNud";
            this._camWidthNud.Size = new System.Drawing.Size(55, 19);
            this._camWidthNud.TabIndex = 21;
            this._camWidthNud.ValueChanged += new System.EventHandler(this.CamWidthNudOnValueChanged);
            // 
            // _camHeightNud
            // 
            this._camHeightNud.Location = new System.Drawing.Point(181, 95);
            this._camHeightNud.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this._camHeightNud.Name = "_camHeightNud";
            this._camHeightNud.Size = new System.Drawing.Size(55, 19);
            this._camHeightNud.TabIndex = 22;
            this._camHeightNud.ValueChanged += new System.EventHandler(this.CamHeightNud_ValueChanged);
            // 
            // _sizeLb
            // 
            this._sizeLb.Location = new System.Drawing.Point(45, 97);
            this._sizeLb.Name = "_sizeLb";
            this._sizeLb.Size = new System.Drawing.Size(63, 12);
            this._sizeLb.TabIndex = 24;
            this._sizeLb.Text = "Size";
            this._sizeLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _mastergainLb
            // 
            this._mastergainLb.Location = new System.Drawing.Point(28, 144);
            this._mastergainLb.Name = "_mastergainLb";
            this._mastergainLb.Size = new System.Drawing.Size(80, 12);
            this._mastergainLb.TabIndex = 25;
            this._mastergainLb.Text = "Master gain";
            this._mastergainLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _singleCapBtn
            // 
            this._singleCapBtn.Location = new System.Drawing.Point(114, 244);
            this._singleCapBtn.Name = "_singleCapBtn";
            this._singleCapBtn.Size = new System.Drawing.Size(120, 20);
            this._singleCapBtn.TabIndex = 26;
            this._singleCapBtn.Text = "Single Capture";
            this._singleCapBtn.UseVisualStyleBackColor = true;
            this._singleCapBtn.Click += new System.EventHandler(this.SingleCapBtnOnClick);
            // 
            // _cameraGb
            // 
            this._cameraGb.Controls.Add(this._autoWbBtn);
            this._cameraGb.Controls.Add(this._delayOffsetLb);
            this._cameraGb.Controls.Add(this._delayOffsetNud);
            this._cameraGb.Controls.Add(this._restartBtn);
            this._cameraGb.Controls.Add(this._colorModeLb);
            this._cameraGb.Controls.Add(this._cmodeCb);
            this._cameraGb.Controls.Add(this._scaleLb);
            this._cameraGb.Controls.Add(this._scalerNud);
            this._cameraGb.Controls.Add(this._singleCapBtn);
            this._cameraGb.Controls.Add(this._gainBoostCb);
            this._cameraGb.Controls.Add(this._mastergainLb);
            this._cameraGb.Controls.Add(this._sizeLb);
            this._cameraGb.Controls.Add(this._camHeightNud);
            this._cameraGb.Controls.Add(this._camWidthNud);
            this._cameraGb.Controls.Add(this._masterGainNud);
            this._cameraGb.Controls.Add(this._pixelclockLb);
            this._cameraGb.Controls.Add(this._pixelClockCb);
            this._cameraGb.Controls.Add(this._shutterModeCb);
            this._cameraGb.Controls.Add(this._triggerLb);
            this._cameraGb.Controls.Add(this._shuttermodeLb);
            this._cameraGb.Controls.Add(this._triggerModeCb);
            this._cameraGb.Location = new System.Drawing.Point(12, 254);
            this._cameraGb.Name = "_cameraGb";
            this._cameraGb.Size = new System.Drawing.Size(250, 331);
            this._cameraGb.TabIndex = 27;
            this._cameraGb.TabStop = false;
            this._cameraGb.Text = "Camera(&C)";
            // 
            // _autoWbBtn
            // 
            this._autoWbBtn.Location = new System.Drawing.Point(114, 295);
            this._autoWbBtn.Name = "_autoWbBtn";
            this._autoWbBtn.Size = new System.Drawing.Size(120, 23);
            this._autoWbBtn.TabIndex = 35;
            this._autoWbBtn.Text = "Auto WB";
            this._autoWbBtn.UseVisualStyleBackColor = true;
            this._autoWbBtn.Click += new System.EventHandler(this.AutoWbBtnOnClick);
            // 
            // _delayOffsetLb
            // 
            this._delayOffsetLb.Location = new System.Drawing.Point(15, 272);
            this._delayOffsetLb.Name = "_delayOffsetLb";
            this._delayOffsetLb.Size = new System.Drawing.Size(96, 17);
            this._delayOffsetLb.TabIndex = 32;
            this._delayOffsetLb.Text = "Delay offset [us]";
            this._delayOffsetLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _delayOffsetNud
            // 
            this._delayOffsetNud.Location = new System.Drawing.Point(114, 270);
            this._delayOffsetNud.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this._delayOffsetNud.Name = "_delayOffsetNud";
            this._delayOffsetNud.Size = new System.Drawing.Size(120, 19);
            this._delayOffsetNud.TabIndex = 31;
            this._delayOffsetNud.ValueChanged += new System.EventHandler(this.DelayOffsetNudOnValueChanged);
            // 
            // _restartBtn
            // 
            this._restartBtn.Location = new System.Drawing.Point(114, 218);
            this._restartBtn.Name = "_restartBtn";
            this._restartBtn.Size = new System.Drawing.Size(120, 20);
            this._restartBtn.TabIndex = 30;
            this._restartBtn.Text = "Restart Capture";
            this._restartBtn.UseVisualStyleBackColor = true;
            this._restartBtn.Click += new System.EventHandler(this.RestartBtnOnClick);
            // 
            // _colorModeLb
            // 
            this._colorModeLb.Location = new System.Drawing.Point(46, 195);
            this._colorModeLb.Name = "_colorModeLb";
            this._colorModeLb.Size = new System.Drawing.Size(63, 12);
            this._colorModeLb.TabIndex = 29;
            this._colorModeLb.Text = "Color Mode";
            this._colorModeLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _cmodeCb
            // 
            this._cmodeCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmodeCb.FormattingEnabled = true;
            this._cmodeCb.Location = new System.Drawing.Point(115, 192);
            this._cmodeCb.Name = "_cmodeCb";
            this._cmodeCb.Size = new System.Drawing.Size(120, 20);
            this._cmodeCb.TabIndex = 28;
            this._cmodeCb.SelectionChangeCommitted += new System.EventHandler(this.ColorModeCbOnSelectedIndexCommitted);
            // 
            // _scaleLb
            // 
            this._scaleLb.Location = new System.Drawing.Point(12, 169);
            this._scaleLb.Name = "_scaleLb";
            this._scaleLb.Size = new System.Drawing.Size(100, 12);
            this._scaleLb.TabIndex = 27;
            this._scaleLb.Text = "Scaler [%]";
            this._scaleLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _scalerNud
            // 
            this._scalerNud.Location = new System.Drawing.Point(115, 167);
            this._scalerNud.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._scalerNud.Name = "_scalerNud";
            this._scalerNud.Size = new System.Drawing.Size(120, 19);
            this._scalerNud.TabIndex = 26;
            this._scalerNud.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this._scalerNud.ValueChanged += new System.EventHandler(this.ScalerNudOnValueChanged);
            // 
            // _connectBtn
            // 
            this._connectBtn.Location = new System.Drawing.Point(147, 35);
            this._connectBtn.Name = "_connectBtn";
            this._connectBtn.Size = new System.Drawing.Size(115, 20);
            this._connectBtn.TabIndex = 32;
            this._connectBtn.Text = "Connect";
            this._connectBtn.UseVisualStyleBackColor = true;
            this._connectBtn.Click += new System.EventHandler(this.ConnectBtnOnClick);
            // 
            // _cameraCb
            // 
            this._cameraCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cameraCb.FormattingEnabled = true;
            this._cameraCb.Location = new System.Drawing.Point(17, 35);
            this._cameraCb.Name = "_cameraCb";
            this._cameraCb.Size = new System.Drawing.Size(120, 20);
            this._cameraCb.TabIndex = 31;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._projectorDetectBtn);
            this.groupBox2.Controls.Add(this._displayColorCb);
            this.groupBox2.Controls.Add(this._refreshScreenBtn);
            this.groupBox2.Controls.Add(this._screenCb);
            this.groupBox2.Controls.Add(this._closeProjectorScreenBtn);
            this.groupBox2.Controls.Add(this._showProjectorScreenBtn);
            this.groupBox2.Controls.Add(this.serialsCb);
            this.groupBox2.Controls.Add(this._portLb);
            this.groupBox2.Controls.Add(this._projectorPowerBtn);
            this.groupBox2.Location = new System.Drawing.Point(268, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(226, 213);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Projector(&P)";
            // 
            // _projectorDetectBtn
            // 
            this._projectorDetectBtn.Location = new System.Drawing.Point(170, 23);
            this._projectorDetectBtn.Name = "_projectorDetectBtn";
            this._projectorDetectBtn.Size = new System.Drawing.Size(47, 22);
            this._projectorDetectBtn.TabIndex = 27;
            this._projectorDetectBtn.Text = "Detect";
            this._projectorDetectBtn.UseVisualStyleBackColor = true;
            this._projectorDetectBtn.Click += new System.EventHandler(this._projectorDetectBtn_Click);
            // 
            // _displayColorCb
            // 
            this._displayColorCb.FormattingEnabled = true;
            this._displayColorCb.Items.AddRange(new object[] {
            "White",
            "Red",
            "Green",
            "Blue",
            "Black"});
            this._displayColorCb.Location = new System.Drawing.Point(97, 129);
            this._displayColorCb.Name = "_displayColorCb";
            this._displayColorCb.Size = new System.Drawing.Size(120, 20);
            this._displayColorCb.TabIndex = 26;
            // 
            // _refreshScreenBtn
            // 
            this._refreshScreenBtn.Location = new System.Drawing.Point(97, 103);
            this._refreshScreenBtn.Name = "_refreshScreenBtn";
            this._refreshScreenBtn.Size = new System.Drawing.Size(120, 20);
            this._refreshScreenBtn.TabIndex = 24;
            this._refreshScreenBtn.Text = "Refresh";
            this._refreshScreenBtn.UseVisualStyleBackColor = true;
            this._refreshScreenBtn.Click += new System.EventHandler(this.RefreshScreenBtnOnClick);
            // 
            // _screenCb
            // 
            this._screenCb.FormattingEnabled = true;
            this._screenCb.Location = new System.Drawing.Point(97, 77);
            this._screenCb.Name = "_screenCb";
            this._screenCb.Size = new System.Drawing.Size(120, 20);
            this._screenCb.TabIndex = 23;
            this._screenCb.SelectedIndexChanged += new System.EventHandler(this._screenCb_SelectedIndexChanged);
            // 
            // _closeProjectorScreenBtn
            // 
            this._closeProjectorScreenBtn.Location = new System.Drawing.Point(97, 181);
            this._closeProjectorScreenBtn.Name = "_closeProjectorScreenBtn";
            this._closeProjectorScreenBtn.Size = new System.Drawing.Size(120, 20);
            this._closeProjectorScreenBtn.TabIndex = 22;
            this._closeProjectorScreenBtn.Text = "Close Screen";
            this._closeProjectorScreenBtn.UseVisualStyleBackColor = true;
            this._closeProjectorScreenBtn.Click += new System.EventHandler(this.CloseProjectorScreenBtnOnClick);
            // 
            // _showProjectorScreenBtn
            // 
            this._showProjectorScreenBtn.Location = new System.Drawing.Point(97, 155);
            this._showProjectorScreenBtn.Name = "_showProjectorScreenBtn";
            this._showProjectorScreenBtn.Size = new System.Drawing.Size(120, 20);
            this._showProjectorScreenBtn.TabIndex = 21;
            this._showProjectorScreenBtn.Text = "Show";
            this._showProjectorScreenBtn.UseVisualStyleBackColor = true;
            this._showProjectorScreenBtn.Click += new System.EventHandler(this.ShowProjectorScreenBtnOnClick);
            // 
            // _syncDelayNud
            // 
            this._syncDelayNud.Increment = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this._syncDelayNud.Location = new System.Drawing.Point(114, 73);
            this._syncDelayNud.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this._syncDelayNud.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this._syncDelayNud.Name = "_syncDelayNud";
            this._syncDelayNud.Size = new System.Drawing.Size(120, 19);
            this._syncDelayNud.TabIndex = 33;
            this._syncDelayNud.ValueChanged += new System.EventHandler(this.SyncDelayExpNudOnValueChanged);
            // 
            // _episcanGb
            // 
            this._episcanGb.Controls.Add(this._veinRb);
            this._episcanGb.Controls.Add(this._resetBtn);
            this._episcanGb.Controls.Add(this._manualModeRb);
            this._episcanGb.Controls.Add(this._indirectModeRb);
            this._episcanGb.Controls.Add(this._directModeRb);
            this._episcanGb.Controls.Add(this._regularModeRb);
            this._episcanGb.Controls.Add(this._syncExpNud);
            this._episcanGb.Controls.Add(this._expLb);
            this._episcanGb.Controls.Add(this._delayLb);
            this._episcanGb.Controls.Add(this._syncDelayNud);
            this._episcanGb.Location = new System.Drawing.Point(12, 59);
            this._episcanGb.Name = "_episcanGb";
            this._episcanGb.Size = new System.Drawing.Size(250, 189);
            this._episcanGb.TabIndex = 29;
            this._episcanGb.TabStop = false;
            this._episcanGb.Text = "Episcan(&E)";
            // 
            // _veinRb
            // 
            this._veinRb.AutoSize = true;
            this._veinRb.Location = new System.Drawing.Point(100, 40);
            this._veinRb.Name = "_veinRb";
            this._veinRb.Size = new System.Drawing.Size(62, 16);
            this._veinRb.TabIndex = 38;
            this._veinRb.Text = "Vein(&V)";
            this._veinRb.UseVisualStyleBackColor = true;
            this._veinRb.CheckedChanged += new System.EventHandler(this.ImagingModeRbOnCheckedChanged);
            // 
            // _resetBtn
            // 
            this._resetBtn.Location = new System.Drawing.Point(159, 123);
            this._resetBtn.Name = "_resetBtn";
            this._resetBtn.Size = new System.Drawing.Size(75, 23);
            this._resetBtn.TabIndex = 35;
            this._resetBtn.Text = "reset";
            this._resetBtn.UseVisualStyleBackColor = true;
            this._resetBtn.Click += new System.EventHandler(this.ResetBtnOnClick);
            // 
            // _manualModeRb
            // 
            this._manualModeRb.AutoSize = true;
            this._manualModeRb.Location = new System.Drawing.Point(14, 40);
            this._manualModeRb.Name = "_manualModeRb";
            this._manualModeRb.Size = new System.Drawing.Size(76, 16);
            this._manualModeRb.TabIndex = 37;
            this._manualModeRb.Text = "Manual(&M)";
            this._manualModeRb.UseVisualStyleBackColor = true;
            this._manualModeRb.CheckedChanged += new System.EventHandler(this.ImagingModeRbOnCheckedChanged);
            // 
            // _indirectModeRb
            // 
            this._indirectModeRb.AutoSize = true;
            this._indirectModeRb.Location = new System.Drawing.Point(176, 18);
            this._indirectModeRb.Name = "_indirectModeRb";
            this._indirectModeRb.Size = new System.Drawing.Size(72, 16);
            this._indirectModeRb.TabIndex = 36;
            this._indirectModeRb.Text = "Indirect(&I)";
            this._indirectModeRb.UseVisualStyleBackColor = true;
            this._indirectModeRb.CheckedChanged += new System.EventHandler(this.ImagingModeRbOnCheckedChanged);
            // 
            // _directModeRb
            // 
            this._directModeRb.AutoSize = true;
            this._directModeRb.Location = new System.Drawing.Point(100, 18);
            this._directModeRb.Name = "_directModeRb";
            this._directModeRb.Size = new System.Drawing.Size(70, 16);
            this._directModeRb.TabIndex = 35;
            this._directModeRb.Text = "Direct(&D)";
            this._directModeRb.UseVisualStyleBackColor = true;
            this._directModeRb.CheckedChanged += new System.EventHandler(this.ImagingModeRbOnCheckedChanged);
            // 
            // _regularModeRb
            // 
            this._regularModeRb.AutoSize = true;
            this._regularModeRb.Checked = true;
            this._regularModeRb.Location = new System.Drawing.Point(14, 18);
            this._regularModeRb.Name = "_regularModeRb";
            this._regularModeRb.Size = new System.Drawing.Size(78, 16);
            this._regularModeRb.TabIndex = 34;
            this._regularModeRb.TabStop = true;
            this._regularModeRb.Text = "Regular(&R)";
            this._regularModeRb.UseVisualStyleBackColor = true;
            this._regularModeRb.CheckedChanged += new System.EventHandler(this.ImagingModeRbOnCheckedChanged);
            // 
            // _syncExpNud
            // 
            this._syncExpNud.DecimalPlaces = 3;
            this._syncExpNud.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this._syncExpNud.Location = new System.Drawing.Point(114, 98);
            this._syncExpNud.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._syncExpNud.Name = "_syncExpNud";
            this._syncExpNud.Size = new System.Drawing.Size(120, 19);
            this._syncExpNud.TabIndex = 31;
            this._syncExpNud.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this._syncExpNud.ValueChanged += new System.EventHandler(this.SyncDelayExpNudOnValueChanged);
            // 
            // _expLb
            // 
            this._expLb.Location = new System.Drawing.Point(18, 98);
            this._expLb.Name = "_expLb";
            this._expLb.Size = new System.Drawing.Size(90, 17);
            this._expLb.TabIndex = 32;
            this._expLb.Text = "Exposure [ms]";
            this._expLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _delayLb
            // 
            this._delayLb.Location = new System.Drawing.Point(39, 75);
            this._delayLb.Name = "_delayLb";
            this._delayLb.Size = new System.Drawing.Size(69, 12);
            this._delayLb.TabIndex = 31;
            this._delayLb.Text = "Delay [us]";
            this._delayLb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _exitBtn
            // 
            this._exitBtn.Location = new System.Drawing.Point(410, 586);
            this._exitBtn.Name = "_exitBtn";
            this._exitBtn.Size = new System.Drawing.Size(75, 23);
            this._exitBtn.TabIndex = 33;
            this._exitBtn.Text = "Exit";
            this._exitBtn.UseVisualStyleBackColor = true;
            this._exitBtn.Click += new System.EventHandler(this.ExitBtnOnClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 612);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(503, 22);
            this.statusStrip1.TabIndex = 34;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // _toolStripStatusLabel1
            // 
            this._toolStripStatusLabel1.Name = "_toolStripStatusLabel1";
            this._toolStripStatusLabel1.Size = new System.Drawing.Size(78, 17);
            this._toolStripStatusLabel1.Text = "Hello Episcan";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ScriptOptionContainer);
            this.groupBox1.Controls.Add(this.ScriptRunButton);
            this.groupBox1.Controls.Add(this.ScriptListBox);
            this.groupBox1.Location = new System.Drawing.Point(268, 255);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 330);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Script";
            // 
            // ScriptOptionContainer
            // 
            this.ScriptOptionContainer.Location = new System.Drawing.Point(11, 103);
            this.ScriptOptionContainer.Name = "ScriptOptionContainer";
            this.ScriptOptionContainer.Size = new System.Drawing.Size(206, 214);
            this.ScriptOptionContainer.TabIndex = 2;
            this.ScriptOptionContainer.TabStop = false;
            this.ScriptOptionContainer.Text = "Options";
            // 
            // ScriptRunButton
            // 
            this.ScriptRunButton.Enabled = false;
            this.ScriptRunButton.Location = new System.Drawing.Point(97, 74);
            this.ScriptRunButton.Name = "ScriptRunButton";
            this.ScriptRunButton.Size = new System.Drawing.Size(120, 23);
            this.ScriptRunButton.TabIndex = 1;
            this.ScriptRunButton.Text = "Run";
            this.ScriptRunButton.UseVisualStyleBackColor = true;
            this.ScriptRunButton.Click += new System.EventHandler(this.ScriptRunButton_Click);
            // 
            // ScriptListBox
            // 
            this.ScriptListBox.FormattingEnabled = true;
            this.ScriptListBox.ItemHeight = 12;
            this.ScriptListBox.Location = new System.Drawing.Point(11, 16);
            this.ScriptListBox.Name = "ScriptListBox";
            this.ScriptListBox.Size = new System.Drawing.Size(206, 52);
            this.ScriptListBox.TabIndex = 0;
            this.ScriptListBox.SelectedIndexChanged += new System.EventHandler(this.ScriptListBox_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 634);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._exitBtn);
            this.Controls.Add(this._episcanGb);
            this.Controls.Add(this._cameraCb);
            this.Controls.Add(this._connectBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._cameraGb);
            this.Controls.Add(this.menuStrip1);
            this.Location = new System.Drawing.Point(114, 270);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Episcan Demo Win";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._masterGainNud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._camWidthNud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._camHeightNud)).EndInit();
            this._cameraGb.ResumeLayout(false);
            this._cameraGb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._delayOffsetNud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._scalerNud)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._syncDelayNud)).EndInit();
            this._episcanGb.ResumeLayout(false);
            this._episcanGb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._syncExpNud)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitXToolStripMenuItem;
        private System.Windows.Forms.CheckBox _gainBoostCb;
        private System.Windows.Forms.NumericUpDown _masterGainNud;
        private System.Windows.Forms.Label _pixelclockLb;
        private System.Windows.Forms.ComboBox _pixelClockCb;
        private System.Windows.Forms.ComboBox _shutterModeCb;
        private System.Windows.Forms.Label _shuttermodeLb;
        private System.Windows.Forms.ComboBox _triggerModeCb;
        private System.Windows.Forms.Label _triggerLb;
        private System.Windows.Forms.ComboBox serialsCb;
        private System.Windows.Forms.Label _portLb;
        private System.Windows.Forms.Button _projectorPowerBtn;
        private System.Windows.Forms.NumericUpDown _camWidthNud;
        private System.Windows.Forms.NumericUpDown _camHeightNud;
        private System.Windows.Forms.Label _sizeLb;
        private System.Windows.Forms.Label _mastergainLb;
        private System.Windows.Forms.Button _singleCapBtn;
        private System.Windows.Forms.GroupBox _cameraGb;
        private System.Windows.Forms.Label _scaleLb;
        private System.Windows.Forms.NumericUpDown _scalerNud;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem camerasCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpHToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label _colorModeLb;
        private System.Windows.Forms.ComboBox _cmodeCb;
        private System.Windows.Forms.Button _restartBtn;
        private System.Windows.Forms.ComboBox _cameraCb;
        private System.Windows.Forms.Button _connectBtn;
        private System.Windows.Forms.NumericUpDown _syncDelayNud;
        private System.Windows.Forms.GroupBox _episcanGb;
        private System.Windows.Forms.Label _delayLb;
        private System.Windows.Forms.NumericUpDown _syncExpNud;
        private System.Windows.Forms.Label _expLb;
        private System.Windows.Forms.RadioButton _regularModeRb;
        private System.Windows.Forms.RadioButton _directModeRb;
        private System.Windows.Forms.RadioButton _manualModeRb;
        private System.Windows.Forms.RadioButton _indirectModeRb;
        private System.Windows.Forms.Label _delayOffsetLb;
        private System.Windows.Forms.NumericUpDown _delayOffsetNud;
        private System.Windows.Forms.Button _showProjectorScreenBtn;
        private System.Windows.Forms.Button _closeProjectorScreenBtn;
        private System.Windows.Forms.ComboBox _screenCb;
        private System.Windows.Forms.Button _exitBtn;
        private System.Windows.Forms.Button _refreshScreenBtn;
        private System.Windows.Forms.ComboBox _displayColorCb;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _toolStripStatusLabel1;
        private System.Windows.Forms.Button _resetBtn;
        private System.Windows.Forms.RadioButton _veinRb;
        private System.Windows.Forms.Button _autoWbBtn;
        private System.Windows.Forms.Button _projectorDetectBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox ScriptOptionContainer;
        private System.Windows.Forms.Button ScriptRunButton;
        private System.Windows.Forms.ListBox ScriptListBox;
    }
}

