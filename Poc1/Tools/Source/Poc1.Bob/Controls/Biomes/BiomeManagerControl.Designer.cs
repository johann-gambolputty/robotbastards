namespace Poc1.Bob.Controls.Biomes
{
	partial class BiomeManagerControl
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.prettyTabControl1 = new Rb.Common.Controls.Tabs.PrettyTabControl( );
			this.texturingTabPage = new System.Windows.Forms.TabPage( );
			this.vegetationTabPage = new System.Windows.Forms.TabPage( );
			this.prettyTabControl1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tableLayoutPanel1.Location = new System.Drawing.Point( 0, 0 );
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 50F ) );
			this.tableLayoutPanel1.Size = new System.Drawing.Size( 200, 280 );
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 200, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 3, 280 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// prettyTabControl1
			// 
			this.prettyTabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
			this.prettyTabControl1.Controls.Add( this.texturingTabPage );
			this.prettyTabControl1.Controls.Add( this.vegetationTabPage );
			this.prettyTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.prettyTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.prettyTabControl1.ItemSize = new System.Drawing.Size( 30, 100 );
			this.prettyTabControl1.Location = new System.Drawing.Point( 203, 0 );
			this.prettyTabControl1.Multiline = true;
			this.prettyTabControl1.Name = "prettyTabControl1";
			this.prettyTabControl1.SelectedIndex = 0;
			this.prettyTabControl1.Size = new System.Drawing.Size( 433, 280 );
			this.prettyTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.prettyTabControl1.TabIndex = 2;
			// 
			// texturingTabPage
			// 
			this.texturingTabPage.Location = new System.Drawing.Point( 104, 4 );
			this.texturingTabPage.Name = "texturingTabPage";
			this.texturingTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.texturingTabPage.Size = new System.Drawing.Size( 325, 272 );
			this.texturingTabPage.TabIndex = 0;
			this.texturingTabPage.Text = "Texturing";
			this.texturingTabPage.UseVisualStyleBackColor = true;
			// 
			// vegetationTabPage
			// 
			this.vegetationTabPage.Location = new System.Drawing.Point( 104, 4 );
			this.vegetationTabPage.Name = "vegetationTabPage";
			this.vegetationTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.vegetationTabPage.Size = new System.Drawing.Size( 325, 272 );
			this.vegetationTabPage.TabIndex = 1;
			this.vegetationTabPage.Text = "Vegetation";
			this.vegetationTabPage.UseVisualStyleBackColor = true;
			// 
			// BiomeManagerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.prettyTabControl1 );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.tableLayoutPanel1 );
			this.Name = "BiomeManagerControl";
			this.Size = new System.Drawing.Size( 636, 280 );
			this.prettyTabControl1.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Splitter splitter1;
		private Rb.Common.Controls.Tabs.PrettyTabControl prettyTabControl1;
		private System.Windows.Forms.TabPage texturingTabPage;
		private System.Windows.Forms.TabPage vegetationTabPage;


	}
}
