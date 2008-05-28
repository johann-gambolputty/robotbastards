namespace Poc1.PlanetBuilder
{
	partial class GroundTypeControl
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
			this.texturePanel = new System.Windows.Forms.Panel();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.moveUpButton = new System.Windows.Forms.Button();
			this.moveDownButton = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// texturePanel
			// 
			this.texturePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.texturePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.texturePanel.Location = new System.Drawing.Point(165, 2);
			this.texturePanel.Name = "texturePanel";
			this.texturePanel.Size = new System.Drawing.Size(40, 40);
			this.texturePanel.TabIndex = 1;
			this.texturePanel.Click += new System.EventHandler(this.texturePanel_Click);
			// 
			// nameTextBox
			// 
			this.nameTextBox.Location = new System.Drawing.Point(34, 12);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(100, 20);
			this.nameTextBox.TabIndex = 2;
			// 
			// moveUpButton
			// 
			this.moveUpButton.Image = global::Poc1.PlanetBuilder.Properties.Resources.MoveUp;
			this.moveUpButton.Location = new System.Drawing.Point(0, 0);
			this.moveUpButton.Name = "moveUpButton";
			this.moveUpButton.Size = new System.Drawing.Size(22, 23);
			this.moveUpButton.TabIndex = 4;
			this.moveUpButton.UseVisualStyleBackColor = true;
			this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
			// 
			// moveDownButton
			// 
			this.moveDownButton.Image = global::Poc1.PlanetBuilder.Properties.Resources.MoveDown;
			this.moveDownButton.Location = new System.Drawing.Point(0, 22);
			this.moveDownButton.Name = "moveDownButton";
			this.moveDownButton.Size = new System.Drawing.Size(22, 23);
			this.moveDownButton.TabIndex = 5;
			this.moveDownButton.UseVisualStyleBackColor = true;
			this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
			// 
			// deleteButton
			// 
			this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.deleteButton.Image = global::Poc1.PlanetBuilder.Properties.Resources.Delete;
			this.deleteButton.Location = new System.Drawing.Point(211, 0);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(23, 23);
			this.deleteButton.TabIndex = 3;
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// GroundTypeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.moveDownButton);
			this.Controls.Add(this.moveUpButton);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.nameTextBox);
			this.Controls.Add(this.texturePanel);
			this.Name = "GroundTypeControl";
			this.Size = new System.Drawing.Size(241, 45);
			this.Load += new System.EventHandler(this.GroundTypeControl_Load);
			this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GroundTypeControl_MouseClick);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel texturePanel;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button moveUpButton;
		private System.Windows.Forms.Button moveDownButton;
	}
}
