using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Components;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Rb.Core.Components;

namespace Poc1.Bob.Controls.Components
{
	public partial class EditableCompositeControl : UserControl, IEditableCompositeView
	{
		public EditableCompositeControl( )
		{
			InitializeComponent( );
		}

		#region IEditableCompositeView Members

		/// <summary>
		/// Event raised when the user requests to edit the current planet template's composition
		/// </summary>
		public event EventHandler EditComposition;

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
		/// User selected a component in the view
		/// </summary>
		public event Action<object> ComponentSelected
		{
			add { compositeView.ComponentSelected += value; }
			remove { compositeView.ComponentSelected -= value; }
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
				editTemplateButton.Enabled = ( value != null );
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

		#region Event Handlers

		private void editTemplateButton_Click( object sender, EventArgs e )
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
