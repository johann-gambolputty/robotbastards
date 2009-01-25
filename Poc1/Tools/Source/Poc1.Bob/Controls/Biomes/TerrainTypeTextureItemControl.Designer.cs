namespace Poc1.Bob.Controls.Biomes
{
	partial class TerrainTypeTextureItemControl
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
			this.selectedCheckBox = new System.Windows.Forms.CheckBox( );
			this.nameTextBox = new System.Windows.Forms.TextBox( );
			this.setTextureButton = new System.Windows.Forms.Button( );
			this.deleteTerrainButton = new System.Windows.Forms.Button( );
			this.SuspendLayout( );
			// 
			// selectedCheckBox
			// 
			this.selectedCheckBox.AutoSize = true;
			this.selectedCheckBox.Location = new System.Drawing.Point( 3, 14 );
			this.selectedCheckBox.Name = "selectedCheckBox";
			this.selectedCheckBox.Size = new System.Drawing.Size( 15, 14 );
			this.selectedCheckBox.TabIndex = 0;
			this.selectedCheckBox.UseVisualStyleBackColor = true;
			this.selectedCheckBox.CheckedChanged += new System.EventHandler( this.showInGraphsCheckBox_CheckedChanged );
			// 
			// nameTextBox
			// 
			this.nameTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.nameTextBox.Location = new System.Drawing.Point( 24, 11 );
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size( 58, 20 );
			this.nameTextBox.TabIndex = 1;
			this.nameTextBox.TextChanged += new System.EventHandler( this.nameTextBox_TextChanged );
			// 
			// setTextureButton
			// 
			this.setTextureButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.setTextureButton.Location = new System.Drawing.Point( 88, 5 );
			this.setTextureButton.Name = "setTextureButton";
			this.setTextureButton.Size = new System.Drawing.Size( 32, 32 );
			this.setTextureButton.TabIndex = 2;
			this.setTextureButton.UseVisualStyleBackColor = true;
			this.setTextureButton.Click += new System.EventHandler( this.setTextureButton_Click );
			// 
			// deleteTerrainButton
			// 
			this.deleteTerrainButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.deleteTerrainButton.Image = global::Poc1.Bob.Properties.Resources.Delete;
			this.deleteTerrainButton.Location = new System.Drawing.Point( 126, 8 );
			this.deleteTerrainButton.Name = "deleteTerrainButton";
			this.deleteTerrainButton.Size = new System.Drawing.Size( 24, 24 );
			this.deleteTerrainButton.TabIndex = 3;
			this.deleteTerrainButton.UseVisualStyleBackColor = true;
			this.deleteTerrainButton.Click += new System.EventHandler( this.deleteTerrainButton_Click );
			// 
			// TerrainTypeTextureControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add( this.deleteTerrainButton );
			this.Controls.Add( this.setTextureButton );
			this.Controls.Add( this.nameTextBox );
			this.Controls.Add( this.selectedCheckBox );
			this.Name = "TerrainTypeTextureControl";
			this.Size = new System.Drawing.Size( 164, 41 );
			this.Paint += new System.Windows.Forms.PaintEventHandler( this.TerrainTypeControl_Paint );
			this.Resize += new System.EventHandler( this.TerrainTypeControl_Resize );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.CheckBox selectedCheckBox;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Button setTextureButton;
		private System.Windows.Forms.Button deleteTerrainButton;
	}
}
