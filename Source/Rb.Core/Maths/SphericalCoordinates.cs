
namespace Rb.Core.Maths
{
	/// <summary>
	/// Contains functions for using spherical coordinates
	/// </summary>
	public static class SphericalCoordinates
	{
		/// <summary>
		/// Converts from a spherical coordinate to a 3d vector
		/// </summary>
		/// <param name="s">First angle, in range [0,2pi). Determines x and z components</param>
		/// <param name="t">Second angle, in range [0,pi). Determines y component</param>
		/// <returns>Returns a vector built from the spherical coordinate</returns>
		public static Vector3 ToVector( float s, float t )
		{
			float sinS = Functions.Sin( s );
			float cosS = Functions.Cos( s );
			float sinT = Functions.Sin( t );
			float cosT = Functions.Cos( t );

	        float x = ( cosS * sinT );
	        float y = ( cosT );
	        float z = ( sinS * sinT );
			return new Vector3( x, y, z );
		}

		public static Vector2 FromNormalizedVector( Vector3 src )
		{
			float s, t;
			FromNormalizedVector( src, out s, out t );
			return new Vector2( s, t );
		}

		public static void FromNormalizedVector( Vector3 src, out float s, out float t )
		{
			s = Functions.Atan( src.Z / src.X );
			t = Functions.Acos( src.Y );
		}
	}
}
