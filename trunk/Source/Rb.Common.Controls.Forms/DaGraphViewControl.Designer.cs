namespace SchemaTest
{
	partial class DaGraphViewControl
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

		#region Composite Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.graphPanel = new SchemaTest.DbPanel();
			this.SuspendLayout();
			// 
			// graphPanel
			// 
			this.graphPanel.Location = new System.Drawing.Point(0, 0);
			this.graphPanel.Name = "graphPanel";
			this.graphPanel.Size = new System.Drawing.Size(150, 150);
			this.graphPanel.TabIndex = 0;
			this.graphPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.graphPanel_Paint);
			this.graphPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.graphPanel_MouseDown);
			// 
			// DaGraphViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this.graphPanel);
			this.Name = "DaGraphViewControl";
			this.ResumeLayout(false);

		}

		#endregion

		private DbPanel graphPanel;
	}
}
