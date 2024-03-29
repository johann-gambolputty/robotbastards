namespace Poc1.PlanetBuilder
{
	partial class TerrainVisualiserControl
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
			this.groupBox1 = new System.Windows.Forms.GroupBox( );
			this.showTerrainTypesRadioButton = new System.Windows.Forms.RadioButton( );
			this.showSlopesRadioButton = new System.Windows.Forms.RadioButton( );
			this.showHeightsRadioButton = new System.Windows.Forms.RadioButton( );
			this.displayPanel = new System.Windows.Forms.Panel( );
			this.label1 = new System.Windows.Forms.Label( );
			this.resolutionComboBox = new System.Windows.Forms.ComboBox( );
			this.terrainDisplay = new Rb.Rendering.Windows.Display( );
			this.groupBox1.SuspendLayout( );
			this.displayPanel.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add( this.resolutionComboBox );
			this.groupBox1.Controls.Add( this.label1 );
			this.groupBox1.Controls.Add( this.showTerrainTypesRadioButton );
			this.groupBox1.Controls.Add( this.showSlopesRadioButton );
			this.groupBox1.Controls.Add( this.showHeightsRadioButton );
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point( 0, 0 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 289, 92 );
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Controls";
			// 
			// showTerrainTypesRadioButton
			// 
			this.showTerrainTypesRadioButton.AutoSize = true;
			this.showTerrainTypesRadioButton.Location = new System.Drawing.Point( 6, 65 );
			this.showTerrainTypesRadioButton.Name = "showTerrainTypesRadioButton";
			this.showTerrainTypesRadioButton.Size = new System.Drawing.Size( 112, 17 );
			this.showTerrainTypesRadioButton.TabIndex = 2;
			this.showTerrainTypesRadioButton.TabStop = true;
			this.showTerrainTypesRadioButton.Text = "Show terrain types";
			this.showTerrainTypesRadioButton.UseVisualStyleBackColor = true;
			this.showTerrainTypesRadioButton.CheckedChanged += new System.EventHandler( this.showTerrainTypesRadioButton_CheckedChanged );
			// 
			// showSlopesRadioButton
			// 
			this.showSlopesRadioButton.AutoSize = true;
			this.showSlopesRadioButton.Location = new System.Drawing.Point( 6, 42 );
			this.showSlopesRadioButton.Name = "showSlopesRadioButton";
			this.showSlopesRadioButton.Size = new System.Drawing.Size( 85, 17 );
			this.showSlopesRadioButton.TabIndex = 1;
			this.showSlopesRadioButton.TabStop = true;
			this.showSlopesRadioButton.Text = "Show slopes";
			this.showSlopesRadioButton.UseVisualStyleBackColor = true;
			this.showSlopesRadioButton.CheckedChanged += new System.EventHandler( this.showSlopesRadioButton_CheckedChanged );
			// 
			// showHeightsRadioButton
			// 
			this.showHeightsRadioButton.AutoSize = true;
			this.showHeightsRadioButton.Checked = true;
			this.showHeightsRadioButton.Location = new System.Drawing.Point( 6, 19 );
			this.showHeightsRadioButton.Name = "showHeightsRadioButton";
			this.showHeightsRadioButton.Size = new System.Drawing.Size( 89, 17 );
			this.showHeightsRadioButton.TabIndex = 0;
			this.showHeightsRadioButton.TabStop = true;
			this.showHeightsRadioButton.Text = "Show heights";
			this.showHeightsRadioButton.UseVisualStyleBackColor = true;
			this.showHeightsRadioButton.CheckedChanged += new System.EventHandler( this.showHeightsRadioButton_CheckedChanged );
			// 
			// displayPanel
			// 
			this.displayPanel.Controls.Add( this.terrainDisplay );
			this.displayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.displayPanel.Location = new System.Drawing.Point( 0, 92 );
			this.displayPanel.Name = "displayPanel";
			this.displayPanel.Size = new System.Drawing.Size( 289, 241 );
			this.displayPanel.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 129, 21 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 60, 13 );
			this.label1.TabIndex = 3;
			this.label1.Text = "Resolution:";
			// 
			// resolutionComboBox
			// 
			this.resolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.resolutionComboBox.FormattingEnabled = true;
			this.resolutionComboBox.Location = new System.Drawing.Point( 195, 18 );
			this.resolutionComboBox.Name = "resolutionComboBox";
			this.resolutionComboBox.Size = new System.Drawing.Size( 88, 21 );
			this.resolutionComboBox.TabIndex = 4;
			this.resolutionComboBox.SelectedIndexChanged += new System.EventHandler( this.resolutionComboBox_SelectedIndexChanged );
			// 
			// terrainDisplay
			// 
			this.terrainDisplay.AllowArrowKeyInputs = false;
			this.terrainDisplay.ColourBits = ( ( byte )( 32 ) );
			this.terrainDisplay.ContinuousRendering = false;
			this.terrainDisplay.DepthBits = ( ( byte )( 24 ) );
			this.terrainDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.terrainDisplay.Location = new System.Drawing.Point( 0, 0 );
			this.terrainDisplay.Name = "terrainDisplay";
			this.terrainDisplay.RenderInterval = 1;
			this.terrainDisplay.Size = new System.Drawing.Size( 289, 241 );
			this.terrainDisplay.StencilBits = ( ( byte )( 0 ) );
			this.terrainDisplay.TabIndex = 0;
			// 
			// TerrainVisualiserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.displayPanel );
			this.Controls.Add( this.groupBox1 );
			this.Name = "TerrainVisualiserControl";
			this.Size = new System.Drawing.Size( 289, 333 );
			this.groupBox1.ResumeLayout( false );
			this.groupBox1.PerformLayout( );
			this.displayPanel.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel displayPanel;
		private System.Windows.Forms.RadioButton showSlopesRadioButton;
		private System.Windows.Forms.RadioButton showHeightsRadioButton;
		private System.Windows.Forms.RadioButton showTerrainTypesRadioButton;
		private Rb.Rendering.Windows.Display terrainDisplay;
		private System.Windows.Forms.ComboBox resolutionComboBox;
		private System.Windows.Forms.Label label1;
	}
}
