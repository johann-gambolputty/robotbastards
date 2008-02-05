using System;

namespace Rb.Core.Maths
{
	public static class Functions
	{
		public static float Sqrt( float x )
		{
			return ( float )Math.Sqrt( x );
		}

		public static float Exp( float x )
		{
			return ( float )Math.Exp( x );
		}

		#region Trigonometry

		public static float Sin( float x )
		{
			return ( float )Math.Sin( x );
		}
		
		public static float Cos( float x )
		{
			return ( float )Math.Cos( x );
		}
		
		public static float Tan( float x )
		{
			return ( float )Math.Tan( x );
		}

		public static float Acos( float x )
		{
			return ( float )Math.Acos( x );
		}

		public static float Atan( float x )
		{
			return ( float )Math.Atan( x );
		}

		public static float Atan2( float x, float y )
		{
			return ( float )Math.Atan2( x, y );
		}

		#endregion
	}
}
