namespace Poc0.LevelEditor
{
	partial class EditorControls
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
			this.tilesTabPage = new System.Windows.Forms.TabPage();
			this.tileTypeSetView = new Poc0.LevelEditor.TileTypeSetListView();
			this.objectsTabPage = new System.Windows.Forms.TabPage();
			this.objectsTreeView = new System.Windows.Forms.TreeView();
			this.tabControl1.SuspendLayout();
			this.tilesTabPage.SuspendLayout();
			this.objectsTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tilesTabPage);
			this.tabControl1.Controls.Add(this.objectsTabPage);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(189, 166);
			this.tabControl1.TabIndex = 0;
			// 
			// tilesTabPage
			// 
			this.tilesTabPage.Controls.Add(this.tileTypeSetView);
			this.tilesTabPage.Location = new System.Drawing.Point(4, 22);
			this.tilesTabPage.Name = "tilesTabPage";
			this.tilesTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.tilesTabPage.Size = new System.Drawing.Size(181, 140);
			this.tilesTabPage.TabIndex = 0;
			this.tilesTabPage.Text = "Tiles";
			this.tilesTabPage.UseVisualStyleBackColor = true;
			// 
			// tileTypeSetView
			// 
			this.tileTypeSetView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tileTypeSetView.Location = new System.Drawing.Point(3, 3);
			this.tileTypeSetView.Name = "tileTypeSetView";
			this.tileTypeSetView.Size = new System.Drawing.Size(175, 134);
			this.tileTypeSetView.TabIndex = 0;
			this.tileTypeSetView.TileTypes = null;
			this.tileTypeSetView.UseCompatibleStateImageBehavior = false;
			this.tileTypeSetView.SelectedIndexChanged += new System.EventHandler(this.tileTypeSetView_SelectedIndexChanged);
			// 
			// objectsTabPage
			// 
			this.objectsTabPage.Controls.Add(this.objectsTreeView);
			this.objectsTabPage.Location = new System.Drawing.Point(4, 22);
			this.objectsTabPage.Name = "objectsTabPage";
			this.objectsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.objectsTabPage.Size = new System.Drawing.Size(181, 140);
			this.objectsTabPage.TabIndex = 1;
			this.objectsTabPage.Text = "Objects";
			this.objectsTabPage.UseVisualStyleBackColor = true;
			// 
			// objectsTreeView
			// 
			this.objectsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectsTreeView.Location = new System.Drawing.Point(3, 3);
			this.objectsTreeView.Name = "objectsTreeView";
			this.objectsTreeView.Size = new System.Drawing.Size(175, 134);
			this.objectsTreeView.TabIndex = 0;
			this.objectsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectsTreeView_AfterSelect);
			// 
			// EditorControls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "EditorControls";
			this.Size = new System.Drawing.Size(189, 166);
			this.tabControl1.ResumeLayout(false);
			this.tilesTabPage.ResumeLayout(false);
			this.objectsTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tilesTabPage;
		private System.Windows.Forms.TabPage objectsTabPage;
		private TileTypeSetListView tileTypeSetView;
		private System.Windows.Forms.TreeView objectsTreeView;

	}
}
