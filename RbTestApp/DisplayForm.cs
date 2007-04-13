using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RbTestApp
{
	/// <summary>
	/// Summary description for DisplayForm.
	/// </summary>
	public class DisplayForm : System.Windows.Forms.Form
	{
		private RbControls.SceneDisplay sceneDisplay1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private RbEngine.Scene.SceneDb	m_Scene;

		public DisplayForm( string setupFilename )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_Scene = ( RbEngine.Scene.SceneDb )ResourceManager.Inst.Load( "server0.xml" );
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
			this.sceneDisplay1.Size = new System.Drawing.Size(292, 266);
			this.sceneDisplay1.StencilBits = ((System.Byte)(0));
			this.sceneDisplay1.TabIndex = 0;
			// 
			// DisplayForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.sceneDisplay1);
			this.Name = "DisplayForm";
			this.Text = "DisplayForm";
			this.Load += new System.EventHandler(this.DisplayForm_Load);
			this.Closed += new System.EventHandler(this.DisplayForm_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		private void DisplayForm_Closed(object sender, System.EventArgs e)
		{
			m_Scene.Dispose( );
			m_Scene = null;
		}

		private void DisplayForm_Load( object sender, System.EventArgs e )
		{
		}
	}
}
