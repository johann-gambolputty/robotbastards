
namespace Rb.Core.Assets.Windows
{
	/// <summary>
	/// Asset location browser control provider
	/// </summary>
	public interface ILocationBrowserProvider
	{
		/// <summary>
		/// Creates a control that implements the ILocationBrowser interface
		/// </summary>
		/// <returns>New <see cref="ILocationBrowser"/> control</returns>
		ILocationBrowser CreateControl( );
	}
}
