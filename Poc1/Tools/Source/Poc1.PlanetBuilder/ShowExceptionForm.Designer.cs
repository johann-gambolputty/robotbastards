namespace Poc1.PlanetBuilder
{
	partial class ShowExceptionForm
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
			this.messageLabel = new System.Windows.Forms.Label( );
			this.okButton = new System.Windows.Forms.Button( );
			this.exceptionTextBox = new System.Windows.Forms.TextBox( );
			this.iconPanel = new System.Windows.Forms.Panel( );
			this.SuspendLayout( );
			// 
			// messageLabel
			// 
			this.messageLabel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.messageLabel.Location = new System.Drawing.Point( 51, 9 );
			this.messageLabel.Name = "messageLabel";
			this.messageLabel.Size = new System.Drawing.Size( 399, 29 );
			this.messageLabel.TabIndex = 0;
			this.messageLabel.Text = "An exception occurred:";
			// 
			// okButton
			// 
			this.okButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point( 378, 231 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size( 75, 23 );
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// exceptionTextBox
			// 
			this.exceptionTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.exceptionTextBox.Location = new System.Drawing.Point( 12, 49 );
			this.exceptionTextBox.Multiline = true;
			this.exceptionTextBox.Name = "exceptionTextBox";
			this.exceptionTextBox.ReadOnly = true;
			this.exceptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.exceptionTextBox.Size = new System.Drawing.Size( 438, 176 );
			this.exceptionTextBox.TabIndex = 2;
			// 
			// iconPanel
			// 
			this.iconPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.iconPanel.Location = new System.Drawing.Point( 12, 9 );
			this.iconPanel.Name = "iconPanel";
			this.iconPanel.Size = new System.Drawing.Size( 34, 34 );
			this.iconPanel.TabIndex = 3;
			// 
			// ShowExceptionForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 465, 266 );
			this.Controls.Add( this.iconPanel );
			this.Controls.Add( this.exceptionTextBox );
			this.Controls.Add( this.okButton );
			this.Controls.Add( this.messageLabel );
			this.Name = "ShowExceptionForm";
			this.Text = "Exception";
			this.Load += new System.EventHandler( this.ShowExceptionForm_Load );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.TextBox exceptionTextBox;
		private System.Windows.Forms.Panel iconPanel;

	}
}