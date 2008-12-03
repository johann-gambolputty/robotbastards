namespace Rb.Common.Controls.Forms.Graphs
{
	partial class GraphControl
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
			this.actualSizeButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// actualSizeButton
			// 
			this.actualSizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.actualSizeButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.actualSizeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.actualSizeButton.Image = global::Rb.Common.Controls.Forms.Properties.Resources.ActualSize;
			this.actualSizeButton.Location = new System.Drawing.Point(3, 153);
			this.actualSizeButton.Name = "actualSizeButton";
			this.actualSizeButton.Size = new System.Drawing.Size(20, 20);
			this.actualSizeButton.TabIndex = 0;
			this.actualSizeButton.UseVisualStyleBackColor = false;
			this.actualSizeButton.Click += new System.EventHandler(this.actualSizeButton_Click);
			// 
			// GraphControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.actualSizeButton);
			this.Name = "GraphControl";
			this.Size = new System.Drawing.Size(241, 177);
			this.Load += new System.EventHandler(this.GraphControl_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GraphControl_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GraphControl_MouseMove);
			this.Resize += new System.EventHandler(this.GraphControl_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphControl_Paint);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GraphControl_MouseUp);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button actualSizeButton;
	}
}
