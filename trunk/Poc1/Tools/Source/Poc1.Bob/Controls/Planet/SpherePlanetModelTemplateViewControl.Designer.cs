using System.Windows.Forms;

namespace Poc1.Bob.Controls.Planet
{
	partial class SpherePlanetModelTemplateViewControl
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
			this.radiusRangeSlider = new RangeSlider( );
			this.label1 = new System.Windows.Forms.Label( );
			this.SuspendLayout( );
			// 
			// radiusRangeSlider
			// 
			this.radiusRangeSlider.MinValue = 10;
			this.radiusRangeSlider.MaxValue = 50000;
			this.radiusRangeSlider.Value = 200;
			this.radiusRangeSlider.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.radiusRangeSlider.Location = new System.Drawing.Point( 9, 31 );
			this.radiusRangeSlider.Name = "radiusRangeSlider";
			this.radiusRangeSlider.Size = new System.Drawing.Size( 185, 38 );
			this.radiusRangeSlider.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 6, 15 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 76, 13 );
			this.label1.TabIndex = 1;
			this.label1.Text = "Planet Radius (km):";
			// 
			// SpherePlanetTemplateViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.radiusRangeSlider );
			this.Name = "SpherePlanetTemplateViewControl";
			this.Size = new System.Drawing.Size( 197, 150 );
			this.radiusRangeSlider.ValueChanged += new System.EventHandler( radiusRangeSlider_ValueChanged );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private RangeSlider radiusRangeSlider;
		private Label label1;

	}
}
