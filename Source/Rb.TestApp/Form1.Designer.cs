namespace Rb.TestApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.display1 = new Rb.Rendering.Windows.Display( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.logListView1 = new Rb.Log.Controls.Vs.VsLogListView( );
			this.SuspendLayout( );
			// 
			// display1
			// 
			this.display1.Dock = System.Windows.Forms.DockStyle.Top;
			this.display1.Location = new System.Drawing.Point( 0, 0 );
			this.display1.Name = "display1";
			this.display1.Size = new System.Drawing.Size( 648, 266 );
			this.display1.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point( 0, 266 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 648, 3 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// logListView1
			// 
			this.logListView1.AllowColumnReorder = true;
			this.logListView1.AllowDrop = true;
			this.logListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.logListView1.ErrorColour = System.Drawing.Color.Red;
			this.logListView1.FullRowSelect = true;
			this.logListView1.InfoColour = System.Drawing.Color.BlanchedAlmond;
			this.logListView1.Location = new System.Drawing.Point( 0, 269 );
			this.logListView1.MultiSelect = false;
			this.logListView1.Name = "logListView1";
			this.logListView1.OwnerDraw = true;
			this.logListView1.Size = new System.Drawing.Size( 648, 154 );
			this.logListView1.TabIndex = 2;
			this.logListView1.UseCompatibleStateImageBehavior = false;
			this.logListView1.VerboseColour = System.Drawing.Color.White;
			this.logListView1.View = System.Windows.Forms.View.Details;
			this.logListView1.WarningColour = System.Drawing.Color.Orange;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 648, 423 );
			this.Controls.Add( this.logListView1 );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.display1 );
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler( this.Form1_Load );
			this.ResumeLayout( false );

        }

        #endregion

		private Rb.Rendering.Windows.Display display1;
		private System.Windows.Forms.Splitter splitter1;
		private Rb.Log.Controls.Vs.VsLogListView logListView1;


	}
}

