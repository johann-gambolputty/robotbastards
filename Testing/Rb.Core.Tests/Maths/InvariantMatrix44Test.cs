
using NUnit.Framework;
using Rb.Core.Maths;

namespace Rb.Core.Tests.Maths
{
	[TestFixture]
	public class InvariantMatrix44Test
	{
		[Test]
		public void TestXRotation( )
		{
			InvariantMatrix44 rotation = InvariantMatrix44.MakeRotationAroundXAxisMatrix( 90 * Constants.DegreesToRadians );
			Assert.IsTrue( rotation.Multiply( new Point3( 1, 0, 0 ) ).DistanceTo( new Point3( 1, 0, 0 ) ) < 0.0001f );
			Assert.IsTrue( rotation.Multiply( new Point3( 0, 1, 0 ) ).DistanceTo( new Point3( 0, 0, 1 ) ) < 0.0001f );
			Assert.IsTrue( rotation.Multiply( new Point3( 0, 0, 1 ) ).DistanceTo( new Point3( 0, -1, 0 ) ) < 0.0001f );
		}

		[Test]
		public void TestYRotation( )
		{
			InvariantMatrix44 rotation = InvariantMatrix44.MakeRotationAroundYAxisMatrix( 90 * Constants.DegreesToRadians );
			Assert.IsTrue( rotation.Multiply( new Point3( 1, 0, 0 ) ).DistanceTo( new Point3( 0, 0, 1 ) ) < 0.0001f );
			Assert.IsTrue( rotation.Multiply( new Point3( 0, 1, 0 ) ).DistanceTo( new Point3( 0, 1, 0 ) ) < 0.0001f );
			Assert.IsTrue( rotation.Multiply( new Point3( 0, 0, 1 ) ).DistanceTo( new Point3( -1, 0, 0 ) ) < 0.0001f );
		}

		[Test]
		public void TestZRotation( )
		{
			InvariantMatrix44 rotation = InvariantMatrix44.MakeRotationAroundZAxisMatrix( 90 * Constants.DegreesToRadians );
			Assert.IsTrue( rotation.Multiply( new Point3( 1, 0, 0 ) ).DistanceTo( new Point3( 0, 1, 0 ) ) < 0.0001f );
			Assert.IsTrue( rotation.Multiply( new Point3( 0, 1, 0 ) ).DistanceTo( new Point3( -1, 0, 0 ) ) < 0.0001f );
			Assert.IsTrue( rotation.Multiply( new Point3( 0, 0, 1 ) ).DistanceTo( new Point3( 0, 0, 1 ) ) < 0.0001f );
		}

		[Test]
		public void TestInvertIdentityMatrix( )
		{
			InvariantMatrix44 lhs = InvariantMatrix44.Identity;
			InvariantMatrix44 rhs = lhs.Invert( );
			Assert.IsTrue( ( lhs * rhs ).IsCloseTo( InvariantMatrix44.Identity, 0.001f ) );
		}

		[Test]
		public void TestInvertScaleMatrix( )
		{
			InvariantMatrix44 lhs = InvariantMatrix44.MakeScaleMatrix( 1, 2, 3 );
			InvariantMatrix44 rhs = lhs.Invert( );
			Assert.IsTrue( ( lhs * rhs ).IsCloseTo( InvariantMatrix44.Identity, 0.001f ) );
		}

		[Test]
		public void TestInvertTranslationMatrix( )
		{
			InvariantMatrix44 lhs = InvariantMatrix44.MakeTranslationMatrix( 1, 2, 3 );
			InvariantMatrix44 rhs = lhs.Invert( );
			Assert.IsTrue( ( lhs * rhs ).IsCloseTo( InvariantMatrix44.Identity, 0.001f ) );
		}

		[Test]
		public void TestInvertRotationAndTranslationMatrix( )
		{
			InvariantMatrix44 lhs = InvariantMatrix44.MakeTranslationMatrix( 1, 2, 3 ) * InvariantMatrix44.MakeRotationAroundXAxisMatrix( 90 * Constants.DegreesToRadians );
			InvariantMatrix44 rhs = lhs.Invert( );
			Assert.IsTrue( ( lhs * rhs ).IsCloseTo( InvariantMatrix44.Identity, 0.001f ) );
		}

		[Test]
		public void TestInvertRotationMatrix( )
		{
			InvariantMatrix44 lhs = InvariantMatrix44.MakeRotationAroundXAxisMatrix( 90 * Constants.DegreesToRadians );
			InvariantMatrix44 rhs = lhs.Invert( );
			Assert.IsTrue( ( lhs * rhs ).IsCloseTo( InvariantMatrix44.Identity, 0.001f ) );

			lhs = InvariantMatrix44.MakeRotationAroundYAxisMatrix( 90 * Constants.DegreesToRadians );
			rhs = lhs.Invert( );
			Assert.IsTrue( ( lhs * rhs ).IsCloseTo( InvariantMatrix44.Identity, 0.001f ) );

			lhs = InvariantMatrix44.MakeRotationAroundZAxisMatrix( 90 * Constants.DegreesToRadians );
			rhs = lhs.Invert( );
			Assert.IsTrue( ( lhs * rhs ).IsCloseTo( InvariantMatrix44.Identity, 0.001f ) );
		}
	}
}
