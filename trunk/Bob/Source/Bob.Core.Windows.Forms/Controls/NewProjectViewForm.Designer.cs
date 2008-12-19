namespace Bob.Core.Windows.Forms.Controls
{
	partial class NewProjectViewForm
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
			this.treeView1 = new System.Windows.Forms.TreeView( );
			this.okButton = new System.Windows.Forms.Button( );
			this.cancelButton = new System.Windows.Forms.Button( );
			this.label1 = new System.Windows.Forms.Label( );
			this.label2 = new System.Windows.Forms.Label( );
			this.projectNameTextBox = new System.Windows.Forms.TextBox( );
			this.SuspendLayout( );
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.treeView1.Location = new System.Drawing.Point( 12, 23 );
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size( 254, 129 );
			this.treeView1.TabIndex = 0;
			// 
			// okButton
			// 
			this.okButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point( 110, 181 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size( 75, 23 );
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 191, 181 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 12, 7 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 75, 13 );
			this.label1.TabIndex = 3;
			this.label1.Text = "Project Types:";
			// 
			// label2
			// 
			this.label2.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 9, 159 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 74, 13 );
			this.label2.TabIndex = 4;
			this.label2.Text = "Project Name:";
			// 
			// projectNameTextBox
			// 
			this.projectNameTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.projectNameTextBox.Location = new System.Drawing.Point( 92, 156 );
			this.projectNameTextBox.Name = "projectNameTextBox";
			this.projectNameTextBox.Size = new System.Drawing.Size( 174, 20 );
			this.projectNameTextBox.TabIndex = 5;
			// 
			// NewProjectViewForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 278, 216 );
			this.Controls.Add( this.projectNameTextBox );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( this.okButton );
			this.Controls.Add( this.treeView1 );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewProjectViewForm";
			this.Text = "New Project";
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox projectNameTextBox;
	}
}