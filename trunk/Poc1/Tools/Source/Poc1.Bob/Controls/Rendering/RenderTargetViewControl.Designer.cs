using Rb.Rendering;

namespace Poc1.Bob.Controls.Rendering
{
	partial class RenderTargetViewControl
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
			RenderTargets.RenderTargetAdded -= OnRenderTargetAdded;
			RenderTargets.RenderTargetRemoved -= OnRenderTargetRemoved;
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
			this.renderTargetListView = new System.Windows.Forms.ListView( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.renderTargetDisplay = new Rb.Rendering.Windows.Display( );
			this.nameColumn = new System.Windows.Forms.ColumnHeader( );
			this.widthColumn = new System.Windows.Forms.ColumnHeader( );
			this.heightColumn = new System.Windows.Forms.ColumnHeader( );
			this.formatColumn = new System.Windows.Forms.ColumnHeader( );
			this.SuspendLayout( );
			// 
			// renderTargetListView
			// 
			this.renderTargetListView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.widthColumn,
            this.heightColumn,
            this.formatColumn} );
			this.renderTargetListView.Dock = System.Windows.Forms.DockStyle.Left;
			this.renderTargetListView.Location = new System.Drawing.Point( 0, 0 );
			this.renderTargetListView.Name = "renderTargetListView";
			this.renderTargetListView.Size = new System.Drawing.Size( 245, 168 );
			this.renderTargetListView.TabIndex = 0;
			this.renderTargetListView.UseCompatibleStateImageBehavior = false;
			this.renderTargetListView.View = System.Windows.Forms.View.Details;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 245, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 10, 168 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// renderTargetDisplay
			// 
			this.renderTargetDisplay.AllowArrowKeyInputs = false;
			this.renderTargetDisplay.ColourBits = ( ( byte )( 32 ) );
			this.renderTargetDisplay.ContinuousRendering = false;
			this.renderTargetDisplay.DepthBits = ( ( byte )( 24 ) );
			this.renderTargetDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.renderTargetDisplay.FocusOnMouseOver = false;
			this.renderTargetDisplay.Location = new System.Drawing.Point( 255, 0 );
			this.renderTargetDisplay.Name = "renderTargetDisplay";
			this.renderTargetDisplay.RenderInterval = 1;
			this.renderTargetDisplay.Size = new System.Drawing.Size( 212, 168 );
			this.renderTargetDisplay.StencilBits = ( ( byte )( 0 ) );
			this.renderTargetDisplay.TabIndex = 2;
			// 
			// nameColumn
			// 
			this.nameColumn.Text = "Name";
			// 
			// widthColumn
			// 
			this.widthColumn.Text = "Width";
			// 
			// heightColumn
			// 
			this.heightColumn.Text = "Height";
			// 
			// formatColumn
			// 
			this.formatColumn.Text = "Format";
			// 
			// RenderTargetViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.renderTargetDisplay );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.renderTargetListView );
			this.Name = "RenderTargetViewControl";
			this.Size = new System.Drawing.Size( 467, 168 );
			this.Load += new System.EventHandler( this.RenderTargetViewControl_Load );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.ListView renderTargetListView;
		private System.Windows.Forms.Splitter splitter1;
		private Rb.Rendering.Windows.Display renderTargetDisplay;
		private System.Windows.Forms.ColumnHeader nameColumn;
		private System.Windows.Forms.ColumnHeader widthColumn;
		private System.Windows.Forms.ColumnHeader heightColumn;
		private System.Windows.Forms.ColumnHeader formatColumn;
	}
}
