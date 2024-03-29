
using Rb.Assets.Files;

namespace Rb.Core.Assets.Windows
{
	/// <summary>
	/// Extends <see cref="FileSystem"/>, adding UI
	/// </summary>
	public class FileLocationManagerWithUI : FileSystem, ILocationBrowserProvider
	{
		/// <summary>
		/// Creates a control that implements the ILocationBrowser interface
		/// </summary>
		/// <returns>New <see cref="LocationTreeBrowser"/> control that can browse the file system</returns>
		public ILocationBrowser CreateControl( )
		{
			return new LocationTreeBrowser( new FileLocationTree( this, BaseDirectory ) );
		}
	}
}
