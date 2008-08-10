namespace Poc1.PlanetBuilder
{
	partial class BuilderForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip1 = new System.Windows.Forms.MenuStrip( );
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.assetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.builderControls1 = new Poc1.PlanetBuilder.BuilderControls( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.testDisplay = new Rb.Rendering.Windows.Display( );
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
			this.menuStrip1.Size = new System.Drawing.Size( 579, 24 );
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
			this.exitToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
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
			this.buildToolStripMenuItem.Size = new System.Drawing.Size( 107, 22 );
			this.buildToolStripMenuItem.Text = "&Build";
			this.buildToolStripMenuItem.Click += new System.EventHandler( this.buildToolStripMenuItem_Click );
			// 
			// builderControls1
			// 
			this.builderControls1.Dock = System.Windows.Forms.DockStyle.Left;
			this.builderControls1.Location = new System.Drawing.Point( 0, 24 );
			this.builderControls1.Name = "builderControls1";
			this.builderControls1.Size = new System.Drawing.Size( 261, 447 );
			this.builderControls1.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 261, 24 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 3, 447 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// testDisplay
			// 
			this.testDisplay.AllowArrowKeyInputs = true;
			this.testDisplay.ColourBits = ( ( byte )( 32 ) );
			this.testDisplay.ContinuousRendering = true;
			this.testDisplay.DepthBits = ( ( byte )( 24 ) );
			this.testDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testDisplay.Location = new System.Drawing.Point( 264, 24 );
			this.testDisplay.Name = "testDisplay";
			this.testDisplay.RenderInterval = 1;
			this.testDisplay.Size = new System.Drawing.Size( 315, 447 );
			this.testDisplay.StencilBits = ( ( byte )( 0 ) );
			this.testDisplay.TabIndex = 2;
			// 
			// BuilderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 579, 471 );
			this.Controls.Add( this.testDisplay );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.builderControls1 );
			this.Controls.Add( this.menuStrip1 );
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "BuilderForm";
			this.Text = "Planet Builder";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Shown += new System.EventHandler( this.BuilderForm_Shown );
			this.Closing += new System.ComponentModel.CancelEventHandler( this.BuilderForm_Closing );
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
		private BuilderControls builderControls1;
		private System.Windows.Forms.Splitter splitter1;
		private Rb.Rendering.Windows.Display testDisplay;
	}
}

