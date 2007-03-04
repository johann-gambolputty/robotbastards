using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using RbEngine;

namespace RbTestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private RbControls.ClientDisplay clientDisplay1;
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

				RbEngine.Resources.ResourceManager.Inst.Setup( ( System.Xml.XmlElement )doc.SelectSingleNode( "/resourceManager" ) );
			}

			//
			// Required for Windows Form Designer support
			//
			
			InitializeComponent();
		}

		private RbEngine.Network.ServerBase			m_Server;
		private RbEngine.Interaction.CommandList	m_Commands;

		private void InputUpdateTimerTick( RbEngine.Scene.Clock clock )
		{
			//	TODO: Should the server be the command target?
			m_Commands.Update( m_Server );
		}

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
			this.clientDisplay1 = new RbControls.ClientDisplay();
			this.SuspendLayout();
			// 
			// clientDisplay1
			// 
			this.clientDisplay1.ColourBits = ((System.Byte)(32));
			this.clientDisplay1.ContinuousRendering = true;
			this.clientDisplay1.DepthBits = ((System.Byte)(24));
			this.clientDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clientDisplay1.Location = new System.Drawing.Point(0, 0);
			this.clientDisplay1.Name = "clientDisplay1";
			this.clientDisplay1.Size = new System.Drawing.Size(104, 85);
			this.clientDisplay1.StencilBits = ((System.Byte)(0));
			this.clientDisplay1.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(104, 85);
			this.Controls.Add(this.clientDisplay1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			//	TODO: Should use unhandled exception handler instead
			bool catchUnhandledExceptions = false;
			if ( !catchUnhandledExceptions )
			{
				Application.Run( new Form1( ) );
			}
			else
			{
				try
				{
					Application.Run( new Form1( ) );
				}
				catch ( Exception exception )
				{
					string exceptionString = ExceptionUtils.ToString( exception );

					Output.WriteLine( TestAppOutput.Error, exceptionString );
					MessageBox.Show( exceptionString );
				}
			}
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			ConnectionForm connectionDlg = new ConnectionForm( );
			if ( connectionDlg.ShowDialog( this ) == DialogResult.Cancel )
			{
				Close( );
				return;
			}

			//	Load the test server file
			RbEngine.Resources.ResourceManager.Inst.Load( "server0.xml" );

			RbEngine.Interaction.CommandList commands = new RbEngine.Interaction.CommandList( );

			RbEngine.Interaction.Command cmd = new RbEngine.Interaction.Command( "forward", "moves forward", ( int )RbEngine.Entities.TestCommands.Forward );
			cmd.AddBinding( new RbEngine.Interaction.CommandKeyInputBinding( Keys.W ) );
			commands.AddCommand( cmd );

			cmd = new RbEngine.Interaction.Command( "back", "moves backwards", ( int )RbEngine.Entities.TestCommands.Back );
			cmd.AddBinding( new RbEngine.Interaction.CommandKeyInputBinding( Keys.S ) );
			commands.AddCommand( cmd );

			cmd = new RbEngine.Interaction.Command( "left", "moves left", ( int )RbEngine.Entities.TestCommands.Left );
			cmd.AddBinding( new RbEngine.Interaction.CommandKeyInputBinding( Keys.A ) );
			commands.AddCommand( cmd );

			cmd = new RbEngine.Interaction.Command( "right", "moves right", ( int )RbEngine.Entities.TestCommands.Right );
			cmd.AddBinding( new RbEngine.Interaction.CommandKeyInputBinding( Keys.D ) );
			commands.AddCommand( cmd );

			cmd = new RbEngine.Interaction.Command( "jump", "Jumps", ( int )RbEngine.Entities.TestCommands.Jump );
			cmd.AddBinding( new RbEngine.Interaction.CommandKeyInputBinding( Keys.Space ) );
			commands.AddCommand( cmd );

			cmd = new RbEngine.Entities.TestLookAtCommand( );
			cmd.AddBinding( new RbEngine.Interaction.CommandMouseMoveInputBinding( ) );
			commands.AddCommand( cmd );

			m_Commands = commands;

			//	Set up server and client displays
			m_Server = RbEngine.Network.ServerManager.Inst.FindServer( "server0" );
			foreach( Control curControl in Controls )
			{
				RbControls.ClientDisplay clientDisplay = curControl as RbControls.ClientDisplay;
				if ( clientDisplay != null )
				{
					commands.BindToClient( clientDisplay.Client );
					clientDisplay.Client.Server = m_Server;
				}
			}

			if ( m_Server.Scene != null )
			{
				m_Server.Scene.GetNamedClock( "updateClock" ).Subscribe( new RbEngine.Scene.Clock.TickDelegate( InputUpdateTimerTick ) );
			}
		
		}

	}
}
