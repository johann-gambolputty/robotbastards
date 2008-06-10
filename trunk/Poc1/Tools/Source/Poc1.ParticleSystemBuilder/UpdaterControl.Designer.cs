namespace Poc1.ParticleSystemBuilder
{
	partial class UpdaterControl
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
			this.updaterTypeComboBox = new System.Windows.Forms.ComboBox();
			this.updaterPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.updaterPropertyGrid);
			this.groupBox1.Controls.Add(this.updaterTypeComboBox);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(150, 150);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Updater";
			// 
			// updaterTypeComboBox
			// 
			this.updaterTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.updaterTypeComboBox.FormattingEnabled = true;
			this.updaterTypeComboBox.Location = new System.Drawing.Point(3, 16);
			this.updaterTypeComboBox.Name = "updaterTypeComboBox";
			this.updaterTypeComboBox.Size = new System.Drawing.Size(144, 21);
			this.updaterTypeComboBox.TabIndex = 0;
			// 
			// updaterPropertyGrid
			// 
			this.updaterPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.updaterPropertyGrid.Location = new System.Drawing.Point(6, 43);
			this.updaterPropertyGrid.Name = "updaterPropertyGrid";
			this.updaterPropertyGrid.Size = new System.Drawing.Size(138, 101);
			this.updaterPropertyGrid.TabIndex = 1;
			this.updaterPropertyGrid.ToolbarVisible = false;
			// 
			// UpdaterControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "UpdaterControl";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox updaterTypeComboBox;
		private System.Windows.Forms.PropertyGrid updaterPropertyGrid;

	}
}
