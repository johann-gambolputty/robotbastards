namespace Poc1.Bob.Controls.Biomes
{
	partial class TerrainTypeListControl
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
			this.terrainTypeLayoutPanel = new System.Windows.Forms.TableLayoutPanel( );
			this.SuspendLayout( );
			// 
			// terrainTypeLayoutPanel
			// 
			this.terrainTypeLayoutPanel.AutoScroll = true;
			this.terrainTypeLayoutPanel.ColumnCount = 1;
			this.terrainTypeLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( ) );
			this.terrainTypeLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.terrainTypeLayoutPanel.Location = new System.Drawing.Point( 0, 0 );
			this.terrainTypeLayoutPanel.Name = "terrainTypeLayoutPanel";
			this.terrainTypeLayoutPanel.RowCount = 1;
			this.terrainTypeLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( ) );
			this.terrainTypeLayoutPanel.Size = new System.Drawing.Size( 199, 222 );
			this.terrainTypeLayoutPanel.TabIndex = 0;
			this.terrainTypeLayoutPanel.MouseClick += new System.Windows.Forms.MouseEventHandler( this.terrainTypeLayoutPanel_MouseClick );
			// 
			// TerrainTypeListControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.terrainTypeLayoutPanel );
			this.Name = "TerrainTypeListControl";
			this.Size = new System.Drawing.Size( 199, 222 );
			this.Load += new System.EventHandler( this.TerrainTypeListControl_Load );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel terrainTypeLayoutPanel;
	}
}
