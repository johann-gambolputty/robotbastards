namespace Poc1.Bob.Controls
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
			this.mainMenu = new System.Windows.Forms.MenuStrip( );
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.mainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel( );
			this.mainMenu.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem} );
			this.mainMenu.Location = new System.Drawing.Point( 0, 0 );
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size( 464, 24 );
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "mainMenuStrip";
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
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size( 41, 20 );
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// mainDockPanel
			// 
			this.mainDockPanel.ActiveAutoHideContent = null;
			this.mainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainDockPanel.Location = new System.Drawing.Point( 0, 24 );
			this.mainDockPanel.Name = "mainDockPanel";
			this.mainDockPanel.Size = new System.Drawing.Size( 464, 380 );
			this.mainDockPanel.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 464, 404 );
			this.Controls.Add( this.mainDockPanel );
			this.Controls.Add( this.mainMenu );
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "Bob";
			this.Load += new System.EventHandler( this.MainForm_Load );
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.MainForm_FormClosing );
			this.mainMenu.ResumeLayout( false );
			this.mainMenu.PerformLayout( );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private WeifenLuo.WinFormsUI.Docking.DockPanel mainDockPanel;
	}
}

