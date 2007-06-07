using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Rb.Core.Utils;

namespace Rb.Core.Tests
{
	[TestFixture]
	public class TestDispatchMap
	{
		public class Base { }
		public class Derived0 : Base { }
		public class Derived1 : Base { }
		public class Derived2 : Base { }
		public class Derived3 : Base { }

		public enum MethodId
		{
			Handler_Base,
			Handler_Derived0,
			Handler_Derived1,
			Handler_Derived2,
			DerivedHandler_Base
		}

		public class Handler
		{
			[Dispatch]
			public virtual MethodId Handle( Base inst ) { return MethodId.Handler_Base; }

			[Dispatch]
			public MethodId Handle( Derived0 inst ) { return MethodId.Handler_Derived0; }

			[Dispatch]
			public static MethodId Handle( Derived1 inst ) { return MethodId.Handler_Derived1; }

			[Dispatch]
			public MethodId Handle( Derived2 inst ) { return MethodId.Handler_Derived2; }
		}

		public class DerivedHandler : Handler
		{
			public override MethodId Handle( Base inst ) { return MethodId.DerivedHandler_Base; }
		}

		[Test]
		public void Test0( )
		{
			DispatchMap map = DispatchMapT< Handler >.Instance;

			Handler handler = new DerivedHandler( );

			MethodId id;

			id = ( MethodId )map.Dispatch( handler, new Base( ) );
			Assert.AreEqual( id, MethodId.DerivedHandler_Base );

			id = ( MethodId )map.Dispatch( handler, new Derived0( ) );
			Assert.AreEqual( id, MethodId.Handler_Derived0 );

			id = ( MethodId )map.Dispatch( handler, new Derived1( ) );
			Assert.AreEqual( id, MethodId.Handler_Derived1 );

			id = ( MethodId )map.Dispatch( handler, new Derived2( ) );
			Assert.AreEqual( id, MethodId.Handler_Derived2 );

			id = ( MethodId )map.Dispatch( handler, new Derived3( ) );
			Assert.AreEqual( id, MethodId.DerivedHandler_Base );

		}
	}
}
