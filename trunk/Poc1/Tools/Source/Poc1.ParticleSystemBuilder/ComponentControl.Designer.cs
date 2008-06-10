namespace Poc1.ParticleSystemBuilder
{
	partial class ComponentControl
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.controlPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.typeComboBox = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.controlPropertyGrid);
			this.groupBox1.Controls.Add(this.typeComboBox);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(150, 121);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Control";
			// 
			// controlPropertyGrid
			// 
			this.controlPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.controlPropertyGrid.HelpVisible = false;
			this.controlPropertyGrid.Location = new System.Drawing.Point(6, 47);
			this.controlPropertyGrid.Name = "controlPropertyGrid";
			this.controlPropertyGrid.Size = new System.Drawing.Size(138, 68);
			this.controlPropertyGrid.TabIndex = 1;
			this.controlPropertyGrid.ToolbarVisible = false;
			// 
			// typeComboBox
			// 
			this.typeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeComboBox.FormattingEnabled = true;
			this.typeComboBox.Location = new System.Drawing.Point(6, 19);
			this.typeComboBox.Name = "typeComboBox";
			this.typeComboBox.Size = new System.Drawing.Size(138, 21);
			this.typeComboBox.TabIndex = 0;
			this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
			// 
			// ParticleControlEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "ParticleControlEditor";
			this.Size = new System.Drawing.Size(150, 121);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox typeComboBox;
		private System.Windows.Forms.PropertyGrid controlPropertyGrid;
	}
}
