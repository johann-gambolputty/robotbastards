namespace Poc1.Bob.Controls.Biomes
{
	partial class BiomeDistributionDisplay
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
			this.SuspendLayout( );
			// 
			// BiomeDistributionDisplay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.DoubleBuffered = true;
			this.Name = "BiomeDistributionDisplay";
			this.Size = new System.Drawing.Size( 171, 156 );
			this.MouseLeave += new System.EventHandler( this.BiomeDistributionDisplay_MouseLeave );
			this.Paint += new System.Windows.Forms.PaintEventHandler( this.BiomeDistributionDisplay_Paint );
			this.MouseMove += new System.Windows.Forms.MouseEventHandler( this.BiomeDistributionDisplay_MouseMove );
			this.MouseDown += new System.Windows.Forms.MouseEventHandler( this.BiomeDistributionDisplay_MouseDown );
			this.Resize += new System.EventHandler( this.BiomeDistributionDisplay_Resize );
			this.MouseUp += new System.Windows.Forms.MouseEventHandler( this.BiomeDistributionDisplay_MouseUp );
			this.ResumeLayout( false );

		}

		#endregion
	}
}
