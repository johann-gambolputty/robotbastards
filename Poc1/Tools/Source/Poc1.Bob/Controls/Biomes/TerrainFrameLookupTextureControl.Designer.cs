namespace Poc1.Bob.Controls.Biomes
{
	partial class TerrainFrameLookupTextureControl
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
			if ( m_Builder != null )
			{
				m_Builder.Dispose( );
				m_Builder = null;
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TerrainFrameLookupTextureControl ) );
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.panel1 = new System.Windows.Forms.Panel( );
			this.altitudeGraphControl = new Rb.Common.Controls.Forms.Graphs.GraphControl( );
			this.label1 = new System.Windows.Forms.Label( );
			this.splitter2 = new System.Windows.Forms.Splitter( );
			this.panel2 = new System.Windows.Forms.Panel( );
			this.slopeGraphControl = new Rb.Common.Controls.Forms.Graphs.GraphControl( );
			this.label2 = new System.Windows.Forms.Label( );
			this.panel1.SuspendLayout( );
			this.panel2.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoScroll = true;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tableLayoutPanel1.Location = new System.Drawing.Point( 0, 0 );
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( ) );
			this.tableLayoutPanel1.Size = new System.Drawing.Size( 179, 222 );
			this.tableLayoutPanel1.TabIndex = 1;
			this.tableLayoutPanel1.MouseClick += new System.Windows.Forms.MouseEventHandler( this.tableLayoutPanel1_MouseClick );
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 179, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 10, 222 );
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.altitudeGraphControl );
			this.panel1.Controls.Add( this.label1 );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point( 189, 0 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 250, 222 );
			this.panel1.TabIndex = 3;
			// 
			// altitudeGraphControl
			// 
			this.altitudeGraphControl.DataWindow = ( ( System.Drawing.RectangleF )( resources.GetObject( "altitudeGraphControl.DataWindow" ) ) );
			this.altitudeGraphControl.DefaultDataWindow = ( ( System.Drawing.RectangleF )( resources.GetObject( "altitudeGraphControl.DefaultDataWindow" ) ) );
			this.altitudeGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.altitudeGraphControl.Location = new System.Drawing.Point( 0, 13 );
			this.altitudeGraphControl.Name = "altitudeGraphControl";
			this.altitudeGraphControl.Size = new System.Drawing.Size( 250, 209 );
			this.altitudeGraphControl.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point( 0, 0 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 250, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Altitude distribution";
			// 
			// splitter2
			// 
			this.splitter2.Location = new System.Drawing.Point( 439, 0 );
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size( 10, 222 );
			this.splitter2.TabIndex = 4;
			this.splitter2.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add( this.slopeGraphControl );
			this.panel2.Controls.Add( this.label2 );
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point( 449, 0 );
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size( 247, 222 );
			this.panel2.TabIndex = 5;
			// 
			// slopeGraphControl
			// 
			this.slopeGraphControl.DataWindow = ( ( System.Drawing.RectangleF )( resources.GetObject( "slopeGraphControl.DataWindow" ) ) );
			this.slopeGraphControl.DefaultDataWindow = ( ( System.Drawing.RectangleF )( resources.GetObject( "slopeGraphControl.DefaultDataWindow" ) ) );
			this.slopeGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.slopeGraphControl.Location = new System.Drawing.Point( 0, 13 );
			this.slopeGraphControl.Name = "slopeGraphControl";
			this.slopeGraphControl.Size = new System.Drawing.Size( 247, 209 );
			this.slopeGraphControl.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point( 0, 0 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 247, 13 );
			this.label2.TabIndex = 0;
			this.label2.Text = "Slope distribution";
			// 
			// TerrainFrameLookupTextureControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.panel2 );
			this.Controls.Add( this.splitter2 );
			this.Controls.Add( this.panel1 );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.tableLayoutPanel1 );
			this.Name = "TerrainFrameLookupTextureControl";
			this.Size = new System.Drawing.Size( 696, 222 );
			this.Load += new System.EventHandler( this.TerrainSeLookupTextureControl_Load );
			this.panel1.ResumeLayout( false );
			this.panel2.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label2;
		private Rb.Common.Controls.Forms.Graphs.GraphControl altitudeGraphControl;
		private Rb.Common.Controls.Forms.Graphs.GraphControl slopeGraphControl;

	}
}
