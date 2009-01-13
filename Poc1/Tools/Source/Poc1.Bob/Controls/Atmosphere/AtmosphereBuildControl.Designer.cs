namespace Poc1.Bob.Controls.Atmosphere
{
	partial class AtmosphereBuildControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.buildSettingsGroupBox = new System.Windows.Forms.GroupBox( );
			this.scatteringResolutionComboBox = new System.Windows.Forms.ComboBox( );
			this.opticalDepthResolutionComboBox = new System.Windows.Forms.ComboBox( );
			this.label1 = new System.Windows.Forms.Label( );
			this.label2 = new System.Windows.Forms.Label( );
			this.label3 = new System.Windows.Forms.Label( );
			this.atmosphereParametersGroupBox = new System.Windows.Forms.GroupBox( );
			this.atmosphereParametersPropertyGrid = new System.Windows.Forms.PropertyGrid( );
			this.attenuationUpDown = new System.Windows.Forms.NumericUpDown( );
			this.buildButton = new System.Windows.Forms.Button( );
			this.buildProgressBar = new System.Windows.Forms.ProgressBar( );
			this.buildSettingsGroupBox.SuspendLayout( );
			this.atmosphereParametersGroupBox.SuspendLayout( );
			( ( System.ComponentModel.ISupportInitialize )( this.attenuationUpDown ) ).BeginInit( );
			this.SuspendLayout( );
			// 
			// buildSettingsGroupBox
			// 
			this.buildSettingsGroupBox.Controls.Add( this.attenuationUpDown );
			this.buildSettingsGroupBox.Controls.Add( this.label3 );
			this.buildSettingsGroupBox.Controls.Add( this.label2 );
			this.buildSettingsGroupBox.Controls.Add( this.label1 );
			this.buildSettingsGroupBox.Controls.Add( this.opticalDepthResolutionComboBox );
			this.buildSettingsGroupBox.Controls.Add( this.scatteringResolutionComboBox );
			this.buildSettingsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.buildSettingsGroupBox.Location = new System.Drawing.Point( 0, 0 );
			this.buildSettingsGroupBox.Name = "buildSettingsGroupBox";
			this.buildSettingsGroupBox.Size = new System.Drawing.Size( 194, 115 );
			this.buildSettingsGroupBox.TabIndex = 0;
			this.buildSettingsGroupBox.TabStop = false;
			this.buildSettingsGroupBox.Text = "Build Settings";
			// 
			// scatteringResolutionComboBox
			// 
			this.scatteringResolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.scatteringResolutionComboBox.FormattingEnabled = true;
			this.scatteringResolutionComboBox.Location = new System.Drawing.Point( 130, 19 );
			this.scatteringResolutionComboBox.Name = "scatteringResolutionComboBox";
			this.scatteringResolutionComboBox.Size = new System.Drawing.Size( 54, 21 );
			this.scatteringResolutionComboBox.TabIndex = 0;
			// 
			// opticalDepthResolutionComboBox
			// 
			this.opticalDepthResolutionComboBox.FormattingEnabled = true;
			this.opticalDepthResolutionComboBox.Location = new System.Drawing.Point( 130, 53 );
			this.opticalDepthResolutionComboBox.Name = "opticalDepthResolutionComboBox";
			this.opticalDepthResolutionComboBox.Size = new System.Drawing.Size( 54, 21 );
			this.opticalDepthResolutionComboBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 21, 22 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 103, 13 );
			this.label1.TabIndex = 2;
			this.label1.Text = "Scattering resolution";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 6, 56 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 118, 13 );
			this.label2.TabIndex = 3;
			this.label2.Text = "Optical depth resolution";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 21, 83 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 102, 13 );
			this.label3.TabIndex = 4;
			this.label3.Text = "Attenuation samples";
			// 
			// atmosphereParametersGroupBox
			// 
			this.atmosphereParametersGroupBox.Controls.Add( this.atmosphereParametersPropertyGrid );
			this.atmosphereParametersGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.atmosphereParametersGroupBox.Location = new System.Drawing.Point( 0, 115 );
			this.atmosphereParametersGroupBox.Name = "atmosphereParametersGroupBox";
			this.atmosphereParametersGroupBox.Size = new System.Drawing.Size( 194, 202 );
			this.atmosphereParametersGroupBox.TabIndex = 1;
			this.atmosphereParametersGroupBox.TabStop = false;
			this.atmosphereParametersGroupBox.Text = "Atmosphere Parameters";
			// 
			// atmosphereParametersPropertyGrid
			// 
			this.atmosphereParametersPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.atmosphereParametersPropertyGrid.HelpVisible = false;
			this.atmosphereParametersPropertyGrid.Location = new System.Drawing.Point( 3, 16 );
			this.atmosphereParametersPropertyGrid.Name = "atmosphereParametersPropertyGrid";
			this.atmosphereParametersPropertyGrid.Size = new System.Drawing.Size( 188, 183 );
			this.atmosphereParametersPropertyGrid.TabIndex = 0;
			this.atmosphereParametersPropertyGrid.ToolbarVisible = false;
			// 
			// attenuationUpDown
			// 
			this.attenuationUpDown.Location = new System.Drawing.Point( 130, 81 );
			this.attenuationUpDown.Minimum = new decimal( new int[] {
            2,
            0,
            0,
            0} );
			this.attenuationUpDown.Name = "attenuationUpDown";
			this.attenuationUpDown.Size = new System.Drawing.Size( 54, 20 );
			this.attenuationUpDown.TabIndex = 9;
			this.attenuationUpDown.Value = new decimal( new int[] {
            5,
            0,
            0,
            0} );
			// 
			// buildButton
			// 
			this.buildButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buildButton.Location = new System.Drawing.Point( 0, 340 );
			this.buildButton.Name = "buildButton";
			this.buildButton.Size = new System.Drawing.Size( 194, 23 );
			this.buildButton.TabIndex = 2;
			this.buildButton.Text = "Build";
			this.buildButton.UseVisualStyleBackColor = true;
			// 
			// buildProgressBar
			// 
			this.buildProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buildProgressBar.Location = new System.Drawing.Point( 0, 317 );
			this.buildProgressBar.Name = "buildProgressBar";
			this.buildProgressBar.Size = new System.Drawing.Size( 194, 23 );
			this.buildProgressBar.TabIndex = 3;
			// 
			// AtmosphereControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.atmosphereParametersGroupBox );
			this.Controls.Add( this.buildProgressBar );
			this.Controls.Add( this.buildButton );
			this.Controls.Add( this.buildSettingsGroupBox );
			this.Name = "AtmosphereControl";
			this.Size = new System.Drawing.Size( 194, 363 );
			this.buildSettingsGroupBox.ResumeLayout( false );
			this.buildSettingsGroupBox.PerformLayout( );
			this.atmosphereParametersGroupBox.ResumeLayout( false );
			( ( System.ComponentModel.ISupportInitialize )( this.attenuationUpDown ) ).EndInit( );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.GroupBox buildSettingsGroupBox;
		private System.Windows.Forms.ComboBox scatteringResolutionComboBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox opticalDepthResolutionComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox atmosphereParametersGroupBox;
		private System.Windows.Forms.PropertyGrid atmosphereParametersPropertyGrid;
		private System.Windows.Forms.NumericUpDown attenuationUpDown;
		private System.Windows.Forms.Button buildButton;
		private System.Windows.Forms.ProgressBar buildProgressBar;
	}
}