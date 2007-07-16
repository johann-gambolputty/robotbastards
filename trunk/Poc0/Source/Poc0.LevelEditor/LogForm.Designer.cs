

namespace Poc0.LevelEditor
{
	partial class LogForm
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
			this.vsLogListView1 = new Rb.Log.Controls.Vs.VsLogListView( );
			this.SuspendLayout( );
			// 
			// vsLogListView1
			// 
			this.vsLogListView1.AllowColumnReorder = true;
			this.vsLogListView1.AllowDrop = true;
			this.vsLogListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vsLogListView1.ErrorColour = System.Drawing.Color.Red;
			this.vsLogListView1.FullRowSelect = true;
			this.vsLogListView1.InfoColour = System.Drawing.Color.BlanchedAlmond;
			this.vsLogListView1.Location = new System.Drawing.Point( 0, 0 );
			this.vsLogListView1.MultiSelect = false;
			this.vsLogListView1.Name = "vsLogListView1";
			this.vsLogListView1.OwnerDraw = true;
			this.vsLogListView1.Size = new System.Drawing.Size( 499, 239 );
			this.vsLogListView1.TabIndex = 0;
			this.vsLogListView1.UseCompatibleStateImageBehavior = false;
			this.vsLogListView1.VerboseColour = System.Drawing.Color.White;
			this.vsLogListView1.View = System.Windows.Forms.View.Details;
			this.vsLogListView1.WarningColour = System.Drawing.Color.Orange;
			// 
			// LogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 499, 239 );
			this.Controls.Add( this.vsLogListView1 );
			this.Name = "LogForm";
			this.Text = "Log";
			this.ResumeLayout( false );

		}

		#endregion

		private Rb.Log.Controls.Vs.VsLogListView vsLogListView1;
	}
}