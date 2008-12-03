using NUnit.Framework;
using Rb.Core.Sets.Classes;
using Rb.Core.Sets.Interfaces;
using Rb.Core.Sets.Classes.Generic;

namespace Rb.Core.Tests
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class TestObjectSet : TestIObjectSet
	{
		protected override object CreateNewObject( )
		{
			return new object( );
		}

		protected override IObjectSet CreateObjectSet( )
		{
			return new ObjectSet( );
		}
	}

	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class TestObjectSetT : TestIObjectSet
	{
		protected override object CreateNewObject( )
		{
			return new object( );
		}

		protected override IObjectSet CreateObjectSet( )
		{
			return new ObjectSet<object>( );
		}
	}
}
