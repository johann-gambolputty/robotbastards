using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace RbControls
{
	/// <summary>
	/// Summary description for OutputDisplay.
	/// </summary>
	public class OutputDisplay : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TabPage messageSourcesTab;
		private System.Windows.Forms.ListBox sourcesList;
		private System.Windows.Forms.Button addSourceTabButton;
		private TabControlEx outputTabs;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OutputDisplay()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.outputTabs = new RbControls.TabControlEx();
			this.messageSourcesTab = new System.Windows.Forms.TabPage();
			this.addSourceTabButton = new System.Windows.Forms.Button();
			this.sourcesList = new System.Windows.Forms.ListBox();
			this.outputTabs.SuspendLayout();
			this.messageSourcesTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// outputTabs
			// 
			this.outputTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.outputTabs.Controls.Add(this.messageSourcesTab);
			this.outputTabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.outputTabs.Location = new System.Drawing.Point(8, 8);
			this.outputTabs.Name = "outputTabs";
			this.outputTabs.Padding = new System.Drawing.Point(10, 3);
			this.outputTabs.SelectedIndex = 0;
			this.outputTabs.Size = new System.Drawing.Size(264, 216);
			this.outputTabs.TabIndex = 0;
			// 
			// messageSourcesTab
			// 
			this.messageSourcesTab.Controls.Add(this.addSourceTabButton);
			this.messageSourcesTab.Controls.Add(this.sourcesList);
			this.messageSourcesTab.Location = new System.Drawing.Point(4, 22);
			this.messageSourcesTab.Name = "messageSourcesTab";
			this.messageSourcesTab.Size = new System.Drawing.Size(256, 190);
			this.messageSourcesTab.TabIndex = 0;
			this.messageSourcesTab.Text = "Sources";
			// 
			// addSourceTabButton
			// 
			this.addSourceTabButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addSourceTabButton.Location = new System.Drawing.Point(8, 160);
			this.addSourceTabButton.Name = "addSourceTabButton";
			this.addSourceTabButton.Size = new System.Drawing.Size(75, 24);
			this.addSourceTabButton.TabIndex = 1;
			this.addSourceTabButton.Text = "Add Tab";
			this.addSourceTabButton.Click += new System.EventHandler(this.addSourceTabButton_Click);
			// 
			// sourcesList
			// 
			this.sourcesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.sourcesList.Location = new System.Drawing.Point(8, 8);
			this.sourcesList.Name = "sourcesList";
			this.sourcesList.Size = new System.Drawing.Size(240, 134);
			this.sourcesList.TabIndex = 0;
			// 
			// OutputDisplay
			// 
			this.Controls.Add(this.outputTabs);
			this.Name = "OutputDisplay";
			this.Size = new System.Drawing.Size(280, 232);
			this.outputTabs.ResumeLayout(false);
			this.messageSourcesTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void addSourceTabButton_Click(object sender, System.EventArgs e)
		{
			TabPage newTabPage = new TabPage( );
			newTabPage.Text = "test";
			outputTabs.Controls.Add( newTabPage );
		}
	}
}
