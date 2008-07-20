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
			this.label7 = new System.Windows.Forms.Label( );
			this.label6 = new System.Windows.Forms.Label( );
			this.label5 = new System.Windows.Forms.Label( );
			this.kScaleUpDown = new System.Windows.Forms.NumericUpDown( );
			this.h0NumericUpDown = new System.Windows.Forms.NumericUpDown( );
			this.groupBox1.SuspendLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.attenuationUpDown ) ).BeginInit( );
			this.groupBox2.SuspendLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.kScaleUpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.h0NumericUpDown ) ).BeginInit( );
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
			this.groupBox2.Controls.Add( this.h0NumericUpDown );
			this.groupBox2.Controls.Add( this.kScaleUpDown );
			this.groupBox2.Controls.Add( this.label7 );
			this.groupBox2.Controls.Add( this.label6 );
			this.groupBox2.Controls.Add( this.label5 );
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point( 0, 133 );
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size( 215, 213 );
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Atmosphere Parameters";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point( 37, 94 );
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size( 59, 13 );
			this.label7.TabIndex = 2;
			this.label7.Text = "Sun Colour";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point( 75, 68 );
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size( 21, 13 );
			this.label6.TabIndex = 1;
			this.label6.Text = "H0";
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
			// h0NumericUpDown
			// 
			this.h0NumericUpDown.DecimalPlaces = 3;
			this.h0NumericUpDown.Increment = new decimal( new int[] {
            1,
            0,
            0,
            131072} );
			this.h0NumericUpDown.Location = new System.Drawing.Point( 99, 66 );
			this.h0NumericUpDown.Maximum = new decimal( new int[] {
            10,
            0,
            0,
            0} );
			this.h0NumericUpDown.Name = "h0NumericUpDown";
			this.h0NumericUpDown.Size = new System.Drawing.Size( 110, 20 );
			this.h0NumericUpDown.TabIndex = 4;
			// 
			// AtmosphereControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.groupBox2 );
			this.Controls.Add( this.groupBox1 );
			this.Name = "AtmosphereControl";
			this.Size = new System.Drawing.Size( 215, 346 );
			this.groupBox1.ResumeLayout( false );
			this.groupBox1.PerformLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.attenuationUpDown ) ).EndInit( );
			this.groupBox2.ResumeLayout( false );
			this.groupBox2.PerformLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.kScaleUpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.h0NumericUpDown ) ).EndInit( );
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
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown h0NumericUpDown;
		private System.Windows.Forms.NumericUpDown kScaleUpDown;
	}
}
