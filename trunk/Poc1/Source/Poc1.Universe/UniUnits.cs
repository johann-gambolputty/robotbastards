
using Rb.Core.Maths;

namespace Poc1.Universe
{
	/// <summary>
	/// Determines the relationship between universe units (as embodied by the UniPoint, UniRay, UniTransform set
	/// of classes) and other units of measurement.
	/// </summary>
	public static class UniUnits
	{
		/// <summary>
		/// Astro render units are for rendering planetary bodies at astronomical distances
		/// </summary>
		public static class AstroRenderUnits
		{
			public const double MulToUnits = 100000;
			public const double MulFromUnits = 1.0 / MulToUnits;
			
			public static long ToUniUnits( double astroRenderUnits )
			{
				return ( long )( astroRenderUnits * MulToUnits );
			}
			
			public static double FromUniUnits( long units )
			{
				return units * MulFromUnits;
			}
		}

		/// <summary>
		/// Render units for standard close-in rendering. Because close-in rendering includes planetary terrain,
		/// which is generated on the surface of a sphere that is > 1000km in radius, 1 render unit == 10 metres
		/// (there's better ways to generate the terrain, but that's it for the moment).
		/// </summary>
		public static class RenderUnits
		{
			public const double MulToUnits = Metres.MulToUnits * 10;
			public const double MulFromUnits = 1.0 / MulToUnits;

			public const float MulToMetresF = ( float )( MulToUnits / Metres.MulToUnits );
			
			public static double ToMetres( double renderUnits )
			{
				return renderUnits * MulToMetresF;
			}

			public static float ToMetres( float renderUnits )
			{
				return renderUnits * MulToMetresF;
			}

			public static long ToUniUnits( double renderUnits )
			{
				return ( long )( renderUnits * MulToUnits );
			}

			public static double FromUniUnits( long units )
			{
				return units * MulFromUnits;
			}
			
			public static Point3 MakeRelativePoint( UniPoint3 origin, UniPoint3 pos )
			{
				double x = FromUniUnits( pos.X - origin.X );
				double y = FromUniUnits( pos.Y - origin.Y );
				double z = FromUniUnits( pos.Z - origin.Z );
				return new Point3( ( float )x, ( float )y, ( float )z );
			}
		}

		/// <summary>
		/// Metres, for readable interpretations of uni units
		/// </summary>
		public static class Metres
		{
			public const double MulToUnits = 1000;
			public const double MulFromUnits = 1.0 / MulToUnits;

			public static long ToUniUnits( double metres )
			{
				return ( long )( metres * MulToUnits );
			}
			
			public static double FromUniUnits( long units )
			{
				return units * MulFromUnits;
			}

			public static Point3 MakeRelativePoint( UniPoint3 origin, UniPoint3 pos )
			{
				double x = FromUniUnits( pos.X - origin.X );
				double y = FromUniUnits( pos.Y - origin.Y );
				double z = FromUniUnits( pos.Z - origin.Z );
				return new Point3( ( float )x, ( float )y, ( float )z );
			}
		}

	}
}
