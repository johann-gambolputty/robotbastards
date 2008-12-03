namespace Poc1.Bob.Controls.Planets
{
	partial class PlanetViewControl
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
			this.terrainDisplay = new Rb.Rendering.Windows.Display( );
			this.SuspendLayout( );
			// 
			// terrainDisplay
			// 
			this.terrainDisplay.AllowArrowKeyInputs = false;
			this.terrainDisplay.ColourBits = ( ( byte )( 32 ) );
			this.terrainDisplay.ContinuousRendering = false;
			this.terrainDisplay.DepthBits = ( ( byte )( 24 ) );
			this.terrainDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.terrainDisplay.Location = new System.Drawing.Point( 0, 0 );
			this.terrainDisplay.Name = "terrainDisplay";
			this.terrainDisplay.RenderInterval = 1;
			this.terrainDisplay.Size = new System.Drawing.Size( 206, 199 );
			this.terrainDisplay.StencilBits = ( ( byte )( 0 ) );
			this.terrainDisplay.TabIndex = 0;
			// 
			// TerrainSamplerViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.terrainDisplay );
			this.Name = "TerrainSamplerViewControl";
			this.Size = new System.Drawing.Size( 206, 199 );
			this.Load += new System.EventHandler( this.TerrainSamplerViewControl_Load );
			this.ResumeLayout( false );

		}

		#endregion

		private Rb.Rendering.Windows.Display terrainDisplay;
	}
}
