using System.Windows.Forms;

namespace Poc1.Bob.Controls.Atmosphere
{
	partial class ScatteringAtmosphereAnalysisForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ScatteringAtmosphereAnalysisForm ) );
			Rb.Common.Controls.Graphs.Classes.GraphAxisDisplay graphAxisDisplay3 = new Rb.Common.Controls.Graphs.Classes.GraphAxisDisplay( );
			Rb.Common.Controls.Graphs.Classes.GraphAxisDisplay graphAxisDisplay4 = new Rb.Common.Controls.Graphs.Classes.GraphAxisDisplay( );
			this.atmosphereGraph = new Rb.Common.Controls.Forms.Graphs.GraphControl( );
			this.xAxisSourceComboBox = new System.Windows.Forms.ComboBox( );
			this.label1 = new System.Windows.Forms.Label( );
			this.yAxisLegendPanel = new System.Windows.Forms.Panel( );
			this.heightTrackBar = new System.Windows.Forms.TrackBar( );
			this.label2 = new System.Windows.Forms.Label( );
			this.label3 = new System.Windows.Forms.Label( );
			this.viewDirectionTrackBar = new System.Windows.Forms.TrackBar( );
			this.label4 = new System.Windows.Forms.Label( );
			this.sunDirectionTrackBar = new System.Windows.Forms.TrackBar( );
			this.gradientPanel = new System.Windows.Forms.Panel( );
			( ( System.ComponentModel.ISupportInitialize )( this.heightTrackBar ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.viewDirectionTrackBar ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.sunDirectionTrackBar ) ).BeginInit( );
			this.SuspendLayout( );
			// 
			// atmosphereGraph
			// 
			this.atmosphereGraph.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.atmosphereGraph.DataWindow = ( ( System.Drawing.RectangleF )( resources.GetObject( "atmosphereGraph.DataWindow" ) ) );
			this.atmosphereGraph.DefaultDataWindow = ( ( System.Drawing.RectangleF )( resources.GetObject( "atmosphereGraph.DefaultDataWindow" ) ) );
			this.atmosphereGraph.Location = new System.Drawing.Point( 29, 12 );
			this.atmosphereGraph.Name = "atmosphereGraph";
			this.atmosphereGraph.RelateControlSizeToDataArea = false;
			this.atmosphereGraph.Size = new System.Drawing.Size( 350, 181 );
			this.atmosphereGraph.TabIndex = 0;
			graphAxisDisplay3.DisplayTitle = false;
			graphAxisDisplay3.LineStep = 1F;
			graphAxisDisplay3.TickFont = new System.Drawing.Font( "Arial", 6F );
			graphAxisDisplay3.Title = "X";
			graphAxisDisplay3.TitleFont = new System.Drawing.Font( "Arial", 6F );
			graphAxisDisplay3.ValueToString = null;
			graphAxisDisplay3.VerticalTitleText = false;
			this.atmosphereGraph.XAxis = graphAxisDisplay3;
			graphAxisDisplay4.DisplayTitle = false;
			graphAxisDisplay4.LineStep = 1F;
			graphAxisDisplay4.TickFont = new System.Drawing.Font( "Arial", 6F );
			graphAxisDisplay4.Title = "Intensity";
			graphAxisDisplay4.TitleFont = new System.Drawing.Font( "Arial", 6F );
			graphAxisDisplay4.ValueToString = null;
			graphAxisDisplay4.VerticalTitleText = false;
			this.atmosphereGraph.YAxis = graphAxisDisplay4;
			// 
			// xAxisSourceComboBox
			// 
			this.xAxisSourceComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.xAxisSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.xAxisSourceComboBox.FormattingEnabled = true;
			this.xAxisSourceComboBox.Location = new System.Drawing.Point( 82, 230 );
			this.xAxisSourceComboBox.Name = "xAxisSourceComboBox";
			this.xAxisSourceComboBox.Size = new System.Drawing.Size( 121, 21 );
			this.xAxisSourceComboBox.TabIndex = 1;
			this.xAxisSourceComboBox.SelectedIndexChanged += new System.EventHandler( this.xAxisSourceComboBox_SelectedIndexChanged );
			// 
			// label1
			// 
			this.label1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 32, 233 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 44, 13 );
			this.label1.TabIndex = 2;
			this.label1.Text = "Source:";
			// 
			// yAxisLegendPanel
			// 
			this.yAxisLegendPanel.Location = new System.Drawing.Point( 6, 56 );
			this.yAxisLegendPanel.Name = "yAxisLegendPanel";
			this.yAxisLegendPanel.Size = new System.Drawing.Size( 23, 93 );
			this.yAxisLegendPanel.TabIndex = 3;
			this.yAxisLegendPanel.Paint += new System.Windows.Forms.PaintEventHandler( this.yAxisLegendPanel_Paint );
			// 
			// heightTrackBar
			// 
			this.heightTrackBar.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.heightTrackBar.LargeChange = 20;
			this.heightTrackBar.Location = new System.Drawing.Point( 102, 258 );
			this.heightTrackBar.Maximum = 100;
			this.heightTrackBar.Name = "heightTrackBar";
			this.heightTrackBar.Size = new System.Drawing.Size( 277, 45 );
			this.heightTrackBar.TabIndex = 4;
			this.heightTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.heightTrackBar.Scroll += new System.EventHandler( this.heightTrackBar_Scroll );
			// 
			// label2
			// 
			this.label2.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 21, 264 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 38, 13 );
			this.label2.TabIndex = 5;
			this.label2.Text = "Height";
			// 
			// label3
			// 
			this.label3.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 21, 293 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 75, 13 );
			this.label3.TabIndex = 7;
			this.label3.Text = "View Direction";
			// 
			// viewDirectionTrackBar
			// 
			this.viewDirectionTrackBar.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.viewDirectionTrackBar.LargeChange = 20;
			this.viewDirectionTrackBar.Location = new System.Drawing.Point( 102, 286 );
			this.viewDirectionTrackBar.Maximum = 100;
			this.viewDirectionTrackBar.Name = "viewDirectionTrackBar";
			this.viewDirectionTrackBar.Size = new System.Drawing.Size( 277, 45 );
			this.viewDirectionTrackBar.TabIndex = 6;
			this.viewDirectionTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.viewDirectionTrackBar.Scroll += new System.EventHandler( this.viewDirectionTrackBar_Scroll );
			// 
			// label4
			// 
			this.label4.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point( 21, 316 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 71, 13 );
			this.label4.TabIndex = 9;
			this.label4.Text = "Sun Direction";
			// 
			// sunDirectionTrackBar
			// 
			this.sunDirectionTrackBar.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.sunDirectionTrackBar.LargeChange = 20;
			this.sunDirectionTrackBar.Location = new System.Drawing.Point( 102, 309 );
			this.sunDirectionTrackBar.Maximum = 100;
			this.sunDirectionTrackBar.Name = "sunDirectionTrackBar";
			this.sunDirectionTrackBar.Size = new System.Drawing.Size( 277, 45 );
			this.sunDirectionTrackBar.TabIndex = 8;
			this.sunDirectionTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.sunDirectionTrackBar.Scroll += new System.EventHandler( this.sunDirectionTrackBar_Scroll );
			// 
			// gradientPanel
			// 
			this.gradientPanel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.gradientPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gradientPanel.Location = new System.Drawing.Point( 29, 199 );
			this.gradientPanel.Name = "gradientPanel";
			this.gradientPanel.Size = new System.Drawing.Size( 350, 25 );
			this.gradientPanel.TabIndex = 10;
			this.gradientPanel.BackgroundImageLayout = ImageLayout.Stretch;
			// 
			// ScatteringAtmosphereAnalysisForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 391, 343 );
			this.Controls.Add( this.gradientPanel );
			this.Controls.Add( this.label4 );
			this.Controls.Add( this.sunDirectionTrackBar );
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.viewDirectionTrackBar );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.heightTrackBar );
			this.Controls.Add( this.yAxisLegendPanel );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.xAxisSourceComboBox );
			this.Controls.Add( this.atmosphereGraph );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ScatteringAtmosphereAnalysisForm";
			this.Text = "Atmosphere Analysis";
			this.Load += new System.EventHandler( this.ScatteringAtmosphereAnalysisForm_Load );
			( ( System.ComponentModel.ISupportInitialize )( this.heightTrackBar ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.viewDirectionTrackBar ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.sunDirectionTrackBar ) ).EndInit( );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private Rb.Common.Controls.Forms.Graphs.GraphControl atmosphereGraph;
		private System.Windows.Forms.ComboBox xAxisSourceComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel yAxisLegendPanel;
		private System.Windows.Forms.TrackBar heightTrackBar;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TrackBar viewDirectionTrackBar;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar sunDirectionTrackBar;
		private System.Windows.Forms.Panel gradientPanel;
	}
}