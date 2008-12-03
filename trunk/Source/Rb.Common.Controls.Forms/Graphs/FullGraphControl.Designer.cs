namespace Rb.Common.Controls.Forms.Graphs
{
	partial class FullGraphControl
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
			this.graphLegendControl1 = new Rb.Common.Controls.Forms.Graphs.GraphLegendControl( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.graphControl1 = new Rb.Common.Controls.Forms.Graphs.GraphControl( );
			this.SuspendLayout( );
			// 
			// graphLegendControl1
			// 
			this.graphLegendControl1.AssociatedGraphControl = null;
			this.graphLegendControl1.Dock = System.Windows.Forms.DockStyle.Left;
			this.graphLegendControl1.Location = new System.Drawing.Point( 0, 0 );
			this.graphLegendControl1.Name = "graphLegendControl1";
			this.graphLegendControl1.Size = new System.Drawing.Size( 147, 241 );
			this.graphLegendControl1.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 147, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 5, 241 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// graphControl1
			// 
			this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphControl1.Location = new System.Drawing.Point( 152, 0 );
			this.graphControl1.Name = "graphControl1";
			this.graphControl1.Size = new System.Drawing.Size( 212, 241 );
			this.graphControl1.TabIndex = 2;
			// 
			// FullGraphControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.graphControl1 );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.graphLegendControl1 );
			this.Name = "FullGraphControl";
			this.Size = new System.Drawing.Size( 364, 241 );
			this.ResumeLayout( false );

		}

		#endregion

		private GraphLegendControl graphLegendControl1;
		private System.Windows.Forms.Splitter splitter1;
		private GraphControl graphControl1;
	}
}
