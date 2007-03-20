using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RbTestApp
{
	public enum SetupType
	{
		Client,
		Server,
		LocalServer
	}

	/// <summary>
	/// Summary description for SetupForm.
	/// </summary>
	public class SetupForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.RadioButton clientRadioButton;
		private System.Windows.Forms.RadioButton serverRadioButton;
		private System.Windows.Forms.RadioButton localServerRadioButton;
		private System.Windows.Forms.Label networkDescriptionLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox setupFileTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox connectionTextBox;

		/// <summary>
		/// Setup type
		/// </summary>
		public SetupType Setup
		{
			get
			{
				return m_Setup;
			}
		}

		/// <summary>
		/// Setup file path
		/// </summary>
		public string SetupFile
		{
			get
			{
				return setupFileTextBox.Text;
			}
		}

		private SetupType m_Setup = SetupType.Client;

		public SetupForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.connectionTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.setupFileTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.networkDescriptionLabel = new System.Windows.Forms.Label();
			this.localServerRadioButton = new System.Windows.Forms.RadioButton();
			this.serverRadioButton = new System.Windows.Forms.RadioButton();
			this.clientRadioButton = new System.Windows.Forms.RadioButton();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.connectionTextBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.setupFileTextBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.networkDescriptionLabel);
			this.groupBox1.Controls.Add(this.localServerRadioButton);
			this.groupBox1.Controls.Add(this.serverRadioButton);
			this.groupBox1.Controls.Add(this.clientRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 176);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Network";
			// 
			// connectionTextBox
			// 
			this.connectionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.connectionTextBox.Location = new System.Drawing.Point(72, 136);
			this.connectionTextBox.Name = "connectionTextBox";
			this.connectionTextBox.Size = new System.Drawing.Size(120, 20);
			this.connectionTextBox.TabIndex = 10;
			this.connectionTextBox.Text = "localhost";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 16);
			this.label2.TabIndex = 9;
			this.label2.Text = "Connection";
			// 
			// setupFileTextBox
			// 
			this.setupFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.setupFileTextBox.Location = new System.Drawing.Point(72, 104);
			this.setupFileTextBox.Name = "setupFileTextBox";
			this.setupFileTextBox.Size = new System.Drawing.Size(120, 20);
			this.setupFileTextBox.TabIndex = 6;
			this.setupFileTextBox.Text = "client0.xml";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 104);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Setup file";
			// 
			// networkDescriptionLabel
			// 
			this.networkDescriptionLabel.Location = new System.Drawing.Point(96, 16);
			this.networkDescriptionLabel.Name = "networkDescriptionLabel";
			this.networkDescriptionLabel.Size = new System.Drawing.Size(96, 80);
			this.networkDescriptionLabel.TabIndex = 3;
			// 
			// localServerRadioButton
			// 
			this.localServerRadioButton.Location = new System.Drawing.Point(8, 64);
			this.localServerRadioButton.Name = "localServerRadioButton";
			this.localServerRadioButton.Size = new System.Drawing.Size(88, 24);
			this.localServerRadioButton.TabIndex = 2;
			this.localServerRadioButton.Text = "Local Server";
			this.localServerRadioButton.CheckedChanged += new System.EventHandler(this.localServerRadioButton_CheckedChanged);
			// 
			// serverRadioButton
			// 
			this.serverRadioButton.Location = new System.Drawing.Point(8, 40);
			this.serverRadioButton.Name = "serverRadioButton";
			this.serverRadioButton.Size = new System.Drawing.Size(80, 24);
			this.serverRadioButton.TabIndex = 1;
			this.serverRadioButton.Text = "Server";
			this.serverRadioButton.CheckedChanged += new System.EventHandler(this.serverRadioButton_CheckedChanged);
			// 
			// clientRadioButton
			// 
			this.clientRadioButton.Checked = true;
			this.clientRadioButton.Location = new System.Drawing.Point(8, 16);
			this.clientRadioButton.Name = "clientRadioButton";
			this.clientRadioButton.Size = new System.Drawing.Size(72, 24);
			this.clientRadioButton.TabIndex = 0;
			this.clientRadioButton.TabStop = true;
			this.clientRadioButton.Text = "Client";
			this.clientRadioButton.CheckedChanged += new System.EventHandler(this.clientRadioButton_CheckedChanged);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(8, 192);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Go!";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(136, 192);
			this.button2.Name = "button2";
			this.button2.TabIndex = 2;
			this.button2.Text = "E&xit";
			// 
			// SetupForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(216, 221);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox1);
			this.Name = "SetupForm";
			this.Text = "RB Setup";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void clientRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			m_Setup = SetupType.Client;
			setupFileTextBox.Text = "client0.xml";
			networkDescriptionLabel.Text = "Client";
			connectionTextBox.Enabled = true;
		}

		private void serverRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			m_Setup = SetupType.Server;
			setupFileTextBox.Text = "server0.xml";
			networkDescriptionLabel.Text = "Server";
			connectionTextBox.Enabled = true;
		}

		private void localServerRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			m_Setup = SetupType.LocalServer;
			setupFileTextBox.Text = "server0.xml";
			networkDescriptionLabel.Text = "Local server (just a regular server for now)";
			connectionTextBox.Enabled = true;
		}
	}
}
