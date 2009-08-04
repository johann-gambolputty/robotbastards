
namespace Goo.Core.Events
{
	/// <summary>
	/// Event subscriber interface
	/// </summary>
	public interface IEventSubscriber<TEvent>
	{
		/// <summary>
		/// Handles a raised event
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="args">Event object</param>
		void OnEvent( object sender, TEvent args );
	}
}
