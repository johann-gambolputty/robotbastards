namespace Poc0.LevelEditor
{
	partial class ObjectPropertyEditor
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
			this.objectPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.subObjectComboBox = new Poc0.LevelEditor.NiceComboBox();
			this.SuspendLayout();
			// 
			// objectPropertyGrid
			// 
			this.objectPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.objectPropertyGrid.Location = new System.Drawing.Point(3, 30);
			this.objectPropertyGrid.Name = "objectPropertyGrid";
			this.objectPropertyGrid.Size = new System.Drawing.Size(167, 196);
			this.objectPropertyGrid.TabIndex = 1;
			// 
			// subObjectComboBox
			// 
			this.subObjectComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.subObjectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.subObjectComboBox.FormattingEnabled = true;
			this.subObjectComboBox.Location = new System.Drawing.Point(3, 3);
			this.subObjectComboBox.Name = "subObjectComboBox";
			this.subObjectComboBox.Size = new System.Drawing.Size(167, 21);
			this.subObjectComboBox.TabIndex = 0;
			this.subObjectComboBox.SelectedIndexChanged += new System.EventHandler(this.subObjectComboBox_SelectedIndexChanged);
			// 
			// ObjectPropertyEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.objectPropertyGrid);
			this.Controls.Add(this.subObjectComboBox);
			this.Name = "ObjectPropertyEditor";
			this.Size = new System.Drawing.Size(173, 229);
			this.ResumeLayout(false);

		}

		#endregion

		private NiceComboBox subObjectComboBox;
		private System.Windows.Forms.PropertyGrid objectPropertyGrid;
	}
}
