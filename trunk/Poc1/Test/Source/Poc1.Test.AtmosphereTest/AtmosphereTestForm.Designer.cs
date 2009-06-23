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
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.display = new Rb.Rendering.Windows.Display( );
			this.SuspendLayout( );
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Left;
			this.propertyGrid1.Location = new System.Drawing.Point( 0, 0 );
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size( 130, 469 );
			this.propertyGrid1.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 130, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 3, 469 );
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// display
			// 
			this.display.AllowArrowKeyInputs = true;
			this.display.ColourBits = ( ( byte )( 32 ) );
			this.display.ContinuousRendering = true;
			this.display.DepthBits = ( ( byte )( 24 ) );
			this.display.Dock = System.Windows.Forms.DockStyle.Fill;
			this.display.FocusOnMouseOver = false;
			this.display.Location = new System.Drawing.Point( 133, 0 );
			this.display.Name = "display";
			this.display.RenderInterval = 1;
			this.display.Size = new System.Drawing.Size( 386, 469 );
			this.display.StencilBits = ( ( byte )( 0 ) );
			this.display.TabIndex = 3;
			// 
			// AtmosphereTestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 519, 469 );
			this.Controls.Add( this.display );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.propertyGrid1 );
			this.Name = "AtmosphereTestForm";
			this.Text = "Atmosphere Test";
			this.Load += new System.EventHandler( this.AtmosphereTestForm_Load );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Splitter splitter1;
		private Rb.Rendering.Windows.Display display;
	}
}

