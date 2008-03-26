
namespace Rb.Assets.Interfaces
{
	/// <summary>
	/// A file is a location (<see cref="ILocation"/>) that can open a stream (<see cref="IStreamSource"/>)
	/// </summary>
	public interface IFile : ILocation, IStreamSource
	{
	}
}
