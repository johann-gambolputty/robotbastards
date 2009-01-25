namespace Poc1.Bob.Controls.Biomes
{
	partial class NewBiomeForm
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
			this.nameLabel = new System.Windows.Forms.Label( );
			this.nameTextBox = new System.Windows.Forms.TextBox( );
			this.okButton = new System.Windows.Forms.Button( );
			this.cancelButton = new System.Windows.Forms.Button( );
			this.SuspendLayout( );
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point( 12, 28 );
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size( 38, 13 );
			this.nameLabel.TabIndex = 0;
			this.nameLabel.Text = "Name:";
			// 
			// nameTextBox
			// 
			this.nameTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.nameTextBox.Location = new System.Drawing.Point( 53, 25 );
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size( 237, 20 );
			this.nameTextBox.TabIndex = 1;
			this.nameTextBox.TextChanged += new System.EventHandler( this.nameTextBox_TextChanged );
			// 
			// okButton
			// 
			this.okButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point( 134, 64 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size( 75, 23 );
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 215, 64 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// BiomeNameForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 302, 99 );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.okButton );
			this.Controls.Add( this.nameTextBox );
			this.Controls.Add( this.nameLabel );
			this.Name = "BiomeNameForm";
			this.Text = "New Biome";
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
	}
}