namespace Poc0.LevelEditor
{
	partial class SelectionControl
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
			this.selectionListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// selectionListBox
			// 
			this.selectionListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectionListBox.FormattingEnabled = true;
			this.selectionListBox.Location = new System.Drawing.Point(0, 0);
			this.selectionListBox.Name = "selectionListBox";
			this.selectionListBox.Size = new System.Drawing.Size(150, 147);
			this.selectionListBox.TabIndex = 0;
			// 
			// SelectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.selectionListBox);
			this.Name = "SelectionControl";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox selectionListBox;
	}
}
