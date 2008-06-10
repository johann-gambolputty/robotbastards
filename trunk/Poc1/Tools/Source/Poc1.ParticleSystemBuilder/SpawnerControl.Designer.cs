namespace Poc1.ParticleSystemBuilder
{
	partial class SpawnerControl
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
			this.spawnRateTypeComboBox = new System.Windows.Forms.ComboBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.spawnRatePropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.spawnerTypeComboBox = new System.Windows.Forms.ComboBox();
			this.spawnerPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.spawnRatePropertyGrid);
			this.groupBox1.Controls.Add(this.spawnRateTypeComboBox);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 123);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Spawn Rate";
			// 
			// spawnRateTypeComboBox
			// 
			this.spawnRateTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.spawnRateTypeComboBox.FormattingEnabled = true;
			this.spawnRateTypeComboBox.Location = new System.Drawing.Point(6, 19);
			this.spawnRateTypeComboBox.Name = "spawnRateTypeComboBox";
			this.spawnRateTypeComboBox.Size = new System.Drawing.Size(188, 21);
			this.spawnRateTypeComboBox.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 123);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(200, 3);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.spawnerPropertyGrid);
			this.groupBox2.Controls.Add(this.spawnerTypeComboBox);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 126);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(200, 119);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Spawner";
			// 
			// spawnRatePropertyGrid
			// 
			this.spawnRatePropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.spawnRatePropertyGrid.Location = new System.Drawing.Point(6, 46);
			this.spawnRatePropertyGrid.Name = "spawnRatePropertyGrid";
			this.spawnRatePropertyGrid.Size = new System.Drawing.Size(188, 71);
			this.spawnRatePropertyGrid.TabIndex = 2;
			this.spawnRatePropertyGrid.ToolbarVisible = false;
			// 
			// spawnerTypeComboBox
			// 
			this.spawnerTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.spawnerTypeComboBox.FormattingEnabled = true;
			this.spawnerTypeComboBox.Location = new System.Drawing.Point(3, 16);
			this.spawnerTypeComboBox.Name = "spawnerTypeComboBox";
			this.spawnerTypeComboBox.Size = new System.Drawing.Size(191, 21);
			this.spawnerTypeComboBox.TabIndex = 0;
			// 
			// spawnerPropertyGrid
			// 
			this.spawnerPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.spawnerPropertyGrid.Location = new System.Drawing.Point(3, 42);
			this.spawnerPropertyGrid.Name = "spawnerPropertyGrid";
			this.spawnerPropertyGrid.Size = new System.Drawing.Size(191, 71);
			this.spawnerPropertyGrid.TabIndex = 3;
			this.spawnerPropertyGrid.ToolbarVisible = false;
			// 
			// SpawnerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.groupBox1);
			this.Name = "SpawnerControl";
			this.Size = new System.Drawing.Size(200, 245);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox spawnRateTypeComboBox;
		private System.Windows.Forms.PropertyGrid spawnRatePropertyGrid;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox spawnerTypeComboBox;
		private System.Windows.Forms.PropertyGrid spawnerPropertyGrid;

	}
}
