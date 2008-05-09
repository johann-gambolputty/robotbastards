namespace Poc1.GameClient
{
	partial class GameClientForm
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
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.profileWindow1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.profileWindow2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.logWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gameDisplay = new Rb.Rendering.Windows.Display();
			this.debugInfoWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(493, 24);
			this.mainMenu.TabIndex = 1;
			this.mainMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profileWindow1ToolStripMenuItem,
            this.profileWindow2ToolStripMenuItem,
            this.logWindowToolStripMenuItem,
            this.debugInfoWindowToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// profileWindow1ToolStripMenuItem
			// 
			this.profileWindow1ToolStripMenuItem.Name = "profileWindow1ToolStripMenuItem";
			this.profileWindow1ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.profileWindow1ToolStripMenuItem.Text = "&Profile Window 1";
			this.profileWindow1ToolStripMenuItem.Click += new System.EventHandler(this.profileWindow1ToolStripMenuItem_Click);
			// 
			// profileWindow2ToolStripMenuItem
			// 
			this.profileWindow2ToolStripMenuItem.Name = "profileWindow2ToolStripMenuItem";
			this.profileWindow2ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.profileWindow2ToolStripMenuItem.Text = "Profile Window 2";
			this.profileWindow2ToolStripMenuItem.Click += new System.EventHandler(this.profileWindow2ToolStripMenuItem_Click);
			// 
			// logWindowToolStripMenuItem
			// 
			this.logWindowToolStripMenuItem.Name = "logWindowToolStripMenuItem";
			this.logWindowToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.logWindowToolStripMenuItem.Text = "&Log Window";
			this.logWindowToolStripMenuItem.Click += new System.EventHandler(this.logWindowToolStripMenuItem_Click);
			// 
			// gameDisplay
			// 
			this.gameDisplay.AllowArrowKeyInputs = true;
			this.gameDisplay.ColourBits = ((byte)(32));
			this.gameDisplay.ContinuousRendering = true;
			this.gameDisplay.DepthBits = ((byte)(24));
			this.gameDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gameDisplay.Location = new System.Drawing.Point(0, 24);
			this.gameDisplay.Name = "gameDisplay";
			this.gameDisplay.RenderInterval = 1;
			this.gameDisplay.Size = new System.Drawing.Size(493, 424);
			this.gameDisplay.StencilBits = ((byte)(0));
			this.gameDisplay.TabIndex = 0;
			// 
			// debugInfoWindowToolStripMenuItem
			// 
			this.debugInfoWindowToolStripMenuItem.Name = "debugInfoWindowToolStripMenuItem";
			this.debugInfoWindowToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.debugInfoWindowToolStripMenuItem.Text = "&Debug Info Window";
			this.debugInfoWindowToolStripMenuItem.Click += new System.EventHandler(this.debugInfoWindowToolStripMenuItem_Click);
			// 
			// GameClientForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(493, 448);
			this.Controls.Add(this.gameDisplay);
			this.Controls.Add(this.mainMenu);
			this.MainMenuStrip = this.mainMenu;
			this.Name = "GameClientForm";
			this.Text = "PUNI Client";
			this.Load += new System.EventHandler(this.GameClientForm_Load);
			this.Shown += new System.EventHandler(this.GameClientForm_Shown);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameClientForm_FormClosing);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Rb.Rendering.Windows.Display gameDisplay;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem profileWindow1ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem profileWindow2ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem logWindowToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem debugInfoWindowToolStripMenuItem;
	}
}

