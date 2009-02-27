using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Components;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Components;

namespace Poc1.Bob.Controls.Components
{
	public partial class EditableCompositeControl : UserControl, IEditableCompositeView
	{
		public EditableCompositeControl( )
		{
			InitializeComponent( );
		}

		#region IPlanetTemplateCompositionView Members

		/// <summary>
		/// Event raised when the user requests to edit the current planet template's composition
		/// </summary>
		public event EventHandler EditComposition;

		/// <summary>
		/// Gets/sets the planet model template to display
		/// </summary>
		public IPlanetModelTemplate PlanetTemplate
		{
			get { return m_Template; }
			set
			{
				m_Template = value;
				compositeView.Composite = value;
				addTemplateButton.Enabled = ( value != null );
			}
		}

		#endregion

		#region ICompositeViewControl Members

		/// <summary>
		/// User double-clicked on a component in the view
		/// </summary>
		public event Action<object> ComponentAction
		{
			add { compositeView.ComponentAction += value; }
			remove { compositeView.ComponentAction -= value; }
		}

		/// <summary>
		/// Gets/sets the displayed component
		/// </summary>
		public IComposite Composite
		{
			get { return compositeView.Composite; }
			set
			{
				compositeView.Composite = value;
				addTemplateButton.Enabled = ( value != null );
			}
		}

		/// <summary>
		/// Refreshes the view
		/// </summary>
		public void RefreshView( )
		{
			compositeView.RefreshView( );
		}

		#endregion

		#region Private Members

		private IPlanetModelTemplate m_Template;

		#region Event Handlers

		private void addTemplateButton_Click( object sender, EventArgs e )
		{
			if ( EditComposition != null )
			{
				EditComposition( this, EventArgs.Empty );
			}
		}

		#endregion

		#endregion
	}
}
