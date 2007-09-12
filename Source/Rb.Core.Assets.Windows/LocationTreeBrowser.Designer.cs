namespace Rb.Core.Assets.Windows
{
	partial class LocationTreeBrowser
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
			this.assetExtensionComboBox = new System.Windows.Forms.ComboBox();
			this.selectedAssetTextBox = new System.Windows.Forms.TextBox();
			this.backButton = new System.Windows.Forms.Button();
			this.upButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.currentFolderView = new System.Windows.Forms.ListView();
			this.foldersComboBox = new Rb.NiceControls.NiceComboBox();
			this.SuspendLayout();
			// 
			// assetExtensionComboBox
			// 
			this.assetExtensionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.assetExtensionComboBox.FormattingEnabled = true;
			this.assetExtensionComboBox.Location = new System.Drawing.Point(44, 169);
			this.assetExtensionComboBox.Name = "assetExtensionComboBox";
			this.assetExtensionComboBox.Size = new System.Drawing.Size(202, 21);
			this.assetExtensionComboBox.TabIndex = 1;
			// 
			// selectedAssetTextBox
			// 
			this.selectedAssetTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.selectedAssetTextBox.Location = new System.Drawing.Point(44, 144);
			this.selectedAssetTextBox.Name = "selectedAssetTextBox";
			this.selectedAssetTextBox.Size = new System.Drawing.Size(202, 20);
			this.selectedAssetTextBox.TabIndex = 2;
			// 
			// backButton
			// 
			this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.backButton.Enabled = false;
			this.backButton.Image = global::Rb.Core.Assets.Windows.Properties.Resources.Back;
			this.backButton.Location = new System.Drawing.Point(192, 3);
			this.backButton.Name = "backButton";
			this.backButton.Size = new System.Drawing.Size(24, 22);
			this.backButton.TabIndex = 4;
			this.backButton.UseVisualStyleBackColor = true;
			this.backButton.Click += new System.EventHandler(this.backButton_Click);
			// 
			// upButton
			// 
			this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.upButton.Image = global::Rb.Core.Assets.Windows.Properties.Resources.Up;
			this.upButton.Location = new System.Drawing.Point(222, 3);
			this.upButton.Name = "upButton";
			this.upButton.Size = new System.Drawing.Size(24, 22);
			this.upButton.TabIndex = 5;
			this.upButton.UseVisualStyleBackColor = true;
			this.upButton.Click += new System.EventHandler(this.upButton_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 147);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Asset:";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 172);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(34, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Type:";
			// 
			// currentFolderView
			// 
			this.currentFolderView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.currentFolderView.FullRowSelect = true;
			this.currentFolderView.Location = new System.Drawing.Point(3, 30);
			this.currentFolderView.Name = "currentFolderView";
			this.currentFolderView.Size = new System.Drawing.Size(243, 108);
			this.currentFolderView.TabIndex = 8;
			this.currentFolderView.UseCompatibleStateImageBehavior = false;
			this.currentFolderView.View = System.Windows.Forms.View.Details;
			this.currentFolderView.DoubleClick += new System.EventHandler(this.currentFolderView_DoubleClick);
			// 
			// foldersComboBox
			// 
			this.foldersComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.foldersComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.foldersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.foldersComboBox.FormattingEnabled = true;
			this.foldersComboBox.Location = new System.Drawing.Point(3, 4);
			this.foldersComboBox.Name = "foldersComboBox";
			this.foldersComboBox.Size = new System.Drawing.Size(183, 21);
			this.foldersComboBox.TabIndex = 9;
			// 
			// LocationTreeBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.foldersComboBox);
			this.Controls.Add(this.currentFolderView);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.upButton);
			this.Controls.Add(this.backButton);
			this.Controls.Add(this.selectedAssetTextBox);
			this.Controls.Add(this.assetExtensionComboBox);
			this.Name = "LocationTreeBrowser";
			this.Size = new System.Drawing.Size(249, 193);
			this.Load += new System.EventHandler(this.LocationTreeBrowser_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox assetExtensionComboBox;
		private System.Windows.Forms.TextBox selectedAssetTextBox;
		private System.Windows.Forms.Button backButton;
		private System.Windows.Forms.Button upButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListView currentFolderView;
		private Rb.NiceControls.NiceComboBox foldersComboBox;
	}
}
