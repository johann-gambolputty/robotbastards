
namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// Base interface for locations (folders or files)
	/// </summary>
	public interface ILocation : ISource
	{
		/// <summary>
		/// Gets the folder that contains this location. Returns null if this location is the root folder
		/// </summary>
		IFolder ParentFolder
		{
			get;
		}

		/// <summary>
		/// Gets the object that manages this location
		/// </summary>
		ILocationManager FileSystem
		{
			get;
		}
	}
}
