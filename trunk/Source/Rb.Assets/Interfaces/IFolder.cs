
namespace Rb.Assets.Interfaces
{
	public interface IFolder : IProvider
	{
		string Name
		{
			get;
		}

		string Path
		{
			get;
		}

		ISource[] Sources
		{
			get;
		}

		IFolder[] SubFolders
		{
			get;
		}
	}
}
