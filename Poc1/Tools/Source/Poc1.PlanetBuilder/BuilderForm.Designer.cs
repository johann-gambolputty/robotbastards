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
			this.builderControls1 = new Poc1.PlanetBuilder.BuilderControls( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.testDisplay = new Rb.Rendering.Windows.Display( );
			this.SuspendLayout( );
			// 
			// builderControls1
			// 
			this.builderControls1.Dock = System.Windows.Forms.DockStyle.Left;
			this.builderControls1.Location = new System.Drawing.Point( 0, 0 );
			this.builderControls1.Name = "builderControls1";
			this.builderControls1.Size = new System.Drawing.Size( 261, 471 );
			this.builderControls1.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 261, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 3, 471 );
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
			this.testDisplay.Location = new System.Drawing.Point( 264, 0 );
			this.testDisplay.Name = "testDisplay";
			this.testDisplay.RenderInterval = 1;
			this.testDisplay.Size = new System.Drawing.Size( 315, 471 );
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
			this.Name = "BuilderForm";
			this.Text = "Planet Builder";
			this.Shown += new System.EventHandler( this.BuilderForm_Shown );
			this.Closing += new System.ComponentModel.CancelEventHandler( BuilderForm_Closing );
			this.ResumeLayout( false );

		}

		#endregion

		private BuilderControls builderControls1;
		private System.Windows.Forms.Splitter splitter1;
		private Rb.Rendering.Windows.Display testDisplay;
	}
}

