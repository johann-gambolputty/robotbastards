using System;
using System.Diagnostics;
using Rb.Core.Maths;

namespace Poc1.Universe
{
	/// <summary>
	/// A point within the universe (OK, solar system...)
	/// </summary>
	[DebuggerDisplay( "({X},{Y},{Z})" )]
	public class UniPoint3
	{

		public UniPoint3( )
		{
			m_X = 0;
			m_Y = 0;
			m_Z = 0;
		}

		public UniPoint3( long x, long y, long z )
		{
			m_X = x;
			m_Y = y;
			m_Z = z;
		}

		public UniPoint3( UniPoint3 src )
		{
			m_X = src.m_X;
			m_Y = src.m_Y;
			m_Z = src.m_Z;
		}

		public void Add( Vector3 vec )
		{
			m_X += ( long )vec.X;
			m_Y += ( long )vec.Y;
			m_Z += ( long )vec.Z;
		}

		public void Copy( UniPoint3 src )
		{
			m_X = src.m_X;
			m_Y = src.m_Y;
			m_Z = src.m_Z;
		}

		/// <summary>
		/// Converts this point to a string
		/// </summary>
		public string ToRenderUnitString( )
		{
			double x = UniUnits.RenderUnits.FromUniUnits( X );
			double y = UniUnits.RenderUnits.FromUniUnits( Y );
			double z = UniUnits.RenderUnits.FromUniUnits( Z );
			return string.Format( "({0:F2},{1:F2},{2:F2})", x, y, z );
		}
		
		public override int GetHashCode( )
		{
			//	Absolutely rubbish hash
			long res = m_X + m_Y + m_Z;
			return unchecked( ( int )res );
		}

		public override bool Equals( object obj )
		{
			UniPoint3 pt = ( obj as UniPoint3 );
			if ( pt == null )
			{
				return false;
			}

			return this == pt;
		}

		public long X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		public long Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		public long Z
		{
			get { return m_Z; }
			set { m_Z = value; }
		}

		public long ManhattanDistanceTo( UniPoint3 pt )
		{
			long diffX = ( m_X - pt.m_X );
			long diffY = ( m_Y - pt.m_Y );
			long diffZ = ( m_Z - pt.m_Z );

			return ( diffX < 0 ? -diffX : diffX ) + ( diffY < 0 ? -diffY : diffY ) + ( diffZ < 0 ? -diffZ : diffZ );
		}
		
		public long SqrDistanceTo( UniPoint3 pt )
		{
			long diffX = ( m_X - pt.m_X );
			long diffY = ( m_Y - pt.m_Y );
			long diffZ = ( m_Z - pt.m_Z );

			return ( diffX * diffX + diffY * diffY + diffZ * diffZ );
		}

		public double DistanceTo( UniPoint3 pt )
		{
			return Math.Sqrt( SqrDistanceTo( pt ) );
		}

		public Vector3 VectorTo( UniPoint3 pt )
		{
			double vecX = ( pt.m_X - m_X );
			double vecY = ( pt.m_Y - m_Y );
			double vecZ = ( pt.m_Z - m_Z );

			double invLength = 1.0 / Math.Sqrt( vecX * vecX + vecY * vecY + vecZ * vecZ );
			return new Vector3( ( float )( vecX * invLength ), ( float )( vecY * invLength ), ( float )( vecZ * invLength ) );
		}

		#region Operators

		public static UniPoint3 operator + ( UniPoint3 pt, Vector3 vec )
		{
			return new UniPoint3( pt.X + ( long )vec.X, pt.Y + ( long )vec.Y, pt.Z + ( long )vec.Z );
		}

		public static UniPoint3 operator - ( UniPoint3 pt, Vector3 vec )
		{
			return new UniPoint3( pt.X - ( long )vec.X, pt.Y - ( long )vec.Y, pt.Z - ( long )vec.Z );
		}
		
		public static bool operator == ( UniPoint3 lhs, UniPoint3 rhs )
		{
			return ( lhs.X == rhs.X ) && ( lhs.Y == rhs.Y ) && ( lhs.Z == rhs.Z );
		}

		public static bool operator != ( UniPoint3 lhs, UniPoint3 rhs )
		{
			return ( lhs.X != rhs.X ) || ( lhs.Y != rhs.Y ) || ( lhs.Z != rhs.Z );
		}

		#endregion

		#region Private Members

		private long m_X;
		private long m_Y;
		private long m_Z;

		#endregion
	}
}
