namespace Poc0.LevelEditor
{
	partial class MaterialsEditorForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			System.Windows.Forms.Button okButton;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( MaterialsEditorForm ) );
			this.cancelButton = new System.Windows.Forms.Button( );
			this.addButton = new System.Windows.Forms.Button( );
			this.removeButton = new System.Windows.Forms.Button( );
			this.panel1 = new System.Windows.Forms.Panel( );
			this.materialProperties = new System.Windows.Forms.PropertyGrid( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.materialsTree = new System.Windows.Forms.TreeView( );
			this.toolStrip1 = new System.Windows.Forms.ToolStrip( );
			this.addToolStripButton = new System.Windows.Forms.ToolStripButton( );
			this.removeToolStripButton = new System.Windows.Forms.ToolStripButton( );
			okButton = new System.Windows.Forms.Button( );
			this.panel1.SuspendLayout( );
			this.toolStrip1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// okButton
			// 
			okButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			okButton.Location = new System.Drawing.Point( 12, 316 );
			okButton.Name = "okButton";
			okButton.Size = new System.Drawing.Size( 75, 23 );
			okButton.TabIndex = 0;
			okButton.Text = "OK";
			okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point( 343, 316 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size( 75, 23 );
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point( 105, 316 );
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size( 75, 23 );
			this.addButton.TabIndex = 3;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point( 237, 316 );
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size( 75, 23 );
			this.removeButton.TabIndex = 4;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.materialProperties );
			this.panel1.Controls.Add( this.splitter1 );
			this.panel1.Controls.Add( this.materialsTree );
			this.panel1.Location = new System.Drawing.Point( 12, 28 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 406, 282 );
			this.panel1.TabIndex = 5;
			// 
			// materialProperties
			// 
			this.materialProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.materialProperties.Location = new System.Drawing.Point( 124, 0 );
			this.materialProperties.Name = "materialProperties";
			this.materialProperties.Size = new System.Drawing.Size( 282, 282 );
			this.materialProperties.TabIndex = 2;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 121, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 3, 282 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// materialsTree
			// 
			this.materialsTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.materialsTree.Location = new System.Drawing.Point( 0, 0 );
			this.materialsTree.Name = "materialsTree";
			this.materialsTree.Size = new System.Drawing.Size( 121, 282 );
			this.materialsTree.TabIndex = 0;
			this.materialsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.materialsTree_AfterSelect );
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripButton,
            this.removeToolStripButton} );
			this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size( 430, 25 );
			this.toolStrip1.TabIndex = 6;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// addToolStripButton
			// 
			this.addToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.addToolStripButton.Image = ( ( System.Drawing.Image )( resources.GetObject( "addToolStripButton.Image" ) ) );
			this.addToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addToolStripButton.Name = "addToolStripButton";
			this.addToolStripButton.Size = new System.Drawing.Size( 30, 22 );
			this.addToolStripButton.Text = "Add";
			// 
			// removeToolStripButton
			// 
			this.removeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.removeToolStripButton.Image = ( ( System.Drawing.Image )( resources.GetObject( "removeToolStripButton.Image" ) ) );
			this.removeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.removeToolStripButton.Name = "removeToolStripButton";
			this.removeToolStripButton.Size = new System.Drawing.Size( 50, 22 );
			this.removeToolStripButton.Text = "Remove";
			// 
			// MaterialsEditorForm
			// 
			this.AcceptButton = okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size( 430, 351 );
			this.Controls.Add( this.toolStrip1 );
			this.Controls.Add( this.panel1 );
			this.Controls.Add( this.removeButton );
			this.Controls.Add( this.addButton );
			this.Controls.Add( this.cancelButton );
			this.Controls.Add( okButton );
			this.Name = "MaterialsEditorForm";
			this.Text = "Materials";
			this.Shown += new System.EventHandler( this.MaterialsEditorForm_Shown );
			this.panel1.ResumeLayout( false );
			this.toolStrip1.ResumeLayout( false );
			this.toolStrip1.PerformLayout( );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PropertyGrid materialProperties;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TreeView materialsTree;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton addToolStripButton;
		private System.Windows.Forms.ToolStripButton removeToolStripButton;
	}
}