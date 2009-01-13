
namespace Poc1.Bob.Core.Interfaces.Templates
{
	public enum ViewDockLocation
	{
		Left,
		Top,
		Bottom,
		Right,
		Middle
	}

	/// <summary>
	/// Manages template view instances
	/// </summary>
	public interface ITemplateViewLayoutManager
	{
		/// <summary>
		/// Shows a specified view, docking it to a specified location
		/// </summary>
		/// <param name="view">View to display</param>
		/// <param name="location">Dock location</param>
		void DockView( TemplateViewInfo view, ViewDockLocation location );

		void CloseCurrentLayout( );
	}
}
