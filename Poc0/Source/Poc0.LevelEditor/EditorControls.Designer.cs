namespace Poc0.LevelEditor
{
	partial class EditorControls
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
			this.tabControl1 = new System.Windows.Forms.TabControl( );
			this.objectsTabPage = new System.Windows.Forms.TabPage( );
			this.objectsTreeView = new System.Windows.Forms.TreeView( );
			this.brushPage = new System.Windows.Forms.TabPage( );
			this.label1 = new System.Windows.Forms.Label( );
			this.csgComboBox = new System.Windows.Forms.ComboBox( );
			this.circleBrushRadioButton = new System.Windows.Forms.RadioButton( );
			this.userBrushRadioButton = new System.Windows.Forms.RadioButton( );
			this.tabControl1.SuspendLayout( );
			this.objectsTabPage.SuspendLayout( );
			this.brushPage.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add( this.objectsTabPage );
			this.tabControl1.Controls.Add( this.brushPage );
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size( 189, 166 );
			this.tabControl1.TabIndex = 0;
			// 
			// objectsTabPage
			// 
			this.objectsTabPage.Controls.Add( this.objectsTreeView );
			this.objectsTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.objectsTabPage.Name = "objectsTabPage";
			this.objectsTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.objectsTabPage.Size = new System.Drawing.Size( 181, 140 );
			this.objectsTabPage.TabIndex = 1;
			this.objectsTabPage.Text = "Objects";
			this.objectsTabPage.UseVisualStyleBackColor = true;
			// 
			// objectsTreeView
			// 
			this.objectsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectsTreeView.Location = new System.Drawing.Point( 3, 3 );
			this.objectsTreeView.Name = "objectsTreeView";
			this.objectsTreeView.Size = new System.Drawing.Size( 175, 134 );
			this.objectsTreeView.TabIndex = 0;
			this.objectsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.objectsTreeView_AfterSelect );
			// 
			// brushPage
			// 
			this.brushPage.Controls.Add( this.label1 );
			this.brushPage.Controls.Add( this.csgComboBox );
			this.brushPage.Controls.Add( this.circleBrushRadioButton );
			this.brushPage.Controls.Add( this.userBrushRadioButton );
			this.brushPage.Location = new System.Drawing.Point( 4, 22 );
			this.brushPage.Name = "brushPage";
			this.brushPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.brushPage.Size = new System.Drawing.Size( 181, 140 );
			this.brushPage.TabIndex = 2;
			this.brushPage.Text = "Brushes";
			this.brushPage.UseVisualStyleBackColor = true;
			this.brushPage.Enter += new System.EventHandler( this.brushPage_Enter );
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 47, 26 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 29, 13 );
			this.label1.TabIndex = 3;
			this.label1.Text = "CSG";
			// 
			// csgComboBox
			// 
			this.csgComboBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.csgComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.csgComboBox.FormattingEnabled = true;
			this.csgComboBox.Location = new System.Drawing.Point( 82, 23 );
			this.csgComboBox.Name = "csgComboBox";
			this.csgComboBox.Size = new System.Drawing.Size( 49, 21 );
			this.csgComboBox.TabIndex = 2;
			this.csgComboBox.SelectedIndexChanged += new System.EventHandler( this.csgComboBox_SelectedIndexChanged );
			// 
			// circleBrushRadioButton
			// 
			this.circleBrushRadioButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.circleBrushRadioButton.AutoSize = true;
			this.circleBrushRadioButton.Location = new System.Drawing.Point( 50, 73 );
			this.circleBrushRadioButton.Name = "circleBrushRadioButton";
			this.circleBrushRadioButton.Size = new System.Drawing.Size( 81, 17 );
			this.circleBrushRadioButton.TabIndex = 1;
			this.circleBrushRadioButton.Text = "Circle Brush";
			this.circleBrushRadioButton.UseVisualStyleBackColor = true;
			// 
			// userBrushRadioButton
			// 
			this.userBrushRadioButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.userBrushRadioButton.AutoSize = true;
			this.userBrushRadioButton.Checked = true;
			this.userBrushRadioButton.Location = new System.Drawing.Point( 50, 50 );
			this.userBrushRadioButton.Name = "userBrushRadioButton";
			this.userBrushRadioButton.Size = new System.Drawing.Size( 77, 17 );
			this.userBrushRadioButton.TabIndex = 0;
			this.userBrushRadioButton.TabStop = true;
			this.userBrushRadioButton.Text = "User Brush";
			this.userBrushRadioButton.UseVisualStyleBackColor = true;
			this.userBrushRadioButton.CheckedChanged += new System.EventHandler( this.userBrushRadioButton_CheckedChanged );
			// 
			// EditorControls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.tabControl1 );
			this.Name = "EditorControls";
			this.Size = new System.Drawing.Size( 189, 166 );
			this.tabControl1.ResumeLayout( false );
			this.objectsTabPage.ResumeLayout( false );
			this.brushPage.ResumeLayout( false );
			this.brushPage.PerformLayout( );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage objectsTabPage;
		private System.Windows.Forms.TreeView objectsTreeView;
		private System.Windows.Forms.TabPage brushPage;
		private System.Windows.Forms.RadioButton userBrushRadioButton;
		private System.Windows.Forms.RadioButton circleBrushRadioButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox csgComboBox;

	}
}