namespace Poc1.Bob.Controls.Biomes
{
	partial class BiomeListControl
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
			this.panel1 = new System.Windows.Forms.Panel( );
			this.removeButton = new System.Windows.Forms.Button( );
			this.addButton = new System.Windows.Forms.Button( );
			this.biomeListBox = new System.Windows.Forms.CheckedListBox( );
			this.panel1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.removeButton );
			this.panel1.Controls.Add( this.addButton );
			this.panel1.Controls.Add( this.biomeListBox );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point( 0, 0 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 175, 215 );
			this.panel1.TabIndex = 0;
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.removeButton.Location = new System.Drawing.Point( 56, 182 );
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size( 57, 23 );
			this.removeButton.TabIndex = 5;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler( this.removeButton_Click );
			// 
			// addButton
			// 
			this.addButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.addButton.Location = new System.Drawing.Point( 3, 182 );
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size( 47, 23 );
			this.addButton.TabIndex = 4;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler( this.addButton_Click );
			// 
			// biomeListBox
			// 
			this.biomeListBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.biomeListBox.FormattingEnabled = true;
			this.biomeListBox.Location = new System.Drawing.Point( 0, 0 );
			this.biomeListBox.Name = "biomeListBox";
			this.biomeListBox.Size = new System.Drawing.Size( 175, 169 );
			this.biomeListBox.TabIndex = 3;
			this.biomeListBox.SelectedIndexChanged += new System.EventHandler( this.biomeListView_SelectedIndexChanged );
			this.biomeListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler( this.biomeListBox_ItemCheck );
			// 
			// BiomeListControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.panel1 );
			this.Name = "BiomeListControl";
			this.Size = new System.Drawing.Size( 175, 215 );
			this.panel1.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.CheckedListBox biomeListBox;

	}
}
