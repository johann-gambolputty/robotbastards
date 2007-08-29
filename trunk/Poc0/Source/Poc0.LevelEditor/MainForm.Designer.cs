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
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.objectPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tileGrid2dDisplay = new Poc0.LevelEditor.TileGridDisplay();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip.SuspendLayout();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
			this.statusStrip.Location = new System.Drawing.Point(0, 398);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(524, 22);
			this.statusStrip.TabIndex = 0;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(524, 24);
			this.menuStrip.TabIndex = 1;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveAsToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(133, 6);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.saveAsToolStripMenuItem.Text = "S&ave As...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(133, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem,
            this.selectionToolStripMenuItem,
            this.toolBoxToolStripMenuItem,
            this.objectPropertiesToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// logToolStripMenuItem
			// 
			this.logToolStripMenuItem.Name = "logToolStripMenuItem";
			this.logToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.logToolStripMenuItem.Text = "&Log";
			this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
			// 
			// selectionToolStripMenuItem
			// 
			this.selectionToolStripMenuItem.Name = "selectionToolStripMenuItem";
			this.selectionToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.selectionToolStripMenuItem.Text = "&Selection";
			this.selectionToolStripMenuItem.Click += new System.EventHandler(this.selectionToolStripMenuItem_Click);
			// 
			// toolBoxToolStripMenuItem
			// 
			this.toolBoxToolStripMenuItem.Name = "toolBoxToolStripMenuItem";
			this.toolBoxToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.toolBoxToolStripMenuItem.Text = "&Tool Box";
			this.toolBoxToolStripMenuItem.Click += new System.EventHandler(this.toolBoxToolStripMenuItem_Click);
			// 
			// objectPropertiesToolStripMenuItem
			// 
			this.objectPropertiesToolStripMenuItem.Name = "objectPropertiesToolStripMenuItem";
			this.objectPropertiesToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.objectPropertiesToolStripMenuItem.Text = "&Object Properties";
			this.objectPropertiesToolStripMenuItem.Click += new System.EventHandler(this.objectPropertiesToolStripMenuItem_Click);
			// 
			// tileGrid2dDisplay
			// 
			this.tileGrid2dDisplay.ColourBits = ((byte)(32));
			this.tileGrid2dDisplay.ContinuousRendering = true;
			this.tileGrid2dDisplay.DepthBits = ((byte)(24));
			this.tileGrid2dDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tileGrid2dDisplay.Location = new System.Drawing.Point(0, 24);
			this.tileGrid2dDisplay.Name = "tileGrid2dDisplay";
			this.tileGrid2dDisplay.Size = new System.Drawing.Size(524, 374);
			this.tileGrid2dDisplay.StencilBits = ((byte)(0));
			this.tileGrid2dDisplay.TabIndex = 2;
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.undoToolStripMenuItem.Text = "&Undo";
			this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.redoToolStripMenuItem.Text = "&Redo";
			this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 420);
			this.Controls.Add(this.tileGrid2dDisplay);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private TileGridDisplay tileGrid2dDisplay;
		private System.Windows.Forms.ToolStripMenuItem selectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolBoxToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem objectPropertiesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
	}
}