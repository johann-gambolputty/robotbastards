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
			Rb.Core.Maths.LineFunction1d lineFunction1d3 = new Rb.Core.Maths.LineFunction1d();
			Rb.Core.Maths.LineFunction1d lineFunction1d4 = new Rb.Core.Maths.LineFunction1d();
			this.groundTypeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.altitudeGraphEditorControl = new Rb.NiceControls.GraphEditorControl();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.slopeGraphEditorControl = new Rb.NiceControls.GraphEditorControl();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groundTypeTableLayoutPanel
			// 
			this.groundTypeTableLayoutPanel.AutoScroll = true;
			this.groundTypeTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.groundTypeTableLayoutPanel.ColumnCount = 1;
			this.groundTypeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
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
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.altitudeGraphEditorControl);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 124);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(236, 121);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Altitude Distribution";
			// 
			// altitudeGraphEditorControl
			// 
			this.altitudeGraphEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.altitudeGraphEditorControl.Function = lineFunction1d3;
			this.altitudeGraphEditorControl.Location = new System.Drawing.Point(3, 16);
			this.altitudeGraphEditorControl.Name = "altitudeGraphEditorControl";
			this.altitudeGraphEditorControl.Size = new System.Drawing.Size(230, 102);
			this.altitudeGraphEditorControl.TabIndex = 0;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(0, 245);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(236, 3);
			this.splitter2.TabIndex = 3;
			this.splitter2.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.slopeGraphEditorControl);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 248);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(236, 139);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Slope Distribution";
			// 
			// slopeGraphEditorControl
			// 
			this.slopeGraphEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.slopeGraphEditorControl.Function = lineFunction1d4;
			this.slopeGraphEditorControl.Location = new System.Drawing.Point(3, 16);
			this.slopeGraphEditorControl.Name = "slopeGraphEditorControl";
			this.slopeGraphEditorControl.Size = new System.Drawing.Size(230, 120);
			this.slopeGraphEditorControl.TabIndex = 1;
			// 
			// GroundTextureControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.groundTypeTableLayoutPanel);
			this.Name = "GroundTextureControl";
			this.Size = new System.Drawing.Size(236, 387);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel groundTypeTableLayoutPanel;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.GroupBox groupBox2;
		private Rb.NiceControls.GraphEditorControl altitudeGraphEditorControl;
		private Rb.NiceControls.GraphEditorControl slopeGraphEditorControl;
	}
}
