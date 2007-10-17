
namespace Rb.Core.Components
{
	/// <summary>
	/// Interface for classes that can handle <see cref="Message"/> objects
	/// </summary>
	public interface IMessageHandler
	{
		/// <summary>
		/// Handles messages
		/// </summary>
		/// <param name="msg">Message to handle</param>
		/// <returns>User-defined return value</returns>
        object HandleMessage( Message msg );
	}
}
