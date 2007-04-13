using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using RbEngine;
using RbEngine.Resources;

namespace RbTestApp
{
	/// <summary>
	/// Summary description for DisplayForm.
	/// </summary>
	public class DisplayForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private RbControls.SceneDisplay sceneDisplay;

		private RbEngine.Scene.SceneDb	m_Scene;
		private string					m_SceneSetupFilename;

		public DisplayForm( string sceneSetupFilename )
		{
			m_SceneSetupFilename = sceneSetupFilename;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Text = sceneSetupFilename;
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
			this.sceneDisplay = new RbControls.SceneDisplay();
			this.SuspendLayout();
			// 
			// sceneDisplay
			// 
			this.sceneDisplay.ColourBits = ((System.Byte)(32));
			this.sceneDisplay.ContinuousRendering = true;
			this.sceneDisplay.DepthBits = ((System.Byte)(24));
			this.sceneDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sceneDisplay.Location = new System.Drawing.Point(0, 0);
			this.sceneDisplay.Name = "sceneDisplay";
			this.sceneDisplay.Scene = null;
			this.sceneDisplay.SceneViewSetupFile = "sceneView0.xml";
			this.sceneDisplay.Size = new System.Drawing.Size(104, 85);
			this.sceneDisplay.StencilBits = ((System.Byte)(0));
			this.sceneDisplay.TabIndex = 0;
			// 
			// DisplayForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(104, 85);
			this.Controls.Add(this.sceneDisplay);
			this.Name = "DisplayForm";
			this.Text = "DisplayForm";
			this.Load += new System.EventHandler(this.DisplayForm_Load);
			this.Closed += new System.EventHandler(this.DisplayForm_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		private void DisplayForm_Closed(object sender, System.EventArgs e)
		{
			if ( m_Scene != null )
			{
				m_Scene.Dispose( );
				m_Scene = null;
			}
		}

		private void DisplayForm_Load( object sender, System.EventArgs e )
		{
			m_Scene = ( RbEngine.Scene.SceneDb )ResourceManager.Inst.Load( m_SceneSetupFilename );
			sceneDisplay.Scene = m_Scene;
		}
	}
}
