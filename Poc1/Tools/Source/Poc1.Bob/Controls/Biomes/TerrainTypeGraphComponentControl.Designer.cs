namespace Poc1.Bob.Controls.Biomes
{
	partial class TerrainTypeGraphComponentControl
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
			this.removeButton = new System.Windows.Forms.Button( );
			this.setTextureButton = new System.Windows.Forms.Button( );
			this.nameTextBox = new System.Windows.Forms.TextBox( );
			this.SuspendLayout( );
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point( 3, 3 );
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size( 20, 38 );
			this.removeButton.TabIndex = 0;
			this.removeButton.Text = "<";
			this.removeButton.UseVisualStyleBackColor = true;
			// 
			// setTextureButton
			// 
			this.setTextureButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.setTextureButton.Location = new System.Drawing.Point( 179, 4 );
			this.setTextureButton.Name = "setTextureButton";
			this.setTextureButton.Size = new System.Drawing.Size( 32, 32 );
			this.setTextureButton.TabIndex = 4;
			this.setTextureButton.UseVisualStyleBackColor = true;
			// 
			// nameTextBox
			// 
			this.nameTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.nameTextBox.Location = new System.Drawing.Point( 34, 10 );
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size( 139, 20 );
			this.nameTextBox.TabIndex = 3;
			// 
			// TerrainTypeGraphComponentControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.setTextureButton );
			this.Controls.Add( this.nameTextBox );
			this.Controls.Add( this.removeButton );
			this.Name = "TerrainTypeGraphComponentControl";
			this.Size = new System.Drawing.Size( 245, 41 );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button setTextureButton;
		private System.Windows.Forms.TextBox nameTextBox;
	}
}
