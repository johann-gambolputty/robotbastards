namespace Rb.ProfileViewerControls
{
	partial class ProfileViewer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.profileTabPage = new System.Windows.Forms.TabPage();
			this.profileGraph1 = new Rb.ProfileViewerControls.ProfileGraph();
			this.dataTabPage = new System.Windows.Forms.TabPage();
			this.profileSectionTreeView = new Rb.ProfileViewerControls.ProfileSectionTreeView();
			this.tabControl1.SuspendLayout();
			this.profileTabPage.SuspendLayout();
			this.dataTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.profileTabPage);
			this.tabControl1.Controls.Add(this.dataTabPage);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(406, 229);
			this.tabControl1.TabIndex = 0;
			// 
			// profileTabPage
			// 
			this.profileTabPage.Controls.Add(this.profileGraph1);
			this.profileTabPage.Location = new System.Drawing.Point(4, 22);
			this.profileTabPage.Name = "profileTabPage";
			this.profileTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.profileTabPage.Size = new System.Drawing.Size(398, 203);
			this.profileTabPage.TabIndex = 0;
			this.profileTabPage.Text = "Profile";
			this.profileTabPage.UseVisualStyleBackColor = true;
			// 
			// profileGraph1
			// 
			this.profileGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.profileGraph1.Location = new System.Drawing.Point(3, 3);
			this.profileGraph1.Name = "profileGraph1";
			this.profileGraph1.Sections = null;
			this.profileGraph1.Size = new System.Drawing.Size(392, 197);
			this.profileGraph1.TabIndex = 0;
			// 
			// dataTabPage
			// 
			this.dataTabPage.Controls.Add(this.profileSectionTreeView);
			this.dataTabPage.Location = new System.Drawing.Point(4, 22);
			this.dataTabPage.Name = "dataTabPage";
			this.dataTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.dataTabPage.Size = new System.Drawing.Size(398, 203);
			this.dataTabPage.TabIndex = 1;
			this.dataTabPage.Text = "Data";
			this.dataTabPage.UseVisualStyleBackColor = true;
			// 
			// profileSectionTreeView
			// 
			this.profileSectionTreeView.CheckBoxes = true;
			this.profileSectionTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.profileSectionTreeView.FullRowSelect = true;
			this.profileSectionTreeView.Location = new System.Drawing.Point(3, 3);
			this.profileSectionTreeView.Name = "profileSectionTreeView";
			this.profileSectionTreeView.ShowPlusMinus = false;
			this.profileSectionTreeView.ShowRootLines = false;
			this.profileSectionTreeView.Size = new System.Drawing.Size(392, 197);
			this.profileSectionTreeView.TabIndex = 0;
			// 
			// ProfileViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "ProfileViewer";
			this.Size = new System.Drawing.Size(406, 229);
			this.tabControl1.ResumeLayout(false);
			this.profileTabPage.ResumeLayout(false);
			this.dataTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage profileTabPage;
		private System.Windows.Forms.TabPage dataTabPage;
		private ProfileGraph profileGraph1;
		private ProfileSectionTreeView profileSectionTreeView;
	}
}
