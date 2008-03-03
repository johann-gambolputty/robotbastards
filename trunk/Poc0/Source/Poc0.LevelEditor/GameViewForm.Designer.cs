namespace Poc0.LevelEditor
{
	partial class GameViewForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( GameViewForm ) );
			this.gameDisplay = new Rb.Rendering.Windows.Display( );
			this.gameTools = new System.Windows.Forms.ToolStrip( );
			this.playButton = new System.Windows.Forms.ToolStripButton( );
			this.pauseButton = new System.Windows.Forms.ToolStripButton( );
			this.dumpBufferButton = new System.Windows.Forms.ToolStripSplitButton( );
			this.shadowBufferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton( );
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton( );
			this.shadowBufferToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem( );
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator( );
			this.gameTools.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// gameDisplay
			// 
			this.gameDisplay.ColourBits = ( ( byte )( 32 ) );
			this.gameDisplay.ContinuousRendering = true;
			this.gameDisplay.DepthBits = ( ( byte )( 24 ) );
			this.gameDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gameDisplay.Location = new System.Drawing.Point( 0, 25 );
			this.gameDisplay.Name = "gameDisplay";
			this.gameDisplay.RenderInterval = 1;
			this.gameDisplay.Size = new System.Drawing.Size( 377, 264 );
			this.gameDisplay.StencilBits = ( ( byte )( 0 ) );
			this.gameDisplay.TabIndex = 0;
			// 
			// gameTools
			// 
			this.gameTools.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.playButton,
            this.pauseButton,
            this.toolStripSeparator1,
            this.toolStripDropDownButton1} );
			this.gameTools.Location = new System.Drawing.Point( 0, 0 );
			this.gameTools.Name = "gameTools";
			this.gameTools.Size = new System.Drawing.Size( 377, 25 );
			this.gameTools.TabIndex = 1;
			this.gameTools.Text = "toolStrip1";
			// 
			// playButton
			// 
			this.playButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.playButton.Image = global::Poc0.LevelEditor.Properties.Resources.Play;
			this.playButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.playButton.Name = "playButton";
			this.playButton.Size = new System.Drawing.Size( 23, 22 );
			this.playButton.Text = "toolStripButton2";
			this.playButton.Click += new System.EventHandler( this.playButton_Click );
			// 
			// pauseButton
			// 
			this.pauseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.pauseButton.Image = global::Poc0.LevelEditor.Properties.Resources.Pause;
			this.pauseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pauseButton.Name = "pauseButton";
			this.pauseButton.Size = new System.Drawing.Size( 23, 22 );
			this.pauseButton.Text = "toolStripButton3";
			this.pauseButton.Click += new System.EventHandler( this.pauseButton_Click );
			// 
			// dumpBufferButton
			// 
			this.dumpBufferButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.dumpBufferButton.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.shadowBufferToolStripMenuItem} );
			this.dumpBufferButton.Image = ( ( System.Drawing.Image )( resources.GetObject( "dumpBufferButton.Image" ) ) );
			this.dumpBufferButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.dumpBufferButton.Name = "dumpBufferButton";
			this.dumpBufferButton.Size = new System.Drawing.Size( 88, 22 );
			this.dumpBufferButton.Text = "Dump Buffers";
			// 
			// shadowBufferToolStripMenuItem
			// 
			this.shadowBufferToolStripMenuItem.Name = "shadowBufferToolStripMenuItem";
			this.shadowBufferToolStripMenuItem.Size = new System.Drawing.Size( 156, 22 );
			this.shadowBufferToolStripMenuItem.Text = "Shadow Buffer";
			this.shadowBufferToolStripMenuItem.Click += new System.EventHandler( this.shadowBufferToolStripMenuItem_Click );
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size( 23, 22 );
			this.toolStripButton1.Text = "toolStripButton1";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.shadowBufferToolStripMenuItem1} );
			this.toolStripDropDownButton1.Image = global::Poc0.LevelEditor.Properties.Resources.Capture;
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size( 75, 22 );
			this.toolStripDropDownButton1.Text = "Capture";
			// 
			// shadowBufferToolStripMenuItem1
			// 
			this.shadowBufferToolStripMenuItem1.Name = "shadowBufferToolStripMenuItem1";
			this.shadowBufferToolStripMenuItem1.Size = new System.Drawing.Size( 156, 22 );
			this.shadowBufferToolStripMenuItem1.Text = "Shadow Buffer";
			this.shadowBufferToolStripMenuItem1.Click += new System.EventHandler( this.shadowBufferToolStripMenuItem_Click );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 6, 25 );
			// 
			// GameViewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 377, 289 );
			this.Controls.Add( this.gameDisplay );
			this.Controls.Add( this.gameTools );
			this.Icon = ( ( System.Drawing.Icon )( resources.GetObject( "$this.Icon" ) ) );
			this.Name = "GameViewForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Game";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.GameViewForm_FormClosing );
			this.Load += new System.EventHandler( this.GameViewForm_Load );
			this.gameTools.ResumeLayout( false );
			this.gameTools.PerformLayout( );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private Rb.Rendering.Windows.Display gameDisplay;
		private System.Windows.Forms.ToolStrip gameTools;
		private System.Windows.Forms.ToolStripSplitButton dumpBufferButton;
		private System.Windows.Forms.ToolStripMenuItem shadowBufferToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton playButton;
		private System.Windows.Forms.ToolStripButton pauseButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
		private System.Windows.Forms.ToolStripMenuItem shadowBufferToolStripMenuItem1;

	}
}