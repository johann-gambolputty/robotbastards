
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
	}
}
