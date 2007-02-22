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
		private RbControls.ClientDisplay clientDisplay2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
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

			commands.BindToControl( clientDisplay1 );
			commands.BindToControl( clientDisplay2 );

			m_Commands = commands;

			Timer inputUpdateTimer = new Timer( );
			inputUpdateTimer.Tick += new EventHandler( InputUpdateTimerTick );
			inputUpdateTimer.Interval = ( int )( 100.0f / 15.0f );
			inputUpdateTimer.Enabled = true;
			inputUpdateTimer.Start( );

			//	Attach the server to the client display
			m_Server						= RbEngine.Network.ServerManager.Inst.FindServer( "server0" );
			clientDisplay1.Client.Server	= m_Server;
			clientDisplay2.Client.Server	= m_Server;
		}

		private RbEngine.Network.ServerBase			m_Server;
		private RbEngine.Interaction.CommandList	m_Commands;

		private void InputUpdateTimerTick( object sender, EventArgs e )
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
			this.clientDisplay2 = new RbControls.ClientDisplay();
			this.SuspendLayout();
			// 
			// clientDisplay1
			// 
			this.clientDisplay1.ColourBits = ((System.Byte)(32));
			this.clientDisplay1.DepthBits = ((System.Byte)(24));
			this.clientDisplay1.DockPadding.All = 1;
			this.clientDisplay1.Location = new System.Drawing.Point(8, 16);
			this.clientDisplay1.Name = "clientDisplay1";
			this.clientDisplay1.Size = new System.Drawing.Size(224, 192);
			this.clientDisplay1.StencilBits = ((System.Byte)(0));
			this.clientDisplay1.TabIndex = 0;
			// 
			// clientDisplay2
			// 
			this.clientDisplay2.ColourBits = ((System.Byte)(32));
			this.clientDisplay2.DepthBits = ((System.Byte)(24));
			this.clientDisplay2.Location = new System.Drawing.Point(240, 192);
			this.clientDisplay2.Name = "clientDisplay2";
			this.clientDisplay2.Size = new System.Drawing.Size(216, 184);
			this.clientDisplay2.StencilBits = ((System.Byte)(0));
			this.clientDisplay2.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 381);
			this.Controls.Add(this.clientDisplay2);
			this.Controls.Add(this.clientDisplay1);
			this.Name = "Form1";
			this.Text = "Form1";
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
}
