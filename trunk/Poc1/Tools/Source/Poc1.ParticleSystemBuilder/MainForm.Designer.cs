namespace Poc1.ParticleSystemBuilder
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
			this.psDisplay = new Rb.Rendering.Windows.Display();
			this.SuspendLayout();
			// 
			// psDisplay
			// 
			this.psDisplay.AllowArrowKeyInputs = false;
			this.psDisplay.ColourBits = ((byte)(32));
			this.psDisplay.ContinuousRendering = true;
			this.psDisplay.DepthBits = ((byte)(24));
			this.psDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.psDisplay.Location = new System.Drawing.Point(0, 0);
			this.psDisplay.Name = "psDisplay";
			this.psDisplay.RenderInterval = 1;
			this.psDisplay.Size = new System.Drawing.Size(349, 317);
			this.psDisplay.StencilBits = ((byte)(0));
			this.psDisplay.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(349, 317);
			this.Controls.Add(this.psDisplay);
			this.Name = "MainForm";
			this.Text = "Particle System Builder";
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.ResumeLayout(false);

		}

		#endregion

		private Rb.Rendering.Windows.Display psDisplay;
	}
}

