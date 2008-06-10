namespace Poc1.ParticleSystemBuilder
{
	partial class CompositeComponentControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompositeComponentControl));
			this.panel1 = new System.Windows.Forms.Panel();
			this.componentsListBox = new System.Windows.Forms.ListBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.addComponentToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
			this.removeToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.selectedComponentPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.panel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.componentsListBox);
			this.panel1.Controls.Add(this.toolStrip1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(150, 134);
			this.panel1.TabIndex = 0;
			// 
			// componentsListBox
			// 
			this.componentsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.componentsListBox.FormattingEnabled = true;
			this.componentsListBox.Location = new System.Drawing.Point(0, 25);
			this.componentsListBox.Name = "componentsListBox";
			this.componentsListBox.Size = new System.Drawing.Size(150, 108);
			this.componentsListBox.TabIndex = 4;
			this.componentsListBox.SelectedIndexChanged += new System.EventHandler(this.componentsListBox_SelectedIndexChanged);
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addComponentToolStripSplitButton,
            this.removeToolStripButton});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStrip1.Size = new System.Drawing.Size(150, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// addComponentToolStripSplitButton
			// 
			this.addComponentToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.addComponentToolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("addComponentToolStripSplitButton.Image")));
			this.addComponentToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addComponentToolStripSplitButton.Name = "addComponentToolStripSplitButton";
			this.addComponentToolStripSplitButton.Size = new System.Drawing.Size(42, 22);
			this.addComponentToolStripSplitButton.Text = "Add";
			// 
			// removeToolStripButton
			// 
			this.removeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.removeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("removeToolStripButton.Image")));
			this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.removeToolStripButton.Name = "removeToolStripButton";
			this.removeToolStripButton.Size = new System.Drawing.Size(50, 22);
			this.removeToolStripButton.Text = "Remove";
			this.removeToolStripButton.Click += new System.EventHandler(this.removeToolStripButton_Click);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 134);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(150, 3);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// selectedComponentPropertyGrid
			// 
			this.selectedComponentPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectedComponentPropertyGrid.Location = new System.Drawing.Point(0, 137);
			this.selectedComponentPropertyGrid.Name = "selectedComponentPropertyGrid";
			this.selectedComponentPropertyGrid.Size = new System.Drawing.Size(150, 146);
			this.selectedComponentPropertyGrid.TabIndex = 7;
			this.selectedComponentPropertyGrid.ToolbarVisible = false;
			// 
			// CompositeComponentControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.selectedComponentPropertyGrid);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Name = "CompositeComponentControl";
			this.Size = new System.Drawing.Size(150, 283);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ListBox componentsListBox;
		private System.Windows.Forms.ToolStripSplitButton addComponentToolStripSplitButton;
		private System.Windows.Forms.ToolStripButton removeToolStripButton;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.PropertyGrid selectedComponentPropertyGrid;
	}
}
