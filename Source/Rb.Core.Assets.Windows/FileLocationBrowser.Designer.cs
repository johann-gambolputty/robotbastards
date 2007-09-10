namespace Rb.Core.Assets.Windows
{
	partial class FileLocationBrowser
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
			this.treeView1 = new System.Windows.Forms.TreeView( );
			this.comboBox1 = new System.Windows.Forms.ComboBox( );
			this.textBox1 = new System.Windows.Forms.TextBox( );
			this.comboBox2 = new System.Windows.Forms.ComboBox( );
			this.button1 = new System.Windows.Forms.Button( );
			this.button2 = new System.Windows.Forms.Button( );
			this.SuspendLayout( );
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point( 3, 27 );
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size( 233, 108 );
			this.treeView1.TabIndex = 0;
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point( 39, 166 );
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size( 197, 21 );
			this.comboBox1.TabIndex = 1;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point( 39, 141 );
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size( 197, 20 );
			this.textBox1.TabIndex = 2;
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point( 3, 3 );
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size( 173, 21 );
			this.comboBox2.TabIndex = 3;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point( 182, 3 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 24, 22 );
			this.button1.TabIndex = 4;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point( 212, 3 );
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size( 24, 22 );
			this.button2.TabIndex = 5;
			this.button2.Text = "button2";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// FileLocationBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.button2 );
			this.Controls.Add( this.button1 );
			this.Controls.Add( this.comboBox2 );
			this.Controls.Add( this.textBox1 );
			this.Controls.Add( this.comboBox1 );
			this.Controls.Add( this.treeView1 );
			this.Name = "FileLocationBrowser";
			this.Size = new System.Drawing.Size( 239, 190 );
			this.ResumeLayout( false );
			this.PerformLayout( );

		}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}
