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
			this.mainDisplay = new Rb.Rendering.Windows.Display( );
			this.mainMenu.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem} );
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
			// mainDisplay
			// 
			this.mainDisplay.AllowArrowKeyInputs = false;
			this.mainDisplay.ColourBits = ( ( byte )( 32 ) );
			this.mainDisplay.ContinuousRendering = true;
			this.mainDisplay.DepthBits = ( ( byte )( 24 ) );
			this.mainDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainDisplay.Location = new System.Drawing.Point( 0, 24 );
			this.mainDisplay.Name = "mainDisplay";
			this.mainDisplay.RenderInterval = 1;
			this.mainDisplay.Size = new System.Drawing.Size( 464, 380 );
			this.mainDisplay.StencilBits = ( ( byte )( 0 ) );
			this.mainDisplay.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 464, 404 );
			this.Controls.Add( this.mainDisplay );
			this.Controls.Add( this.mainMenu );
			this.MainMenuStrip = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "Bob";
			this.Load += new System.EventHandler( this.MainForm_Load );
			this.mainMenu.ResumeLayout( false );
			this.mainMenu.PerformLayout( );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private Rb.Rendering.Windows.Display mainDisplay;
	}
}

