namespace Poc1.PlanetBuilder
{
	partial class GroundTextureControl
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
			Rb.Core.Maths.LineFunction1d lineFunction1d1 = new Rb.Core.Maths.LineFunction1d();
			Rb.Core.Maths.LineFunction1d lineFunction1d2 = new Rb.Core.Maths.LineFunction1d();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroundTextureControl));
			this.groundTypeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.altitudeGraphEditorControl = new Rb.NiceControls.GraphEditorControl();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.slopeGraphEditorControl = new Rb.NiceControls.GraphEditorControl();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.fileToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groundTypeTableLayoutPanel
			// 
			this.groundTypeTableLayoutPanel.AutoScroll = true;
			this.groundTypeTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.groundTypeTableLayoutPanel.ColumnCount = 1;
			this.groundTypeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.groundTypeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.groundTypeTableLayoutPanel.Location = new System.Drawing.Point(0, 25);
			this.groundTypeTableLayoutPanel.Name = "groundTypeTableLayoutPanel";
			this.groundTypeTableLayoutPanel.RowCount = 1;
			this.groundTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.groundTypeTableLayoutPanel.Size = new System.Drawing.Size(236, 99);
			this.groundTypeTableLayoutPanel.TabIndex = 0;
			this.groundTypeTableLayoutPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.groundTypeTableLayoutPanel_MouseClick);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 124);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(236, 3);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.altitudeGraphEditorControl);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 127);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(236, 143);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Altitude Distribution";
			// 
			// altitudeGraphEditorControl
			// 
			this.altitudeGraphEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.altitudeGraphEditorControl.Function = lineFunction1d1;
			this.altitudeGraphEditorControl.Location = new System.Drawing.Point(3, 16);
			this.altitudeGraphEditorControl.Name = "altitudeGraphEditorControl";
			this.altitudeGraphEditorControl.Size = new System.Drawing.Size(230, 124);
			this.altitudeGraphEditorControl.TabIndex = 0;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(0, 270);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(236, 3);
			this.splitter2.TabIndex = 3;
			this.splitter2.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.slopeGraphEditorControl);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 273);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(236, 130);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Slope Distribution";
			// 
			// slopeGraphEditorControl
			// 
			this.slopeGraphEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.slopeGraphEditorControl.Function = lineFunction1d2;
			this.slopeGraphEditorControl.Location = new System.Drawing.Point(3, 16);
			this.slopeGraphEditorControl.Name = "slopeGraphEditorControl";
			this.slopeGraphEditorControl.Size = new System.Drawing.Size(230, 111);
			this.slopeGraphEditorControl.TabIndex = 1;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripDropDownButton,
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(236, 25);
			this.toolStrip1.TabIndex = 6;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// fileToolStripDropDownButton
			// 
			this.fileToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.fileToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
			this.fileToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("fileToolStripDropDownButton.Image")));
			this.fileToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.fileToolStripDropDownButton.Name = "fileToolStripDropDownButton";
			this.fileToolStripDropDownButton.Size = new System.Drawing.Size(36, 22);
			this.fileToolStripDropDownButton.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "&New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.exportToToolStripMenuItem});
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(52, 22);
			this.toolStripButton1.Text = "Export";
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exportToolStripMenuItem.Text = "&Export";
			this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
			// 
			// exportToToolStripMenuItem
			// 
			this.exportToToolStripMenuItem.Name = "exportToToolStripMenuItem";
			this.exportToToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exportToToolStripMenuItem.Text = "Export &To...";
			this.exportToToolStripMenuItem.Click += new System.EventHandler(this.exportToToolStripMenuItem_Click);
			// 
			// GroundTextureControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.groundTypeTableLayoutPanel);
			this.Controls.Add(this.toolStrip1);
			this.Name = "GroundTextureControl";
			this.Size = new System.Drawing.Size(236, 403);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel groundTypeTableLayoutPanel;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.GroupBox groupBox2;
		private Rb.NiceControls.GraphEditorControl altitudeGraphEditorControl;
		private Rb.NiceControls.GraphEditorControl slopeGraphEditorControl;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripDropDownButton fileToolStripDropDownButton;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToToolStripMenuItem;
	}
}
