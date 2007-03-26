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
		private RbControls.SceneDisplay sceneDisplay1;
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

		private RbEngine.Scene.SceneDb	m_Scene;

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
			this.SuspendLayout();
			// 
			// sceneDisplay1
			// 
			this.sceneDisplay1.ColourBits = ((System.Byte)(32));
			this.sceneDisplay1.ContinuousRendering = true;
			this.sceneDisplay1.DepthBits = ((System.Byte)(24));
			this.sceneDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sceneDisplay1.Location = new System.Drawing.Point(0, 0);
			this.sceneDisplay1.Name = "sceneDisplay1";
			this.sceneDisplay1.Scene = null;
			this.sceneDisplay1.SceneViewSetupFile = "sceneView0.xml";
			this.sceneDisplay1.Size = new System.Drawing.Size(400, 357);
			this.sceneDisplay1.StencilBits = ((System.Byte)(0));
			this.sceneDisplay1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 357);
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
		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			//	Dispose of the scene
			if ( m_Scene != null )
			{
				m_Scene.Dispose( );
				m_Scene = null;
			}
		}
	}
}
