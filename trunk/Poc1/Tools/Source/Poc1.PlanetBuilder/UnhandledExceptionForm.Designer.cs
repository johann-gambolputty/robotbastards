namespace Poc1.PlanetBuilder
{
	partial class UnhandledExceptionForm
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip( );
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.assetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.menuStrip1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.assetsToolStripMenuItem} );
			this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size( 292, 24 );
			this.menuStrip1.TabIndex = 0;
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
			this.exitToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler( this.exitToolStripMenuItem_Click );
			// 
			// assetsToolStripMenuItem
			// 
			this.assetsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.buildToolStripMenuItem} );
			this.assetsToolStripMenuItem.Name = "assetsToolStripMenuItem";
			this.assetsToolStripMenuItem.Size = new System.Drawing.Size( 51, 20 );
			this.assetsToolStripMenuItem.Text = "&Assets";
			// 
			// buildToolStripMenuItem
			// 
			this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
			this.buildToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
			this.buildToolStripMenuItem.Text = "&Build";
			this.buildToolStripMenuItem.Click += new System.EventHandler( this.buildToolStripMenuItem_Click );
			// 
			// UnhandledExceptionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 292, 266 );
			this.Controls.Add( this.menuStrip1 );
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "UnhandledExceptionForm";
			this.Text = "UnhandledExceptionForm";
			this.menuStrip1.ResumeLayout( false );
			this.menuStrip1.PerformLayout( );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem assetsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
	}
}