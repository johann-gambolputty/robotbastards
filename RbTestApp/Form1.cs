using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using RbEngine;
using RbEngine.Resources;

namespace RbTestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//	Test to force initialisation of the typemanager
			//	TODO: REMOVEME
			RbEngine.Components.MessageTypeManager.Inst.ToString( );

			//	Load the rendering implementation assembly
			string renderAssemblyName = System.Configuration.ConfigurationSettings.AppSettings[ "renderAssembly" ];
			if ( renderAssemblyName == null )
			{
				Output.WriteLineCall( Output.RenderingError, "The name of a render assembly was not provided in the application configuration file" );
			}
			else
			{
				RbEngine.Rendering.RenderFactory.Load( renderAssemblyName );
			}

			//	Load the resource manager setup file
			string resourceSetupPath = System.Configuration.ConfigurationSettings.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath != null )
			{
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument( );
				doc.Load( resourceSetupPath );

				ResourceManager.Inst.Setup( ( System.Xml.XmlElement )doc.SelectSingleNode( "/resourceManager" ) );
			}

			//	Load in the test commands
			RbEngine.Interaction.CommandList testCommandList = RbEngine.Interaction.CommandListManager.CreateFromEnum( typeof( RbEngine.Entities.TestCommands ) );

			LoadParameters parameters = new LoadParameters( testCommandList );
			ResourceManager.Inst.Load( "testCommandInputs0.xml", parameters );

			//
			// Required for Windows Form Designer support
			//

			InitializeComponent();
		}

		private System.Windows.Forms.Button createClientButton;
		private System.Windows.Forms.Button createServerButton;
		private System.Windows.Forms.TextBox clientIpTextBox;
		private RbControls.OutputDisplay outputDisplay1;
		private System.Windows.Forms.Label label1;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.createClientButton = new System.Windows.Forms.Button();
			this.createServerButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.clientIpTextBox = new System.Windows.Forms.TextBox();
			this.outputDisplay1 = new RbControls.OutputDisplay();
			this.SuspendLayout();
			// 
			// createClientButton
			// 
			this.createClientButton.Location = new System.Drawing.Point(8, 8);
			this.createClientButton.Name = "createClientButton";
			this.createClientButton.Size = new System.Drawing.Size(104, 24);
			this.createClientButton.TabIndex = 0;
			this.createClientButton.Text = "Create client";
			this.createClientButton.Click += new System.EventHandler(this.createClientButton_Click);
			// 
			// createServerButton
			// 
			this.createServerButton.Location = new System.Drawing.Point(8, 40);
			this.createServerButton.Name = "createServerButton";
			this.createServerButton.Size = new System.Drawing.Size(104, 24);
			this.createServerButton.TabIndex = 1;
			this.createServerButton.Text = "Create server";
			this.createServerButton.Click += new System.EventHandler(this.createServerButton_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(120, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "IP";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// clientIpTextBox
			// 
			this.clientIpTextBox.Location = new System.Drawing.Point(136, 8);
			this.clientIpTextBox.Name = "clientIpTextBox";
			this.clientIpTextBox.Size = new System.Drawing.Size(184, 20);
			this.clientIpTextBox.TabIndex = 3;
			this.clientIpTextBox.Text = "localhost";
			// 
			// outputDisplay1
			// 
			this.outputDisplay1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.outputDisplay1.Location = new System.Drawing.Point(8, 72);
			this.outputDisplay1.Name = "outputDisplay1";
			this.outputDisplay1.Size = new System.Drawing.Size(312, 224);
			this.outputDisplay1.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(328, 301);
			this.Controls.Add(this.outputDisplay1);
			this.Controls.Add(this.clientIpTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.createServerButton);
			this.Controls.Add(this.createClientButton);
			this.Name = "Form1";
			this.Text = "RB";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Closed += new System.EventHandler(this.Form1_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler( ExceptionUtils.UnhandledThreadExceptionHandler );
			Application.Run( new Form1( ) );
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			/*
			SetupForm setupDlg = new SetupForm( );
			if ( setupDlg.ShowDialog( this ) == DialogResult.Cancel )
			{
				this.Close( );
				Output.WriteLine( TestAppOutput.Info, "User exit" );
				return;
			}

			//	Load the test server file
			string setupFile = setupDlg.SetupFile;
			m_Scene = ( RbEngine.Scene.SceneDb )ResourceManager.Inst.Load( setupFile );

			//	Set up server and client displays
			if ( m_Scene != null )
			{
				foreach( Control curControl in Controls )
				{
					RbControls.SceneDisplay sceneDisplay = curControl as RbControls.SceneDisplay;
					if ( sceneDisplay != null )
					{
						sceneDisplay.Scene = m_Scene;
					}
				}
			}
			*/

		//	m_ServerScene = ( RbEngine.Scene.SceneDb )ResourceManager.Inst.Load( "server0.xml" );
		//	m_ClientScene = ( RbEngine.Scene.SceneDb )ResourceManager.Inst.Load( "client0.xml" );

		//	serverDisplay.Scene = m_ServerScene;
		//	clientDisplay.Scene = m_ClientScene;
		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			//	Dispose of the scene
		//	if ( m_Scene != null )
		//	{
		//		m_Scene.Dispose( );
		//		m_Scene = null;
		//	}
			//	m_ServerScene.Dispose( );
			//	m_ServerScene = null;
			//	m_ClientScene.Dispose( );
			//	m_ClientScene = null;
		}

		private void createClientButton_Click(object sender, System.EventArgs e)
		{
			DisplayForm form = new DisplayForm( "client0.xml" );
			form.Show( );
		}

		private void createServerButton_Click(object sender, System.EventArgs e)
		{
			DisplayForm form = new DisplayForm( "server0.xml" );
			form.Show( );
		}
	}
}
