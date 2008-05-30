namespace Poc1.ParticleSystemBuilder
{
	partial class RenderControl
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
			this.stationaryRadioButton = new System.Windows.Forms.RadioButton();
			this.circleRadioButton = new System.Windows.Forms.RadioButton();
			this.figureOfEightRadioButton = new System.Windows.Forms.RadioButton();
			this.followMouseRadioButton = new System.Windows.Forms.RadioButton();
			this.renderPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// stationaryRadioButton
			// 
			this.stationaryRadioButton.AutoSize = true;
			this.stationaryRadioButton.Location = new System.Drawing.Point(0, 0);
			this.stationaryRadioButton.Name = "stationaryRadioButton";
			this.stationaryRadioButton.Size = new System.Drawing.Size(72, 17);
			this.stationaryRadioButton.TabIndex = 0;
			this.stationaryRadioButton.TabStop = true;
			this.stationaryRadioButton.Text = "Stationary";
			this.stationaryRadioButton.UseVisualStyleBackColor = true;
			this.stationaryRadioButton.CheckedChanged += new System.EventHandler(this.stationaryRadioButton_CheckedChanged);
			// 
			// circleRadioButton
			// 
			this.circleRadioButton.AutoSize = true;
			this.circleRadioButton.Location = new System.Drawing.Point(0, 23);
			this.circleRadioButton.Name = "circleRadioButton";
			this.circleRadioButton.Size = new System.Drawing.Size(51, 17);
			this.circleRadioButton.TabIndex = 1;
			this.circleRadioButton.TabStop = true;
			this.circleRadioButton.Text = "Circle";
			this.circleRadioButton.UseVisualStyleBackColor = true;
			this.circleRadioButton.CheckedChanged += new System.EventHandler(this.circleRadioButton_CheckedChanged);
			// 
			// figureOfEightRadioButton
			// 
			this.figureOfEightRadioButton.AutoSize = true;
			this.figureOfEightRadioButton.Location = new System.Drawing.Point(3, 46);
			this.figureOfEightRadioButton.Name = "figureOfEightRadioButton";
			this.figureOfEightRadioButton.Size = new System.Drawing.Size(93, 17);
			this.figureOfEightRadioButton.TabIndex = 2;
			this.figureOfEightRadioButton.TabStop = true;
			this.figureOfEightRadioButton.Text = "Figure of Eight";
			this.figureOfEightRadioButton.UseVisualStyleBackColor = true;
			this.figureOfEightRadioButton.CheckedChanged += new System.EventHandler(this.figureOfEightRadioButton_CheckedChanged);
			// 
			// followMouseRadioButton
			// 
			this.followMouseRadioButton.AutoSize = true;
			this.followMouseRadioButton.Location = new System.Drawing.Point(3, 69);
			this.followMouseRadioButton.Name = "followMouseRadioButton";
			this.followMouseRadioButton.Size = new System.Drawing.Size(108, 17);
			this.followMouseRadioButton.TabIndex = 3;
			this.followMouseRadioButton.TabStop = true;
			this.followMouseRadioButton.Text = "Follow the Mouse";
			this.followMouseRadioButton.UseVisualStyleBackColor = true;
			this.followMouseRadioButton.CheckedChanged += new System.EventHandler(this.followMouseRadioButton_CheckedChanged);
			// 
			// renderPanel
			// 
			this.renderPanel.Location = new System.Drawing.Point(117, 3);
			this.renderPanel.Name = "renderPanel";
			this.renderPanel.Size = new System.Drawing.Size(147, 88);
			this.renderPanel.TabIndex = 4;
			// 
			// RenderControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.renderPanel);
			this.Controls.Add(this.followMouseRadioButton);
			this.Controls.Add(this.figureOfEightRadioButton);
			this.Controls.Add(this.circleRadioButton);
			this.Controls.Add(this.stationaryRadioButton);
			this.Name = "RenderControl";
			this.Size = new System.Drawing.Size(267, 94);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton stationaryRadioButton;
		private System.Windows.Forms.RadioButton circleRadioButton;
		private System.Windows.Forms.RadioButton figureOfEightRadioButton;
		private System.Windows.Forms.RadioButton followMouseRadioButton;
		private System.Windows.Forms.Panel renderPanel;
	}
}
