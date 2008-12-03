using System;
using NUnit.Framework;
using Rb.Core.Components;

namespace Rb.Core.Tests
{
	/// <summary>
	/// Tests <see cref="ObjectTypeMap"/>
	/// </summary>
	[TestFixture]
	public class TestObjectTypeMap
	{
		/// <summary>
		/// Adding a null reference to an object type map should throw an ArgumentNullException
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestAddNullObjectShouldThrow( )
		{
			new ObjectTypeMap<object>( ).Add( null );
		}

		/// <summary>
		/// Removing a null reference to an object type map should throw an ArgumentNullException
		/// </summary>
		[Test, ExpectedException( typeof( ArgumentNullException ) )]
		public void TestRemoveNullObjectShouldThrow( )
		{
			new ObjectTypeMap<object>( ).Remove( null );
		}

		/// <summary>
		/// Tests adding one object to a map
		/// </summary>
		[Test]
		public void TestAddOneObject( )
		{
			ObjectTypeMap<BaseClass> map = new ObjectTypeMap<BaseClass>( );
			DerivedClass obj = new DerivedClass( );
			map.Add( obj );
			IsFirst( map, obj );
			Contains( map, obj );

			map.Remove( obj );
			DoesNotContain( map, obj );
		}

		/// <summary>
		/// Tests adding one object to a map
		/// </summary>
		[Test]
		public void TestAddMultipleObjects( )
		{
			ObjectTypeMap<BaseClass> map = new ObjectTypeMap<BaseClass>( );
			DerivedClass obj0 = new DerivedClass( );
			DerivedClass obj1 = new DerivedClass( );
			map.Add( obj0 );
			map.Add( obj1 );
			IsFirst( map, obj0 );
			Contains( map, obj0 );
			Contains( map, obj1 );

			map.Remove( obj0 );
			DoesNotContain( map, obj0 );
			Contains( map, obj1 );

			map.Remove( obj1 );
			DoesNotContain( map, obj1 );
		}

		#region Private Members

		/// <summary>
		/// Checks that an object does not exist in a map, using <see cref="ObjectTypeMap{T}.GetObjectsOfType{X}"/>
		/// </summary>
		private static void DoesNotContain( ObjectTypeMap<BaseClass> map, BaseClass obj )
		{
			Assert.IsFalse( map.GetObjectsOfType( typeof( BaseClass ) ).Contains( obj ) );
			Assert.IsFalse( map.GetObjectsOfType( typeof( DerivedClass ) ).Contains( obj ) );
			Assert.IsFalse( map.GetObjectsOfType( typeof( IInterface0 ) ).Contains( obj ) );
			Assert.IsFalse( map.GetObjectsOfType( typeof( IInterface1 ) ).Contains( obj ) );

			Assert.IsFalse( map.GetObjectsOfType<BaseClass>( ).Contains( obj ) );
			Assert.IsFalse( map.GetObjectsOfType<DerivedClass>( ).Contains( ( DerivedClass )obj ) );
			Assert.IsFalse( map.GetObjectsOfType<IInterface0>( ).Contains( obj ) );
			Assert.IsFalse( map.GetObjectsOfType<IInterface1>( ).Contains( ( IInterface1 )obj ) );

			Assert.IsFalse( map.Values.Contains( obj ) );
		}


		/// <summary>
		/// Checks that an object exists in a map, using <see cref="ObjectTypeMap{T}.GetObjectsOfType{X}"/>
		/// </summary>
		private static void Contains( ObjectTypeMap<BaseClass> map, BaseClass obj )
		{
			Assert.Contains( obj, map.GetObjectsOfType( typeof( BaseClass ) ) );
			Assert.Contains( obj, map.GetObjectsOfType( typeof( DerivedClass ) ) );
			Assert.Contains( obj, map.GetObjectsOfType( typeof( IInterface0 ) ) );
			Assert.Contains( obj, map.GetObjectsOfType( typeof( IInterface1 ) ) );

			Assert.Contains( obj, map.GetObjectsOfType<BaseClass>( ) );
			Assert.Contains( obj, map.GetObjectsOfType<DerivedClass>( ) );
			Assert.Contains( obj, map.GetObjectsOfType<IInterface0>( ) );
			Assert.Contains( obj, map.GetObjectsOfType<IInterface1>( ) );

			Assert.Contains( obj, map.Values );
		}

		/// <summary>
		/// Checks that an object exists in a map, using <see cref="ObjectTypeMap{T}.GetFirstOfType{T}"/>
		/// </summary>
		private static void IsFirst( ObjectTypeMap<BaseClass> map, BaseClass obj )
		{
			Assert.AreEqual( obj, map.GetFirstOfType( typeof( BaseClass ) ) );
			Assert.AreEqual( obj, map.GetFirstOfType( typeof( DerivedClass ) ) );
			Assert.AreEqual( obj, map.GetFirstOfType( typeof( IInterface0 ) ) );
			Assert.AreEqual( obj, map.GetFirstOfType( typeof( IInterface1 ) ) );

			Assert.AreEqual( obj, map.GetFirstOfType<BaseClass>( ) );
			Assert.AreEqual( obj, map.GetFirstOfType<DerivedClass>( ) );
			Assert.AreEqual( obj, map.GetFirstOfType<IInterface0>( ) );
			Assert.AreEqual( obj, map.GetFirstOfType<IInterface1>( ) );
		}

		private interface IInterface0
		{
		}

		private interface IInterface1
		{
		}

		private class BaseClass : IInterface0
		{
		}

		private class DerivedClass : BaseClass, IInterface1
		{
		}

		#endregion

	}
}
