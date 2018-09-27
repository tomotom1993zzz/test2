namespace DelaySweepDemo
{
    partial class CalibrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.planeDistanceInput = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.baselineInput = new System.Windows.Forms.NumericUpDown();
            this.focalLengthInput = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.calibrationStartButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.planeDistanceInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.baselineInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.focalLengthInput)).BeginInit();
            this.SuspendLayout();
            // 
            // planeDistanceInput
            // 
            this.planeDistanceInput.Location = new System.Drawing.Point(125, 12);
            this.planeDistanceInput.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.planeDistanceInput.Name = "planeDistanceInput";
            this.planeDistanceInput.Size = new System.Drawing.Size(120, 19);
            this.planeDistanceInput.TabIndex = 0;
            this.planeDistanceInput.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Plane Distance [mm]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baseline [mm]";
            // 
            // baselineInput
            // 
            this.baselineInput.Location = new System.Drawing.Point(125, 38);
            this.baselineInput.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.baselineInput.Name = "baselineInput";
            this.baselineInput.Size = new System.Drawing.Size(120, 19);
            this.baselineInput.TabIndex = 3;
            this.baselineInput.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // focalLengthInput
            // 
            this.focalLengthInput.DecimalPlaces = 3;
            this.focalLengthInput.Location = new System.Drawing.Point(125, 63);
            this.focalLengthInput.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.focalLengthInput.Name = "focalLengthInput";
            this.focalLengthInput.Size = new System.Drawing.Size(120, 19);
            this.focalLengthInput.TabIndex = 4;
            this.focalLengthInput.Value = new decimal(new int[] {
            1222700,
            0,
            0,
            196608});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Focal Length [px]";
            // 
            // calibrationStartButton
            // 
            this.calibrationStartButton.Location = new System.Drawing.Point(170, 88);
            this.calibrationStartButton.Name = "calibrationStartButton";
            this.calibrationStartButton.Size = new System.Drawing.Size(75, 23);
            this.calibrationStartButton.TabIndex = 6;
            this.calibrationStartButton.Text = "Start";
            this.calibrationStartButton.UseVisualStyleBackColor = true;
            this.calibrationStartButton.Click += new System.EventHandler(this.calibrationStartButton_Click);
            // 
            // CalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 122);
            this.Controls.Add(this.calibrationStartButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.focalLengthInput);
            this.Controls.Add(this.baselineInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.planeDistanceInput);
            this.Name = "CalibrationForm";
            this.Text = "CalibrationForm";
            ((System.ComponentModel.ISupportInitialize)(this.planeDistanceInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.baselineInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.focalLengthInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown planeDistanceInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown baselineInput;
        private System.Windows.Forms.NumericUpDown focalLengthInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button calibrationStartButton;
    }
}