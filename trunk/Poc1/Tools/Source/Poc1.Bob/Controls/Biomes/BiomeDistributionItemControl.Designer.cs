namespace Poc1.Bob.Controls.Biomes
{
	partial class BiomeDistributionItemControl
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
			this.upButton = new System.Windows.Forms.Button( );
			this.biomeName = new System.Windows.Forms.Label( );
			this.downButton = new System.Windows.Forms.Button( );
			this.SuspendLayout( );
			// 
			// upButton
			// 
			this.upButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.upButton.Location = new System.Drawing.Point( 14, 6 );
			this.upButton.Name = "upButton";
			this.upButton.Size = new System.Drawing.Size( 24, 16 );
			this.upButton.TabIndex = 0;
			this.upButton.UseVisualStyleBackColor = true;
			this.upButton.Click += new System.EventHandler( this.upButton_Click );
			// 
			// biomeName
			// 
			this.biomeName.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.biomeName.AutoSize = true;
			this.biomeName.Location = new System.Drawing.Point( 44, 19 );
			this.biomeName.Name = "biomeName";
			this.biomeName.Size = new System.Drawing.Size( 35, 13 );
			this.biomeName.TabIndex = 2;
			this.biomeName.Text = "label1";
			// 
			// downButton
			// 
			this.downButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.downButton.Location = new System.Drawing.Point( 14, 28 );
			this.downButton.Name = "downButton";
			this.downButton.Size = new System.Drawing.Size( 24, 16 );
			this.downButton.TabIndex = 3;
			this.downButton.UseVisualStyleBackColor = true;
			this.downButton.Click += new System.EventHandler( this.downButton_Click );
			// 
			// BiomeDistributionItemControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.downButton );
			this.Controls.Add( this.biomeName );
			this.Controls.Add( this.upButton );
			this.Name = "BiomeDistributionItemControl";
			this.Size = new System.Drawing.Size( 130, 50 );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.Button upButton;
		private System.Windows.Forms.Label biomeName;
		private System.Windows.Forms.Button downButton;
	}
}
