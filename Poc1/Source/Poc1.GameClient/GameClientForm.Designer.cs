namespace Poc1.GameClient
{
	partial class GameClientForm
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
			this.gameDisplay = new Rb.Rendering.Windows.Display( );
			this.SuspendLayout( );
			// 
			// gameDisplay
			// 
			this.gameDisplay.ColourBits = ( ( byte )( 32 ) );
			this.gameDisplay.ContinuousRendering = true;
			this.gameDisplay.DepthBits = ( ( byte )( 24 ) );
			this.gameDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gameDisplay.Location = new System.Drawing.Point( 0, 0 );
			this.gameDisplay.Name = "gameDisplay";
			this.gameDisplay.RenderInterval = 1;
			this.gameDisplay.Size = new System.Drawing.Size( 493, 448 );
			this.gameDisplay.StencilBits = ( ( byte )( 0 ) );
			this.gameDisplay.TabIndex = 0;
			// 
			// GameClientForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 493, 448 );
			this.Controls.Add( this.gameDisplay );
			this.Name = "GameClientForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler( this.GameClientForm_Load );
			this.Shown += new System.EventHandler( this.GameClientForm_Shown );
			this.ResumeLayout( false );

		}

		#endregion

		private Rb.Rendering.Windows.Display gameDisplay;
	}
}

