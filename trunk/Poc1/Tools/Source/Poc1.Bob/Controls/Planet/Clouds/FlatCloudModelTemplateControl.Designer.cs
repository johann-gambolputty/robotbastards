namespace Poc1.Bob.Controls.Planet.Clouds
{
	partial class FlatCloudModelTemplateControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.cloudModelTemplateControl1 = new Poc1.Bob.Controls.Planet.Clouds.CloudModelTemplateControl( );
			this.SuspendLayout( );
			// 
			// cloudModelTemplateControl1
			// 
			this.cloudModelTemplateControl1.Location = new System.Drawing.Point( 3, 3 );
			this.cloudModelTemplateControl1.Name = "cloudModelTemplateControl1";
			this.cloudModelTemplateControl1.Size = new System.Drawing.Size( 320, 143 );
			this.cloudModelTemplateControl1.TabIndex = 0;
			// 
			// FlatCloudModelTemplateControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.cloudModelTemplateControl1 );
			this.Name = "FlatCloudModelTemplateControl";
			this.Size = new System.Drawing.Size( 327, 145 );
			this.ResumeLayout( false );

		}

		#endregion

		private CloudModelTemplateControl cloudModelTemplateControl1;
	}
}
