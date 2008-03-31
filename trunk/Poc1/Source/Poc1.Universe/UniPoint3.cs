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
		
		public double SqrDistanceTo( UniPoint3 pt )
		{
			double diffX = ( m_X - pt.m_X );
			double diffY = ( m_Y - pt.m_Y );
			double diffZ = ( m_Z - pt.m_Z );

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

		#endregion

		#region Private Members

		private long m_X;
		private long m_Y;
		private long m_Z;

		#endregion
	}
}
