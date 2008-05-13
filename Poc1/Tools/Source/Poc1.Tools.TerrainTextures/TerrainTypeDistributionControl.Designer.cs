namespace Poc1.Tools.TerrainTextures
{
	partial class TerrainTypeDistributionControl
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
			this.SuspendLayout();
			// 
			// TerrainTypeDistributionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.DoubleBuffered = true;
			this.Name = "TerrainTypeDistributionControl";
			this.Size = new System.Drawing.Size(265, 125);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.TerrainTypeDistributionControl_Paint);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TerrainTypeDistributionControl_MouseMove);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TerrainTypeDistributionControl_KeyUp);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TerrainTypeDistributionControl_MouseDown);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TerrainTypeDistributionControl_MouseUp);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TerrainTypeDistributionControl_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion
	}
}
