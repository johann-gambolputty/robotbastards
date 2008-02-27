namespace Poc0.LevelEditor.EditModes.Controls
{
	partial class LevelGeometryEditModeControl
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
			this.savedBrushComboBox = new System.Windows.Forms.ComboBox();
			this.savedBrushRadioButton = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.circleEdgeCountUpDown = new System.Windows.Forms.NumericUpDown();
			this.showCollisionChamferCheckBox = new System.Windows.Forms.CheckBox();
			this.circleBrushRadioButton = new System.Windows.Forms.RadioButton();
			this.polygonBrushRadioButton = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.circleEdgeCountUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// savedBrushComboBox
			// 
			this.savedBrushComboBox.FormattingEnabled = true;
			this.savedBrushComboBox.Location = new System.Drawing.Point(25, 96);
			this.savedBrushComboBox.Name = "savedBrushComboBox";
			this.savedBrushComboBox.Size = new System.Drawing.Size(148, 21);
			this.savedBrushComboBox.TabIndex = 15;
			// 
			// savedBrushRadioButton
			// 
			this.savedBrushRadioButton.AutoSize = true;
			this.savedBrushRadioButton.Location = new System.Drawing.Point(4, 73);
			this.savedBrushRadioButton.Name = "savedBrushRadioButton";
			this.savedBrushRadioButton.Size = new System.Drawing.Size(86, 17);
			this.savedBrushRadioButton.TabIndex = 14;
			this.savedBrushRadioButton.Text = "Saved Brush";
			this.savedBrushRadioButton.UseVisualStyleBackColor = true;
			this.savedBrushRadioButton.CheckedChanged += new System.EventHandler(this.savedBrushRadioButton_CheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(22, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 13;
			this.label1.Text = "Edges";
			// 
			// circleEdgeCountUpDown
			// 
			this.circleEdgeCountUpDown.Location = new System.Drawing.Point(65, 47);
			this.circleEdgeCountUpDown.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.circleEdgeCountUpDown.Name = "circleEdgeCountUpDown";
			this.circleEdgeCountUpDown.Size = new System.Drawing.Size(52, 20);
			this.circleEdgeCountUpDown.TabIndex = 12;
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
			this.showCollisionChamferCheckBox.Location = new System.Drawing.Point(4, 134);
			this.showCollisionChamferCheckBox.Name = "showCollisionChamferCheckBox";
			this.showCollisionChamferCheckBox.Size = new System.Drawing.Size(134, 17);
			this.showCollisionChamferCheckBox.TabIndex = 11;
			this.showCollisionChamferCheckBox.Text = "Show collision chamfer";
			this.showCollisionChamferCheckBox.UseVisualStyleBackColor = true;
			this.showCollisionChamferCheckBox.CheckedChanged += new System.EventHandler(this.showCollisionChamferCheckBox_CheckedChanged);
			// 
			// circleBrushRadioButton
			// 
			this.circleBrushRadioButton.AutoSize = true;
			this.circleBrushRadioButton.Location = new System.Drawing.Point(4, 27);
			this.circleBrushRadioButton.Name = "circleBrushRadioButton";
			this.circleBrushRadioButton.Size = new System.Drawing.Size(81, 17);
			this.circleBrushRadioButton.TabIndex = 10;
			this.circleBrushRadioButton.Text = "Circle Brush";
			this.circleBrushRadioButton.UseVisualStyleBackColor = true;
			this.circleBrushRadioButton.CheckedChanged += new System.EventHandler(this.circleBrushRadioButton_CheckedChanged);
			// 
			// polygonBrushRadioButton
			// 
			this.polygonBrushRadioButton.AutoSize = true;
			this.polygonBrushRadioButton.Checked = true;
			this.polygonBrushRadioButton.Location = new System.Drawing.Point(4, 4);
			this.polygonBrushRadioButton.Name = "polygonBrushRadioButton";
			this.polygonBrushRadioButton.Size = new System.Drawing.Size(93, 17);
			this.polygonBrushRadioButton.TabIndex = 9;
			this.polygonBrushRadioButton.TabStop = true;
			this.polygonBrushRadioButton.Text = "Polygon Brush";
			this.polygonBrushRadioButton.UseVisualStyleBackColor = true;
			this.polygonBrushRadioButton.CheckedChanged += new System.EventHandler(this.polygonBrushRadioButton_CheckedChanged);
			// 
			// LevelGeometryEditModeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.savedBrushComboBox);
			this.Controls.Add(this.savedBrushRadioButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.circleEdgeCountUpDown);
			this.Controls.Add(this.showCollisionChamferCheckBox);
			this.Controls.Add(this.circleBrushRadioButton);
			this.Controls.Add(this.polygonBrushRadioButton);
			this.Name = "LevelGeometryEditModeControl";
			this.Size = new System.Drawing.Size(180, 168);
			((System.ComponentModel.ISupportInitialize)(this.circleEdgeCountUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox savedBrushComboBox;
		private System.Windows.Forms.RadioButton savedBrushRadioButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown circleEdgeCountUpDown;
		private System.Windows.Forms.CheckBox showCollisionChamferCheckBox;
		private System.Windows.Forms.RadioButton circleBrushRadioButton;
		private System.Windows.Forms.RadioButton polygonBrushRadioButton;
	}
}
