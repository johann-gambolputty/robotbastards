namespace Rb.Common.Controls.Forms.Categories
{
	partial class CategoryControl
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
			this.categoryView = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.filterTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// categoryView
			// 
			this.categoryView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.categoryView.Location = new System.Drawing.Point(0, 26);
			this.categoryView.Name = "categoryView";
			this.categoryView.Size = new System.Drawing.Size(150, 124);
			this.categoryView.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Filter:";
			// 
			// filterTextBox
			// 
			this.filterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.filterTextBox.Location = new System.Drawing.Point(41, 0);
			this.filterTextBox.Name = "filterTextBox";
			this.filterTextBox.Size = new System.Drawing.Size(109, 20);
			this.filterTextBox.TabIndex = 2;
			this.filterTextBox.TextChanged += new System.EventHandler(this.filterTextBox_TextChanged);
			// 
			// CategoryControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.filterTextBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.categoryView);
			this.Name = "CategoryControl";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView categoryView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox filterTextBox;
	}
}
