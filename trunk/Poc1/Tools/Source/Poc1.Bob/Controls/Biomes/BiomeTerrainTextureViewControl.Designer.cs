namespace Poc1.Bob.Controls.Biomes
{
	partial class BiomeTerrainTextureViewControl
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
			Poc1.Bob.Core.Classes.Biomes.Models.BiomeListModel biomeListModel1 = new Poc1.Bob.Core.Classes.Biomes.Models.BiomeListModel( );
			this.slopeDistributionControl = new Poc1.Bob.Controls.Biomes.TerrainTypeDistributionControl( );
			this.altitudeDistributionControl = new Poc1.Bob.Controls.Biomes.TerrainTypeDistributionControl( );
			this.terrainTypeTextureListControl1 = new Poc1.Bob.Controls.Biomes.TerrainTypeTextureListControl( );
			this.SuspendLayout( );
			// 
			// slopeDistributionControl
			// 
			this.slopeDistributionControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.slopeDistributionControl.Location = new System.Drawing.Point( 0, 362 );
			this.slopeDistributionControl.Name = "slopeDistributionControl";
			this.slopeDistributionControl.Size = new System.Drawing.Size( 262, 167 );
			this.slopeDistributionControl.TabIndex = 2;
			// 
			// altitudeDistributionControl
			// 
			this.altitudeDistributionControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.altitudeDistributionControl.Location = new System.Drawing.Point( 0, 212 );
			this.altitudeDistributionControl.Name = "altitudeDistributionControl";
			this.altitudeDistributionControl.Size = new System.Drawing.Size( 262, 150 );
			this.altitudeDistributionControl.TabIndex = 1;
			// 
			// terrainTypeTextureListControl1
			// 
			this.terrainTypeTextureListControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.terrainTypeTextureListControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.terrainTypeTextureListControl1.Location = new System.Drawing.Point( 0, 97 );
			this.terrainTypeTextureListControl1.Name = "terrainTypeTextureListControl1";
			this.terrainTypeTextureListControl1.Size = new System.Drawing.Size( 262, 115 );
			this.terrainTypeTextureListControl1.TabIndex = 0;
			this.terrainTypeTextureListControl1.TerrainTypes = null;
			// 
			// BiomeTerrainTextureViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.slopeDistributionControl );
			this.Controls.Add( this.altitudeDistributionControl );
			this.Controls.Add( this.terrainTypeTextureListControl1 );
			this.Name = "BiomeTerrainTextureViewControl";
			this.Size = new System.Drawing.Size( 262, 529 );
			this.ResumeLayout( false );

		}

		#endregion

		private TerrainTypeTextureListControl terrainTypeTextureListControl1;
		private TerrainTypeDistributionControl altitudeDistributionControl;
		private TerrainTypeDistributionControl slopeDistributionControl;
	}
}
