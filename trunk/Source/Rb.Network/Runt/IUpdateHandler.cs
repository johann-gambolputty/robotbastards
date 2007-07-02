using Rb.Core.Components;

namespace Rb.Network.Runt
{
	/// <summary>
	/// An update handler handles UpdateMessage messages sent to an UpdateTarget
	/// </summary>
	public interface IUpdateHandler : IUnique
	{
		/// <summary>
		/// Handles an update message
		/// </summary>
		void Handle( UpdateMessage msg );
	}
}
