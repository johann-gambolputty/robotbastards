namespace Goo.Common.Ui.WinForms.Controllers.CompositeEditor
{
	partial class CompositeEditorView
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.compositeTreeView = new System.Windows.Forms.TreeView( );
			this.toolStrip1 = new System.Windows.Forms.ToolStrip( );
			this.SuspendLayout( );
			// 
			// compositeTreeView
			// 
			this.compositeTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.compositeTreeView.Location = new System.Drawing.Point( 0, 25 );
			this.compositeTreeView.Name = "compositeTreeView";
			this.compositeTreeView.Size = new System.Drawing.Size( 316, 321 );
			this.compositeTreeView.TabIndex = 0;
			this.compositeTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler( this.compositeTreeView_MouseClick );
			this.compositeTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler( this.compositeTreeView_AfterSelect );
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Location = new System.Drawing.Point( 0, 0 );
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size( 316, 25 );
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// CompositeEditorView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.compositeTreeView );
			this.Controls.Add( this.toolStrip1 );
			this.Name = "CompositeEditorView";
			this.Size = new System.Drawing.Size( 316, 346 );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.TreeView compositeTreeView;
		private System.Windows.Forms.ToolStrip toolStrip1;
	}
}
