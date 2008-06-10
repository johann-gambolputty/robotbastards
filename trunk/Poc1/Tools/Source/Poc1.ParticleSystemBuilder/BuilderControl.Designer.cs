namespace Poc1.ParticleSystemBuilder
{
	partial class BuilderControl
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
			this.renderControlEditor = new Poc1.ParticleSystemBuilder.ComponentControl();
			this.killerControlEditor = new Poc1.ParticleSystemBuilder.ComponentControl();
			this.updateControlEditor = new Poc1.ParticleSystemBuilder.ComponentControl();
			this.spawnControlEditor = new Poc1.ParticleSystemBuilder.ComponentControl();
			this.spawnRateControlEditor = new Poc1.ParticleSystemBuilder.ComponentControl();
			this.SuspendLayout();
			// 
			// renderControlEditor
			// 
			this.renderControlEditor.ControlDisplayName = "Render Control";
			this.renderControlEditor.ControlTypes = null;
			this.renderControlEditor.Dock = System.Windows.Forms.DockStyle.Top;
			this.renderControlEditor.Location = new System.Drawing.Point(0, 573);
			this.renderControlEditor.Name = "renderControlEditor";
			this.renderControlEditor.Size = new System.Drawing.Size(260, 193);
			this.renderControlEditor.TabIndex = 4;
			// 
			// killerControlEditor
			// 
			this.killerControlEditor.ControlDisplayName = "Cull Control";
			this.killerControlEditor.ControlTypes = null;
			this.killerControlEditor.Dock = System.Windows.Forms.DockStyle.Top;
			this.killerControlEditor.Location = new System.Drawing.Point(0, 382);
			this.killerControlEditor.Name = "killerControlEditor";
			this.killerControlEditor.Size = new System.Drawing.Size(260, 191);
			this.killerControlEditor.TabIndex = 3;
			// 
			// updateControlEditor
			// 
			this.updateControlEditor.ControlDisplayName = "Update Control";
			this.updateControlEditor.ControlTypes = null;
			this.updateControlEditor.Dock = System.Windows.Forms.DockStyle.Top;
			this.updateControlEditor.Location = new System.Drawing.Point(0, 191);
			this.updateControlEditor.Name = "updateControlEditor";
			this.updateControlEditor.Size = new System.Drawing.Size(260, 191);
			this.updateControlEditor.TabIndex = 2;
			// 
			// spawnControlEditor
			// 
			this.spawnControlEditor.ControlDisplayName = "Spawn Control";
			this.spawnControlEditor.ControlTypes = null;
			this.spawnControlEditor.Dock = System.Windows.Forms.DockStyle.Top;
			this.spawnControlEditor.Location = new System.Drawing.Point(0, 0);
			this.spawnControlEditor.Name = "spawnControlEditor";
			this.spawnControlEditor.Size = new System.Drawing.Size(260, 191);
			this.spawnControlEditor.TabIndex = 1;
			// 
			// spawnRateControlEditor
			// 
			this.spawnRateControlEditor.ControlDisplayName = "Spawn Rate Control";
			this.spawnRateControlEditor.ControlTypes = null;
			this.spawnRateControlEditor.Dock = System.Windows.Forms.DockStyle.Top;
			this.spawnRateControlEditor.Location = new System.Drawing.Point(0, 766);
			this.spawnRateControlEditor.Name = "spawnRateControlEditor";
			this.spawnRateControlEditor.Size = new System.Drawing.Size(260, 177);
			this.spawnRateControlEditor.TabIndex = 0;
			// 
			// BuilderControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.renderControlEditor);
			this.Controls.Add(this.killerControlEditor);
			this.Controls.Add(this.updateControlEditor);
			this.Controls.Add(this.spawnControlEditor);
			this.Controls.Add(this.spawnRateControlEditor);
			this.Name = "BuilderControl";
			this.Size = new System.Drawing.Size(260, 956);
			this.Load += new System.EventHandler(this.BuilderControl_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private ComponentControl renderControlEditor;
		private ComponentControl killerControlEditor;
		private ComponentControl updateControlEditor;
		private ComponentControl spawnControlEditor;
		private ComponentControl spawnRateControlEditor;





	}
}
