namespace Poc1.Bob.Controls.Templates
{
	partial class CreateTemplateInstanceForm
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
			this.templateSelectorView1 = new Poc1.Bob.Controls.Templates.TemplateSelectorView( );
			this.panel1 = new System.Windows.Forms.Panel( );
			this.cancelButton = new System.Windows.Forms.Button( );
			this.createButton = new System.Windows.Forms.Button( );
			this.instanceNameTextBox = new System.Windows.Forms.TextBox( );
			this.nameLabel = new System.Windows.Forms.Label( );
			this.panel1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// templateSelectorView1
			// 
			this.templateSelectorView1.Description = "Description";
			this.templateSelectorView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.templateSelectorView1.Location = new System.Drawing.Point( 0, 0 );
			this.templateSelectorView1.Name = "templateSelectorView1";
			this.templateSelectorView1.RootGroup = null;
			this.templateSelectorView1.Size = new System.Drawing.Size( 293, 201 );
			this.templateSelectorView1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.cancelButton );
			this.panel1.Controls.Add( this.createButton );
			this.panel1.Controls.Add( this.instanceNameTextBox );
			this.panel1.Controls.Add( this.nameLabel );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point( 0, 201 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 293, 78 );
			this.panel1.TabIndex = 1;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 215, 43 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// createButton
			// 
			this.createButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.createButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.createButton.Location = new System.Drawing.Point( 134, 43 );
			this.createButton.Name = "createButton";
			this.createButton.Size = new System.Drawing.Size( 75, 23 );
			this.createButton.TabIndex = 4;
			this.createButton.Text = "Create";
			this.createButton.UseVisualStyleBackColor = true;
			// 
			// instanceNameTextBox
			// 
			this.instanceNameTextBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.instanceNameTextBox.Location = new System.Drawing.Point( 56, 7 );
			this.instanceNameTextBox.Name = "instanceNameTextBox";
			this.instanceNameTextBox.Size = new System.Drawing.Size( 234, 20 );
			this.instanceNameTextBox.TabIndex = 3;
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point( 12, 10 );
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size( 38, 13 );
			this.nameLabel.TabIndex = 2;
			this.nameLabel.Text = "Name:";
			// 
			// CreateTemplateInstanceForm
			// 
			this.AcceptButton = this.createButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 293, 279 );
			this.Controls.Add( this.templateSelectorView1 );
			this.Controls.Add( this.panel1 );
			this.Name = "CreateTemplateInstanceForm";
			this.Text = "Create Instance...";
			this.panel1.ResumeLayout( false );
			this.panel1.PerformLayout( );
			this.ResumeLayout( false );

		}

		#endregion

		private TemplateSelectorView templateSelectorView1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.TextBox instanceNameTextBox;
		private System.Windows.Forms.Button createButton;
		private System.Windows.Forms.Button cancelButton;
	}
}