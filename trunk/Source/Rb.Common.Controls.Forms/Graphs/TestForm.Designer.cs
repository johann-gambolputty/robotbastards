using System.Windows.Forms;

namespace Rb.Common.Controls.Forms.Graphs
{
	partial class TestForm
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
			this.graphControl1 = new Rb.Common.Controls.Forms.Graphs.GraphControl();
			this.graphComponentsControl1 = new Rb.Common.Controls.Forms.Graphs.GraphComponentsControl();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.numberBarControl1 = new Rb.Common.Controls.Forms.NumberBarControl();
			this.SuspendLayout();
			// 
			// graphControl1
			// 
			this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphControl1.Location = new System.Drawing.Point(150, 0);
			this.graphControl1.Name = "graphControl1";
			this.graphControl1.Size = new System.Drawing.Size(318, 280);
			this.graphControl1.TabIndex = 0;
			// 
			// graphComponentsControl1
			// 
			this.graphComponentsControl1.AssociatedGraphControl = null;
			this.graphComponentsControl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.graphComponentsControl1.Location = new System.Drawing.Point(0, 0);
			this.graphComponentsControl1.Name = "graphComponentsControl1";
			this.graphComponentsControl1.Size = new System.Drawing.Size(150, 280);
			this.graphComponentsControl1.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.Color.CornflowerBlue;
			this.splitter1.Location = new System.Drawing.Point(150, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 280);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// numberBarControl1
			// 
			this.numberBarControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.numberBarControl1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.numberBarControl1.BarColour = System.Drawing.Color.SteelBlue;
			this.numberBarControl1.BarShadeColour = System.Drawing.Color.LightSteelBlue;
			this.numberBarControl1.Location = new System.Drawing.Point(12, 228);
			this.numberBarControl1.MaximumValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numberBarControl1.MinimumValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.numberBarControl1.Name = "numberBarControl1";
			this.numberBarControl1.Size = new System.Drawing.Size(132, 52);
			this.numberBarControl1.Step = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numberBarControl1.TabIndex = 3;
			this.numberBarControl1.TickFont = new System.Drawing.Font("Arial", 6F);
			this.numberBarControl1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.numberBarControl1.ValuePrecision = 2;
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(468, 280);
			this.Controls.Add(this.numberBarControl1);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.graphControl1);
			this.Controls.Add(this.graphComponentsControl1);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestForm_FormClosing);
			this.Load += new System.EventHandler(this.TestForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private GraphControl graphControl1;
		private GraphComponentsControl graphComponentsControl1;
		private System.Windows.Forms.Splitter splitter1;
		private NumberBarControl numberBarControl1;
	}
}