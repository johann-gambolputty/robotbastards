namespace Poc1.Tools.TerrainTextures
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
			this.tabControl = new System.Windows.Forms.TabControl( );
			this.terrainTypesTabPage = new System.Windows.Forms.TabPage( );
			this.terrainFunctionsTabPage = new System.Windows.Forms.TabPage( );
			this.mainMenu = new System.Windows.Forms.MenuStrip( );
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator( );
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator( );
			this.tabControl.SuspendLayout( );
			this.mainMenu.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add( this.terrainTypesTabPage );
			this.tabControl.Controls.Add( this.terrainFunctionsTabPage );
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point( 0, 24 );
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size( 354, 339 );
			this.tabControl.TabIndex = 0;
			// 
			// terrainTypesTabPage
			// 
			this.terrainTypesTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.terrainTypesTabPage.Name = "terrainTypesTabPage";
			this.terrainTypesTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.terrainTypesTabPage.Size = new System.Drawing.Size( 346, 313 );
			this.terrainTypesTabPage.TabIndex = 0;
			this.terrainTypesTabPage.Text = "Terrain Types";
			this.terrainTypesTabPage.UseVisualStyleBackColor = true;
			// 
			// terrainFunctionsTabPage
			// 
			this.terrainFunctionsTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.terrainFunctionsTabPage.Name = "terrainFunctionsTabPage";
			this.terrainFunctionsTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.terrainFunctionsTabPage.Size = new System.Drawing.Size( 322, 245 );
			this.terrainFunctionsTabPage.TabIndex = 1;
			this.terrainFunctionsTabPage.Text = "Terrain Functions";
			this.terrainFunctionsTabPage.UseVisualStyleBackColor = true;
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem} );
			this.mainMenu.Location = new System.Drawing.Point( 0, 0 );
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size( 354, 24 );
			this.mainMenu.TabIndex = 1;
			this.mainMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem} );
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size( 35, 20 );
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.exitToolStripMenuItem.Text = "E&xit";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.openToolStripMenuItem.Text = "&Open";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 149, 6 );
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.saveToolStripMenuItem.Text = "&Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.saveAsToolStripMenuItem.Text = "Save &As...";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.newToolStripMenuItem.Text = "&New";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size( 149, 6 );
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 354, 363 );
			this.Controls.Add( this.tabControl );
			this.Controls.Add( this.mainMenu );
			this.MainMenuStrip = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "Terrain Textures";
			this.tabControl.ResumeLayout( false );
			this.mainMenu.ResumeLayout( false );
			this.mainMenu.PerformLayout( );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage terrainTypesTabPage;
		private System.Windows.Forms.TabPage terrainFunctionsTabPage;
		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	}
}

