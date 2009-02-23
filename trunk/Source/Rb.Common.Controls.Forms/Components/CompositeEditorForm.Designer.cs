namespace Rb.Common.Controls.Forms.Components
{
	partial class CompositeEditorForm
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.currentCompositeView = new CompositeViewControl();
			this.label2 = new System.Windows.Forms.Label();
			this.splitterPanel = new System.Windows.Forms.Panel();
			this.removeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.availableComponentTypesView = new AvailableComponentTypesViewControl();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.splitterPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.splitterPanel, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 305);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.availableComponentTypesView);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(191, 299);
			this.panel1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(191, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Available Templates";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.currentCompositeView);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(260, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(191, 299);
			this.panel2.TabIndex = 3;
			// 
			// currentCompositeView
			// 
			this.currentCompositeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.currentCompositeView.Location = new System.Drawing.Point(0, 13);
			this.currentCompositeView.Name = "currentCompositeView";
			this.currentCompositeView.Size = new System.Drawing.Size(191, 286);
			this.currentCompositeView.TabIndex = 3;
			this.currentCompositeView.Composite = null;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(191, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Current Composite";
			// 
			// splitterPanel
			// 
			this.splitterPanel.Controls.Add(this.removeButton);
			this.splitterPanel.Controls.Add(this.addButton);
			this.splitterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitterPanel.Location = new System.Drawing.Point(200, 3);
			this.splitterPanel.Name = "splitterPanel";
			this.splitterPanel.Size = new System.Drawing.Size(54, 299);
			this.splitterPanel.TabIndex = 4;
			this.splitterPanel.MouseLeave += new System.EventHandler(this.splitterPanel_MouseLeave);
			this.splitterPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitterPanel_MouseDown);
			this.splitterPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.splitterPanel_MouseMove);
			this.splitterPanel.MouseEnter += new System.EventHandler(this.splitterPanel_MouseEnter);
			this.splitterPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitterPanel_MouseUp);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.removeButton.Location = new System.Drawing.Point(0, 42);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(54, 23);
			this.removeButton.TabIndex = 1;
			this.removeButton.Text = "<<";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.Location = new System.Drawing.Point(0, 13);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(54, 23);
			this.addButton.TabIndex = 0;
			this.addButton.Text = ">>";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(310, 323);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(391, 323);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// availableComponentTypesView
			// 
			this.availableComponentTypesView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.availableComponentTypesView.Location = new System.Drawing.Point(0, 13);
			this.availableComponentTypesView.Name = "availableComponentTypesView";
			this.availableComponentTypesView.Size = new System.Drawing.Size(191, 286);
			this.availableComponentTypesView.TabIndex = 2;
			this.availableComponentTypesView.Types = null;
			// 
			// TemplateCompositionEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(478, 354);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "TemplateCompositionEditorForm";
			this.Text = "Composite Editor";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.splitterPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel2;
		private CompositeViewControl currentCompositeView;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel splitterPanel;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
		private AvailableComponentTypesViewControl availableComponentTypesView;
	}
}