
using System;

namespace Rb.Core.Assets.Windows
{
	/// <summary>
	/// Asset location browser control interface
	/// </summary>
	public interface ILocationBrowser
	{
		/// <summary>
		/// Gets the currently selected list of sources
		/// </summary>
		ISource[] Sources
		{
			get;
		}

		/// <summary>
		/// Event, invoked when the user has made his selection of assets (e.g. by double clicking on a location, or
		///  pressing return)
		/// </summary>
		event EventHandler SelectionChosen;
	}
}
