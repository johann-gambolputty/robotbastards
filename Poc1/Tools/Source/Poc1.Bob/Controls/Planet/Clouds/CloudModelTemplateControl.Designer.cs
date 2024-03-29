namespace Poc1.Bob.Controls.Planet.Clouds
{
	partial class CloudModelTemplateControl
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
			this.lowCoverageRangeSlider = new Poc1.Bob.Controls.RangeSlider( );
			this.highCoverRangeSlider = new Poc1.Bob.Controls.RangeSlider( );
			this.cloudLayerHeightRangeSlider = new Poc1.Bob.Controls.RangeSlider( );
			this.label1 = new System.Windows.Forms.Label( );
			this.label2 = new System.Windows.Forms.Label( );
			this.label3 = new System.Windows.Forms.Label( );
			this.SuspendLayout( );
			// 
			// lowCoverageRangeSlider
			// 
			this.lowCoverageRangeSlider.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.lowCoverageRangeSlider.Location = new System.Drawing.Point( 123, 0 );
			this.lowCoverageRangeSlider.MaxValue = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			this.lowCoverageRangeSlider.MinValue = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			this.lowCoverageRangeSlider.Name = "lowCoverageRangeSlider";
			this.lowCoverageRangeSlider.Size = new System.Drawing.Size( 194, 42 );
			this.lowCoverageRangeSlider.SliderEnabled = true;
			this.lowCoverageRangeSlider.TabIndex = 0;
			this.lowCoverageRangeSlider.Value = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			// 
			// highCoverRangeSlider
			// 
			this.highCoverRangeSlider.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.highCoverRangeSlider.Location = new System.Drawing.Point( 125, 48 );
			this.highCoverRangeSlider.MaxValue = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			this.highCoverRangeSlider.MinValue = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			this.highCoverRangeSlider.Name = "highCoverRangeSlider";
			this.highCoverRangeSlider.Size = new System.Drawing.Size( 192, 42 );
			this.highCoverRangeSlider.SliderEnabled = true;
			this.highCoverRangeSlider.TabIndex = 1;
			this.highCoverRangeSlider.Value = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			// 
			// cloudLayerHeightRangeSlider
			// 
			this.cloudLayerHeightRangeSlider.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.cloudLayerHeightRangeSlider.Location = new System.Drawing.Point( 123, 96 );
			this.cloudLayerHeightRangeSlider.MaxValue = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			this.cloudLayerHeightRangeSlider.MinValue = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			this.cloudLayerHeightRangeSlider.Name = "cloudLayerHeightRangeSlider";
			this.cloudLayerHeightRangeSlider.Size = new System.Drawing.Size( 194, 42 );
			this.cloudLayerHeightRangeSlider.SliderEnabled = true;
			this.cloudLayerHeightRangeSlider.TabIndex = 2;
			this.cloudLayerHeightRangeSlider.Value = new decimal( new int[] {
            0,
            0,
            0,
            0} );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 5, 12 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 114, 13 );
			this.label1.TabIndex = 3;
			this.label1.Text = "Low Coverage Range:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 3, 61 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 116, 13 );
			this.label2.TabIndex = 4;
			this.label2.Text = "High Coverage Range:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 19, 113 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 100, 13 );
			this.label3.TabIndex = 5;
			this.label3.Text = "Cloud Layer Height:";
			// 
			// CloudModelTemplateControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.label3 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.cloudLayerHeightRangeSlider );
			this.Controls.Add( this.highCoverRangeSlider );
			this.Controls.Add( this.lowCoverageRangeSlider );
			this.Name = "CloudModelTemplateControl";
			this.Size = new System.Drawing.Size( 320, 143 );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private RangeSlider lowCoverageRangeSlider;
		private RangeSlider highCoverRangeSlider;
		private RangeSlider cloudLayerHeightRangeSlider;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;

	}
}
