using System.Windows.Forms;
using Rb.Common.Controls.Forms.Components;

namespace Poc1.Bob.Controls.Components
{
	partial class EditableCompositeControl
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
			this.addTemplateButton = new System.Windows.Forms.Button( );
			this.compositeView = new Rb.Common.Controls.Forms.Components.CompositeViewControl( );
			this.SuspendLayout( );
			// 
			// addTemplateButton
			// 
			this.addTemplateButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.addTemplateButton.Location = new System.Drawing.Point( 3, 189 );
			this.addTemplateButton.Name = "addTemplateButton";
			this.addTemplateButton.Size = new System.Drawing.Size( 75, 23 );
			this.addTemplateButton.TabIndex = 1;
			this.addTemplateButton.Text = "Add...";
			this.addTemplateButton.UseVisualStyleBackColor = true;
			this.addTemplateButton.Enabled = false;
			this.addTemplateButton.Click += new System.EventHandler( this.addTemplateButton_Click );
			// 
			// compositeView
			// 
			this.compositeView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.compositeView.Composite = null;
			this.compositeView.Location = new System.Drawing.Point( 0, 0 );
			this.compositeView.Name = "compositeView";
			this.compositeView.Size = new System.Drawing.Size( 217, 183 );
			this.compositeView.TabIndex = 2;
			// 
			// PlanetTemplateCompositionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.addTemplateButton );
			this.Controls.Add( this.compositeView );
			this.Name = "PlanetTemplateCompositionControl";
			this.Size = new System.Drawing.Size( 220, 215 );
			this.ResumeLayout( false );

		}

		#endregion

		private CompositeViewControl compositeView;
		private System.Windows.Forms.Button addTemplateButton;
	}
}
