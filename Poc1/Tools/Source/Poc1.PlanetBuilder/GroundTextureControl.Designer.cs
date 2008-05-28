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
			this.groundTypeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.currentSelectionGroupBox = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// groundTypeTableLayoutPanel
			// 
			this.groundTypeTableLayoutPanel.AutoScroll = true;
			this.groundTypeTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.groundTypeTableLayoutPanel.ColumnCount = 1;
			this.groundTypeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.groundTypeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.groundTypeTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.groundTypeTableLayoutPanel.Name = "groundTypeTableLayoutPanel";
			this.groundTypeTableLayoutPanel.RowCount = 1;
			this.groundTypeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.groundTypeTableLayoutPanel.Size = new System.Drawing.Size(236, 121);
			this.groundTypeTableLayoutPanel.TabIndex = 0;
			this.groundTypeTableLayoutPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.groundTypeTableLayoutPanel_MouseClick);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 121);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(236, 3);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// currentSelectionGroupBox
			// 
			this.currentSelectionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.currentSelectionGroupBox.Location = new System.Drawing.Point(0, 124);
			this.currentSelectionGroupBox.Name = "currentSelectionGroupBox";
			this.currentSelectionGroupBox.Size = new System.Drawing.Size(236, 175);
			this.currentSelectionGroupBox.TabIndex = 2;
			this.currentSelectionGroupBox.TabStop = false;
			this.currentSelectionGroupBox.Text = "Selection";
			// 
			// GroundTextureControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.currentSelectionGroupBox);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.groundTypeTableLayoutPanel);
			this.Name = "GroundTextureControl";
			this.Size = new System.Drawing.Size(236, 299);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel groundTypeTableLayoutPanel;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.GroupBox currentSelectionGroupBox;
	}
}
