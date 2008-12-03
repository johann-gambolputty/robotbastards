using System;
using NUnit.Framework;
using Rb.Core.Sets.Interfaces;

namespace Rb.Core.Tests
{
	/// <summary>
	/// Tests on the base inteface <see cref="IObjectSet"/>
	/// </summary>
	public abstract class TestIObjectSet
	{
		/// <summary>
		/// Ensures that a new object set has a valid (non-null) service map
		/// </summary>
		[Test]
		public void TestObjectSetHasValidServiceMap( )
		{
			Assert.IsNotNull( CreateObjectSet( ).Services );
		}

		/// <summary>
		/// Passing null to <see cref="IObjectSet.Add"/> should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestAddNullObjectThrows( )
		{
			CreateObjectSet( ).Add( null );
		}

		/// <summary>
		/// Passing null to <see cref="IObjectSet.Remove"/> should throw
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestRemoveNullObjectThrows( )
		{
			CreateObjectSet( ).Remove( null );
		}

		/// <summary>
		/// Adding an object to an object set should make the object visible using the <see cref="IObjectSet.Contains"/> method
		/// </summary>
		[Test]
		public void TestContains( )
		{
			object obj = CreateNewObject( );
			IObjectSet set = CreateObjectSet( );
			Assert.IsFalse( set.Contains( obj ) );
			set.Add( obj );
			Assert.IsTrue( set.Contains( obj ) );
			set.Remove( obj );
			Assert.IsFalse( set.Contains( obj ) );
		}

		/// <summary>
		/// Ensures that the add and remove events fire as expected
		/// </summary>
		[Test]
		public void TestAddAndRemoveEvents( )
		{
			//	TODO: AP: Could do with rhino mocks here
			bool addedRaised = false;
			bool removedRaised = false;

			IObjectSet set = CreateObjectSet( );
			object obj = CreateNewObject( );
			set.ObjectAdded +=
				delegate( IObjectSet addSet, object addObj )
					{
						Assert.AreEqual( set, addSet );
						Assert.AreEqual( obj, addObj );
						addedRaised = true;
					};
			set.ObjectRemoved += 
				delegate ( IObjectSet remSet, object remObj )
					{
						Assert.AreEqual( set, remSet );
						Assert.AreEqual( obj, remObj );
						removedRaised = true;
					};
			set.Add( obj );
			Assert.IsTrue( addedRaised );
			Assert.IsFalse( removedRaised );
			addedRaised = false;
			set.Remove( obj );
			Assert.IsFalse( addedRaised );
			Assert.IsTrue( removedRaised );
		}

		/// <summary>
		/// Creates a new object to add to a set
		/// </summary>
		protected abstract object CreateNewObject( );

		/// <summary>
		/// Creates an object set to test
		/// </summary>
		protected abstract IObjectSet CreateObjectSet( );
	}
}
