
using NUnit.Framework;
using Rb.Core.Maths;

namespace Rb.Tesselator.Tests
{
	[TestFixture]
	public class TestTesselator
	{
		[Test]
		public void TestSquare( )
		{
			TesselatorInput input = new TesselatorInput( );

			input.Polygon = new Point2[]
				{
					new Point2( 0, 0 ), 
					new Point2( 10, 0 ), 
					new Point2( 10, 10 ), 
					new Point2( 0, 10 )
				};

			Tesselator.PolygonLists polyLists = Tesselator.Tesselate( input );
			Assert.AreEqual( 1, polyLists.Lists.Length );
			Assert.AreEqual( 2, polyLists.Lists[ 0 ].NumPrimitives );
			Assert.AreEqual( 4, polyLists.Lists[ 0 ].Indices.Length );
		}
	}
}
