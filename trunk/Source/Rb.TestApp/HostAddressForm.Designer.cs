namespace Rb.TestApp
{
	partial class HostAddressForm
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
			this.okButton = new System.Windows.Forms.Button( );
			this.cancelButton = new System.Windows.Forms.Button( );
			this.label1 = new System.Windows.Forms.Label( );
			this.ipAddressTextBox = new System.Windows.Forms.TextBox( );
			this.label2 = new System.Windows.Forms.Label( );
			this.portTextBox = new System.Windows.Forms.TextBox( );
			this.SuspendLayout( );
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point( 12, 75 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size( 75, 23 );
			this.okButton.TabIndex = 0;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 123, 75 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 58, 13 );
			this.label1.TabIndex = 2;
			this.label1.Text = "IP Address";
			// 
			// ipAddressTextBox
			// 
			this.ipAddressTextBox.Location = new System.Drawing.Point( 76, 6 );
			this.ipAddressTextBox.Name = "ipAddressTextBox";
			this.ipAddressTextBox.Size = new System.Drawing.Size( 122, 20 );
			this.ipAddressTextBox.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 44, 36 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 26, 13 );
			this.label2.TabIndex = 4;
			this.label2.Text = "Port";
			// 
			// portTextBox
			// 
			this.portTextBox.Location = new System.Drawing.Point( 76, 36 );
			this.portTextBox.Name = "portTextBox";
			this.portTextBox.Size = new System.Drawing.Size( 122, 20 );
			this.portTextBox.TabIndex = 5;
			// 
			// IpEndPointForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 210, 110 );
			this.Controls.Add( this.portTextBox );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.ipAddressTextBox );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.okButton );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "IpEndPointForm";
			this.Text = "Address";
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox ipAddressTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox portTextBox;
	}
}