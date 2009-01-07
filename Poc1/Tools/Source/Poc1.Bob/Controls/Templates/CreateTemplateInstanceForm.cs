using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Templates;

namespace Poc1.Bob.Controls.Templates
{
	public partial class CreateTemplateInstanceForm : Form, ICreateTemplateInstanceView
	{
		public CreateTemplateInstanceForm( )
		{
			InitializeComponent( );
		}

		#region ICreateInstanceView Members

		/// <summary>
		/// Gets/sets whether or not the OK button is enabled
		/// </summary>
		public bool OkEnabled
		{
			get { return createButton.Enabled; }
			set { createButton.Enabled = value; }
		}

		/// <summary>
		/// Gets/sets the name of the new template instance
		/// </summary>
		public string InstanceName
		{
			get { return instanceNameTextBox.Text; }
			set { instanceNameTextBox.Text = value; }
		}

		/// <summary>
		/// Gets the project type selection sub-view
		/// </summary>
		public ITemplateSelectorView SelectionView
		{
			get { return templateSelectorView1; }
		}

		/// <summary>
		/// Shows this form
		/// </summary>
		/// <returns>Returns true if the selected template should be instanced</returns>
		public bool ShowView( )
		{
			return ShowDialog( ) == DialogResult.OK;
		}

		#endregion
	}
}