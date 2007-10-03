namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	partial class NewObjectControl
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
			this.typeView = new System.Windows.Forms.ListBox( );
			this.label1 = new System.Windows.Forms.Label( );
			this.textBox1 = new System.Windows.Forms.TextBox( );
			this.SuspendLayout( );
			// 
			// typeView
			// 
			this.typeView.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.typeView.Location = new System.Drawing.Point( 3, 3 );
			this.typeView.Name = "typeView";
			this.typeView.Size = new System.Drawing.Size( 198, 147 );
			this.typeView.TabIndex = 0;
			this.typeView.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.typeView_DrawItem );
			this.typeView.DoubleClick += new System.EventHandler( this.typeView_DoubleClick );
			// 
			// label1
			// 
			this.label1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 3, 156 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 38, 13 );
			this.label1.TabIndex = 1;
			this.label1.Text = "Name:";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.textBox1.Location = new System.Drawing.Point( 47, 153 );
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size( 154, 20 );
			this.textBox1.TabIndex = 2;
			// 
			// NewObjectControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.textBox1 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.typeView );
			this.Name = "NewObjectControl";
			this.Size = new System.Drawing.Size( 204, 194 );
			this.Load += new System.EventHandler( this.NewObjectControl_Load );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.ListBox typeView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
	}
}
