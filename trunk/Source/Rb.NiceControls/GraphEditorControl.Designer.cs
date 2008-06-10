namespace Rb.NiceControls
{
	partial class GraphEditorControl
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
			this.graphTypeComboBox = new System.Windows.Forms.ComboBox( );
			this.graphPanel = new System.Windows.Forms.Panel( );
			this.SuspendLayout( );
			// 
			// graphTypeComboBox
			// 
			this.graphTypeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.graphTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.graphTypeComboBox.FormattingEnabled = true;
			this.graphTypeComboBox.Location = new System.Drawing.Point( 0, 0 );
			this.graphTypeComboBox.Name = "graphTypeComboBox";
			this.graphTypeComboBox.Size = new System.Drawing.Size( 150, 21 );
			this.graphTypeComboBox.TabIndex = 0;
			this.graphTypeComboBox.SelectedIndexChanged += new System.EventHandler( this.graphTypeComboBox_SelectedIndexChanged );
			// 
			// graphPanel
			// 
			this.graphPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.graphPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphPanel.Location = new System.Drawing.Point( 0, 21 );
			this.graphPanel.Name = "graphPanel";
			this.graphPanel.Size = new System.Drawing.Size( 150, 129 );
			this.graphPanel.TabIndex = 1;
			// 
			// GraphEditorControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add( this.graphPanel );
			this.Controls.Add( this.graphTypeComboBox );
			this.Name = "GraphEditorControl";
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.ComboBox graphTypeComboBox;
		private System.Windows.Forms.Panel graphPanel;
	}
}
