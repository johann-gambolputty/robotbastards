namespace Poc0.LevelEditor
{
	partial class MainForm
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
			this.statusStrip = new System.Windows.Forms.StatusStrip( );
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar( );
			this.menuStrip = new System.Windows.Forms.MenuStrip( );
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.splitContainer1 = new System.Windows.Forms.SplitContainer( );
			this.tileTypeSetListView1 = new Poc0.LevelEditor.TileTypeSetListView( );
			this.display1 = new Poc0.LevelEditor.TileGridDisplay( );
			this.statusStrip.SuspendLayout( );
			this.menuStrip.SuspendLayout( );
			this.splitContainer1.Panel1.SuspendLayout( );
			this.splitContainer1.Panel2.SuspendLayout( );
			this.splitContainer1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1} );
			this.statusStrip.Location = new System.Drawing.Point( 0, 434 );
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size( 570, 22 );
			this.statusStrip.TabIndex = 0;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size( 100, 16 );
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem} );
			this.menuStrip.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size( 570, 24 );
			this.menuStrip.TabIndex = 1;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem} );
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size( 35, 20 );
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler( this.exitToolStripMenuItem_Click );
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem} );
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size( 41, 20 );
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// logToolStripMenuItem
			// 
			this.logToolStripMenuItem.Name = "logToolStripMenuItem";
			this.logToolStripMenuItem.Size = new System.Drawing.Size( 102, 22 );
			this.logToolStripMenuItem.Text = "&Log";
			this.logToolStripMenuItem.Click += new System.EventHandler( this.logToolStripMenuItem_Click );
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 24 );
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add( this.tileTypeSetListView1 );
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add( this.display1 );
			this.splitContainer1.Size = new System.Drawing.Size( 570, 410 );
			this.splitContainer1.SplitterDistance = 173;
			this.splitContainer1.TabIndex = 2;
			// 
			// tileTypeSetListView1
			// 
			this.tileTypeSetListView1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.tileTypeSetListView1.Location = new System.Drawing.Point( 3, 3 );
			this.tileTypeSetListView1.Name = "tileTypeSetListView1";
			this.tileTypeSetListView1.Size = new System.Drawing.Size( 167, 151 );
			this.tileTypeSetListView1.TabIndex = 0;
			this.tileTypeSetListView1.TileTypes = null;
			this.tileTypeSetListView1.UseCompatibleStateImageBehavior = false;
			this.tileTypeSetListView1.SelectedIndexChanged += new System.EventHandler( this.tileTypeSetListView1_SelectedIndexChanged );
			// 
			// display1
			// 
			this.display1.ColourBits = ( ( byte )( 32 ) );
			this.display1.ContinuousRendering = true;
			this.display1.DepthBits = ( ( byte )( 24 ) );
			this.display1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.display1.Location = new System.Drawing.Point( 0, 0 );
			this.display1.Name = "display1";
			this.display1.Size = new System.Drawing.Size( 393, 410 );
			this.display1.StencilBits = ( ( byte )( 0 ) );
			this.display1.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 570, 456 );
			this.Controls.Add( this.splitContainer1 );
			this.Controls.Add( this.statusStrip );
			this.Controls.Add( this.menuStrip );
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Load += new System.EventHandler( this.MainForm_Load );
			this.statusStrip.ResumeLayout( false );
			this.statusStrip.PerformLayout( );
			this.menuStrip.ResumeLayout( false );
			this.menuStrip.PerformLayout( );
			this.splitContainer1.Panel1.ResumeLayout( false );
			this.splitContainer1.Panel2.ResumeLayout( false );
			this.splitContainer1.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
		private TileGridDisplay display1;
		private TileTypeSetListView tileTypeSetListView1;
	}
}