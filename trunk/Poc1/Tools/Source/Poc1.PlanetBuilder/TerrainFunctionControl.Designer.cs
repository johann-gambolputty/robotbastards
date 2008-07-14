namespace Poc1.PlanetBuilder
{
	partial class TerrainFunctionControl
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
			this.heightFunctionGroupBox = new System.Windows.Forms.GroupBox( );
			this.heightFunctionPropertyGrid = new System.Windows.Forms.PropertyGrid( );
			this.label1 = new System.Windows.Forms.Label( );
			this.heightFunctionComboBox = new System.Windows.Forms.ComboBox( );
			this.regenerateMeshButton = new System.Windows.Forms.Button( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.panel1 = new System.Windows.Forms.Panel( );
			this.groundOffsetEnableCheckBox = new System.Windows.Forms.CheckBox( );
			this.groundOffsetFunctionGroupBox = new System.Windows.Forms.GroupBox( );
			this.groundFunctionPropertyGrid = new System.Windows.Forms.PropertyGrid( );
			this.label2 = new System.Windows.Forms.Label( );
			this.groundFunctionComboBox = new System.Windows.Forms.ComboBox( );
			this.heightFunctionGroupBox.SuspendLayout( );
			this.panel1.SuspendLayout( );
			this.groundOffsetFunctionGroupBox.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// heightFunctionGroupBox
			// 
			this.heightFunctionGroupBox.Controls.Add( this.heightFunctionPropertyGrid );
			this.heightFunctionGroupBox.Controls.Add( this.label1 );
			this.heightFunctionGroupBox.Controls.Add( this.heightFunctionComboBox );
			this.heightFunctionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.heightFunctionGroupBox.Location = new System.Drawing.Point( 0, 0 );
			this.heightFunctionGroupBox.Name = "heightFunctionGroupBox";
			this.heightFunctionGroupBox.Size = new System.Drawing.Size( 217, 134 );
			this.heightFunctionGroupBox.TabIndex = 0;
			this.heightFunctionGroupBox.TabStop = false;
			this.heightFunctionGroupBox.Text = "Height Function";
			// 
			// heightFunctionPropertyGrid
			// 
			this.heightFunctionPropertyGrid.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.heightFunctionPropertyGrid.HelpVisible = false;
			this.heightFunctionPropertyGrid.Location = new System.Drawing.Point( 10, 46 );
			this.heightFunctionPropertyGrid.Name = "heightFunctionPropertyGrid";
			this.heightFunctionPropertyGrid.Size = new System.Drawing.Size( 201, 82 );
			this.heightFunctionPropertyGrid.TabIndex = 2;
			this.heightFunctionPropertyGrid.ToolbarVisible = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 7, 22 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 51, 13 );
			this.label1.TabIndex = 1;
			this.label1.Text = "Function:";
			// 
			// heightFunctionComboBox
			// 
			this.heightFunctionComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.heightFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.heightFunctionComboBox.FormattingEnabled = true;
			this.heightFunctionComboBox.Location = new System.Drawing.Point( 64, 19 );
			this.heightFunctionComboBox.Name = "heightFunctionComboBox";
			this.heightFunctionComboBox.Size = new System.Drawing.Size( 147, 21 );
			this.heightFunctionComboBox.TabIndex = 0;
			this.heightFunctionComboBox.SelectedIndexChanged += new System.EventHandler( this.heightFunctionComboBox_SelectedIndexChanged );
			// 
			// regenerateMeshButton
			// 
			this.regenerateMeshButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.regenerateMeshButton.Location = new System.Drawing.Point( 0, 308 );
			this.regenerateMeshButton.Name = "regenerateMeshButton";
			this.regenerateMeshButton.Size = new System.Drawing.Size( 217, 23 );
			this.regenerateMeshButton.TabIndex = 2;
			this.regenerateMeshButton.Text = "Regenerate Mesh";
			this.regenerateMeshButton.UseVisualStyleBackColor = true;
			this.regenerateMeshButton.Click += new System.EventHandler( this.regenerateMeshButton_Click );
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point( 0, 134 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 217, 5 );
			this.splitter1.TabIndex = 3;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.groundOffsetEnableCheckBox );
			this.panel1.Controls.Add( this.groundOffsetFunctionGroupBox );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point( 0, 139 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 217, 169 );
			this.panel1.TabIndex = 4;
			// 
			// groundOffsetEnableCheckBox
			// 
			this.groundOffsetEnableCheckBox.AutoSize = true;
			this.groundOffsetEnableCheckBox.Location = new System.Drawing.Point( 6, 1 );
			this.groundOffsetEnableCheckBox.Name = "groundOffsetEnableCheckBox";
			this.groundOffsetEnableCheckBox.Size = new System.Drawing.Size( 15, 14 );
			this.groundOffsetEnableCheckBox.TabIndex = 6;
			this.groundOffsetEnableCheckBox.UseVisualStyleBackColor = true;
			this.groundOffsetEnableCheckBox.CheckedChanged += new System.EventHandler( this.groundOffsetEnableCheckBox_CheckedChanged );
			// 
			// groundOffsetFunctionGroupBox
			// 
			this.groundOffsetFunctionGroupBox.Controls.Add( this.groundFunctionPropertyGrid );
			this.groundOffsetFunctionGroupBox.Controls.Add( this.label2 );
			this.groundOffsetFunctionGroupBox.Controls.Add( this.groundFunctionComboBox );
			this.groundOffsetFunctionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groundOffsetFunctionGroupBox.Enabled = false;
			this.groundOffsetFunctionGroupBox.Location = new System.Drawing.Point( 0, 0 );
			this.groundOffsetFunctionGroupBox.Name = "groundOffsetFunctionGroupBox";
			this.groundOffsetFunctionGroupBox.Size = new System.Drawing.Size( 217, 169 );
			this.groundOffsetFunctionGroupBox.TabIndex = 5;
			this.groundOffsetFunctionGroupBox.TabStop = false;
			this.groundOffsetFunctionGroupBox.Text = "     Ground Offset Function";
			// 
			// groundFunctionPropertyGrid
			// 
			this.groundFunctionPropertyGrid.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.groundFunctionPropertyGrid.HelpVisible = false;
			this.groundFunctionPropertyGrid.Location = new System.Drawing.Point( 6, 50 );
			this.groundFunctionPropertyGrid.Name = "groundFunctionPropertyGrid";
			this.groundFunctionPropertyGrid.Size = new System.Drawing.Size( 204, 113 );
			this.groundFunctionPropertyGrid.TabIndex = 5;
			this.groundFunctionPropertyGrid.ToolbarVisible = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 3, 26 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 51, 13 );
			this.label2.TabIndex = 4;
			this.label2.Text = "Function:";
			// 
			// groundFunctionComboBox
			// 
			this.groundFunctionComboBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.groundFunctionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.groundFunctionComboBox.FormattingEnabled = true;
			this.groundFunctionComboBox.Location = new System.Drawing.Point( 60, 23 );
			this.groundFunctionComboBox.Name = "groundFunctionComboBox";
			this.groundFunctionComboBox.Size = new System.Drawing.Size( 150, 21 );
			this.groundFunctionComboBox.TabIndex = 3;
			this.groundFunctionComboBox.SelectedIndexChanged += new System.EventHandler( this.groundFunctionComboBox_SelectedIndexChanged );
			// 
			// TerrainFunctionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.panel1 );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.regenerateMeshButton );
			this.Controls.Add( this.heightFunctionGroupBox );
			this.Name = "TerrainFunctionControl";
			this.Size = new System.Drawing.Size( 217, 331 );
			this.Load += new System.EventHandler( this.TerrainFunctionControl_Load );
			this.heightFunctionGroupBox.ResumeLayout( false );
			this.heightFunctionGroupBox.PerformLayout( );
			this.panel1.ResumeLayout( false );
			this.panel1.PerformLayout( );
			this.groundOffsetFunctionGroupBox.ResumeLayout( false );
			this.groundOffsetFunctionGroupBox.PerformLayout( );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.GroupBox heightFunctionGroupBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox heightFunctionComboBox;
		private System.Windows.Forms.PropertyGrid heightFunctionPropertyGrid;
		private System.Windows.Forms.Button regenerateMeshButton;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groundOffsetFunctionGroupBox;
		private System.Windows.Forms.CheckBox groundOffsetEnableCheckBox;
		private System.Windows.Forms.PropertyGrid groundFunctionPropertyGrid;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox groundFunctionComboBox;
	}
}
