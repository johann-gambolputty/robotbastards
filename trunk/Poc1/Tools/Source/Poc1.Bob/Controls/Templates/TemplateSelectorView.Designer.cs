namespace Poc1.Bob.Controls.Templates
{
	partial class TemplateSelectorView
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
			this.components = new System.ComponentModel.Container( );
			this.templateTreeView = new System.Windows.Forms.TreeView( );
			this.templateImageList = new System.Windows.Forms.ImageList( this.components );
			this.descriptionLabel = new System.Windows.Forms.Label( );
			this.SuspendLayout( );
			// 
			// templateTreeView
			// 
			this.templateTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.templateTreeView.ImageIndex = 0;
			this.templateTreeView.ImageList = this.templateImageList;
			this.templateTreeView.Location = new System.Drawing.Point( 0, 0 );
			this.templateTreeView.Name = "templateTreeView";
			this.templateTreeView.SelectedImageIndex = 0;
			this.templateTreeView.Size = new System.Drawing.Size( 218, 262 );
			this.templateTreeView.TabIndex = 0;
			this.templateTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.templateTreeView_AfterSelect );
			// 
			// templateImageList
			// 
			this.templateImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.templateImageList.ImageSize = new System.Drawing.Size( 16, 16 );
			this.templateImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.descriptionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.descriptionLabel.Location = new System.Drawing.Point( 0, 262 );
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size( 218, 17 );
			this.descriptionLabel.TabIndex = 1;
			this.descriptionLabel.Text = "Description";
			// 
			// TemplateSelectorView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.templateTreeView );
			this.Controls.Add( this.descriptionLabel );
			this.Name = "TemplateSelectorView";
			this.Size = new System.Drawing.Size( 218, 279 );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TreeView templateTreeView;
		private System.Windows.Forms.ImageList templateImageList;
		private System.Windows.Forms.Label descriptionLabel;
	}
}
