namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	partial class ServicesEditorForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.removeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.servicesTree = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.objectPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.panel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.servicesTree);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(147, 295);
			this.panel1.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.removeButton);
			this.panel3.Controls.Add(this.addButton);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 240);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(147, 55);
			this.panel3.TabIndex = 1;
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.removeButton.Location = new System.Drawing.Point(86, 14);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(55, 25);
			this.removeButton.TabIndex = 1;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.addButton.Location = new System.Drawing.Point(3, 14);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(55, 25);
			this.addButton.TabIndex = 0;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			// 
			// servicesTree
			// 
			this.servicesTree.Dock = System.Windows.Forms.DockStyle.Top;
			this.servicesTree.Location = new System.Drawing.Point(0, 0);
			this.servicesTree.Name = "servicesTree";
			this.servicesTree.Size = new System.Drawing.Size(147, 240);
			this.servicesTree.TabIndex = 0;
			this.servicesTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.servicesTree_AfterSelect);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(147, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 295);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.panel2.Controls.Add(this.objectPropertyGrid);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(150, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(192, 295);
			this.panel2.TabIndex = 2;
			// 
			// objectPropertyGrid
			// 
			this.objectPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectPropertyGrid.Location = new System.Drawing.Point(0, 0);
			this.objectPropertyGrid.Name = "objectPropertyGrid";
			this.objectPropertyGrid.Size = new System.Drawing.Size(192, 295);
			this.objectPropertyGrid.TabIndex = 0;
			// 
			// ServicesEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(342, 295);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Name = "ServicesEditorForm";
			this.Text = "Services";
			this.Shown += new System.EventHandler(this.ServicesEditorForm_Shown);
			this.panel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TreeView servicesTree;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.PropertyGrid objectPropertyGrid;
	}
}