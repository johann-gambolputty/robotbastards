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
			this.deleteButton = new System.Windows.Forms.Button( );
			this.createButton = new System.Windows.Forms.Button( );
			this.biomeListBox = new System.Windows.Forms.CheckedListBox( );
			this.panel1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.deleteButton );
			this.panel1.Controls.Add( this.createButton );
			this.panel1.Controls.Add( this.biomeListBox );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point( 0, 0 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 175, 215 );
			this.panel1.TabIndex = 0;
			// 
			// deleteButton
			// 
			this.deleteButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.deleteButton.Location = new System.Drawing.Point( 69, 182 );
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size( 60, 23 );
			this.deleteButton.TabIndex = 5;
			this.deleteButton.Text = "Delete";
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler( this.deleteButton_Click );
			// 
			// createButton
			// 
			this.createButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.createButton.Location = new System.Drawing.Point( 3, 182 );
			this.createButton.Name = "createButton";
			this.createButton.Size = new System.Drawing.Size( 60, 23 );
			this.createButton.TabIndex = 4;
			this.createButton.Text = "Create";
			this.createButton.UseVisualStyleBackColor = true;
			this.createButton.Click += new System.EventHandler( this.createButton_Click );
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
			this.biomeListBox.KeyUp += new System.Windows.Forms.KeyEventHandler( this.biomeListBox_KeyUp );
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
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button createButton;
		private System.Windows.Forms.CheckedListBox biomeListBox;

	}
}
