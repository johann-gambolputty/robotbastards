namespace Poc1.Bob.Controls.Planet.Terrain
{
	partial class HomogenousProcTerrainTemplateControl
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
			this.terrainPropertyGrid = new System.Windows.Forms.PropertyGrid( );
			this.SuspendLayout( );
			// 
			// terrainPropertyGrid
			// 
			this.terrainPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.terrainPropertyGrid.Location = new System.Drawing.Point( 0, 0 );
			this.terrainPropertyGrid.Name = "terrainPropertyGrid";
			this.terrainPropertyGrid.Size = new System.Drawing.Size( 150, 150 );
			this.terrainPropertyGrid.TabIndex = 0;
			// 
			// HomogenousProcTerrainControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.terrainPropertyGrid );
			this.Name = "HomogenousProcTerrainControl";
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.PropertyGrid terrainPropertyGrid;
	}
}
