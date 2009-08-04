using Goo.Core.Services.Events;
using NUnit.Framework;

namespace Goo.Core.Test.Services.Events
{
	[TestFixture]
	public class EventServiceTest : AbstractEventServiceTest
	{
		/// <summary>
		/// Creates an event service to test
		/// </summary>
		protected override IEventService CreateEventService()
		{
			return new EventService( );
		}
	}
}
