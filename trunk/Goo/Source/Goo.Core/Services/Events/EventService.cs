using System;
using System.Collections.Generic;
using Goo.Core.Events;
using log4net;
using Rb.Core.Utils;
using Wintellect.Threading.ResourceLocks;

namespace Goo.Core.Services.Events
{
	/// <summary>
	/// Event service implementation
	/// </summary>
	public class EventService : IEventService
	{
		#region IEventService Members

		/// <summary>
		/// Raises an event
		/// </summary>
		/// <typeparam name="TEvent">Event type</typeparam>
		/// <param name="sender">Event sender</param>
		/// <param name="evt">Event object</param>
		public void Raise<TEvent>( object sender, TEvent evt )
		{
			Arguments.CheckNotNull( evt, "evt" );
			m_Log.DebugFormat( "Raising event {0} from sender {1}", evt, sender );

			List<ISubscriberList> subscribersToClean = null;
			using ( m_Lock.WaitToRead( ) )
			{
				for ( Type eventType = evt.GetType( ); eventType != null; eventType = eventType.BaseType )
				{
					ISubscriberList subscribers;
					if ( !m_Subscribers.TryGetValue( eventType, out subscribers ) )
					{
						continue;
					}
					if ( subscribers.RaiseForEach( sender, evt ) )
					{
						subscribersToClean = subscribersToClean ?? new List<ISubscriberList>( );
						subscribersToClean.Add( subscribers );
					}
				}
			}

			if ( subscribersToClean == null )
			{
				return;
			}
			using ( m_Lock.WaitToWrite( ) )
			{
				foreach ( ISubscriberList subscribers in subscribersToClean )
				{
					subscribers.Clean( );
				}
			}
		}

		/// <summary>
		/// Subscribes an object to a given event type
		/// </summary>
		/// <typeparam name="TEvent">Event type</typeparam>
		/// <param name="subscriber">Subscriber object</param>
		/// <param name="weakReference">
		/// If true, the event service only keeps a weak reference to the subscriber, allowing it to be
		/// garbage collected without having to unsubscribe.
		/// </param>
		public void Subscribe<TEvent>( IEventSubscriber<TEvent> subscriber, bool weakReference )
		{
			m_Log.InfoFormat( "Subscribing {0} to event type {1}", subscriber, typeof( TEvent ) );
			using ( m_Lock.WaitToWrite( ) )
			{
				ISubscriberList subscribers;
				if ( !m_Subscribers.TryGetValue( typeof( TEvent ), out subscribers ) )
				{
					subscribers = new SubscriberList<TEvent>( );
					m_Subscribers.Add( typeof( TEvent ), subscribers );
				}
				subscribers.Add( subscriber, weakReference );
			}
		}

		/// <summary>
		/// Unsubscribes an object from a given event type
		/// </summary>
		/// <typeparam name="TEvent">Event type</typeparam>
		/// <param name="subscriber">Subscriber object</param>
		public void Unsubscribe<TEvent>( IEventSubscriber<TEvent> subscriber )
		{
			m_Log.InfoFormat( "Unsubscribing {0} to event type {1}", subscriber, typeof( TEvent ) );
			using ( m_Lock.WaitToWrite( ) )
			{
				ISubscriberList subscribers = m_Subscribers[ typeof( TEvent ) ];
				subscribers.Remove( subscriber );
			}
		}

		#endregion

		#region Private Members

		private readonly ResourceLock m_Lock = new OneManyResourceLock( );
		private readonly IDictionary<Type, ISubscriberList> m_Subscribers = new Dictionary<Type, ISubscriberList>( );
		private readonly ILog m_Log = LogManager.GetLogger( typeof( EventService ) );

		private interface ISubscriberList
		{
			/// <summary>
			/// Adds a subscriber to the list
			/// </summary>
			void Add<TActualEvent>( IEventSubscriber<TActualEvent> subscriber, bool weakReference );

			/// <summary>
			/// Removes a subscriber from the list
			/// </summary>
			void Remove<TActualEvent>( IEventSubscriber<TActualEvent> subscriber );

			/// <summary>
			/// Removes all dead subscribers from the subscription list
			/// </summary>
			void Clean( );

			/// <summary>
			/// Raises an event for each subscriber in the list. Returns true if the list needs to be cleaned
			/// </summary>
			bool RaiseForEach<TActualEvent>( object sender, TActualEvent actualEvent );
		}

		/// <summary>
		/// Maintains a list of subscribers by weak reference
		/// </summary>
		/// <typeparam name="TEvent">Underlying event</typeparam>
		private class SubscriberList<TEvent> : ISubscriberList
		{
			/// <summary>
			/// Adds a subscriber to the list
			/// </summary>
			public void Add<TActualEvent>( IEventSubscriber<TActualEvent> subscriber, bool weakReference )
			{
				Clean( );
				if ( weakReference )
				{
					m_WeakSubscribers.Add( new WeakReference( subscriber ) );
				}
				else
				{
					m_Subscribers.Add( subscriber );
				}
			}

			/// <summary>
			/// Removes a subscriber from the list
			/// </summary>
			public void Remove<TActualEvent>( IEventSubscriber<TActualEvent> subscriber )
			{
				Clean( );
				m_Subscribers.Remove( subscriber );
				for ( int subIndex = 0; subIndex < m_WeakSubscribers.Count; ++subIndex )
				{
					if ( m_WeakSubscribers[ subIndex ].Target == subscriber )
					{
						m_WeakSubscribers.RemoveAt( subIndex );
						return;
					}
				}
			}

			/// <summary>
			/// Removes all dead subscribers from the subscription list
			/// </summary>
			public void Clean( )
			{
				for ( int subIndex = 0; subIndex < m_WeakSubscribers.Count; )
				{
					if ( !m_WeakSubscribers[ subIndex ].IsAlive )
					{
						m_WeakSubscribers.RemoveAt( subIndex );
						continue;
					}
					++subIndex;
				}
			}

			/// <summary>
			/// Raises an event for each subscriber in the list
			/// </summary>
			public bool RaiseForEach<TActualEvent>( object sender, TActualEvent actualEvent )
			{
				TEvent castEvent = ( TEvent )( object )actualEvent;
				bool needsClean = false;
				foreach ( IEventSubscriber<TEvent> subscriber in m_Subscribers )
				{
					subscriber.OnEvent( sender, castEvent );
				}
				foreach ( WeakReference subscriberRef in m_WeakSubscribers )
				{
					IEventSubscriber<TEvent> subscriber = ( IEventSubscriber<TEvent> )subscriberRef.Target;
					if ( subscriber != null )
					{
						subscriber.OnEvent( sender, castEvent );
					}
					else
					{
						needsClean = true;
					}
				}
				return needsClean;
			}

			#region Private Members

			private readonly List<WeakReference> m_WeakSubscribers = new List<WeakReference>( );
			private readonly List<object> m_Subscribers = new List<object>( );

			#endregion
		}

		#endregion
	}
}
