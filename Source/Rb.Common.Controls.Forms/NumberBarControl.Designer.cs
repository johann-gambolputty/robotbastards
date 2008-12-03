namespace Rb.Common.Controls.Forms
{
	partial class NumberBarControl
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
			this.barPanel = new Rb.Common.Controls.Forms.DbPanel();
			this.stepUpButton = new System.Windows.Forms.Button();
			this.stepDownButton = new System.Windows.Forms.Button();
			this.valueLabel = new System.Windows.Forms.Label();
			this.barPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// barPanel
			// 
			this.barPanel.BackColor = System.Drawing.Color.Transparent;
			this.barPanel.Controls.Add(this.stepUpButton);
			this.barPanel.Controls.Add(this.stepDownButton);
			this.barPanel.Controls.Add(this.valueLabel);
			this.barPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.barPanel.Location = new System.Drawing.Point(0, 0);
			this.barPanel.Name = "barPanel";
			this.barPanel.Size = new System.Drawing.Size(182, 27);
			this.barPanel.TabIndex = 0;
			this.barPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.barPanel_Paint);
			// 
			// stepUpButton
			// 
			this.stepUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.stepUpButton.BackgroundImage = global::Rb.Common.Controls.Forms.Properties.Resources.SmallUp;
			this.stepUpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.stepUpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.stepUpButton.ForeColor = System.Drawing.Color.Transparent;
			this.stepUpButton.Location = new System.Drawing.Point(154, 0);
			this.stepUpButton.Name = "stepUpButton";
			this.stepUpButton.Size = new System.Drawing.Size(17, 8);
			this.stepUpButton.TabIndex = 3;
			this.stepUpButton.UseVisualStyleBackColor = true;
			this.stepUpButton.Click += new System.EventHandler(this.stepUpButton_Click);
			// 
			// stepDownButton
			// 
			this.stepDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.stepDownButton.BackgroundImage = global::Rb.Common.Controls.Forms.Properties.Resources.SmallDown;
			this.stepDownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.stepDownButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.stepDownButton.ForeColor = System.Drawing.Color.Transparent;
			this.stepDownButton.Location = new System.Drawing.Point(154, 8);
			this.stepDownButton.Name = "stepDownButton";
			this.stepDownButton.Size = new System.Drawing.Size(17, 8);
			this.stepDownButton.TabIndex = 4;
			this.stepDownButton.UseVisualStyleBackColor = true;
			this.stepDownButton.Click += new System.EventHandler(this.stepDownButton_Click);
			// 
			// valueLabel
			// 
			this.valueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.valueLabel.BackColor = System.Drawing.Color.Transparent;
			this.valueLabel.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.valueLabel.Location = new System.Drawing.Point(13, 0);
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size(141, 16);
			this.valueLabel.TabIndex = 0;
			this.valueLabel.Text = "0.000";
			this.valueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.valueLabel.MouseLeave += new System.EventHandler(this.valueLabel_MouseLeave);
			this.valueLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.valueLabel_MouseDown);
			this.valueLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.valueLabel_MouseMove);
			this.valueLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.valueLabel_MouseUp);
			// 
			// NumberBarControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.barPanel);
			this.Name = "NumberBarControl";
			this.Size = new System.Drawing.Size(182, 27);
			this.barPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private DbPanel barPanel;
		private System.Windows.Forms.Label valueLabel;
		private System.Windows.Forms.Button stepUpButton;
		private System.Windows.Forms.Button stepDownButton;

	}
}
