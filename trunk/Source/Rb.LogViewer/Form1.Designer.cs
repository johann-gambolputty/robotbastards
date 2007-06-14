namespace Rb.LogViewer
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
            this.m_logView = new Rb.Log.Controls.Vs.VsLogListView();
            this.SuspendLayout();
            // 
            // m_logView
            // 
            this.m_logView.AllowColumnReorder = true;
            this.m_logView.AllowDrop = true;
            this.m_logView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_logView.ErrorColour = System.Drawing.Color.Red;
            this.m_logView.FullRowSelect = true;
            this.m_logView.InfoColour = System.Drawing.Color.BlanchedAlmond;
            this.m_logView.Location = new System.Drawing.Point(0, 0);
            this.m_logView.MultiSelect = false;
            this.m_logView.Name = "m_logView";
            this.m_logView.OwnerDraw = true;
            this.m_logView.Size = new System.Drawing.Size(733, 284);
            this.m_logView.TabIndex = 0;
            this.m_logView.Text = "";
            this.m_logView.UseCompatibleStateImageBehavior = false;
            this.m_logView.VerboseColour = System.Drawing.Color.White;
            this.m_logView.View = System.Windows.Forms.View.Details;
            this.m_logView.WarningColour = System.Drawing.Color.Orange;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(733, 284);
            this.Controls.Add(this.m_logView);
            this.Name = "Form1";
            this.Text = "Log viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private Rb.Log.Controls.Vs.VsLogListView m_logView;
    }
}

