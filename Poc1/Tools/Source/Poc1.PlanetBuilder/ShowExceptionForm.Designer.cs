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
			this.button1 = new System.Windows.Forms.Button( );
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
			this.messageLabel.Size = new System.Drawing.Size( 226, 29 );
			this.messageLabel.TabIndex = 0;
			this.messageLabel.Text = "An exception occurred:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point( 12, 231 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 75, 23 );
			this.button1.TabIndex = 1;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// exceptionTextBox
			// 
			this.exceptionTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.exceptionTextBox.Location = new System.Drawing.Point( 12, 41 );
			this.exceptionTextBox.Multiline = true;
			this.exceptionTextBox.Name = "exceptionTextBox";
			this.exceptionTextBox.ReadOnly = true;
			this.exceptionTextBox.Size = new System.Drawing.Size( 265, 184 );
			this.exceptionTextBox.TabIndex = 2;
			// 
			// iconPanel
			// 
			this.iconPanel.Location = new System.Drawing.Point( 12, 9 );
			this.iconPanel.Name = "iconPanel";
			this.iconPanel.Size = new System.Drawing.Size( 33, 26 );
			this.iconPanel.TabIndex = 3;
			// 
			// ShowExceptionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 292, 266 );
			this.Controls.Add( this.iconPanel );
			this.Controls.Add( this.exceptionTextBox );
			this.Controls.Add( this.button1 );
			this.Controls.Add( this.messageLabel );
			this.Name = "ShowExceptionForm";
			this.Text = "Exception";
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox exceptionTextBox;
		private System.Windows.Forms.Panel iconPanel;

	}
}