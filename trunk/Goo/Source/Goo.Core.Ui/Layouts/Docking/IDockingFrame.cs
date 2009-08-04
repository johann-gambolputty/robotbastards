using Goo.Core.Mvc;
using Goo.Core.Ui.Layouts.Docking;

namespace Goo.Core.Ui.Layouts.Docking
{
	/// <summary>
	/// Docking frame
	/// </summary>
	public interface IDockingFrame
	{
		/// <summary>
		/// Gets/sets the title of this frame
		/// </summary>
		string Title
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the view displayed in this frame
		/// </summary>
		IView View
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets whether or not this frame is visible
		/// </summary>
		bool Visible
		{
			get; set;
		}

		/// <summary>
		/// Shows the frame in a given location
		/// </summary>
		/// <param name="location"></param>
		void Show( DockLocation location );
	}
}
