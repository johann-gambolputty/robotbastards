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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ObjectUITypeEditorForm ) );
			this.tabControl1 = new System.Windows.Forms.TabControl( );
			this.tabPage1 = new System.Windows.Forms.TabPage( );
			this.tabPage2 = new System.Windows.Forms.TabPage( );
			this.okButton = new System.Windows.Forms.Button( );
			this.cancelButton = new System.Windows.Forms.Button( );
			this.locationManagerCombo = new System.Windows.Forms.ComboBox( );
			this.locationManagerControlPanel = new System.Windows.Forms.Panel( );
			this.tabControl1.SuspendLayout( );
			this.tabPage1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.tabControl1.Controls.Add( this.tabPage1 );
			this.tabControl1.Controls.Add( this.tabPage2 );
			this.tabControl1.Location = new System.Drawing.Point( 0, 2 );
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size( 293, 246 );
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add( this.locationManagerControlPanel );
			this.tabPage1.Controls.Add( this.locationManagerCombo );
			this.tabPage1.Location = new System.Drawing.Point( 4, 22 );
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage1.Size = new System.Drawing.Size( 285, 220 );
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Asset";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point( 4, 22 );
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding( 3 );
			this.tabPage2.Size = new System.Drawing.Size( 285, 220 );
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Instance";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point( 4, 254 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size( 75, 23 );
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 214, 254 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// locationManagerCombo
			// 
			this.locationManagerCombo.FormattingEnabled = true;
			this.locationManagerCombo.Location = new System.Drawing.Point( 3, 196 );
			this.locationManagerCombo.Name = "locationManagerCombo";
			this.locationManagerCombo.Size = new System.Drawing.Size( 279, 21 );
			this.locationManagerCombo.TabIndex = 0;
			// 
			// locationManagerControlPanel
			// 
			this.locationManagerControlPanel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.locationManagerControlPanel.Location = new System.Drawing.Point( 4, 6 );
			this.locationManagerControlPanel.Name = "locationManagerControlPanel";
			this.locationManagerControlPanel.Size = new System.Drawing.Size( 277, 184 );
			this.locationManagerControlPanel.TabIndex = 1;
			// 
			// ObjectUITypeEditorForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 292, 289 );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.okButton );
			this.Controls.Add( this.tabControl1 );
			this.Icon = ( ( System.Drawing.Icon )( resources.GetObject( "$this.Icon" ) ) );
			this.Name = "ObjectUITypeEditorForm";
			this.ShowInTaskbar = false;
			this.Text = "Object Source";
			this.tabControl1.ResumeLayout( false );
			this.tabPage1.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ComboBox locationManagerCombo;
		private System.Windows.Forms.Panel locationManagerControlPanel;
	}
}