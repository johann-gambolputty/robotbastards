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
			this.nameColumn = new System.Windows.Forms.ColumnHeader( );
			this.widthColumn = new System.Windows.Forms.ColumnHeader( );
			this.heightColumn = new System.Windows.Forms.ColumnHeader( );
			this.formatColumn = new System.Windows.Forms.ColumnHeader( );
			this.renderTargetDisplay = new Rb.Rendering.Windows.Display( );
			this.continuousRefreshCheckbox = new System.Windows.Forms.CheckBox( );
			this.refreshViewButton = new System.Windows.Forms.Button( );
			this.SuspendLayout( );
			// 
			// renderTargetListView
			// 
			this.renderTargetListView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.renderTargetListView.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.widthColumn,
            this.heightColumn,
            this.formatColumn} );
			this.renderTargetListView.Location = new System.Drawing.Point( 0, 0 );
			this.renderTargetListView.Name = "renderTargetListView";
			this.renderTargetListView.Size = new System.Drawing.Size( 346, 108 );
			this.renderTargetListView.TabIndex = 0;
			this.renderTargetListView.UseCompatibleStateImageBehavior = false;
			this.renderTargetListView.View = System.Windows.Forms.View.Details;
			// 
			// nameColumn
			// 
			this.nameColumn.Text = "Name";
			this.nameColumn.Width = 158;
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
			// renderTargetDisplay
			// 
			this.renderTargetDisplay.AllowArrowKeyInputs = false;
			this.renderTargetDisplay.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.renderTargetDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.renderTargetDisplay.ColourBits = ( ( byte )( 32 ) );
			this.renderTargetDisplay.ContinuousRendering = false;
			this.renderTargetDisplay.DepthBits = ( ( byte )( 24 ) );
			this.renderTargetDisplay.FocusOnMouseOver = false;
			this.renderTargetDisplay.Location = new System.Drawing.Point( 3, 114 );
			this.renderTargetDisplay.Name = "renderTargetDisplay";
			this.renderTargetDisplay.RenderInterval = 1;
			this.renderTargetDisplay.Size = new System.Drawing.Size( 343, 243 );
			this.renderTargetDisplay.StencilBits = ( ( byte )( 0 ) );
			this.renderTargetDisplay.TabIndex = 2;
			this.renderTargetDisplay.OnRender += new Rb.Rendering.Interfaces.Objects.RenderDelegate( this.renderTargetDisplay_OnRender );
			// 
			// continuousRefreshCheckbox
			// 
			this.continuousRefreshCheckbox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.continuousRefreshCheckbox.AutoSize = true;
			this.continuousRefreshCheckbox.Location = new System.Drawing.Point( 8, 366 );
			this.continuousRefreshCheckbox.Name = "continuousRefreshCheckbox";
			this.continuousRefreshCheckbox.Size = new System.Drawing.Size( 119, 17 );
			this.continuousRefreshCheckbox.TabIndex = 4;
			this.continuousRefreshCheckbox.Text = "Continuous Refresh";
			this.continuousRefreshCheckbox.UseVisualStyleBackColor = true;
			this.continuousRefreshCheckbox.CheckedChanged += new System.EventHandler( this.continuousRefreshCheckbox_CheckedChanged );
			// 
			// refreshViewButton
			// 
			this.refreshViewButton.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.refreshViewButton.Location = new System.Drawing.Point( 265, 360 );
			this.refreshViewButton.Name = "refreshViewButton";
			this.refreshViewButton.Size = new System.Drawing.Size( 81, 23 );
			this.refreshViewButton.TabIndex = 3;
			this.refreshViewButton.Text = "Refresh View";
			this.refreshViewButton.UseVisualStyleBackColor = true;
			// 
			// RenderTargetViewControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.continuousRefreshCheckbox );
			this.Controls.Add( this.refreshViewButton );
			this.Controls.Add( this.renderTargetDisplay );
			this.Controls.Add( this.renderTargetListView );
			this.Name = "RenderTargetViewControl";
			this.Size = new System.Drawing.Size( 349, 386 );
			this.Load += new System.EventHandler( this.RenderTargetViewControl_Load );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.ListView renderTargetListView;
		private Rb.Rendering.Windows.Display renderTargetDisplay;
		private System.Windows.Forms.ColumnHeader nameColumn;
		private System.Windows.Forms.ColumnHeader widthColumn;
		private System.Windows.Forms.ColumnHeader heightColumn;
		private System.Windows.Forms.ColumnHeader formatColumn;
		private System.Windows.Forms.CheckBox continuousRefreshCheckbox;
		private System.Windows.Forms.Button refreshViewButton;
	}
}
