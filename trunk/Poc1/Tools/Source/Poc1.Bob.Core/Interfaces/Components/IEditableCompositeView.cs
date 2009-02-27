using System;
using Rb.Common.Controls.Components;

namespace Poc1.Bob.Core.Interfaces.Components
{
	/// <summary>
	/// Planet template composition view
	/// </summary>
	public interface IEditableCompositeView : ICompositeViewControl
	{
		/// <summary>
		/// Event raised when the user requests to edit the current planet template's composition
		/// </summary>
		event EventHandler EditComposition;
	}
}
