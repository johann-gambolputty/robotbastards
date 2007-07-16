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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip( );
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar( );
			this.menuStrip1 = new System.Windows.Forms.MenuStrip( );
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.splitContainer1 = new System.Windows.Forms.SplitContainer( );
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.statusStrip1.SuspendLayout( );
			this.menuStrip1.SuspendLayout( );
			this.splitContainer1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1} );
			this.statusStrip1.Location = new System.Drawing.Point( 0, 425 );
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size( 512, 22 );
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size( 100, 16 );
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem} );
			this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size( 512, 24 );
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
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
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point( 0, 24 );
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Size = new System.Drawing.Size( 512, 401 );
			this.splitContainer1.SplitterDistance = 190;
			this.splitContainer1.TabIndex = 2;
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
			this.logToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.logToolStripMenuItem.Text = "&Log";
			this.logToolStripMenuItem.Click += new System.EventHandler( this.logToolStripMenuItem_Click );
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 512, 447 );
			this.Controls.Add( this.splitContainer1 );
			this.Controls.Add( this.statusStrip1 );
			this.Controls.Add( this.menuStrip1 );
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.statusStrip1.ResumeLayout( false );
			this.statusStrip1.PerformLayout( );
			this.menuStrip1.ResumeLayout( false );
			this.menuStrip1.PerformLayout( );
			this.splitContainer1.ResumeLayout( false );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
	}
}