namespace Poc1.Bob.Controls.Planet.Terrain
{
	partial class HomogenousProcTerrainTemplateControl
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
			this.functionTabControl = new System.Windows.Forms.TabControl( );
			this.heightFunctionTabPage = new System.Windows.Forms.TabPage( );
			this.heightFunctionPropertyGrid = new System.Windows.Forms.PropertyGrid( );
			this.heightFunctionTypeComboBox = new System.Windows.Forms.ComboBox( );
			this.groundFunctionTabPage = new System.Windows.Forms.TabPage( );
			this.groundFunctionPropertyGrid = new System.Windows.Forms.PropertyGrid( );
			this.groundFunctionTypeComboBox = new System.Windows.Forms.ComboBox( );
			this.rebuildModelsButton = new System.Windows.Forms.Button( );
			this.functionTabControl.SuspendLayout( );
			this.heightFunctionTabPage.SuspendLayout( );
			this.groundFunctionTabPage.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// functionTabControl
			// 
			this.functionTabControl.Controls.Add( this.heightFunctionTabPage );
			this.functionTabControl.Controls.Add( this.groundFunctionTabPage );
			this.functionTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.functionTabControl.Location = new System.Drawing.Point( 0, 0 );
			this.functionTabControl.Name = "functionTabControl";
			this.functionTabControl.SelectedIndex = 0;
			this.functionTabControl.Size = new System.Drawing.Size( 209, 274 );
			this.functionTabControl.TabIndex = 0;
			// 
			// heightFunctionTabPage
			// 
			this.heightFunctionTabPage.Controls.Add( this.heightFunctionPropertyGrid );
			this.heightFunctionTabPage.Controls.Add( this.heightFunctionTypeComboBox );
			this.heightFunctionTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.heightFunctionTabPage.Name = "heightFunctionTabPage";
			this.heightFunctionTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.heightFunctionTabPage.Size = new System.Drawing.Size( 201, 248 );
			this.heightFunctionTabPage.TabIndex = 0;
			this.heightFunctionTabPage.Text = "Height Function";
			this.heightFunctionTabPage.UseVisualStyleBackColor = true;
			// 
			// heightFunctionPropertyGrid
			// 
			this.heightFunctionPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.heightFunctionPropertyGrid.Location = new System.Drawing.Point( 3, 24 );
			this.heightFunctionPropertyGrid.Name = "heightFunctionPropertyGrid";
			this.heightFunctionPropertyGrid.Size = new System.Drawing.Size( 195, 221 );
			this.heightFunctionPropertyGrid.TabIndex = 1;
			// 
			// heightFunctionTypeComboBox
			// 
			this.heightFunctionTypeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.heightFunctionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.heightFunctionTypeComboBox.FormattingEnabled = true;
			this.heightFunctionTypeComboBox.Location = new System.Drawing.Point( 3, 3 );
			this.heightFunctionTypeComboBox.Name = "heightFunctionTypeComboBox";
			this.heightFunctionTypeComboBox.Size = new System.Drawing.Size( 195, 21 );
			this.heightFunctionTypeComboBox.TabIndex = 0;
			this.heightFunctionTypeComboBox.SelectedIndexChanged += new System.EventHandler( this.heightFunctionTypeComboBox_SelectedIndexChanged );
			// 
			// groundFunctionTabPage
			// 
			this.groundFunctionTabPage.Controls.Add( this.groundFunctionPropertyGrid );
			this.groundFunctionTabPage.Controls.Add( this.groundFunctionTypeComboBox );
			this.groundFunctionTabPage.Location = new System.Drawing.Point( 4, 22 );
			this.groundFunctionTabPage.Name = "groundFunctionTabPage";
			this.groundFunctionTabPage.Padding = new System.Windows.Forms.Padding( 3 );
			this.groundFunctionTabPage.Size = new System.Drawing.Size( 201, 248 );
			this.groundFunctionTabPage.TabIndex = 1;
			this.groundFunctionTabPage.Text = "Ground Function";
			this.groundFunctionTabPage.UseVisualStyleBackColor = true;
			// 
			// groundFunctionPropertyGrid
			// 
			this.groundFunctionPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groundFunctionPropertyGrid.Location = new System.Drawing.Point( 3, 24 );
			this.groundFunctionPropertyGrid.Name = "groundFunctionPropertyGrid";
			this.groundFunctionPropertyGrid.Size = new System.Drawing.Size( 195, 221 );
			this.groundFunctionPropertyGrid.TabIndex = 1;
			// 
			// groundFunctionTypeComboBox
			// 
			this.groundFunctionTypeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.groundFunctionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.groundFunctionTypeComboBox.FormattingEnabled = true;
			this.groundFunctionTypeComboBox.Location = new System.Drawing.Point( 3, 3 );
			this.groundFunctionTypeComboBox.Name = "groundFunctionTypeComboBox";
			this.groundFunctionTypeComboBox.Size = new System.Drawing.Size( 195, 21 );
			this.groundFunctionTypeComboBox.TabIndex = 0;
			this.groundFunctionTypeComboBox.SelectedIndexChanged += new System.EventHandler( this.groundFunctionTypeComboBox_SelectedIndexChanged );
			// 
			// rebuildModelsButton
			// 
			this.rebuildModelsButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.rebuildModelsButton.Location = new System.Drawing.Point( 0, 274 );
			this.rebuildModelsButton.Name = "rebuildModelsButton";
			this.rebuildModelsButton.Size = new System.Drawing.Size( 209, 23 );
			this.rebuildModelsButton.TabIndex = 1;
			this.rebuildModelsButton.Text = "Rebuild...";
			this.rebuildModelsButton.UseVisualStyleBackColor = true;
			this.rebuildModelsButton.Click += new System.EventHandler( this.rebuildModelsButton_Click );
			// 
			// HomogenousProcTerrainTemplateControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.functionTabControl );
			this.Controls.Add( this.rebuildModelsButton );
			this.Name = "HomogenousProcTerrainTemplateControl";
			this.Size = new System.Drawing.Size( 209, 297 );
			this.functionTabControl.ResumeLayout( false );
			this.heightFunctionTabPage.ResumeLayout( false );
			this.groundFunctionTabPage.ResumeLayout( false );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.TabControl functionTabControl;
		private System.Windows.Forms.TabPage heightFunctionTabPage;
		private System.Windows.Forms.TabPage groundFunctionTabPage;
		private System.Windows.Forms.PropertyGrid heightFunctionPropertyGrid;
		private System.Windows.Forms.ComboBox heightFunctionTypeComboBox;
		private System.Windows.Forms.PropertyGrid groundFunctionPropertyGrid;
		private System.Windows.Forms.ComboBox groundFunctionTypeComboBox;
		private System.Windows.Forms.Button rebuildModelsButton;

	}
}
