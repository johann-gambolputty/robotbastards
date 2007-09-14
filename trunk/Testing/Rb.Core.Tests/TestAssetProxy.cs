using System;
using NUnit.Framework;
using Rb.Core.Assets;

namespace Rb.Core.Tests
{
	[TestFixture]
	public class TestAssetProxy
	{
		/// <summary>
		/// Base test interface
		/// </summary>
		public interface ITestBase
		{
			/// <summary>
			/// Returns input multiplied by 10
			/// </summary>
			int Call( int input );
		}

		/// <summary>
		/// Test interface. We want to generate a proxy that implements this interface, and ITestBase
		/// </summary>
		public interface ITest : ITestBase
		{
			event Action< ITest > PreCall;

			/// <summary>
			/// Name getter/setter
			/// </summary>
			string Name
			{
				set;
				get;
			}
		}

		/// <summary>
		/// Implementation of ITest. The proxy will implement ITest, but defer all calls/properties/events to an
		/// instance of this class
		/// </summary>
		public class TestImpl : ITest
		{
			#region ITest Members

			public event Action< ITest > PreCall;

			public string Name
			{
				set { m_Name = value; }
				get { return m_Name; }
			}

			public int Call(int input)
			{
				if ( PreCall != null )
				{
					PreCall( this );
				}
				return input * 10;
			}

			#endregion

			private string m_Name = "pie";
		}

		private static void ChangeName( ITest test )
		{
			test.Name = "badgers";
		}

		/// <summary>
		/// Tests <see cref="AssetProxy"/>
		/// </summary>
		[Test]
		public void Test0( )
		{
			ITest proxy = ( ITest )AssetProxy.CreateProxy( typeof( ITest ), new TestImpl( ) );

			//	Test property change (interface property setter/getter implementation)
			proxy.Name = "PIE";
			Assert.AreEqual( proxy.Name, "PIE" );

			//	Test method call
			Assert.AreEqual( proxy.Call( 5 ), 50 );

			//	Test event add, remove, raise
			proxy.PreCall += ChangeName;				//	Subscribe ChangeName - will change name to "badgers" on Call()
			Assert.AreEqual( proxy.Call( -1 ), -10 );	//	Call will raise PreCall event -> ChangeName
			Assert.AreEqual( proxy.Name, "badgers" );	//	Name should now be "badgers"
			proxy.Name = "pie";							//	Reset the name
			proxy.PreCall -= ChangeName;				//	Unsubscribe ChangeName
			Assert.AreEqual( proxy.Call( 0 ), 0 );		//	Call should not raise any events
			Assert.AreEqual( proxy.Name, "pie" );		//	Therefore, name should be unchanged
		}

	}
}
