namespace Poc1.Bob.Controls.Rendering
{
	partial class RenderTargetViewForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.renderTargetViewControl1 = new Poc1.Bob.Controls.Rendering.RenderTargetViewControl( );
			this.okButton = new System.Windows.Forms.Button( );
			this.SuspendLayout( );
			// 
			// renderTargetViewControl1
			// 
			this.renderTargetViewControl1.Anchor = ( ( System.Windows.Forms.AnchorStyles )( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
						| System.Windows.Forms.AnchorStyles.Left )
						| System.Windows.Forms.AnchorStyles.Right ) ) );
			this.renderTargetViewControl1.Location = new System.Drawing.Point( 12, 12 );
			this.renderTargetViewControl1.Name = "renderTargetViewControl1";
			this.renderTargetViewControl1.Size = new System.Drawing.Size( 460, 200 );
			this.renderTargetViewControl1.TabIndex = 0;
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point( 397, 218 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size( 75, 23 );
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// RenderTargetViewForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 484, 253 );
			this.Controls.Add( this.okButton );
			this.Controls.Add( this.renderTargetViewControl1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "RenderTargetViewForm";
			this.Text = "RenderTargetViewForm";
			this.ResumeLayout( false );

		}

		#endregion

		private RenderTargetViewControl renderTargetViewControl1;
		private System.Windows.Forms.Button okButton;
	}
}