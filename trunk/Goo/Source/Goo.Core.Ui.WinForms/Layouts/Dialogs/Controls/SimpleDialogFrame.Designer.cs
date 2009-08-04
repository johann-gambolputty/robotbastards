namespace Goo.Common.WinForms.Layouts.Dialogs.Controls
{
	partial class SimpleDialogFrame
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
			this.hostPanel = new System.Windows.Forms.Panel( );
			this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel( );
			this.SuspendLayout( );
			// 
			// hostPanel
			// 
			this.hostPanel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.hostPanel.Location = new System.Drawing.Point( 3, 2 );
			this.hostPanel.Name = "hostPanel";
			this.hostPanel.Size = new System.Drawing.Size( 286, 223 );
			this.hostPanel.TabIndex = 1;
			// 
			// buttonPanel
			// 
			this.buttonPanel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.buttonPanel.Location = new System.Drawing.Point( 3, 231 );
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Size = new System.Drawing.Size( 286, 30 );
			this.buttonPanel.TabIndex = 2;
			// 
			// SimpleDialogFrame
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 292, 266 );
			this.Controls.Add( this.buttonPanel );
			this.Controls.Add( this.hostPanel );
			this.KeyPreview = true;
			this.Name = "SimpleDialogFrame";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler( this.SimpleDialogFrame_FormClosed );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Panel hostPanel;
		private System.Windows.Forms.FlowLayoutPanel buttonPanel;
	}
}