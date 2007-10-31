namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	partial class ObjectUITypeEditorForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.sourceTabs = new System.Windows.Forms.TabControl( );
			this.createObjectTabPage = new System.Windows.Forms.TabPage( );
			this.newObjectControl = new Rb.Tools.LevelEditor.Core.Controls.Forms.NewObjectControl( );
			this.assetTabPage = new System.Windows.Forms.TabPage( );
			this.loadLayoutContainer = new System.Windows.Forms.Panel( );
			this.locationManagerControlPanel = new System.Windows.Forms.Panel( );
			this.loadLayoutSplitter = new System.Windows.Forms.Splitter( );
			this.loadParametersGrid = new System.Windows.Forms.PropertyGrid( );
			this.toggleAdvancedButton = new System.Windows.Forms.Button( );
			this.locationManagerComboBox = new System.Windows.Forms.ComboBox( );
			this.okButton = new System.Windows.Forms.Button( );
			this.cancelButton = new System.Windows.Forms.Button( );
			this.sourceTabs.SuspendLayout( );
			this.createObjectTabPage.SuspendLayout( );
			this.assetTabPage.SuspendLayout( );
			this.loadLayoutContainer.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// sourceTabs
			// 
			this.sourceTabs.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.sourceTabs.Controls.Add( this.createObjectTabPage );
			this.sourceTabs.Controls.Add( this.assetTabPage );
			this.sourceTabs.Location = new System.Drawing.Point( 12, 12 );
			this.sourceTabs.Name = "sourceTabs";
			this.sourceTabs.SelectedIndex = 0;
			this.sourceTabs.Size = new System.Drawing.Size( 368, 292 );
			this.sourceTabs.TabIndex = 0;
			// 
			// createObjectTabPage
			// 
			this.createObjectTabPage.Controls.Add( this.newObjectControl );
			this.createObjectTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.createObjectTabPage.Name = "createObjectTabPage";
			this.createObjectTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.createObjectTabPage.Size = new System.Drawing.Size( 360, 266 );
			this.createObjectTabPage.TabIndex = 0;
			this.createObjectTabPage.Text = "Create";
			this.createObjectTabPage.UseVisualStyleBackColor = true;
			// 
			// newObjectControl
			// 
			this.newObjectControl.BaseType = null;
			this.newObjectControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.newObjectControl.Location = new System.Drawing.Point( 3, 3 );
			this.newObjectControl.Name = "newObjectControl";
			this.newObjectControl.Size = new System.Drawing.Size( 354, 260 );
			this.newObjectControl.TabIndex = 0;
			// 
			// assetTabPage
			// 
			this.assetTabPage.Controls.Add( this.loadLayoutContainer );
			this.assetTabPage.Controls.Add( this.toggleAdvancedButton );
			this.assetTabPage.Controls.Add( this.locationManagerComboBox );
			this.assetTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.assetTabPage.Name = "assetTabPage";
			this.assetTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.assetTabPage.Size = new System.Drawing.Size( 360, 266 );
			this.assetTabPage.TabIndex = 1;
			this.assetTabPage.Text = "Load";
			this.assetTabPage.UseVisualStyleBackColor = true;
			// 
			// loadLayoutContainer
			// 
			this.loadLayoutContainer.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.loadLayoutContainer.Controls.Add( this.locationManagerControlPanel );
			this.loadLayoutContainer.Controls.Add( this.loadLayoutSplitter );
			this.loadLayoutContainer.Controls.Add( this.loadParametersGrid );
			this.loadLayoutContainer.Location = new System.Drawing.Point( 6, 33 );
			this.loadLayoutContainer.Name = "loadLayoutContainer";
			this.loadLayoutContainer.Size = new System.Drawing.Size( 348, 198 );
			this.loadLayoutContainer.TabIndex = 3;
			// 
			// locationManagerControlPanel
			// 
			this.locationManagerControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locationManagerControlPanel.Location = new System.Drawing.Point( 0, 0 );
			this.locationManagerControlPanel.Name = "locationManagerControlPanel";
			this.locationManagerControlPanel.Size = new System.Drawing.Size( 193, 198 );
			this.locationManagerControlPanel.TabIndex = 0;
			// 
			// loadLayoutSplitter
			// 
			this.loadLayoutSplitter.Dock = System.Windows.Forms.DockStyle.Right;
			this.loadLayoutSplitter.Location = new System.Drawing.Point( 193, 0 );
			this.loadLayoutSplitter.Name = "loadLayoutSplitter";
			this.loadLayoutSplitter.Size = new System.Drawing.Size( 10, 198 );
			this.loadLayoutSplitter.TabIndex = 1;
			this.loadLayoutSplitter.TabStop = false;
			// 
			// loadParametersGrid
			// 
			this.loadParametersGrid.Dock = System.Windows.Forms.DockStyle.Right;
			this.loadParametersGrid.Location = new System.Drawing.Point( 203, 0 );
			this.loadParametersGrid.Name = "loadParametersGrid";
			this.loadParametersGrid.Size = new System.Drawing.Size( 145, 198 );
			this.loadParametersGrid.TabIndex = 2;
			this.loadParametersGrid.ToolbarVisible = false;
			// 
			// toggleAdvancedButton
			// 
			this.toggleAdvancedButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.toggleAdvancedButton.Location = new System.Drawing.Point( 270, 237 );
			this.toggleAdvancedButton.Name = "toggleAdvancedButton";
			this.toggleAdvancedButton.Size = new System.Drawing.Size( 84, 23 );
			this.toggleAdvancedButton.TabIndex = 2;
			this.toggleAdvancedButton.Text = "Advanced >>";
			this.toggleAdvancedButton.UseVisualStyleBackColor = true;
			this.toggleAdvancedButton.Click += new System.EventHandler( this.toggleAdvancedButton_Click );
			// 
			// locationManagerComboBox
			// 
			this.locationManagerComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.locationManagerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.locationManagerComboBox.FormattingEnabled = true;
			this.locationManagerComboBox.Location = new System.Drawing.Point( 6, 6 );
			this.locationManagerComboBox.Name = "locationManagerComboBox";
			this.locationManagerComboBox.Size = new System.Drawing.Size( 348, 21 );
			this.locationManagerComboBox.TabIndex = 0;
			this.locationManagerComboBox.SelectedIndexChanged += new System.EventHandler( this.locationManagerComboBox_SelectedIndexChanged );
			// 
			// okButton
			// 
			this.okButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point( 12, 310 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size( 75, 23 );
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler( this.okButton_Click );
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 305, 310 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// ObjectUITypeEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 392, 345 );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.okButton );
			this.Controls.Add( this.sourceTabs );
			this.Name = "ObjectUITypeEditorForm";
			this.Text = "New Object";
			this.sourceTabs.ResumeLayout( false );
			this.createObjectTabPage.ResumeLayout( false );
			this.assetTabPage.ResumeLayout( false );
			this.loadLayoutContainer.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TabControl sourceTabs;
		private System.Windows.Forms.TabPage createObjectTabPage;
		private System.Windows.Forms.TabPage assetTabPage;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ComboBox locationManagerComboBox;
		private NewObjectControl newObjectControl;
		private System.Windows.Forms.Button toggleAdvancedButton;
		private System.Windows.Forms.Panel loadLayoutContainer;
		private System.Windows.Forms.PropertyGrid loadParametersGrid;
		private System.Windows.Forms.Splitter loadLayoutSplitter;
		private System.Windows.Forms.Panel locationManagerControlPanel;
	}
}