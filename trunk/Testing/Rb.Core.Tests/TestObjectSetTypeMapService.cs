using System;
using NUnit.Framework;
using Rb.Core.Sets.Classes;

namespace Rb.Core.Tests
{
	[TestFixture]
	public class TestObjectSetTypeMapService
	{
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestAssignToNullSetShouldThrow( )
		{
			new ObjectSetTypeMapService( null );
		}


	}
}
