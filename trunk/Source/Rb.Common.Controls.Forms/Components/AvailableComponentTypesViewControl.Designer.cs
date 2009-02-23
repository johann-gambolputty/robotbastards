using Rb.Common.Controls.Forms.Categories;

namespace Rb.Common.Controls.Forms.Components
{
	partial class AvailableComponentTypesViewControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AvailableComponentTypesViewControl));
			this.templateTypeCategoryControl = new CategoryControl();
			this.SuspendLayout();
			// 
			// templateTypeCategoryControl
			// 
			this.templateTypeCategoryControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.templateTypeCategoryControl.Location = new System.Drawing.Point(0, 0);
			this.templateTypeCategoryControl.Name = "templateTypeCategoryControl";
			this.templateTypeCategoryControl.Size = new System.Drawing.Size(166, 267);
			this.templateTypeCategoryControl.TabIndex = 0;
			// 
			// AvailableTemplatesViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.templateTypeCategoryControl);
			this.Name = "AvailableTemplatesViewControl";
			this.Size = new System.Drawing.Size(166, 267);
			this.ResumeLayout(false);

		}

		#endregion

		private CategoryControl templateTypeCategoryControl;

	}
}
