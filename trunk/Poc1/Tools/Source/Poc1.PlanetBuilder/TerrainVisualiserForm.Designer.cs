namespace Poc1.PlanetBuilder
{
	partial class TerrainVisualiserForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.terrainVisualiserControl1 = new Poc1.PlanetBuilder.TerrainVisualiserControl( );
			this.SuspendLayout( );
			// 
			// terrainVisualiserControl1
			// 
			this.terrainVisualiserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.terrainVisualiserControl1.Location = new System.Drawing.Point( 0, 0 );
			this.terrainVisualiserControl1.Name = "terrainVisualiserControl1";
			this.terrainVisualiserControl1.Size = new System.Drawing.Size( 292, 266 );
			this.terrainVisualiserControl1.TabIndex = 0;
			// 
			// TerrainVisualiserForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 292, 266 );
			this.Controls.Add( this.terrainVisualiserControl1 );
			this.Name = "TerrainVisualiserForm";
			this.Text = "TerrainVisualiserForm";
			this.ResumeLayout( false );

		}

		#endregion

		private TerrainVisualiserControl terrainVisualiserControl1;
	}
}