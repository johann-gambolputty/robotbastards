namespace Poc1.AtmosphereTest
{
	partial class AtmosphereTestForm
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
			this.display = new Rb.Rendering.Windows.Display( );
			this.SuspendLayout( );
			// 
			// display
			// 
			this.display.AllowArrowKeyInputs = true;
			this.display.ColourBits = ( ( byte )( 32 ) );
			this.display.ContinuousRendering = true;
			this.display.DepthBits = ( ( byte )( 24 ) );
			this.display.Dock = System.Windows.Forms.DockStyle.Fill;
			this.display.FocusOnMouseOver = false;
			this.display.Location = new System.Drawing.Point( 0, 0 );
			this.display.Name = "display";
			this.display.RenderInterval = 1;
			this.display.Size = new System.Drawing.Size( 519, 469 );
			this.display.StencilBits = ( ( byte )( 0 ) );
			this.display.TabIndex = 0;
			// 
			// AtmosphereTestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 519, 469 );
			this.Controls.Add( this.display );
			this.Name = "AtmosphereTestForm";
			this.Text = "Atmosphere Test";
			this.Load += new System.EventHandler( this.AtmosphereTestForm_Load );
			this.ResumeLayout( false );

		}

		#endregion

		private Rb.Rendering.Windows.Display display;
	}
}

