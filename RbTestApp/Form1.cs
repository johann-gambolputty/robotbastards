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
			RbParticleSystem.ParticleSystem testSystem = new RbParticleSystem.ParticleSystem( );

			testSystem.AddUpdater( new RbParticleSystem.ParticleSystemUpdater( ) );

			//	Test to force initialisation of the typemanager
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
			LoadParameters parameters = new LoadParameters( RbEngine.Entities.TestUserEntityController.Commands );
			ResourceManager.Inst.Load( "testCommandInputs0.xml", parameters );

			//
			// Required for Windows Form Designer support
			//

			InitializeComponent();
		}

		private RbControls.SceneDisplay serverDisplay;
		private RbControls.SceneDisplay clientDisplay;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;

		private RbEngine.Scene.SceneDb	m_ServerScene;
		private RbEngine.Scene.SceneDb	m_ClientScene;

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
			this.serverDisplay = new RbControls.SceneDisplay();
			this.clientDisplay = new RbControls.SceneDisplay();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// serverDisplay
			// 
			this.serverDisplay.ColourBits = ((System.Byte)(32));
			this.serverDisplay.ContinuousRendering = true;
			this.serverDisplay.DepthBits = ((System.Byte)(24));
			this.serverDisplay.Location = new System.Drawing.Point(8, 8);
			this.serverDisplay.Name = "serverDisplay";
			this.serverDisplay.Scene = null;
			this.serverDisplay.SceneViewSetupFile = "sceneView0.xml";
			this.serverDisplay.Size = new System.Drawing.Size(224, 248);
			this.serverDisplay.StencilBits = ((System.Byte)(0));
			this.serverDisplay.TabIndex = 0;
			// 
			// clientDisplay
			// 
			this.clientDisplay.ColourBits = ((System.Byte)(32));
			this.clientDisplay.ContinuousRendering = true;
			this.clientDisplay.DepthBits = ((System.Byte)(24));
			this.clientDisplay.Location = new System.Drawing.Point(240, 8);
			this.clientDisplay.Name = "clientDisplay";
			this.clientDisplay.Scene = null;
			this.clientDisplay.SceneViewSetupFile = "sceneView0.xml";
			this.clientDisplay.Size = new System.Drawing.Size(224, 248);
			this.clientDisplay.StencilBits = ((System.Byte)(0));
			this.clientDisplay.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 264);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Server";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(240, 264);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Client";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 294);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.clientDisplay);
			this.Controls.Add(this.serverDisplay);
			this.Name = "Form1";
			this.Text = "Form1";
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
			
			m_ServerScene = ( RbEngine.Scene.SceneDb )ResourceManager.Inst.Load( "server0.xml" );
			m_ClientScene = ( RbEngine.Scene.SceneDb )ResourceManager.Inst.Load( "client0.xml" );

			serverDisplay.Scene = m_ServerScene;
			clientDisplay.Scene = m_ClientScene;


		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			//	Dispose of the scene
		//	if ( m_Scene != null )
		//	{
		//		m_Scene.Dispose( );
		//		m_Scene = null;
		//	}
			m_ServerScene.Dispose( );
			m_ServerScene = null;
			m_ClientScene.Dispose( );
			m_ClientScene = null;
		}
	}
}
