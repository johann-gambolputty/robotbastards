namespace Rb.TextureViewer
{
	partial class TextureForm
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
			this.display1 = new Rb.Rendering.Windows.Display( );
			this.SuspendLayout( );
			// 
			// display1
			// 
			this.display1.AllowArrowKeyInputs = true;
			this.display1.ColourBits = ( ( byte )( 32 ) );
			this.display1.ContinuousRendering = true;
			this.display1.DepthBits = ( ( byte )( 24 ) );
			this.display1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.display1.Location = new System.Drawing.Point( 0, 0 );
			this.display1.Name = "display1";
			this.display1.RenderInterval = 1;
			this.display1.Size = new System.Drawing.Size( 292, 266 );
			this.display1.StencilBits = ( ( byte )( 0 ) );
			this.display1.TabIndex = 0;
			this.display1.OnRender += new System.EventHandler( this.display1_OnRender );
			// 
			// TextureForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 292, 266 );
			this.Controls.Add( this.display1 );
			this.Name = "TextureForm";
			this.Text = "TextureForm";
			this.Shown += new System.EventHandler( this.TextureForm_Shown );
			this.ResumeLayout( false );

		}

		#endregion

		private Rb.Rendering.Windows.Display display1;
	}
}