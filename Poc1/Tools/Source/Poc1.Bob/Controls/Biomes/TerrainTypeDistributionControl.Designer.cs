namespace Poc1.Bob.Controls.Biomes
{
	partial class TerrainTypeDistributionControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( TerrainTypeDistributionControl ) );
			Rb.Common.Controls.Graphs.Classes.GraphAxisDisplay graphAxisDisplay1 = new Rb.Common.Controls.Graphs.Classes.GraphAxisDisplay( );
			Rb.Common.Controls.Graphs.Classes.GraphAxisDisplay graphAxisDisplay2 = new Rb.Common.Controls.Graphs.Classes.GraphAxisDisplay( );
			this.distributionGraphControl = new Rb.Common.Controls.Forms.Graphs.GraphControl( );
			this.SuspendLayout( );
			// 
			// distributionGraphControl
			// 
			this.distributionGraphControl.DataWindow = ( ( System.Drawing.RectangleF )( resources.GetObject( "distributionGraphControl.DataWindow" ) ) );
			this.distributionGraphControl.DefaultDataWindow = ( ( System.Drawing.RectangleF )( resources.GetObject( "distributionGraphControl.DefaultDataWindow" ) ) );
			this.distributionGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.distributionGraphControl.Location = new System.Drawing.Point( 0, 0 );
			this.distributionGraphControl.Name = "distributionGraphControl";
			this.distributionGraphControl.RelateControlSizeToDataArea = false;
			this.distributionGraphControl.Size = new System.Drawing.Size( 186, 163 );
			this.distributionGraphControl.TabIndex = 0;
			graphAxisDisplay1.DisplayTitle = false;
			graphAxisDisplay1.LineStep = 0.1F;
			graphAxisDisplay1.TickFont = new System.Drawing.Font( "Arial", 6F );
			graphAxisDisplay1.Title = "X";
			graphAxisDisplay1.TitleFont = new System.Drawing.Font( "Arial", 6F );
			graphAxisDisplay1.ValueToString = null;
			graphAxisDisplay1.VerticalTitleText = false;
			this.distributionGraphControl.XAxis = graphAxisDisplay1;
			graphAxisDisplay2.DisplayTitle = false;
			graphAxisDisplay2.LineStep = 0.1F;
			graphAxisDisplay2.TickFont = new System.Drawing.Font( "Arial", 6F );
			graphAxisDisplay2.Title = "Y";
			graphAxisDisplay2.TitleFont = new System.Drawing.Font( "Arial", 6F );
			graphAxisDisplay2.ValueToString = null;
			graphAxisDisplay2.VerticalTitleText = false;
			this.distributionGraphControl.YAxis = graphAxisDisplay2;
			// 
			// TerrainTypeDistributionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.distributionGraphControl );
			this.Name = "TerrainTypeDistributionControl";
			this.Size = new System.Drawing.Size( 186, 163 );
			this.ResumeLayout( false );

		}

		#endregion

		private Rb.Common.Controls.Forms.Graphs.GraphControl distributionGraphControl;



	}
}
