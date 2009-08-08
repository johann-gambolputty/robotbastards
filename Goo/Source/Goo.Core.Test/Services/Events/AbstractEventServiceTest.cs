using System;
using System.ComponentModel;
using Goo.Core.Events;
using Goo.Core.Services.Events;
using log4net.Config;
using NUnit.Framework;
using Rhino.Mocks;

namespace Goo.Core.Test.Services.Events
{
	public abstract class AbstractEventServiceTest
	{
		[Test]
		public void TestBaseEventSubscriberIsTriggeredByDerivedEvent( )
		{
			IEventSubscriber<object> objectSubscriber = m_Mocks.StrictMock<IEventSubscriber<object>>( );
			IEventSubscriber<EventArgs> eventArgsSubscriber = m_Mocks.StrictMock<IEventSubscriber<EventArgs>>( );
			IEventSubscriber<CancelEventArgs> cancelEventArgsSubscriber = m_Mocks.StrictMock<IEventSubscriber<CancelEventArgs>>( );
			CancelEventArgs evt = new CancelEventArgs( );
			using ( m_Mocks.Record( ) )
			{
				objectSubscriber.OnEvent( this, evt );
				eventArgsSubscriber.OnEvent( this, evt );
				cancelEventArgsSubscriber.OnEvent( this, evt );
			}
			using ( m_Mocks.Playback( ) )
			{
				IEventService service = CreateEventService( );
				service.Subscribe( objectSubscriber, false );
				service.Subscribe( eventArgsSubscriber, false );
				service.Subscribe( cancelEventArgsSubscriber, false );
				service.Raise( this, evt );
			}
			m_Mocks.VerifyAll( );
		}

		[Test]
		public void TestSubscriptionDoesntExtendObjectLifetime( )
		{
			IEventSubscriber<object> subscriber = new NullEventSubscriber<object>( );
			CreateEventService( ).Subscribe( subscriber, true );
			WeakReference subscriberRef = new WeakReference( subscriber );
			subscriber = null;
			GC.Collect( );
			GC.WaitForPendingFinalizers( );
			GC.Collect( );
			Assert.IsFalse( subscriberRef.IsAlive );
		}

		private class NullEventSubscriber<TEvent> : IEventSubscriber<TEvent>
		{
			#region IEventSubscriber<TEvent> Members

			/// <summary>
			/// Handles a raised event
			/// </summary>
			/// <param name="sender">Event sender</param>
			/// <param name="args">Event object</param>
			public void OnEvent( object sender, TEvent args )
			{
			}

			#endregion
		}

		protected abstract IEventService CreateEventService( );

		protected AbstractEventServiceTest( )
		{
			BasicConfigurator.Configure( );
		}

		private readonly MockRepository m_Mocks = new MockRepository();
	}
}
