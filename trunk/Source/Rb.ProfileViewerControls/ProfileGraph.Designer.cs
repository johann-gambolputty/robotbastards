namespace Rb.ProfileViewerControls
{
	partial class ProfileGraph
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
			this.zoomInButton = new System.Windows.Forms.Button();
			this.zoomOutButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// zoomInButton
			// 
			this.zoomInButton.Location = new System.Drawing.Point(3, 0);
			this.zoomInButton.Name = "zoomInButton";
			this.zoomInButton.Size = new System.Drawing.Size(16, 22);
			this.zoomInButton.TabIndex = 0;
			this.zoomInButton.Text = "+";
			this.zoomInButton.UseVisualStyleBackColor = true;
			this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
			// 
			// zoomOutButton
			// 
			this.zoomOutButton.Location = new System.Drawing.Point(21, 0);
			this.zoomOutButton.Name = "zoomOutButton";
			this.zoomOutButton.Size = new System.Drawing.Size(16, 22);
			this.zoomOutButton.TabIndex = 1;
			this.zoomOutButton.Text = "-";
			this.zoomOutButton.UseVisualStyleBackColor = true;
			this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
			// 
			// ProfileGraph
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.zoomOutButton);
			this.Controls.Add(this.zoomInButton);
			this.Name = "ProfileGraph";
			this.Load += new System.EventHandler(this.ProfileGraph_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ProfileGraph_Paint);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button zoomInButton;
		private System.Windows.Forms.Button zoomOutButton;
	}
}
