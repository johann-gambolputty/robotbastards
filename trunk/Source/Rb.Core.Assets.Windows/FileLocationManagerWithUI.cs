
namespace Rb.Core.Assets.Windows
{
	/// <summary>
	/// Extends <see cref="FileLocationManager"/>, adding UI
	/// </summary>
	class FileLocationManagerWithUI : FileLocationManager, ILocationBrowserProvider
	{
		/// <summary>
		/// Creates a control that implements the ILocationBrowser interface
		/// </summary>
		/// <returns>New <see cref="FileLocationBrowser"/> control</returns>
		public ILocationBrowser CreateControl( )
		{
			return new LocationTreeBrowser( );
		}
	}
}
