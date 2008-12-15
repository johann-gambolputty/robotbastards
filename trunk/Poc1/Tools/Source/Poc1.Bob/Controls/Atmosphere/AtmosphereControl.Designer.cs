namespace Poc1.Bob.Controls.Atmosphere
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
			this.qualityGroupBox = new System.Windows.Forms.GroupBox( );
			this.atmosphereModelPropertyGrid = new System.Windows.Forms.PropertyGrid( );
			this.SuspendLayout( );
			// 
			// qualityGroupBox
			// 
			this.qualityGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.qualityGroupBox.Location = new System.Drawing.Point( 0, 0 );
			this.qualityGroupBox.Name = "qualityGroupBox";
			this.qualityGroupBox.Size = new System.Drawing.Size( 194, 100 );
			this.qualityGroupBox.TabIndex = 0;
			this.qualityGroupBox.TabStop = false;
			this.qualityGroupBox.Text = "Quality";
			// 
			// atmosphereModelPropertyGrid
			// 
			this.atmosphereModelPropertyGrid.Dock = System.Windows.Forms.DockStyle.Top;
			this.atmosphereModelPropertyGrid.Location = new System.Drawing.Point( 0, 100 );
			this.atmosphereModelPropertyGrid.Name = "atmosphereModelPropertyGrid";
			this.atmosphereModelPropertyGrid.Size = new System.Drawing.Size( 194, 161 );
			this.atmosphereModelPropertyGrid.TabIndex = 1;
			// 
			// AtmosphereControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.atmosphereModelPropertyGrid );
			this.Controls.Add( this.qualityGroupBox );
			this.Name = "AtmosphereControl";
			this.Size = new System.Drawing.Size( 194, 363 );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.GroupBox qualityGroupBox;
		private System.Windows.Forms.PropertyGrid atmosphereModelPropertyGrid;
	}
}
