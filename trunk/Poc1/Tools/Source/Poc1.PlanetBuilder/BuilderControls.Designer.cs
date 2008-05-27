namespace Poc1.PlanetBuilder
{
	partial class BuilderControls
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
			this.builderControlsTabControl = new System.Windows.Forms.TabControl();
			this.groundTextureTabPage = new System.Windows.Forms.TabPage();
			this.terrainFunctionTabPage = new System.Windows.Forms.TabPage();
			this.atmosphereTabPage = new System.Windows.Forms.TabPage();
			this.groundTextureControl = new Poc1.PlanetBuilder.GroundTextureControl();
			this.terrainFunctionControl = new Poc1.PlanetBuilder.TerrainFunctionControl();
			this.atmosphereControl = new Poc1.PlanetBuilder.AtmosphereControl();
			this.builderControlsTabControl.SuspendLayout();
			this.groundTextureTabPage.SuspendLayout();
			this.terrainFunctionTabPage.SuspendLayout();
			this.atmosphereTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// builderControlsTabControl
			// 
			this.builderControlsTabControl.Controls.Add(this.groundTextureTabPage);
			this.builderControlsTabControl.Controls.Add(this.terrainFunctionTabPage);
			this.builderControlsTabControl.Controls.Add(this.atmosphereTabPage);
			this.builderControlsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.builderControlsTabControl.Location = new System.Drawing.Point(0, 0);
			this.builderControlsTabControl.Name = "builderControlsTabControl";
			this.builderControlsTabControl.SelectedIndex = 0;
			this.builderControlsTabControl.Size = new System.Drawing.Size(260, 327);
			this.builderControlsTabControl.TabIndex = 0;
			// 
			// groundTextureTabPage
			// 
			this.groundTextureTabPage.Controls.Add(this.groundTextureControl);
			this.groundTextureTabPage.Location = new System.Drawing.Point(4, 22);
			this.groundTextureTabPage.Name = "groundTextureTabPage";
			this.groundTextureTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.groundTextureTabPage.Size = new System.Drawing.Size(252, 301);
			this.groundTextureTabPage.TabIndex = 0;
			this.groundTextureTabPage.Text = "Ground Textures";
			this.groundTextureTabPage.UseVisualStyleBackColor = true;
			// 
			// terrainFunctionTabPage
			// 
			this.terrainFunctionTabPage.Controls.Add(this.terrainFunctionControl);
			this.terrainFunctionTabPage.Location = new System.Drawing.Point(4, 22);
			this.terrainFunctionTabPage.Name = "terrainFunctionTabPage";
			this.terrainFunctionTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.terrainFunctionTabPage.Size = new System.Drawing.Size(252, 301);
			this.terrainFunctionTabPage.TabIndex = 1;
			this.terrainFunctionTabPage.Text = "Terrain Function";
			this.terrainFunctionTabPage.UseVisualStyleBackColor = true;
			// 
			// atmosphereTabPage
			// 
			this.atmosphereTabPage.Controls.Add(this.atmosphereControl);
			this.atmosphereTabPage.Location = new System.Drawing.Point(4, 22);
			this.atmosphereTabPage.Name = "atmosphereTabPage";
			this.atmosphereTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.atmosphereTabPage.Size = new System.Drawing.Size(252, 301);
			this.atmosphereTabPage.TabIndex = 2;
			this.atmosphereTabPage.Text = "Atmosphere";
			this.atmosphereTabPage.UseVisualStyleBackColor = true;
			// 
			// groundTextureControl
			// 
			this.groundTextureControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groundTextureControl.Location = new System.Drawing.Point(3, 3);
			this.groundTextureControl.Name = "groundTextureControl";
			this.groundTextureControl.Size = new System.Drawing.Size(246, 295);
			this.groundTextureControl.TabIndex = 0;
			// 
			// terrainFunctionControl
			// 
			this.terrainFunctionControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.terrainFunctionControl.Location = new System.Drawing.Point(3, 3);
			this.terrainFunctionControl.Name = "terrainFunctionControl";
			this.terrainFunctionControl.Size = new System.Drawing.Size(246, 295);
			this.terrainFunctionControl.TabIndex = 0;
			// 
			// atmosphereControl
			// 
			this.atmosphereControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.atmosphereControl.Location = new System.Drawing.Point(3, 3);
			this.atmosphereControl.Name = "atmosphereControl";
			this.atmosphereControl.Size = new System.Drawing.Size(246, 295);
			this.atmosphereControl.TabIndex = 0;
			// 
			// BuilderControls
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.builderControlsTabControl);
			this.Name = "BuilderControls";
			this.Size = new System.Drawing.Size(260, 327);
			this.builderControlsTabControl.ResumeLayout(false);
			this.groundTextureTabPage.ResumeLayout(false);
			this.terrainFunctionTabPage.ResumeLayout(false);
			this.atmosphereTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl builderControlsTabControl;
		private System.Windows.Forms.TabPage groundTextureTabPage;
		private System.Windows.Forms.TabPage terrainFunctionTabPage;
		private System.Windows.Forms.TabPage atmosphereTabPage;
		private GroundTextureControl groundTextureControl;
		private TerrainFunctionControl terrainFunctionControl;
		private AtmosphereControl atmosphereControl;
	}
}
