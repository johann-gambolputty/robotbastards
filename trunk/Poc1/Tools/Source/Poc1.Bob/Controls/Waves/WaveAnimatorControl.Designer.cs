using Rb.Common.Controls.Forms;

namespace Poc1.Bob.Controls.Waves
{
	partial class WaveAnimatorControl
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
			this.components = new System.ComponentModel.Container( );
			this.animationTimer = new System.Windows.Forms.Timer( this.components );
			this.panel2 = new System.Windows.Forms.Panel( );
			this.progressBar1 = new System.Windows.Forms.ProgressBar( );
			this.buildButton = new System.Windows.Forms.Button( );
			this.animationPropertyGrid = new System.Windows.Forms.PropertyGrid( );
			this.splitter1 = new System.Windows.Forms.Splitter( );
			this.panel1 = new System.Windows.Forms.Panel( );
			this.animationPanel = new Rb.Common.Controls.Forms.DbPanel( );
			this.enableAnimationCheckBox = new System.Windows.Forms.CheckBox( );
			this.panel2.SuspendLayout( );
			this.panel1.SuspendLayout( );
			this.SuspendLayout( );
			// 
			// animationTimer
			// 
			this.animationTimer.Enabled = true;
			this.animationTimer.Interval = 20;
			this.animationTimer.Tick += new System.EventHandler( this.animationTimer_Tick );
			// 
			// panel2
			// 
			this.panel2.Controls.Add( this.animationPropertyGrid );
			this.panel2.Controls.Add( this.progressBar1 );
			this.panel2.Controls.Add( this.buildButton );
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point( 0, 0 );
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size( 152, 230 );
			this.panel2.TabIndex = 4;
			// 
			// progressBar1
			// 
			this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressBar1.Location = new System.Drawing.Point( 0, 184 );
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size( 152, 23 );
			this.progressBar1.TabIndex = 5;
			// 
			// buildButton
			// 
			this.buildButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buildButton.Location = new System.Drawing.Point( 0, 207 );
			this.buildButton.Name = "buildButton";
			this.buildButton.Size = new System.Drawing.Size( 152, 23 );
			this.buildButton.TabIndex = 4;
			this.buildButton.Text = "Build";
			this.buildButton.UseVisualStyleBackColor = true;
			this.buildButton.Click += new System.EventHandler( this.buildButton_Click );
			// 
			// animationPropertyGrid
			// 
			this.animationPropertyGrid.CommandsVisibleIfAvailable = false;
			this.animationPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.animationPropertyGrid.Location = new System.Drawing.Point( 0, 0 );
			this.animationPropertyGrid.Name = "animationPropertyGrid";
			this.animationPropertyGrid.Size = new System.Drawing.Size( 152, 184 );
			this.animationPropertyGrid.TabIndex = 3;
			this.animationPropertyGrid.ToolbarVisible = false;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point( 0, 230 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 152, 3 );
			this.splitter1.TabIndex = 5;
			this.splitter1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add( this.animationPanel );
			this.panel1.Controls.Add( this.enableAnimationCheckBox );
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point( 0, 233 );
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size( 152, 68 );
			this.panel1.TabIndex = 6;
			// 
			// animationPanel
			// 
			this.animationPanel.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.animationPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.animationPanel.Location = new System.Drawing.Point( 3, 3 );
			this.animationPanel.Name = "animationPanel";
			this.animationPanel.Size = new System.Drawing.Size( 37, 61 );
			this.animationPanel.TabIndex = 0;
			// 
			// enableAnimationCheckBox
			// 
			this.enableAnimationCheckBox.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.enableAnimationCheckBox.AutoSize = true;
			this.enableAnimationCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.enableAnimationCheckBox.Location = new System.Drawing.Point( 44, 47 );
			this.enableAnimationCheckBox.Name = "enableAnimationCheckBox";
			this.enableAnimationCheckBox.Size = new System.Drawing.Size( 105, 17 );
			this.enableAnimationCheckBox.TabIndex = 1;
			this.enableAnimationCheckBox.Text = "Pause Animation";
			this.enableAnimationCheckBox.UseVisualStyleBackColor = true;
			// 
			// WaveAnimatorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.panel1 );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.panel2 );
			this.MinimumSize = new System.Drawing.Size( 152, 301 );
			this.Name = "WaveAnimatorControl";
			this.Size = new System.Drawing.Size( 152, 301 );
			this.panel2.ResumeLayout( false );
			this.panel1.ResumeLayout( false );
			this.panel1.PerformLayout( );
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Timer animationTimer;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button buildButton;
		private System.Windows.Forms.PropertyGrid animationPropertyGrid;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private DbPanel animationPanel;
		private System.Windows.Forms.CheckBox enableAnimationCheckBox;
	}
}
