namespace Poc1.PlanetBuilder
{
	partial class GroundTextureControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Rb.Core.Maths.LineFunction1d lineFunction1d11 = new Rb.Core.Maths.LineFunction1d( );
			Rb.Core.Maths.LineFunction1d lineFunction1d12 = new Rb.Core.Maths.LineFunction1d( );
			this.groundTypeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.groupBox1 = new System.Windows.Forms.GroupBox( );
			this.altitudeGraphEditorControl = new Rb.NiceControls.GraphEditorControl( );
			this.splitter2 = new System.Windows.Forms.Splitter( );
			this.groupBox2 = new System.Windows.Forms.GroupBox( );
			this.slopeGraphEditorControl = new Rb.NiceControls.GraphEditorControl( );
			this.panel1 = new System.Windows.Forms.Panel( );
			this.saveAsButton = new System.Windows.Forms.Button( );
			this.saveButton = new System.Windows.Forms.Button( );
			this.loadButton = new System.Windows.Forms.Button( );
			this.exportButton = new System.Windows.Forms.Button( );
			this.exportAsButton = new System.Windows.Forms.Button( );
			this.groupBox1.SuspendLayout( );
			this.groupBox2.SuspendLayout( );
			this.panel1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// groundTypeTableLayoutPanel
			// 
			this.groundTypeTableLayoutPanel.AutoScroll = true;
			this.groundTypeTableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.groundTypeTableLayoutPanel.ColumnCount = 1;
			this.groundTypeTableLayoutPanel.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( ) );
			this.groundTypeTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.groundTypeTableLayoutPanel.Location = new System.Drawing.Point( 0, 0 );
			this.groundTypeTableLayoutPanel.Name = "groundTypeTableLayoutPanel";
			this.groundTypeTableLayoutPanel.RowCount = 1;
			this.groundTypeTableLayoutPanel.RowStyles.Add( new System.Windows.Forms.RowStyle( ) );
			this.groundTypeTableLayoutPanel.Size = new System.Drawing.Size( 236, 99 );
			this.groundTypeTableLayoutPanel.TabIndex = 0;
			this.groundTypeTableLayoutPanel.MouseClick += new System.Windows.Forms.MouseEventHandler( this.groundTypeTableLayoutPanel_MouseClick );
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point( 0, 99 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 236, 3 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add( this.altitudeGraphEditorControl );
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point( 0, 102 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 236, 121 );
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Altitude Distribution";
			// 
			// altitudeGraphEditorControl
			// 
			this.altitudeGraphEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.altitudeGraphEditorControl.Function = lineFunction1d11;
			this.altitudeGraphEditorControl.Location = new System.Drawing.Point( 3, 16 );
			this.altitudeGraphEditorControl.Name = "altitudeGraphEditorControl";
			this.altitudeGraphEditorControl.Size = new System.Drawing.Size( 230, 102 );
			this.altitudeGraphEditorControl.TabIndex = 0;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point( 0, 223 );
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size( 236, 3 );
			this.splitter2.TabIndex = 3;
			this.splitter2.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add( this.slopeGraphEditorControl );
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point( 0, 226 );
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size( 236, 124 );
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Slope Distribution";
			// 
			// slopeGraphEditorControl
			// 
			this.slopeGraphEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.slopeGraphEditorControl.Function = lineFunction1d12;
			this.slopeGraphEditorControl.Location = new System.Drawing.Point( 3, 16 );
			this.slopeGraphEditorControl.Name = "slopeGraphEditorControl";
			this.slopeGraphEditorControl.Size = new System.Drawing.Size( 230, 105 );
			this.slopeGraphEditorControl.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add( this.exportAsButton );
			this.panel1.Controls.Add( this.exportButton );
			this.panel1.Controls.Add( this.saveAsButton );
			this.panel1.Controls.Add( this.saveButton );
			this.panel1.Controls.Add( this.loadButton );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point( 0, 350 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 236, 53 );
			this.panel1.TabIndex = 5;
			// 
			// saveAsButton
			// 
			this.saveAsButton.Location = new System.Drawing.Point( 158, 0 );
			this.saveAsButton.Name = "saveAsButton";
			this.saveAsButton.Size = new System.Drawing.Size( 75, 23 );
			this.saveAsButton.TabIndex = 2;
			this.saveAsButton.Text = "Save As...";
			this.saveAsButton.UseVisualStyleBackColor = true;
			this.saveAsButton.Click += new System.EventHandler( this.saveAsButton_Click );
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point( 80, 0 );
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size( 75, 23 );
			this.saveButton.TabIndex = 1;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler( this.saveButton_Click );
			// 
			// loadButton
			// 
			this.loadButton.Location = new System.Drawing.Point( 0, 0 );
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size( 75, 23 );
			this.loadButton.TabIndex = 0;
			this.loadButton.Text = "Load";
			this.loadButton.UseVisualStyleBackColor = true;
			this.loadButton.Click += new System.EventHandler( this.loadButton_Click );
			// 
			// exportButton
			// 
			this.exportButton.Location = new System.Drawing.Point( 80, 23 );
			this.exportButton.Name = "exportButton";
			this.exportButton.Size = new System.Drawing.Size( 75, 23 );
			this.exportButton.TabIndex = 3;
			this.exportButton.Text = "Export";
			this.exportButton.UseVisualStyleBackColor = true;
			this.exportButton.Click += new System.EventHandler( this.exportButton_Click );
			// 
			// exportAsButton
			// 
			this.exportAsButton.Location = new System.Drawing.Point( 158, 23 );
			this.exportAsButton.Name = "exportAsButton";
			this.exportAsButton.Size = new System.Drawing.Size( 75, 23 );
			this.exportAsButton.TabIndex = 4;
			this.exportAsButton.Text = "Export As...";
			this.exportAsButton.UseVisualStyleBackColor = true;
			this.exportAsButton.Click += new System.EventHandler( this.exportAsButton_Click );
			// 
			// GroundTextureControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.groupBox2 );
			this.Controls.Add( this.splitter2 );
			this.Controls.Add( this.groupBox1 );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.groundTypeTableLayoutPanel );
			this.Controls.Add( this.panel1 );
			this.Name = "GroundTextureControl";
			this.Size = new System.Drawing.Size( 236, 403 );
			this.groupBox1.ResumeLayout( false );
			this.groupBox2.ResumeLayout( false );
			this.panel1.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel groundTypeTableLayoutPanel;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.GroupBox groupBox2;
		private Rb.NiceControls.GraphEditorControl altitudeGraphEditorControl;
		private Rb.NiceControls.GraphEditorControl slopeGraphEditorControl;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button saveAsButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.Button exportButton;
		private System.Windows.Forms.Button exportAsButton;
	}
}
