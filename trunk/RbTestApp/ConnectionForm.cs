using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RbTestApp
{
	/// <summary>
	/// Summary description for ConnectionForm.
	/// </summary>
	public class ConnectionForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.TextBox connectionTextBox;
		private System.Windows.Forms.RadioButton radioButtonServer;
		private System.Windows.Forms.RadioButton radioButtonClient;
		private System.Windows.Forms.Label labelConnectTo;
		private System.Windows.Forms.RadioButton radioButtonLocal;
		private System.Windows.Forms.Label labelInfo;

		public string	ConnectionString
		{
			get
			{
				if ( Client )
				{
					return connectionTextBox.Text;
				}

				System.Net.IPAddress[] addresses = System.Net.Dns.Resolve( System.Net.Dns.GetHostName( ) ).AddressList;
				string hostIp = addresses[ 0 ].ToString( );
				return hostIp;
			}
		}

		/// <summary>
		/// Returns true if the local server option was selected
		/// </summary>
		public bool		LocalServer
		{
			get
			{
				return radioButtonLocal.Checked;
			}
		}
		
		/// <summary>
		/// Returns true if the client option was selected
		/// </summary>
		public bool		Client
		{
			get
			{
				return radioButtonClient.Checked;
			}
		}

		/// <summary>
		/// Returns true if the server option was selected
		/// </summary>
		public bool		Server
		{
			get
			{
				return radioButtonServer.Checked;
			}
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ConnectionForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			radioButtonServer.Checked = true;

			//	Add the local IP address to the connection string text box
			System.Net.IPAddress[] addresses = System.Net.Dns.Resolve( System.Net.Dns.GetHostName( ) ).AddressList;
			string hostIp = addresses[ 0 ].ToString( );
			connectionTextBox.Text = hostIp;
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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.labelConnectTo = new System.Windows.Forms.Label();
			this.connectionTextBox = new System.Windows.Forms.TextBox();
			this.radioButtonServer = new System.Windows.Forms.RadioButton();
			this.radioButtonClient = new System.Windows.Forms.RadioButton();
			this.labelInfo = new System.Windows.Forms.Label();
			this.radioButtonLocal = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(152, 144);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 11;
			this.buttonCancel.Text = "Cancel";
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Location = new System.Drawing.Point(8, 144);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 10;
			this.buttonOk.Text = "OK";
			// 
			// labelConnectTo
			// 
			this.labelConnectTo.Enabled = false;
			this.labelConnectTo.Location = new System.Drawing.Point(8, 112);
			this.labelConnectTo.Name = "labelConnectTo";
			this.labelConnectTo.Size = new System.Drawing.Size(64, 16);
			this.labelConnectTo.TabIndex = 9;
			this.labelConnectTo.Text = "Connect to:";
			// 
			// connectionTextBox
			// 
			this.connectionTextBox.Enabled = false;
			this.connectionTextBox.Location = new System.Drawing.Point(80, 112);
			this.connectionTextBox.Name = "connectionTextBox";
			this.connectionTextBox.Size = new System.Drawing.Size(152, 20);
			this.connectionTextBox.TabIndex = 8;
			this.connectionTextBox.Text = "localhost";
			// 
			// radioButtonServer
			// 
			this.radioButtonServer.Location = new System.Drawing.Point(8, 80);
			this.radioButtonServer.Name = "radioButtonServer";
			this.radioButtonServer.Size = new System.Drawing.Size(56, 16);
			this.radioButtonServer.TabIndex = 7;
			this.radioButtonServer.Text = "Server";
			this.radioButtonServer.CheckedChanged += new System.EventHandler(this.radioButtonServer_CheckedChanged);
			// 
			// radioButtonClient
			// 
			this.radioButtonClient.Location = new System.Drawing.Point(8, 48);
			this.radioButtonClient.Name = "radioButtonClient";
			this.radioButtonClient.Size = new System.Drawing.Size(56, 16);
			this.radioButtonClient.TabIndex = 6;
			this.radioButtonClient.Text = "Client";
			this.radioButtonClient.CheckedChanged += new System.EventHandler(this.radioButtonClient_CheckedChanged);
			// 
			// labelInfo
			// 
			this.labelInfo.Location = new System.Drawing.Point(80, 8);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(152, 88);
			this.labelInfo.TabIndex = 12;
			this.labelInfo.Text = "Choose client or server setup";
			// 
			// radioButtonLocal
			// 
			this.radioButtonLocal.Location = new System.Drawing.Point(8, 16);
			this.radioButtonLocal.Name = "radioButtonLocal";
			this.radioButtonLocal.Size = new System.Drawing.Size(56, 16);
			this.radioButtonLocal.TabIndex = 13;
			this.radioButtonLocal.Text = "Local";
			this.radioButtonLocal.CheckedChanged += new System.EventHandler(this.radioButtonLocal_CheckedChanged);
			// 
			// ConnectionForm
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(240, 172);
			this.Controls.Add(this.radioButtonLocal);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.labelConnectTo);
			this.Controls.Add(this.connectionTextBox);
			this.Controls.Add(this.radioButtonServer);
			this.Controls.Add(this.radioButtonClient);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Name = "ConnectionForm";
			this.Text = "ConnectionForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void radioButtonServer_CheckedChanged(object sender, System.EventArgs e)
		{
			connectionTextBox.Enabled = false;
			labelConnectTo.Enabled = false;
			labelInfo.Text = "Runs a server, to which clients can connect";
		}

		private void radioButtonClient_CheckedChanged(object sender, System.EventArgs e)
		{
			connectionTextBox.Enabled = true;
			labelConnectTo.Enabled = true;
			labelInfo.Text = "Runs a client - specify the server to connect to in the \"Connect To:\" box";
		}

		private void radioButtonLocal_CheckedChanged(object sender, System.EventArgs e)
		{
			connectionTextBox.Enabled = false;
			labelConnectTo.Enabled = false;
			labelInfo.Text = "Run local client-server setup";
		}
	}
}
