namespace Poc0.LevelEditor
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
			this.assetTabPage = new System.Windows.Forms.TabPage( );
			this.locationManagerControlPanel = new System.Windows.Forms.Panel( );
			this.locationManagerComboBox = new System.Windows.Forms.ComboBox( );
			this.okButton = new System.Windows.Forms.Button( );
			this.cancelButton = new System.Windows.Forms.Button( );
			this.newObjectControl = new Poc0.LevelEditor.NewObjectControl( );
			this.sourceTabs.SuspendLayout( );
			this.createObjectTabPage.SuspendLayout( );
			this.assetTabPage.SuspendLayout( );
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
			// assetTabPage
			// 
			this.assetTabPage.Controls.Add( this.locationManagerControlPanel );
			this.assetTabPage.Controls.Add( this.locationManagerComboBox );
			this.assetTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.assetTabPage.Name = "assetTabPage";
			this.assetTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.assetTabPage.Size = new System.Drawing.Size( 360, 266 );
			this.assetTabPage.TabIndex = 1;
			this.assetTabPage.Text = "Load";
			this.assetTabPage.UseVisualStyleBackColor = true;
			// 
			// locationManagerControlPanel
			// 
			this.locationManagerControlPanel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.locationManagerControlPanel.Location = new System.Drawing.Point( 6, 33 );
			this.locationManagerControlPanel.Name = "locationManagerControlPanel";
			this.locationManagerControlPanel.Size = new System.Drawing.Size( 345, 227 );
			this.locationManagerControlPanel.TabIndex = 1;
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
			// newObjectControl
			// 
			this.newObjectControl.BaseType = null;
			this.newObjectControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.newObjectControl.Location = new System.Drawing.Point( 3, 3 );
			this.newObjectControl.Name = "newObjectControl";
			this.newObjectControl.Size = new System.Drawing.Size( 354, 260 );
			this.newObjectControl.TabIndex = 0;
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
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TabControl sourceTabs;
		private System.Windows.Forms.TabPage createObjectTabPage;
		private System.Windows.Forms.TabPage assetTabPage;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ComboBox locationManagerComboBox;
		private System.Windows.Forms.Panel locationManagerControlPanel;
		private NewObjectControl newObjectControl;
	}
}