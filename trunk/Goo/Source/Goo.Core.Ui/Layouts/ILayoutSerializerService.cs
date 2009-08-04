using System.IO;

namespace Goo.Core.Ui.Layouts
{
	/// <summary>
	/// Implemented by services that can provide serialized layouts for the <see cref="ILayoutManagerService"/>
	/// </summary>
	public interface ILayoutSerializerService
	{
		/// <summary>
		/// Loads the service layout
		/// </summary>
		/// <param name="stream">Stream containing serialized service layout</param>
		void Load( Stream stream );

		/// <summary>
		/// Saves the service layout
		/// </summary>
		/// <param name="stream">Stream to write serialized layout to</param>
		/// <returns>Returns true if anything was saved to the stream</returns>
		bool Save( Stream stream);
	}
}
