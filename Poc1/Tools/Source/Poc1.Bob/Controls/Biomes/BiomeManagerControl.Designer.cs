

namespace Poc1.Bob.Controls.Biomes
{
	partial class BiomeManagerControl
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
			this.biomeTabControl = new System.Windows.Forms.TabControl( );
			this.texturingTabPage = new System.Windows.Forms.TabPage( );
			this.vegetationTabPage = new System.Windows.Forms.TabPage( );
			this.biomeTextureControl1 = new Poc1.Bob.Controls.Biomes.BiomeTerrainTextureViewControl( );
			this.biomeTabControl.SuspendLayout( );
			this.texturingTabPage.SuspendLayout( );
			this.vegetationTabPage.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// biomeTabControl
			// 
			this.biomeTabControl.Controls.Add( this.texturingTabPage );
			this.biomeTabControl.Controls.Add( this.vegetationTabPage );
			this.biomeTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.biomeTabControl.Location = new System.Drawing.Point( 0, 0 );
			this.biomeTabControl.Name = "biomeTabControl";
			this.biomeTabControl.SelectedIndex = 0;
			this.biomeTabControl.Size = new System.Drawing.Size( 396, 528 );
			this.biomeTabControl.TabIndex = 2;
			this.biomeTabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
			this.biomeTabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.biomeTabControl.ItemSize = new System.Drawing.Size( 30, 100 );
			this.biomeTabControl.Multiline = true;
			this.biomeTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.biomeTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.biomeTabControl_DrawItem );
			// 
			// texturingTabPage
			// 
			this.texturingTabPage.Controls.Add( this.biomeTextureControl1 );
			this.texturingTabPage.Location = new System.Drawing.Point( 104, 4 );
			this.texturingTabPage.Name = "texturingTabPage";
			this.texturingTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.texturingTabPage.Size = new System.Drawing.Size( 288, 520 );
			this.texturingTabPage.TabIndex = 0;
			this.texturingTabPage.Text = "Texturing";
			this.texturingTabPage.UseVisualStyleBackColor = true;

			// 
			// vegetationTabPage
			// 
			this.vegetationTabPage.Location = new System.Drawing.Point( 104, 4 );
			this.vegetationTabPage.Name = "vegetationTabPage";
			this.vegetationTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.vegetationTabPage.Size = new System.Drawing.Size( 288, 520 );
			this.vegetationTabPage.TabIndex = 0;
			this.vegetationTabPage.Text = "Vegetation";
			this.vegetationTabPage.UseVisualStyleBackColor = true;
			// 
			// biomeTextureControl1
			// 
			this.biomeTextureControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.biomeTextureControl1.Location = new System.Drawing.Point( 3, 3 );
			this.biomeTextureControl1.Name = "biomeTextureControl1";
			this.biomeTextureControl1.Size = new System.Drawing.Size( 282, 514 );
			this.biomeTextureControl1.TabIndex = 0;
			// 
			// BiomeManagerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.biomeTabControl );
			this.Name = "BiomeManagerControl";
			this.Size = new System.Drawing.Size( 396, 528 );
			this.biomeTabControl.ResumeLayout( false );
			this.texturingTabPage.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TabControl biomeTabControl;
		private System.Windows.Forms.TabPage texturingTabPage;
		private System.Windows.Forms.TabPage vegetationTabPage;
		private BiomeTerrainTextureViewControl biomeTextureControl1;


	}
}
