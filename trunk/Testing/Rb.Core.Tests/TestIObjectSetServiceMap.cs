using System;
using NUnit.Framework;
using Rb.Core.Sets.Classes;
using Rb.Core.Sets.Interfaces;

namespace Rb.Core.Tests
{
	/// <summary>
	/// Tests implementations of <see cref="IObjectSetServiceMap"/>
	/// </summary>
	public abstract class TestIObjectSetServiceMap
	{
		/// <summary>
		/// Attempting to retrieve a service that is not in a service map should return null (not throw)
		/// </summary>
		[Test]
		public void TestRetrieveNonExistantServiceIsNull( )
		{
			Assert.IsNull( CreateServiceMap( ).Service<MockObjectSetService>( ) );
		}

		/// <summary>
		/// Attempting to retrieve a service that is not in a service map should return null (not throw)
		/// </summary>
		[Test]
		public void TestAddRemoveService( )
		{
			MockObjectSetService service = new MockObjectSetService( );
			IObjectSetServiceMap map = CreateServiceMap( );
			map.AddService( service );
			Assert.AreEqual( service, map.Service<MockObjectSetService>( ) );
			map.RemoveService( service );
			Assert.IsNull( map.Service<MockObjectSetService>( ) );

		}

		/// <summary>
		/// Attempting to add a null reference to the service map should fail
		/// </summary>
		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestAddNullServiceShouldThrow( )
		{
			CreateServiceMap( ).AddService( null );
		}

		/// <summary>
		/// Attempting to remove a null reference to the service map should fail
		/// </summary>
		[Test]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void TestRemoveNullServiceShouldThrow( )
		{
			CreateServiceMap( ).RemoveService( null );
		}

		/// <summary>
		/// Attempting to retrieve a service that is not in a service map should return null (not throw)
		/// </summary>
		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestAddingTwoEqualServicesShouldThrow( )
		{
			IObjectSetServiceMap map = CreateServiceMap( );
			map.AddService( new MockObjectSetService( ) );
			map.AddService( new MockObjectSetService( ) );
		}

		/// <summary>
		/// Adding a service without a sub-class or interface marked with the <see cref="ObjectSetServiceAttribute"/>
		/// should throw an ArgumentException
		/// </summary>
		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void TestAddingAServiceWithoutObjectSetServiceAttributeShouldThrow( )
		{
			CreateServiceMap( ).AddService( new MockObjectSetInvalidService( ) );
		}

		#region Protected Members

		/// <summary>
		/// Creates an object set service map for testing
		/// </summary>
		protected abstract IObjectSetServiceMap CreateServiceMap( );

		#endregion

		#region Private Members

		[ObjectSetService]
		private class MockObjectSetService : IObjectSetService
		{
		}

		private class MockObjectSetInvalidService : IObjectSetService
		{
		}

		#endregion
	}
}
