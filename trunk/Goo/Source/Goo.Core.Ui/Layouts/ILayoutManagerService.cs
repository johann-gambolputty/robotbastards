
using System.IO;

namespace Goo.Core.Ui.Layouts
{
	/// <summary>
	/// Manages layouts of all services implementing <see cref="ILayoutSerializerService"/>
	/// </summary>
	public interface ILayoutManagerService
	{
		/// <summary>
		/// Loads layouts
		/// </summary>
		void LoadLayouts( Stream stream );

		/// <summary>
		/// Saves layouts
		/// </summary>
		void SaveLayouts( Stream stream );
	}
}
