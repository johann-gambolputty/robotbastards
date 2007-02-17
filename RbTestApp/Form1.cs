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
		private RbControls.Display display1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			string renderAssemblyName = System.Configuration.ConfigurationSettings.AppSettings[ "renderAssembly" ];
			if ( renderAssemblyName == null )
			{
				Output.WriteLineCall( Output.RenderingError, "The name of a render assembly was not provided in the application configuration file" );
			}
			else
			{
				RbEngine.Rendering.RenderFactory.Load( renderAssemblyName );
			}

			string resourceSetupPath = System.Configuration.ConfigurationSettings.AppSettings[ "resourceSetupPath" ];
			if ( resourceSetupPath != null )
			{
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument( );
				doc.Load( resourceSetupPath );

				RbEngine.Resources.ResourceManager.Inst.Setup( ( System.Xml.XmlElement )doc.SelectSingleNode( "/resourceManager" ) );
				RbEngine.Resources.ResourceManager.Inst.Load( "scene0.xml" );
			}
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			this.display1 = new RbControls.Display();
			this.SuspendLayout();
			// 
			// display1
			// 
			this.display1.ColourBits = ((System.Byte)(32));
			this.display1.DepthBits = ((System.Byte)(24));
			this.display1.Location = new System.Drawing.Point(8, 8);
			this.display1.Name = "display1";
			this.display1.Size = new System.Drawing.Size(280, 256);
			this.display1.StencilBits = ((System.Byte)(0));
			this.display1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(296, 269);
			this.Controls.Add(this.display1);
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
