namespace Poc1.Bob.Controls
{
	partial class RangeSlider
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
			this.minValueUpDown = new System.Windows.Forms.NumericUpDown( );
			this.maxValueUpDown = new System.Windows.Forms.NumericUpDown( );
			this.valueScrollBar = new System.Windows.Forms.HScrollBar( );
			this.valueLabel = new System.Windows.Forms.Label( );
			( ( System.ComponentModel.ISupportInitialize )( this.minValueUpDown ) ).BeginInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.maxValueUpDown ) ).BeginInit( );
			this.SuspendLayout( );
			// 
			// minValueUpDown
			// 
			this.minValueUpDown.DecimalPlaces = 2;
			this.minValueUpDown.Location = new System.Drawing.Point( 0, 20 );
			this.minValueUpDown.Maximum = new decimal( new int[] {
            1000000000,
            0,
            0,
            0} );
			this.minValueUpDown.Minimum = new decimal( new int[] {
            1000000000,
            0,
            0,
            -2147483648} );
			this.minValueUpDown.Name = "minValueUpDown";
			this.minValueUpDown.Size = new System.Drawing.Size( 58, 20 );
			this.minValueUpDown.TabIndex = 0;
			this.minValueUpDown.ValueChanged += new System.EventHandler( this.minValueUpDown_ValueChanged );
			// 
			// maxValueUpDown
			// 
			this.maxValueUpDown.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.maxValueUpDown.DecimalPlaces = 2;
			this.maxValueUpDown.Location = new System.Drawing.Point( 133, 20 );
			this.maxValueUpDown.Maximum = new decimal( new int[] {
            1000000000,
            0,
            0,
            0} );
			this.maxValueUpDown.Minimum = new decimal( new int[] {
            1000000000,
            0,
            0,
            -2147483648} );
			this.maxValueUpDown.Name = "maxValueUpDown";
			this.maxValueUpDown.Size = new System.Drawing.Size( 60, 20 );
			this.maxValueUpDown.TabIndex = 1;
			this.maxValueUpDown.ValueChanged += new System.EventHandler( this.maxValueUpDown_ValueChanged );
			// 
			// valueScrollBar
			// 
			this.valueScrollBar.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.valueScrollBar.Location = new System.Drawing.Point( 0, 0 );
			this.valueScrollBar.Name = "valueScrollBar";
			this.valueScrollBar.Size = new System.Drawing.Size( 193, 17 );
			this.valueScrollBar.TabIndex = 3;
			this.valueScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler( this.valueScrollBar_Scroll );
			// 
			// valueLabel
			// 
			this.valueLabel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.valueLabel.Location = new System.Drawing.Point( 64, 20 );
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size( 63, 18 );
			this.valueLabel.TabIndex = 4;
			this.valueLabel.Text = "0";
			this.valueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// RangeSlider
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.valueLabel );
			this.Controls.Add( this.valueScrollBar );
			this.Controls.Add( this.maxValueUpDown );
			this.Controls.Add( this.minValueUpDown );
			this.Name = "RangeSlider";
			this.Size = new System.Drawing.Size( 193, 42 );
			( ( System.ComponentModel.ISupportInitialize )( this.minValueUpDown ) ).EndInit( );
			( ( System.ComponentModel.ISupportInitialize )( this.maxValueUpDown ) ).EndInit( );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.NumericUpDown minValueUpDown;
		private System.Windows.Forms.NumericUpDown maxValueUpDown;
		private System.Windows.Forms.HScrollBar valueScrollBar;
		private System.Windows.Forms.Label valueLabel;
	}
}
