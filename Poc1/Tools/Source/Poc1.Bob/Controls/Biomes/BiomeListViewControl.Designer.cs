namespace Poc1.Bob.Controls.Biomes
{
	partial class BiomeListViewControl
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
			System.Windows.Forms.ColumnHeader nameColumnHeader;
			this.addButton = new System.Windows.Forms.Button( );
			this.removeButton = new System.Windows.Forms.Button( );
			this.biomeListView = new System.Windows.Forms.ListView( );
			nameColumnHeader = new System.Windows.Forms.ColumnHeader( );
			this.SuspendLayout( );
			// 
			// addButton
			// 
			this.addButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.addButton.Location = new System.Drawing.Point( 3, 128 );
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size( 75, 23 );
			this.addButton.TabIndex = 1;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler( this.addButton_Click );
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.removeButton.Location = new System.Drawing.Point( 136, 127 );
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size( 75, 23 );
			this.removeButton.TabIndex = 2;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler( this.removeButton_Click );
			// 
			// biomeListView
			// 
			this.biomeListView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.biomeListView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            nameColumnHeader} );
			this.biomeListView.FullRowSelect = true;
			this.biomeListView.GridLines = true;
			this.biomeListView.LabelEdit = true;
			this.biomeListView.Location = new System.Drawing.Point( 3, 3 );
			this.biomeListView.Name = "biomeListView";
			this.biomeListView.Size = new System.Drawing.Size( 205, 119 );
			this.biomeListView.TabIndex = 3;
			this.biomeListView.UseCompatibleStateImageBehavior = false;
			this.biomeListView.View = System.Windows.Forms.View.Details;
			this.biomeListView.SelectedIndexChanged += new System.EventHandler( this.biomeListView_SelectedIndexChanged );
			// 
			// nameColumnHeader
			// 
			nameColumnHeader.Text = "Name";
			nameColumnHeader.Width = 200;
			// 
			// BiomeListViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.biomeListView );
			this.Controls.Add( this.removeButton );
			this.Controls.Add( this.addButton );
			this.Name = "BiomeListViewControl";
			this.Size = new System.Drawing.Size( 211, 154 );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.ListView biomeListView;
	}
}
