
namespace Rb.Assets.Interfaces
{
	public interface IProvider
	{
		bool IsPathValid( string path );

		IFolder GetFolder( string path );

		ISource GetSource( string path );
	}
}
