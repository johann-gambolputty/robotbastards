namespace Poc1.ParticleSystemBuilder
{
	partial class ParticleSystemEditor
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
			this.componentTabControl = new System.Windows.Forms.TabControl();
			this.spawnerTabPage = new System.Windows.Forms.TabPage();
			this.updaterTabPage = new System.Windows.Forms.TabPage();
			this.updaterComponentsControl = new Poc1.ParticleSystemBuilder.CompositeComponentControl();
			this.killerTabPage = new System.Windows.Forms.TabPage();
			this.rendererTabPage = new System.Windows.Forms.TabPage();
			this.spawnRateComponentControl = new Poc1.ParticleSystemBuilder.ComponentControl();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.spawnerComponentControl = new Poc1.ParticleSystemBuilder.ComponentControl();
			this.killerComponentsControl = new Poc1.ParticleSystemBuilder.CompositeComponentControl();
			this.rendererComponentsControl = new Poc1.ParticleSystemBuilder.CompositeComponentControl();
			this.componentTabControl.SuspendLayout();
			this.spawnerTabPage.SuspendLayout();
			this.updaterTabPage.SuspendLayout();
			this.killerTabPage.SuspendLayout();
			this.rendererTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// componentTabControl
			// 
			this.componentTabControl.Controls.Add(this.spawnerTabPage);
			this.componentTabControl.Controls.Add(this.updaterTabPage);
			this.componentTabControl.Controls.Add(this.killerTabPage);
			this.componentTabControl.Controls.Add(this.rendererTabPage);
			this.componentTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.componentTabControl.Location = new System.Drawing.Point(0, 0);
			this.componentTabControl.Name = "componentTabControl";
			this.componentTabControl.SelectedIndex = 0;
			this.componentTabControl.Size = new System.Drawing.Size(206, 389);
			this.componentTabControl.TabIndex = 0;
			// 
			// spawnerTabPage
			// 
			this.spawnerTabPage.Controls.Add(this.spawnerComponentControl);
			this.spawnerTabPage.Controls.Add(this.splitter1);
			this.spawnerTabPage.Controls.Add(this.spawnRateComponentControl);
			this.spawnerTabPage.Location = new System.Drawing.Point(4, 22);
			this.spawnerTabPage.Name = "spawnerTabPage";
			this.spawnerTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.spawnerTabPage.Size = new System.Drawing.Size(198, 363);
			this.spawnerTabPage.TabIndex = 1;
			this.spawnerTabPage.Text = "Spawner";
			this.spawnerTabPage.UseVisualStyleBackColor = true;
			// 
			// updaterTabPage
			// 
			this.updaterTabPage.Controls.Add(this.updaterComponentsControl);
			this.updaterTabPage.Location = new System.Drawing.Point(4, 22);
			this.updaterTabPage.Name = "updaterTabPage";
			this.updaterTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.updaterTabPage.Size = new System.Drawing.Size(198, 363);
			this.updaterTabPage.TabIndex = 2;
			this.updaterTabPage.Text = "Updater";
			this.updaterTabPage.UseVisualStyleBackColor = true;
			// 
			// updaterComponentsControl
			// 
			this.updaterComponentsControl.ComponentTypes = new System.Type[0];
			this.updaterComponentsControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.updaterComponentsControl.Location = new System.Drawing.Point(3, 3);
			this.updaterComponentsControl.Name = "updaterComponentsControl";
			this.updaterComponentsControl.Size = new System.Drawing.Size(192, 357);
			this.updaterComponentsControl.TabIndex = 0;
			// 
			// killerTabPage
			// 
			this.killerTabPage.Controls.Add(this.killerComponentsControl);
			this.killerTabPage.Location = new System.Drawing.Point(4, 22);
			this.killerTabPage.Name = "killerTabPage";
			this.killerTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.killerTabPage.Size = new System.Drawing.Size(198, 363);
			this.killerTabPage.TabIndex = 3;
			this.killerTabPage.Text = "Killer";
			this.killerTabPage.UseVisualStyleBackColor = true;
			// 
			// rendererTabPage
			// 
			this.rendererTabPage.Controls.Add(this.rendererComponentsControl);
			this.rendererTabPage.Location = new System.Drawing.Point(4, 22);
			this.rendererTabPage.Name = "rendererTabPage";
			this.rendererTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.rendererTabPage.Size = new System.Drawing.Size(198, 363);
			this.rendererTabPage.TabIndex = 4;
			this.rendererTabPage.Text = "Renderer";
			this.rendererTabPage.UseVisualStyleBackColor = true;
			// 
			// spawnRateComponentControl
			// 
			this.spawnRateComponentControl.ControlDisplayName = "Control";
			this.spawnRateComponentControl.ControlTypes = null;
			this.spawnRateComponentControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.spawnRateComponentControl.Location = new System.Drawing.Point(3, 3);
			this.spawnRateComponentControl.Name = "spawnRateComponentControl";
			this.spawnRateComponentControl.Size = new System.Drawing.Size(192, 180);
			this.spawnRateComponentControl.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(3, 183);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(192, 3);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// spawnerComponentControl
			// 
			this.spawnerComponentControl.ControlDisplayName = "Control";
			this.spawnerComponentControl.ControlTypes = null;
			this.spawnerComponentControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spawnerComponentControl.Location = new System.Drawing.Point(3, 186);
			this.spawnerComponentControl.Name = "spawnerComponentControl";
			this.spawnerComponentControl.Size = new System.Drawing.Size(192, 174);
			this.spawnerComponentControl.TabIndex = 2;
			// 
			// killerComponentsControl
			// 
			this.killerComponentsControl.ComponentTypes = new System.Type[0];
			this.killerComponentsControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.killerComponentsControl.Location = new System.Drawing.Point(3, 3);
			this.killerComponentsControl.Name = "killerComponentsControl";
			this.killerComponentsControl.Size = new System.Drawing.Size(192, 357);
			this.killerComponentsControl.TabIndex = 0;
			// 
			// rendererComponentsControl
			// 
			this.rendererComponentsControl.ComponentTypes = new System.Type[0];
			this.rendererComponentsControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rendererComponentsControl.Location = new System.Drawing.Point(3, 3);
			this.rendererComponentsControl.Name = "rendererComponentsControl";
			this.rendererComponentsControl.Size = new System.Drawing.Size(192, 357);
			this.rendererComponentsControl.TabIndex = 0;
			// 
			// ParticleSystemEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.componentTabControl);
			this.Name = "ParticleSystemEditor";
			this.Size = new System.Drawing.Size(206, 389);
			this.Load += new System.EventHandler(this.ParticleSystemEditor_Load);
			this.componentTabControl.ResumeLayout(false);
			this.spawnerTabPage.ResumeLayout(false);
			this.updaterTabPage.ResumeLayout(false);
			this.killerTabPage.ResumeLayout(false);
			this.rendererTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl componentTabControl;
		private System.Windows.Forms.TabPage spawnerTabPage;
		private System.Windows.Forms.TabPage updaterTabPage;
		private System.Windows.Forms.TabPage killerTabPage;
		private System.Windows.Forms.TabPage rendererTabPage;
		private CompositeComponentControl updaterComponentsControl;
		private ComponentControl spawnerComponentControl;
		private System.Windows.Forms.Splitter splitter1;
		private ComponentControl spawnRateComponentControl;
		private CompositeComponentControl killerComponentsControl;
		private CompositeComponentControl rendererComponentsControl;
	}
}
