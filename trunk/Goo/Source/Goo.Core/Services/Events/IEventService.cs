using Goo.Core.Events;

namespace Goo.Core.Services.Events
{
	/// <summary>
	/// Event subscription service
	/// </summary>
	/// <remarks>
	/// A central event bus.
	/// Classes satisfying this interface should also not have an effect on the lifetime of subscribers (i.e.
	/// an object should not leak simply due to being subscribed to this service).
	/// </remarks>
	public interface IEventService
	{
		/// <summary>
		/// Raises an event
		/// </summary>
		/// <typeparam name="TEvent">Event type</typeparam>
		/// <param name="sender">Event sender</param>
		/// <param name="evt">Event object</param>
		void Raise<TEvent>( object sender, TEvent evt );

		/// <summary>
		/// Subscribes an object to a given event type
		/// </summary>
		/// <typeparam name="TEvent">Event type</typeparam>
		/// <param name="subscriber">Subscriber object</param>
		void Subscribe<TEvent>( IEventSubscriber<TEvent> subscriber );

		/// <summary>
		/// Unsubscribes an object from a given event type
		/// </summary>
		/// <typeparam name="TEvent">Event type</typeparam>
		/// <param name="subscriber">Subscriber object</param>
		void Unsubscribe<TEvent>( IEventSubscriber<TEvent> subscriber );
	}
}
