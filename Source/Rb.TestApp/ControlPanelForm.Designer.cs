namespace Rb.TestApp
{
	partial class ControlPanelForm
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
			this.clientHostButton = new System.Windows.Forms.Button( );
			this.serverHostButton = new System.Windows.Forms.Button( );
			this.localHostButton = new System.Windows.Forms.Button( );
			this.serverIpCombo = new System.Windows.Forms.ComboBox( );
			this.label1 = new System.Windows.Forms.Label( );
			this.groupBox1 = new System.Windows.Forms.GroupBox( );
			this.editServerIpAddressButton = new System.Windows.Forms.Button( );
			this.groupBox2 = new System.Windows.Forms.GroupBox( );
			this.resourceProviderCombo = new System.Windows.Forms.ComboBox( );
			this.label4 = new System.Windows.Forms.Label( );
			this.browseInputFileButton = new System.Windows.Forms.Button( );
			this.browseSceneFileButton = new System.Windows.Forms.Button( );
			this.inputFileCombo = new System.Windows.Forms.ComboBox( );
			this.sceneFileCombo = new System.Windows.Forms.ComboBox( );
			this.label3 = new System.Windows.Forms.Label( );
			this.label2 = new System.Windows.Forms.Label( );
			this.groupBox1.SuspendLayout( );
			this.groupBox2.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// clientHostButton
			// 
			this.clientHostButton.Location = new System.Drawing.Point( 21, 19 );
			this.clientHostButton.Name = "clientHostButton";
			this.clientHostButton.Size = new System.Drawing.Size( 112, 23 );
			this.clientHostButton.TabIndex = 0;
			this.clientHostButton.Text = "New Client Host";
			this.clientHostButton.UseVisualStyleBackColor = true;
			this.clientHostButton.Click += new System.EventHandler( this.clientHostButton_Click );
			// 
			// serverHostButton
			// 
			this.serverHostButton.Location = new System.Drawing.Point( 21, 48 );
			this.serverHostButton.Name = "serverHostButton";
			this.serverHostButton.Size = new System.Drawing.Size( 112, 23 );
			this.serverHostButton.TabIndex = 1;
			this.serverHostButton.Text = "New Server Host";
			this.serverHostButton.UseVisualStyleBackColor = true;
			this.serverHostButton.Click += new System.EventHandler( this.serverHostButton_Click );
			// 
			// localHostButton
			// 
			this.localHostButton.Location = new System.Drawing.Point( 21, 76 );
			this.localHostButton.Name = "localHostButton";
			this.localHostButton.Size = new System.Drawing.Size( 112, 23 );
			this.localHostButton.TabIndex = 2;
			this.localHostButton.Text = "New Local Host";
			this.localHostButton.UseVisualStyleBackColor = true;
			this.localHostButton.Click += new System.EventHandler( this.localHostButton_Click );
			// 
			// serverIpCombo
			// 
			this.serverIpCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.serverIpCombo.FormattingEnabled = true;
			this.serverIpCombo.Location = new System.Drawing.Point( 193, 19 );
			this.serverIpCombo.Name = "serverIpCombo";
			this.serverIpCombo.Size = new System.Drawing.Size( 120, 21 );
			this.serverIpCombo.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point( 139, 16 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 48, 30 );
			this.label1.TabIndex = 4;
			this.label1.Text = "Server Address";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add( this.editServerIpAddressButton );
			this.groupBox1.Controls.Add( this.label1 );
			this.groupBox1.Controls.Add( this.serverIpCombo );
			this.groupBox1.Controls.Add( this.localHostButton );
			this.groupBox1.Controls.Add( this.serverHostButton );
			this.groupBox1.Controls.Add( this.clientHostButton );
			this.groupBox1.Location = new System.Drawing.Point( 9, 12 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 347, 116 );
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Host";
			// 
			// editServerIpAddressButton
			// 
			this.editServerIpAddressButton.Image = global::Rb.TestApp.Properties.Resources.EditInformation;
			this.editServerIpAddressButton.Location = new System.Drawing.Point( 317, 18 );
			this.editServerIpAddressButton.Name = "editServerIpAddressButton";
			this.editServerIpAddressButton.Size = new System.Drawing.Size( 24, 24 );
			this.editServerIpAddressButton.TabIndex = 5;
			this.editServerIpAddressButton.UseVisualStyleBackColor = true;
			this.editServerIpAddressButton.Click += new System.EventHandler( this.editServerIpAddressButton_Click );
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add( this.resourceProviderCombo );
			this.groupBox2.Controls.Add( this.label4 );
			this.groupBox2.Controls.Add( this.browseInputFileButton );
			this.groupBox2.Controls.Add( this.browseSceneFileButton );
			this.groupBox2.Controls.Add( this.inputFileCombo );
			this.groupBox2.Controls.Add( this.sceneFileCombo );
			this.groupBox2.Controls.Add( this.label3 );
			this.groupBox2.Controls.Add( this.label2 );
			this.groupBox2.Location = new System.Drawing.Point( 9, 134 );
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size( 347, 108 );
			this.groupBox2.TabIndex = 8;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Setup";
			// 
			// resourceProviderCombo
			// 
			this.resourceProviderCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.resourceProviderCombo.FormattingEnabled = true;
			this.resourceProviderCombo.Location = new System.Drawing.Point( 69, 19 );
			this.resourceProviderCombo.Name = "resourceProviderCombo";
			this.resourceProviderCombo.Size = new System.Drawing.Size( 244, 21 );
			this.resourceProviderCombo.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point( 9, 22 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 46, 13 );
			this.label4.TabIndex = 6;
			this.label4.Text = "Provider";
			// 
			// browseInputFileButton
			// 
			this.browseInputFileButton.Image = global::Rb.TestApp.Properties.Resources.OpenFolder;
			this.browseInputFileButton.Location = new System.Drawing.Point( 317, 74 );
			this.browseInputFileButton.Name = "browseInputFileButton";
			this.browseInputFileButton.Size = new System.Drawing.Size( 24, 23 );
			this.browseInputFileButton.TabIndex = 5;
			this.browseInputFileButton.UseVisualStyleBackColor = true;
			this.browseInputFileButton.Click += new System.EventHandler( this.browseInputFileButton_Click );
			// 
			// browseSceneFileButton
			// 
			this.browseSceneFileButton.Image = global::Rb.TestApp.Properties.Resources.OpenFolder;
			this.browseSceneFileButton.Location = new System.Drawing.Point( 317, 48 );
			this.browseSceneFileButton.Name = "browseSceneFileButton";
			this.browseSceneFileButton.Size = new System.Drawing.Size( 23, 23 );
			this.browseSceneFileButton.TabIndex = 4;
			this.browseSceneFileButton.UseVisualStyleBackColor = true;
			this.browseSceneFileButton.Click += new System.EventHandler( this.browseSceneFileButton_Click );
			// 
			// inputFileCombo
			// 
			this.inputFileCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inputFileCombo.FormattingEnabled = true;
			this.inputFileCombo.Location = new System.Drawing.Point( 69, 75 );
			this.inputFileCombo.Name = "inputFileCombo";
			this.inputFileCombo.Size = new System.Drawing.Size( 244, 21 );
			this.inputFileCombo.TabIndex = 3;
			// 
			// sceneFileCombo
			// 
			this.sceneFileCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.sceneFileCombo.FormattingEnabled = true;
			this.sceneFileCombo.Location = new System.Drawing.Point( 69, 48 );
			this.sceneFileCombo.Name = "sceneFileCombo";
			this.sceneFileCombo.Size = new System.Drawing.Size( 244, 21 );
			this.sceneFileCombo.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 16, 78 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 47, 13 );
			this.label3.TabIndex = 1;
			this.label3.Text = "Input file";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 9, 51 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 54, 13 );
			this.label2.TabIndex = 0;
			this.label2.Text = "Scene file";
			// 
			// ControlPanelForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 367, 254 );
			this.Controls.Add( this.groupBox2 );
			this.Controls.Add( this.groupBox1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "ControlPanelForm";
			this.Text = "Control Panel";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.ControlPanelForm_FormClosing );
			this.groupBox1.ResumeLayout( false );
			this.groupBox2.ResumeLayout( false );
			this.groupBox2.PerformLayout( );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Button clientHostButton;
		private System.Windows.Forms.Button serverHostButton;
		private System.Windows.Forms.Button localHostButton;
		private System.Windows.Forms.ComboBox serverIpCombo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox inputFileCombo;
		private System.Windows.Forms.ComboBox sceneFileCombo;
		private System.Windows.Forms.Button browseSceneFileButton;
		private System.Windows.Forms.Button browseInputFileButton;
		private System.Windows.Forms.Button editServerIpAddressButton;
		private System.Windows.Forms.ComboBox resourceProviderCombo;
		private System.Windows.Forms.Label label4;

	}
}