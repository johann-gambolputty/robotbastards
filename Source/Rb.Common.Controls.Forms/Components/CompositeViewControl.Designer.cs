namespace Rb.Common.Controls.Forms.Components
{
	partial class CompositeViewControl
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
			this.components = new System.ComponentModel.Container();
			this.compositeView = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// compositeView
			// 
			this.compositeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.compositeView.HideSelection = false;
			this.compositeView.Location = new System.Drawing.Point(0, 0);
			this.compositeView.Name = "compositeView";
			this.compositeView.Size = new System.Drawing.Size(243, 200);
			this.compositeView.TabIndex = 1;
			this.compositeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.compositeView_MouseDoubleClick);
			this.compositeView.ImageList = imageList;
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// CompositeViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.compositeView);
			this.Name = "CompositeViewControl";
			this.Size = new System.Drawing.Size(243, 200);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView compositeView;
		private System.Windows.Forms.ImageList imageList;
	}
}
