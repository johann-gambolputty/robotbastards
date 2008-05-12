namespace Poc1.Tools.TerrainTextures
{
	partial class TerrainTypeEditorControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerrainTypeEditorControl));
			this.typeNameTextBox = new System.Windows.Forms.TextBox();
			this.texturePanel = new System.Windows.Forms.Panel();
			this.deleteButton = new System.Windows.Forms.Button();
			this.slopeControl = new Poc1.Tools.TerrainTextures.TerrainTypeDistributionControl();
			this.elevationControl = new Poc1.Tools.TerrainTextures.TerrainTypeDistributionControl();
			this.SuspendLayout();
			// 
			// typeNameTextBox
			// 
			this.typeNameTextBox.Location = new System.Drawing.Point(3, 29);
			this.typeNameTextBox.Name = "typeNameTextBox";
			this.typeNameTextBox.Size = new System.Drawing.Size(100, 20);
			this.typeNameTextBox.TabIndex = 2;
			this.typeNameTextBox.TextChanged += new System.EventHandler(this.typeNameTextBox_TextChanged);
			// 
			// texturePanel
			// 
			this.texturePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.texturePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.texturePanel.Location = new System.Drawing.Point(438, 7);
			this.texturePanel.Name = "texturePanel";
			this.texturePanel.Size = new System.Drawing.Size(64, 64);
			this.texturePanel.TabIndex = 4;
			this.texturePanel.Click += new System.EventHandler(this.texturePanel_Click);
			// 
			// deleteButton
			// 
			this.deleteButton.BackgroundImage = global::Poc1.Tools.TerrainTextures.Properties.Resources.Delete;
			this.deleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.deleteButton.Location = new System.Drawing.Point(505, 7);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(19, 23);
			this.deleteButton.TabIndex = 5;
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
			// 
			// slopeControl
			// 
			this.slopeControl.BackColor = System.Drawing.Color.Transparent;
			this.slopeControl.Distribution = ((Poc1.Tools.TerrainTextures.Core.IDistribution)(resources.GetObject("slopeControl.Distribution")));
			this.slopeControl.Location = new System.Drawing.Point(269, 0);
			this.slopeControl.Name = "slopeControl";
			this.slopeControl.Size = new System.Drawing.Size(154, 78);
			this.slopeControl.TabIndex = 3;
			// 
			// elevationControl
			// 
			this.elevationControl.BackColor = System.Drawing.Color.Transparent;
			this.elevationControl.Distribution = ((Poc1.Tools.TerrainTextures.Core.IDistribution)(resources.GetObject("elevationControl.Distribution")));
			this.elevationControl.Location = new System.Drawing.Point(109, 0);
			this.elevationControl.Name = "elevationControl";
			this.elevationControl.Size = new System.Drawing.Size(154, 78);
			this.elevationControl.TabIndex = 0;
			// 
			// TerrainTypeEditorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.texturePanel);
			this.Controls.Add(this.slopeControl);
			this.Controls.Add(this.typeNameTextBox);
			this.Controls.Add(this.elevationControl);
			this.Name = "TerrainTypeEditorControl";
			this.Size = new System.Drawing.Size(527, 78);
			this.Load += new System.EventHandler(this.TerrainTypeEditorControl_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TerrainTypeDistributionControl elevationControl;
		private System.Windows.Forms.TextBox typeNameTextBox;
		private TerrainTypeDistributionControl slopeControl;
		private System.Windows.Forms.Panel texturePanel;
		private System.Windows.Forms.Button deleteButton;
	}
}
