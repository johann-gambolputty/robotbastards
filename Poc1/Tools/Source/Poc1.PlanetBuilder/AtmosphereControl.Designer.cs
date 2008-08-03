namespace Poc1.PlanetBuilder
{
	partial class AtmosphereControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox( );
			this.label4 = new System.Windows.Forms.Label( );
			this.attenuationUpDown = new System.Windows.Forms.NumericUpDown( );
			this.label3 = new System.Windows.Forms.Label( );
			this.label2 = new System.Windows.Forms.Label( );
			this.label1 = new System.Windows.Forms.Label( );
			this.sunAngleSamplesComboBox = new System.Windows.Forms.ComboBox( );
			this.viewAngleSamplesComboBox = new System.Windows.Forms.ComboBox( );
			this.heightSamplesComboBox = new System.Windows.Forms.ComboBox( );
			this.buildProgressBar = new System.Windows.Forms.ProgressBar( );
			this.buildButton = new System.Windows.Forms.Button( );
			this.groupBox2 = new System.Windows.Forms.GroupBox( );
			this.outscatterFudgeUpDown = new System.Windows.Forms.NumericUpDown( );
			this.mieFudgeUpDown = new System.Windows.Forms.NumericUpDown( );
			this.label12 = new System.Windows.Forms.Label( );
			this.rayleighFudgeUpDown = new System.Windows.Forms.NumericUpDown( );
			this.label10 = new System.Windows.Forms.Label( );
			this.label11 = new System.Windows.Forms.Label( );
			this.outscatterDistanceFudgeUpDown = new System.Windows.Forms.NumericUpDown( );
			this.inscatterDistanceFudgeUpDown = new System.Windows.Forms.NumericUpDown( );
			this.label9 = new System.Windows.Forms.Label( );
			this.label7 = new System.Windows.Forms.Label( );
			this.rH0UpDown = new System.Windows.Forms.NumericUpDown( );
			this.label8 = new System.Windows.Forms.Label( );
			this.mH0UpDown = new System.Windows.Forms.NumericUpDown( );
			this.kScaleUpDown = new System.Windows.Forms.NumericUpDown( );
			this.label6 = new System.Windows.Forms.Label( );
			this.label5 = new System.Windows.Forms.Label( );
			this.groupBox3 = new System.Windows.Forms.GroupBox( );
			this.phaseWeightUpDown = new System.Windows.Forms.NumericUpDown( );
			this.phaseCoeffUpDown = new System.Windows.Forms.NumericUpDown( );
			this.label13 = new System.Windows.Forms.Label( );
			this.label14 = new System.Windows.Forms.Label( );
			this.groupBox1.SuspendLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.attenuationUpDown ) ).BeginInit( );
			this.groupBox2.SuspendLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.outscatterFudgeUpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.mieFudgeUpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.rayleighFudgeUpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.outscatterDistanceFudgeUpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.inscatterDistanceFudgeUpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.rH0UpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.mH0UpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.kScaleUpDown ) ).BeginInit( );
			this.groupBox3.SuspendLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.phaseWeightUpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.phaseCoeffUpDown ) ).BeginInit( );
			this.SuspendLayout( );
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add( this.label4 );
			this.groupBox1.Controls.Add( this.attenuationUpDown );
			this.groupBox1.Controls.Add( this.label3 );
			this.groupBox1.Controls.Add( this.label2 );
			this.groupBox1.Controls.Add( this.label1 );
			this.groupBox1.Controls.Add( this.sunAngleSamplesComboBox );
			this.groupBox1.Controls.Add( this.viewAngleSamplesComboBox );
			this.groupBox1.Controls.Add( this.heightSamplesComboBox );
			this.groupBox1.Controls.Add( this.buildProgressBar );
			this.groupBox1.Controls.Add( this.buildButton );
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 215, 133 );
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Build Settings";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point( 15, 20 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 90, 26 );
			this.label4.TabIndex = 9;
			this.label4.Text = "Attenuation Samples";
			// 
			// attenuationUpDown
			// 
			this.attenuationUpDown.Location = new System.Drawing.Point( 18, 49 );
			this.attenuationUpDown.Minimum = new decimal( new int[] {
            2,
            0,
            0,
            0} );
			this.attenuationUpDown.Name = "attenuationUpDown";
			this.attenuationUpDown.Size = new System.Drawing.Size( 40, 20 );
			this.attenuationUpDown.TabIndex = 8;
			this.attenuationUpDown.Value = new decimal( new int[] {
            5,
            0,
            0,
            0} );
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 119, 76 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 30, 13 );
			this.label3.TabIndex = 7;
			this.label3.Text = "Light";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 119, 49 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 30, 13 );
			this.label2.TabIndex = 6;
			this.label2.Text = "View";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 111, 22 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 38, 13 );
			this.label1.TabIndex = 5;
			this.label1.Text = "Height";
			// 
			// sunAngleSamplesComboBox
			// 
			this.sunAngleSamplesComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.sunAngleSamplesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sunAngleSamplesComboBox.FormattingEnabled = true;
			this.sunAngleSamplesComboBox.Location = new System.Drawing.Point( 155, 73 );
			this.sunAngleSamplesComboBox.Name = "sunAngleSamplesComboBox";
			this.sunAngleSamplesComboBox.Size = new System.Drawing.Size( 54, 21 );
			this.sunAngleSamplesComboBox.TabIndex = 4;
			// 
			// viewAngleSamplesComboBox
			// 
			this.viewAngleSamplesComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.viewAngleSamplesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.viewAngleSamplesComboBox.FormattingEnabled = true;
			this.viewAngleSamplesComboBox.Location = new System.Drawing.Point( 155, 46 );
			this.viewAngleSamplesComboBox.Name = "viewAngleSamplesComboBox";
			this.viewAngleSamplesComboBox.Size = new System.Drawing.Size( 54, 21 );
			this.viewAngleSamplesComboBox.TabIndex = 3;
			// 
			// heightSamplesComboBox
			// 
			this.heightSamplesComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.heightSamplesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.heightSamplesComboBox.FormattingEnabled = true;
			this.heightSamplesComboBox.Location = new System.Drawing.Point( 155, 19 );
			this.heightSamplesComboBox.Name = "heightSamplesComboBox";
			this.heightSamplesComboBox.Size = new System.Drawing.Size( 54, 21 );
			this.heightSamplesComboBox.TabIndex = 2;
			// 
			// buildProgressBar
			// 
			this.buildProgressBar.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.buildProgressBar.Location = new System.Drawing.Point( 99, 103 );
			this.buildProgressBar.Name = "buildProgressBar";
			this.buildProgressBar.Size = new System.Drawing.Size( 110, 23 );
			this.buildProgressBar.Step = 1;
			this.buildProgressBar.TabIndex = 1;
			// 
			// buildButton
			// 
			this.buildButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.buildButton.Location = new System.Drawing.Point( 18, 103 );
			this.buildButton.Name = "buildButton";
			this.buildButton.Size = new System.Drawing.Size( 75, 23 );
			this.buildButton.TabIndex = 0;
			this.buildButton.Text = "Build";
			this.buildButton.UseVisualStyleBackColor = true;
			this.buildButton.Click += new System.EventHandler( this.buildButton_Click );
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add( this.outscatterFudgeUpDown );
			this.groupBox2.Controls.Add( this.mieFudgeUpDown );
			this.groupBox2.Controls.Add( this.label12 );
			this.groupBox2.Controls.Add( this.rayleighFudgeUpDown );
			this.groupBox2.Controls.Add( this.label10 );
			this.groupBox2.Controls.Add( this.label11 );
			this.groupBox2.Controls.Add( this.outscatterDistanceFudgeUpDown );
			this.groupBox2.Controls.Add( this.inscatterDistanceFudgeUpDown );
			this.groupBox2.Controls.Add( this.label9 );
			this.groupBox2.Controls.Add( this.label7 );
			this.groupBox2.Controls.Add( this.rH0UpDown );
			this.groupBox2.Controls.Add( this.label8 );
			this.groupBox2.Controls.Add( this.mH0UpDown );
			this.groupBox2.Controls.Add( this.kScaleUpDown );
			this.groupBox2.Controls.Add( this.label6 );
			this.groupBox2.Controls.Add( this.label5 );
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point( 0, 133 );
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size( 215, 317 );
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Atmosphere Parameters";
			// 
			// outscatterFudgeUpDown
			// 
			this.outscatterFudgeUpDown.DecimalPlaces = 3;
			this.outscatterFudgeUpDown.Increment = new decimal( new int[] {
            20,
            0,
            0,
            0} );
			this.outscatterFudgeUpDown.Location = new System.Drawing.Point( 99, 213 );
			this.outscatterFudgeUpDown.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
			this.outscatterFudgeUpDown.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
			this.outscatterFudgeUpDown.Name = "outscatterFudgeUpDown";
			this.outscatterFudgeUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.outscatterFudgeUpDown.TabIndex = 16;
			// 
			// mieFudgeUpDown
			// 
			this.mieFudgeUpDown.DecimalPlaces = 3;
			this.mieFudgeUpDown.Increment = new decimal( new int[] {
            20,
            0,
            0,
            0} );
			this.mieFudgeUpDown.Location = new System.Drawing.Point( 99, 281 );
			this.mieFudgeUpDown.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
			this.mieFudgeUpDown.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
			this.mieFudgeUpDown.Name = "mieFudgeUpDown";
			this.mieFudgeUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.mieFudgeUpDown.TabIndex = 14;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point( 6, 211 );
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size( 87, 30 );
			this.label12.TabIndex = 15;
			this.label12.Text = "Outscatter Fudge Factor";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// rayleighFudgeUpDown
			// 
			this.rayleighFudgeUpDown.DecimalPlaces = 3;
			this.rayleighFudgeUpDown.Increment = new decimal( new int[] {
            20,
            0,
            0,
            0} );
			this.rayleighFudgeUpDown.Location = new System.Drawing.Point( 99, 248 );
			this.rayleighFudgeUpDown.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
			this.rayleighFudgeUpDown.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
			this.rayleighFudgeUpDown.Name = "rayleighFudgeUpDown";
			this.rayleighFudgeUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.rayleighFudgeUpDown.TabIndex = 13;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point( 6, 244 );
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size( 87, 30 );
			this.label10.TabIndex = 12;
			this.label10.Text = "Rayleigh Fudge Factor";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point( 6, 279 );
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size( 87, 30 );
			this.label11.TabIndex = 11;
			this.label11.Text = "Mie Fudge Factor";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// outscatterDistanceFudgeUpDown
			// 
			this.outscatterDistanceFudgeUpDown.DecimalPlaces = 3;
			this.outscatterDistanceFudgeUpDown.Increment = new decimal( new int[] {
            20,
            0,
            0,
            0} );
			this.outscatterDistanceFudgeUpDown.Location = new System.Drawing.Point( 99, 186 );
			this.outscatterDistanceFudgeUpDown.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
			this.outscatterDistanceFudgeUpDown.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
			this.outscatterDistanceFudgeUpDown.Name = "outscatterDistanceFudgeUpDown";
			this.outscatterDistanceFudgeUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.outscatterDistanceFudgeUpDown.TabIndex = 10;
			// 
			// inscatterDistanceFudgeUpDown
			// 
			this.inscatterDistanceFudgeUpDown.DecimalPlaces = 3;
			this.inscatterDistanceFudgeUpDown.Increment = new decimal( new int[] {
            20,
            0,
            0,
            0} );
			this.inscatterDistanceFudgeUpDown.Location = new System.Drawing.Point( 99, 153 );
			this.inscatterDistanceFudgeUpDown.Maximum = new decimal( new int[] {
            10000,
            0,
            0,
            0} );
			this.inscatterDistanceFudgeUpDown.Minimum = new decimal( new int[] {
            10000,
            0,
            0,
            -2147483648} );
			this.inscatterDistanceFudgeUpDown.Name = "inscatterDistanceFudgeUpDown";
			this.inscatterDistanceFudgeUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.inscatterDistanceFudgeUpDown.TabIndex = 9;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point( 6, 181 );
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size( 87, 30 );
			this.label9.TabIndex = 8;
			this.label9.Text = "Outscatter Distance Fudge Factor";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point( 6, 147 );
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size( 87, 30 );
			this.label7.TabIndex = 7;
			this.label7.Text = "Inscatter Distance Fudge Factor";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// rH0UpDown
			// 
			this.rH0UpDown.DecimalPlaces = 3;
			this.rH0UpDown.Increment = new decimal( new int[] {
            1,
            0,
            0,
            131072} );
			this.rH0UpDown.Location = new System.Drawing.Point( 99, 115 );
			this.rH0UpDown.Maximum = new decimal( new int[] {
            10,
            0,
            0,
            0} );
			this.rH0UpDown.Minimum = new decimal( new int[] {
            10,
            0,
            0,
            -2147483648} );
			this.rH0UpDown.Name = "rH0UpDown";
			this.rH0UpDown.Size = new System.Drawing.Size( 110, 20 );
			this.rH0UpDown.TabIndex = 6;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point( 6, 108 );
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size( 87, 30 );
			this.label8.TabIndex = 5;
			this.label8.Text = "Rayleigh Density Height";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// mH0UpDown
			// 
			this.mH0UpDown.DecimalPlaces = 3;
			this.mH0UpDown.Increment = new decimal( new int[] {
            1,
            0,
            0,
            131072} );
			this.mH0UpDown.Location = new System.Drawing.Point( 99, 78 );
			this.mH0UpDown.Maximum = new decimal( new int[] {
            10,
            0,
            0,
            0} );
			this.mH0UpDown.Minimum = new decimal( new int[] {
            10,
            0,
            0,
            -2147483648} );
			this.mH0UpDown.Name = "mH0UpDown";
			this.mH0UpDown.Size = new System.Drawing.Size( 110, 20 );
			this.mH0UpDown.TabIndex = 4;
			// 
			// kScaleUpDown
			// 
			this.kScaleUpDown.DecimalPlaces = 2;
			this.kScaleUpDown.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
			this.kScaleUpDown.Location = new System.Drawing.Point( 99, 40 );
			this.kScaleUpDown.Maximum = new decimal( new int[] {
            5,
            0,
            0,
            0} );
			this.kScaleUpDown.Minimum = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
			this.kScaleUpDown.Name = "kScaleUpDown";
			this.kScaleUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.kScaleUpDown.TabIndex = 3;
			this.kScaleUpDown.Value = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point( 6, 71 );
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size( 87, 30 );
			this.label6.TabIndex = 1;
			this.label6.Text = "Mie Density Height";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point( 49, 42 );
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size( 44, 13 );
			this.label5.TabIndex = 0;
			this.label5.Text = "K Scale";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add( this.phaseWeightUpDown );
			this.groupBox3.Controls.Add( this.phaseCoeffUpDown );
			this.groupBox3.Controls.Add( this.label13 );
			this.groupBox3.Controls.Add( this.label14 );
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox3.Location = new System.Drawing.Point( 0, 450 );
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size( 215, 100 );
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Rendering Parameters";
			// 
			// phaseWeightUpDown
			// 
			this.phaseWeightUpDown.DecimalPlaces = 3;
			this.phaseWeightUpDown.Increment = new decimal( new int[] {
            1,
            0,
            0,
            131072} );
			this.phaseWeightUpDown.Location = new System.Drawing.Point( 99, 65 );
			this.phaseWeightUpDown.Maximum = new decimal( new int[] {
            10,
            0,
            0,
            0} );
			this.phaseWeightUpDown.Minimum = new decimal( new int[] {
            10,
            0,
            0,
            -2147483648} );
			this.phaseWeightUpDown.Name = "phaseWeightUpDown";
			this.phaseWeightUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.phaseWeightUpDown.TabIndex = 8;
			this.phaseWeightUpDown.ValueChanged += new System.EventHandler( this.phaseWeightUpDown_ValueChanged );
			// 
			// phaseCoeffUpDown
			// 
			this.phaseCoeffUpDown.DecimalPlaces = 2;
			this.phaseCoeffUpDown.Increment = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
			this.phaseCoeffUpDown.Location = new System.Drawing.Point( 99, 27 );
			this.phaseCoeffUpDown.Maximum = new decimal( new int[] {
            5,
            0,
            0,
            0} );
			this.phaseCoeffUpDown.Minimum = new decimal( new int[] {
            5,
            0,
            0,
            -2147483648} );
			this.phaseCoeffUpDown.Name = "phaseCoeffUpDown";
			this.phaseCoeffUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.phaseCoeffUpDown.TabIndex = 7;
			this.phaseCoeffUpDown.Value = new decimal( new int[] {
            1,
            0,
            0,
            65536} );
			this.phaseCoeffUpDown.ValueChanged += new System.EventHandler( this.phaseCoeffUpDown_ValueChanged );
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point( 19, 67 );
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size( 74, 13 );
			this.label13.TabIndex = 6;
			this.label13.Text = "Phase Weight";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point( 6, 29 );
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size( 90, 13 );
			this.label14.TabIndex = 5;
			this.label14.Text = "Phase Coefficient";
			// 
			// AtmosphereControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.groupBox3 );
			this.Controls.Add( this.groupBox2 );
			this.Controls.Add( this.groupBox1 );
			this.Name = "AtmosphereControl";
			this.Size = new System.Drawing.Size( 215, 550 );
			this.Load += new System.EventHandler( this.AtmosphereControl_Load );
			this.groupBox1.ResumeLayout( false );
			this.groupBox1.PerformLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.attenuationUpDown ) ).EndInit( );
			this.groupBox2.ResumeLayout( false );
			this.groupBox2.PerformLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.outscatterFudgeUpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.mieFudgeUpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.rayleighFudgeUpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.outscatterDistanceFudgeUpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.inscatterDistanceFudgeUpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.rH0UpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.mH0UpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.kScaleUpDown ) ).EndInit( );
			this.groupBox3.ResumeLayout( false );
			this.groupBox3.PerformLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.phaseWeightUpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.phaseCoeffUpDown ) ).EndInit( );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ProgressBar buildProgressBar;
		private System.Windows.Forms.Button buildButton;
		private System.Windows.Forms.ComboBox heightSamplesComboBox;
		private System.Windows.Forms.ComboBox sunAngleSamplesComboBox;
		private System.Windows.Forms.ComboBox viewAngleSamplesComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown attenuationUpDown;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown mH0UpDown;
		private System.Windows.Forms.NumericUpDown kScaleUpDown;
		private System.Windows.Forms.NumericUpDown rH0UpDown;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown outscatterDistanceFudgeUpDown;
		private System.Windows.Forms.NumericUpDown inscatterDistanceFudgeUpDown;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown mieFudgeUpDown;
		private System.Windows.Forms.NumericUpDown rayleighFudgeUpDown;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.NumericUpDown outscatterFudgeUpDown;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.NumericUpDown phaseWeightUpDown;
		private System.Windows.Forms.NumericUpDown phaseCoeffUpDown;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
	}
}
