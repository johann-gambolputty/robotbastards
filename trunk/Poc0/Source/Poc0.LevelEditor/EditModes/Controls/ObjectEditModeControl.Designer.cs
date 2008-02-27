namespace Poc0.LevelEditor.EditModes.Controls
{
	partial class ObjectEditModeControl
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
			this.objectsTreeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// objectsTreeView
			// 
			this.objectsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectsTreeView.Location = new System.Drawing.Point(0, 0);
			this.objectsTreeView.Name = "objectsTreeView";
			this.objectsTreeView.Size = new System.Drawing.Size(150, 150);
			this.objectsTreeView.TabIndex = 0;
			this.objectsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectsTreeView_AfterSelect);
			// 
			// ObjectEditModeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.objectsTreeView);
			this.Name = "ObjectEditModeControl";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView objectsTreeView;
	}
}
