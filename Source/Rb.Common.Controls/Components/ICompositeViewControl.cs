using System;
using Rb.Core.Components;

namespace Rb.Common.Controls.Components
{
	/// <summary>
	/// Interface for a view that displays a composite
	/// </summary>
	public interface ICompositeViewControl
	{
		/// <summary>
		/// User selected a component in the view
		/// </summary>
		event Action<object> ComponentSelected;

		/// <summary>
		/// User double-clicked on a component in the view
		/// </summary>
		event Action<object> ComponentAction;

		/// <summary>
		/// Gets/sets the displayed component
		/// </summary>
		IComposite Composite
		{
			get; set;
		}

		/// <summary>
		/// Refreshes the view
		/// </summary>
		void RefreshView( );
	}
}
