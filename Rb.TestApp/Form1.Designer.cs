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
            this.logListView1 = new Rb.Log.Controls.LogListView();
            this.SuspendLayout();
            // 
            // logListView1
            // 
            this.logListView1.AllowColumnReorder = true;
            this.logListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logListView1.FullRowSelect = true;
            this.logListView1.GridLines = true;
            this.logListView1.Location = new System.Drawing.Point(0, 0);
            this.logListView1.MultiSelect = false;
            this.logListView1.Name = "logListView1";
            this.logListView1.OwnerDraw = true;
            this.logListView1.Size = new System.Drawing.Size(648, 258);
            this.logListView1.TabIndex = 0;
            this.logListView1.UseCompatibleStateImageBehavior = false;
            this.logListView1.View = System.Windows.Forms.View.Details;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 258);
            this.Controls.Add(this.logListView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

		private Rb.Log.Controls.LogListView logListView1;
    }
}

