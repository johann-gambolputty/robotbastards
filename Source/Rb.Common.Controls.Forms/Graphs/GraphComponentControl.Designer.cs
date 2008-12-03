namespace Rb.Common.Controls.Forms.Graphs
{
	partial class GraphComponentControl
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
			this.enableCheckbox = new System.Windows.Forms.CheckBox();
			this.graphNameLabel = new System.Windows.Forms.Label();
			this.valueLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// enableCheckbox
			// 
			this.enableCheckbox.AutoSize = true;
			this.enableCheckbox.BackColor = System.Drawing.Color.Transparent;
			this.enableCheckbox.Location = new System.Drawing.Point(6, 7);
			this.enableCheckbox.Name = "enableCheckbox";
			this.enableCheckbox.Size = new System.Drawing.Size(15, 14);
			this.enableCheckbox.TabIndex = 0;
			this.enableCheckbox.UseVisualStyleBackColor = false;
			this.enableCheckbox.CheckedChanged += new System.EventHandler(this.enableCheckbox_CheckedChanged);
			// 
			// graphNameLabel
			// 
			this.graphNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.graphNameLabel.BackColor = System.Drawing.Color.Transparent;
			this.graphNameLabel.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.graphNameLabel.Location = new System.Drawing.Point(24, 6);
			this.graphNameLabel.Name = "graphNameLabel";
			this.graphNameLabel.Size = new System.Drawing.Size(99, 14);
			this.graphNameLabel.TabIndex = 1;
			this.graphNameLabel.Text = "graph name";
			this.graphNameLabel.Click += new System.EventHandler(this.graphNameLabel_Click);
			// 
			// valueLabel
			// 
			this.valueLabel.BackColor = System.Drawing.Color.Transparent;
			this.valueLabel.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.valueLabel.Location = new System.Drawing.Point(24, 20);
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size(99, 14);
			this.valueLabel.TabIndex = 2;
			this.valueLabel.Text = "Value:";
			this.valueLabel.Click += new System.EventHandler(this.valueLabel_Click);
			// 
			// GraphComponentControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.valueLabel);
			this.Controls.Add(this.graphNameLabel);
			this.Controls.Add(this.enableCheckbox);
			this.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.Name = "GraphComponentControl";
			this.Size = new System.Drawing.Size(150, 41);
			this.Load += new System.EventHandler(this.GraphComponentControl_Load);
			this.Click += new System.EventHandler(this.GraphComponentControl_Click);
			this.Resize += new System.EventHandler(this.GraphComponentControl_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphComponentControl_Paint);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox enableCheckbox;
		private System.Windows.Forms.Label graphNameLabel;
		private System.Windows.Forms.Label valueLabel;
	}
}
