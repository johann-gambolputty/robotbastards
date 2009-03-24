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
			this.editTemplateButton = new System.Windows.Forms.Button( );
			this.compositeView = new Rb.Common.Controls.Forms.Components.CompositeViewControl( );
			this.SuspendLayout( );
			// 
			// editTemplateButton
			// 
			this.editTemplateButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.editTemplateButton.Enabled = false;
			this.editTemplateButton.Location = new System.Drawing.Point( 3, 189 );
			this.editTemplateButton.Name = "editTemplateButton";
			this.editTemplateButton.Size = new System.Drawing.Size( 75, 23 );
			this.editTemplateButton.TabIndex = 1;
			this.editTemplateButton.Text = "Edit...";
			this.editTemplateButton.UseVisualStyleBackColor = true;
			this.editTemplateButton.Click += new System.EventHandler( this.editTemplateButton_Click );
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
			// EditableCompositeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.editTemplateButton );
			this.Controls.Add( this.compositeView );
			this.Name = "EditableCompositeControl";
			this.Size = new System.Drawing.Size( 220, 215 );
			this.ResumeLayout( false );

		}

		#endregion

		private CompositeViewControl compositeView;
		private Button editTemplateButton;
	}
}
