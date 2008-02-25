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
			this.objectsTabPage = new System.Windows.Forms.TabPage();
			this.objectsTreeView = new System.Windows.Forms.TreeView();
			this.brushPage = new System.Windows.Forms.TabPage();
			this.savedBrushComboBox = new System.Windows.Forms.ComboBox();
			this.savedBrushRadioButton = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.circleEdgeCountUpDown = new System.Windows.Forms.NumericUpDown();
			this.showCollisionChamferCheckBox = new System.Windows.Forms.CheckBox();
			this.circleBrushRadioButton = new System.Windows.Forms.RadioButton();
			this.polygonBrushRadioButton = new System.Windows.Forms.RadioButton();
			this.tabControl1.SuspendLayout();
			this.objectsTabPage.SuspendLayout();
			this.brushPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.circleEdgeCountUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.objectsTabPage);
			this.tabControl1.Controls.Add(this.brushPage);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(189, 185);
			this.tabControl1.TabIndex = 0;
			// 
			// objectsTabPage
			// 
			this.objectsTabPage.Controls.Add(this.objectsTreeView);
			this.objectsTabPage.Location = new System.Drawing.Point(4, 22);
			this.objectsTabPage.Name = "objectsTabPage";
			this.objectsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.objectsTabPage.Size = new System.Drawing.Size(181, 159);
			this.objectsTabPage.TabIndex = 1;
			this.objectsTabPage.Text = "Objects";
			this.objectsTabPage.UseVisualStyleBackColor = true;
			// 
			// objectsTreeView
			// 
			this.objectsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectsTreeView.Location = new System.Drawing.Point(3, 3);
			this.objectsTreeView.Name = "objectsTreeView";
			this.objectsTreeView.Size = new System.Drawing.Size(175, 153);
			this.objectsTreeView.TabIndex = 0;
			this.objectsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectsTreeView_AfterSelect);
			// 
			// brushPage
			// 
			this.brushPage.Controls.Add(this.savedBrushComboBox);
			this.brushPage.Controls.Add(this.savedBrushRadioButton);
			this.brushPage.Controls.Add(this.label1);
			this.brushPage.Controls.Add(this.circleEdgeCountUpDown);
			this.brushPage.Controls.Add(this.showCollisionChamferCheckBox);
			this.brushPage.Controls.Add(this.circleBrushRadioButton);
			this.brushPage.Controls.Add(this.polygonBrushRadioButton);
			this.brushPage.Location = new System.Drawing.Point(4, 22);
			this.brushPage.Name = "brushPage";
			this.brushPage.Padding = new System.Windows.Forms.Padding(3);
			this.brushPage.Size = new System.Drawing.Size(181, 159);
			this.brushPage.TabIndex = 2;
			this.brushPage.Text = "Brushes";
			this.brushPage.UseVisualStyleBackColor = true;
			this.brushPage.Enter += new System.EventHandler(this.brushPage_Enter);
			// 
			// savedBrushComboBox
			// 
			this.savedBrushComboBox.FormattingEnabled = true;
			this.savedBrushComboBox.Location = new System.Drawing.Point(27, 98);
			this.savedBrushComboBox.Name = "savedBrushComboBox";
			this.savedBrushComboBox.Size = new System.Drawing.Size(148, 21);
			this.savedBrushComboBox.TabIndex = 8;
			// 
			// savedBrushRadioButton
			// 
			this.savedBrushRadioButton.AutoSize = true;
			this.savedBrushRadioButton.Location = new System.Drawing.Point(6, 75);
			this.savedBrushRadioButton.Name = "savedBrushRadioButton";
			this.savedBrushRadioButton.Size = new System.Drawing.Size(86, 17);
			this.savedBrushRadioButton.TabIndex = 7;
			this.savedBrushRadioButton.Text = "Saved Brush";
			this.savedBrushRadioButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(24, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Edges";
			// 
			// circleEdgeCountUpDown
			// 
			this.circleEdgeCountUpDown.Location = new System.Drawing.Point(67, 49);
			this.circleEdgeCountUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.circleEdgeCountUpDown.Name = "circleEdgeCountUpDown";
			this.circleEdgeCountUpDown.Size = new System.Drawing.Size(52, 20);
			this.circleEdgeCountUpDown.TabIndex = 5;
			this.circleEdgeCountUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.circleEdgeCountUpDown.ValueChanged += new System.EventHandler(this.circleEdgeCountUpDown_ValueChanged);
			// 
			// showCollisionChamferCheckBox
			// 
			this.showCollisionChamferCheckBox.AutoSize = true;
			this.showCollisionChamferCheckBox.Location = new System.Drawing.Point(6, 136);
			this.showCollisionChamferCheckBox.Name = "showCollisionChamferCheckBox";
			this.showCollisionChamferCheckBox.Size = new System.Drawing.Size(134, 17);
			this.showCollisionChamferCheckBox.TabIndex = 4;
			this.showCollisionChamferCheckBox.Text = "Show collision chamfer";
			this.showCollisionChamferCheckBox.UseVisualStyleBackColor = true;
			// 
			// circleBrushRadioButton
			// 
			this.circleBrushRadioButton.AutoSize = true;
			this.circleBrushRadioButton.Location = new System.Drawing.Point(6, 29);
			this.circleBrushRadioButton.Name = "circleBrushRadioButton";
			this.circleBrushRadioButton.Size = new System.Drawing.Size(81, 17);
			this.circleBrushRadioButton.TabIndex = 1;
			this.circleBrushRadioButton.Text = "Circle Brush";
			this.circleBrushRadioButton.UseVisualStyleBackColor = true;
			this.circleBrushRadioButton.CheckedChanged += new System.EventHandler(this.circleBrushRadioButton_CheckedChanged);
			// 
			// polygonBrushRadioButton
			// 
			this.polygonBrushRadioButton.AutoSize = true;
			this.polygonBrushRadioButton.Checked = true;
			this.polygonBrushRadioButton.Location = new System.Drawing.Point(6, 6);
			this.polygonBrushRadioButton.Name = "polygonBrushRadioButton";
			this.polygonBrushRadioButton.Size = new System.Drawing.Size(93, 17);
			this.polygonBrushRadioButton.TabIndex = 0;
			this.polygonBrushRadioButton.TabStop = true;
			this.polygonBrushRadioButton.Text = "Polygon Brush";
			this.polygonBrushRadioButton.UseVisualStyleBackColor = true;
			this.polygonBrushRadioButton.CheckedChanged += new System.EventHandler(this.userBrushRadioButton_CheckedChanged);
			// 
			// EditorControls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "EditorControls";
			this.Size = new System.Drawing.Size(189, 185);
			this.tabControl1.ResumeLayout(false);
			this.objectsTabPage.ResumeLayout(false);
			this.brushPage.ResumeLayout(false);
			this.brushPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.circleEdgeCountUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage objectsTabPage;
		private System.Windows.Forms.TreeView objectsTreeView;
		private System.Windows.Forms.TabPage brushPage;
		private System.Windows.Forms.RadioButton polygonBrushRadioButton;
		private System.Windows.Forms.RadioButton circleBrushRadioButton;
		private System.Windows.Forms.CheckBox showCollisionChamferCheckBox;
		private System.Windows.Forms.RadioButton savedBrushRadioButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown circleEdgeCountUpDown;
		private System.Windows.Forms.ComboBox savedBrushComboBox;

	}
}
