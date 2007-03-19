using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
usifg System.Data;
using RbEngine;
using RbEngine.Resources;

namespace RbTestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private RbControls.SceneDisplay sceneDisplay1;
		private System.Windows.Forms.Splitter splitter1;
		private RbControls.SceneDisplay sceneDisplay2;
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

			//	Add the render factory to the builder
			RbEngine.Components.Builder.Main.AddFactory( RbEngine.Rendering.RenderFactory.Inst );

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

		private RbEngine.Network.ServerBase			m_Server;

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
			this.sceneDisplay1 = new RbControls.SceneDisplay();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.sceneDisplay2 = new RbControls.SceneDisplay();
			this.SuspendLayout();
			// 
			// sceneDisplay1
			// 
			this.sceneDisplay1.ColourBits = ((System.Byte)(32));
			this.sceneDisplay1.ContinuousRendering = true;
			this.sceneDisplay1.DepthBits = ((System.Byte)(24));
			this.sceneDisplay1.Dock = System.Windows.Forms.DockStyle.Left;
			this.sceneDisplay1.Location = new System.Drawing.Point(0, 0);
			this.sceneDisplay1.Name = "sceneDisplay1";
			this.sceneDisplay1.Scene = null;
			this.sceneDisplay1.SceneViewSetupFile = "sceneView0.xml";
			this.sceneDisplay1.Size = new System.Drawing.Size(192, 334);
			this.sceneDisplay1.StencilBits = ((System.Byte)(0));
			this.sceneDisplay1.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(192, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(8, 334);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// sceneDisplay2
			// 
			this.sceneDisplay2.ColourBits = ((System.Byte)(32));
			this.sceneDisplay2.ContinuousRendering = true;
			this.sceneDisplay2.DepthBits = ((System.Byte)(24));
			this.sceneDisplay2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sceneDisplay2.Location = new System.Drawing.Point(200, 0);
			this.sceneDisplay2.Name = "sceneDisplay2";
			this.sceneDisplay2.Scene = null;
			this.sceneDisplay2.SceneViewSetupFile = "sceneView0.xml";
			this.sceneDisplay2.Size = new System.Drawing.Size(264, 334);
			this.sceneDisplay2.StencilBits = ((System.Byte)(0));
			this.sceneDisplay2.TabIndex = 2;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 334);
			this.Controls.Add(this.sceneDisplay2);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.sceneDisplay1);
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
			//	Load the test server file
			string setupFile = "server0.xml";
			ResourceManager.Inst.Load( setupFile );

			//	Set up server and client displays
			m_Server = RbEngine.Network.ServerManager.Inst.FindServer( "server0" );
			if ( m_Server != null )
			{
				foreach( Control curControl in Controls )
				{
					RbControls.SceneDisplay sceneDisplay = curControl as RbControls.SceneDisplay;
					if ( sceneDisplay != null )
					{
						sceneDisplay.Scene = m_Server.Scene;
					}
				}
			}
		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			//	Dispose of the scene
			if ( ( m_Server != null ) && ( m_Server.Scene != null ) )
			{
				m_Server.Scene.Dispose( );
				m_Server.Scene = null;
				m_Server = null;
			}
		}
	}
}
