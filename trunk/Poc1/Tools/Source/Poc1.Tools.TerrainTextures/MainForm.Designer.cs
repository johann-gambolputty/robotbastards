namespace Poc1.Tools.TerrainTextures
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.terrainTypeControlsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.terrainTypeEditorControl2 = new Poc1.Tools.TerrainTextures.TerrainTypeEditorControl();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.heightFunctionPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.heightFunctionComboBox = new System.Windows.Forms.ComboBox();
			this.applyLightingCheckBox = new System.Windows.Forms.CheckBox();
			this.newHeightMapButton = new System.Windows.Forms.Button();
			this.samplePanel = new System.Windows.Forms.Panel();
			this.terrainTypeEditorControl3 = new Poc1.Tools.TerrainTextures.TerrainTypeEditorControl();
			this.exportToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.mainMenu.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(454, 35);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Texture";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(141, 35);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(97, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Altitude Distribution";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(292, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Slope Distribution";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Name";
			// 
			// terrainTypeControlsLayoutPanel
			// 
			this.terrainTypeControlsLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.terrainTypeControlsLayoutPanel.AutoScroll = true;
			this.terrainTypeControlsLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
			this.terrainTypeControlsLayoutPanel.ColumnCount = 1;
			this.terrainTypeControlsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.terrainTypeControlsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.terrainTypeControlsLayoutPanel.Location = new System.Drawing.Point(9, 51);
			this.terrainTypeControlsLayoutPanel.Name = "terrainTypeControlsLayoutPanel";
			this.terrainTypeControlsLayoutPanel.RowCount = 1;
			this.terrainTypeControlsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.terrainTypeControlsLayoutPanel.Size = new System.Drawing.Size(547, 394);
			this.terrainTypeControlsLayoutPanel.TabIndex = 0;
			this.terrainTypeControlsLayoutPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.terrainTypeControlsLayoutPanel_MouseClick);
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(907, 24);
			this.mainMenu.TabIndex = 1;
			this.mainMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportToToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "&New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
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
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(444, 15);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Texture";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(272, 15);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(97, 13);
			this.label6.TabIndex = 3;
			this.label6.Text = "Altitude Distribution";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(114, 15);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(89, 13);
			this.label7.TabIndex = 2;
			this.label7.Text = "Slope Distribution";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(8, 15);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(35, 13);
			this.label8.TabIndex = 1;
			this.label8.Text = "Name";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.tableLayoutPanel1.AutoScroll = true;
			this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.terrainTypeEditorControl2, 0, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 31);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 313F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(531, 314);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// terrainTypeEditorControl2
			// 
			this.terrainTypeEditorControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.terrainTypeEditorControl2.Enabled = false;
			this.terrainTypeEditorControl2.Location = new System.Drawing.Point(4, 4);
			this.terrainTypeEditorControl2.Name = "terrainTypeEditorControl2";
			this.terrainTypeEditorControl2.Size = new System.Drawing.Size(513, 78);
			this.terrainTypeEditorControl2.TabIndex = 0;
			this.terrainTypeEditorControl2.TerrainType = ((Poc1.Tools.TerrainTextures.Core.TerrainType)(resources.GetObject("terrainTypeEditorControl2.TerrainType")));
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.tableLayoutPanel2.AutoScroll = true;
			this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 100);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.heightFunctionPropertyGrid);
			this.groupBox1.Controls.Add(this.heightFunctionComboBox);
			this.groupBox1.Controls.Add(this.applyLightingCheckBox);
			this.groupBox1.Controls.Add(this.newHeightMapButton);
			this.groupBox1.Controls.Add(this.samplePanel);
			this.groupBox1.Location = new System.Drawing.Point(562, 45);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(333, 400);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sample";
			// 
			// heightFunctionPropertyGrid
			// 
			this.heightFunctionPropertyGrid.HelpVisible = false;
			this.heightFunctionPropertyGrid.Location = new System.Drawing.Point(215, 19);
			this.heightFunctionPropertyGrid.Name = "heightFunctionPropertyGrid";
			this.heightFunctionPropertyGrid.Size = new System.Drawing.Size(112, 49);
			this.heightFunctionPropertyGrid.TabIndex = 4;
			this.heightFunctionPropertyGrid.ToolbarVisible = false;
			// 
			// heightFunctionComboBox
			// 
			this.heightFunctionComboBox.FormattingEnabled = true;
			this.heightFunctionComboBox.Location = new System.Drawing.Point(113, 21);
			this.heightFunctionComboBox.Name = "heightFunctionComboBox";
			this.heightFunctionComboBox.Size = new System.Drawing.Size(94, 21);
			this.heightFunctionComboBox.TabIndex = 3;
			// 
			// applyLightingCheckBox
			// 
			this.applyLightingCheckBox.AutoSize = true;
			this.applyLightingCheckBox.Location = new System.Drawing.Point(7, 48);
			this.applyLightingCheckBox.Name = "applyLightingCheckBox";
			this.applyLightingCheckBox.Size = new System.Drawing.Size(92, 17);
			this.applyLightingCheckBox.TabIndex = 2;
			this.applyLightingCheckBox.Text = "Apply Lighting";
			this.applyLightingCheckBox.UseVisualStyleBackColor = true;
			this.applyLightingCheckBox.CheckedChanged += new System.EventHandler(this.applyLightingCheckBox_CheckedChanged);
			// 
			// newHeightMapButton
			// 
			this.newHeightMapButton.Location = new System.Drawing.Point(6, 19);
			this.newHeightMapButton.Name = "newHeightMapButton";
			this.newHeightMapButton.Size = new System.Drawing.Size(101, 23);
			this.newHeightMapButton.TabIndex = 1;
			this.newHeightMapButton.Text = "New Height Map";
			this.newHeightMapButton.UseVisualStyleBackColor = true;
			this.newHeightMapButton.Click += new System.EventHandler(this.newHeightMapButton_Click);
			// 
			// samplePanel
			// 
			this.samplePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.samplePanel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.samplePanel.Location = new System.Drawing.Point(7, 74);
			this.samplePanel.Name = "samplePanel";
			this.samplePanel.Size = new System.Drawing.Size(320, 320);
			this.samplePanel.TabIndex = 0;
			// 
			// terrainTypeEditorControl3
			// 
			this.terrainTypeEditorControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.terrainTypeEditorControl3.Enabled = false;
			this.terrainTypeEditorControl3.Location = new System.Drawing.Point(4, 4);
			this.terrainTypeEditorControl3.Name = "terrainTypeEditorControl3";
			this.terrainTypeEditorControl3.Size = new System.Drawing.Size(513, 78);
			this.terrainTypeEditorControl3.TabIndex = 0;
			this.terrainTypeEditorControl3.TerrainType = ((Poc1.Tools.TerrainTextures.Core.TerrainType)(resources.GetObject("terrainTypeEditorControl3.TerrainType")));
			// 
			// exportToolStripMenuItem
			// 
			this.exportToToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exportToToolStripMenuItem.Text = "Export &To...";
			this.exportToToolStripMenuItem.Click += new System.EventHandler(this.exportToToolStripMenuItem_Click);
			// 
			// exportToolStripMenuItem1
			// 
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem1";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exportToolStripMenuItem.Text = "&Export";
			this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.ClientSize = new System.Drawing.Size(907, 457);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.mainMenu);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.terrainTypeControlsLayoutPanel);
			this.MainMenuStrip = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "Terrain Textures";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.TableLayoutPanel terrainTypeControlsLayoutPanel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private TerrainTypeEditorControl terrainTypeEditorControl2;
		private TerrainTypeEditorControl terrainTypeEditorControl3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel samplePanel;
		private System.Windows.Forms.CheckBox applyLightingCheckBox;
		private System.Windows.Forms.Button newHeightMapButton;
		private System.Windows.Forms.PropertyGrid heightFunctionPropertyGrid;
		private System.Windows.Forms.ComboBox heightFunctionComboBox;
		private System.Windows.Forms.ToolStripMenuItem exportToToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
	}
}

