namespace Rb.Log.Controls
{
	partial class LogListView
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
			this.m_IdColumn = new System.Windows.Forms.ColumnHeader( );
			this.m_FileColumn = new System.Windows.Forms.ColumnHeader( );
			this.m_LineColumn = new System.Windows.Forms.ColumnHeader( );
			this.m_ColumnColumn = new System.Windows.Forms.ColumnHeader( );
			this.m_MethodColumn = new System.Windows.Forms.ColumnHeader( );
			this.m_SourceColumn = new System.Windows.Forms.ColumnHeader( );
			this.m_TimeColumn = new System.Windows.Forms.ColumnHeader( );
			this.m_ThreadColumn = new System.Windows.Forms.ColumnHeader( );
			this.m_MessageColumn = new System.Windows.Forms.ColumnHeader( );
			this.SuspendLayout( );
			// 
			// m_IdColumn
			// 
			this.m_IdColumn.Text = "ID";
			this.m_IdColumn.Width = 29;
			// 
			// m_FileColumn
			// 
			this.m_FileColumn.Text = "File";
			this.m_FileColumn.Width = 113;
			// 
			// m_LineColumn
			// 
			this.m_LineColumn.Text = "Line";
			this.m_LineColumn.Width = 41;
			// 
			// m_ColumnColumn
			// 
			this.m_ColumnColumn.Text = "Column";
			this.m_ColumnColumn.Width = 51;
			// 
			// m_MethodColumn
			// 
			this.m_MethodColumn.Text = "Method";
			// 
			// m_SourceColumn
			// 
			this.m_SourceColumn.Text = "Source";
			// 
			// m_TimeColumn
			// 
			this.m_TimeColumn.Text = "Time";
			// 
			// m_ThreadColumn
			// 
			this.m_ThreadColumn.Text = "Thread";
			// 
			// m_MessageColumn
			// 
			this.m_MessageColumn.Text = "Message";
			// 
			// LogListView
			// 
			this.AllowColumnReorder = true;
			this.Columns.AddRange( new System.Windows.Forms.ColumnHeader[ ] {
            this.m_IdColumn,
            this.m_FileColumn,
            this.m_LineColumn,
            this.m_ColumnColumn,
            this.m_MethodColumn,
            this.m_SourceColumn,
            this.m_TimeColumn,
            this.m_ThreadColumn,
            this.m_MessageColumn} );
			this.FullRowSelect = true;
			this.MultiSelect = false;
			this.Name = "logListView";
			this.Size = new System.Drawing.Size( 796, 277 );
			this.UseCompatibleStateImageBehavior = false;
			this.View = System.Windows.Forms.View.Details;
			this.SelectedIndexChanged += new System.EventHandler( this.LogListView_SelectedIndexChanged );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.ColumnHeader m_FileColumn;
		private System.Windows.Forms.ColumnHeader m_LineColumn;
		private System.Windows.Forms.ColumnHeader m_ColumnColumn;
		private System.Windows.Forms.ColumnHeader m_SourceColumn;
		private System.Windows.Forms.ColumnHeader m_IdColumn;
		private System.Windows.Forms.ColumnHeader m_MethodColumn;
		private System.Windows.Forms.ColumnHeader m_TimeColumn;
		private System.Windows.Forms.ColumnHeader m_ThreadColumn;
		private System.Windows.Forms.ColumnHeader m_MessageColumn;
	}
}
