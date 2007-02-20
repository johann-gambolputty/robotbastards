using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// World position
	/// </summary>
	public class WorldPoint3
	{
		public WorldPoint3( )
		{
		}

		public long X
		{
			get { return m_Pt[ 0 ]; }
			set { m_Pt[ 0 ] = value; }
		}
		
		public long Y
		{
			get { return m_Pt[ 1 ]; }
			set { m_Pt[ 1 ] = value; }
		}

		public long Z
		{
			get { return m_Pt[ 2 ]; }
			set { m_Pt[ 2 ] = value; }
		}

		public static Vector3	operator - ( WorldPoint3 pt1, WorldPoint3 pt2 )
		{
			return new Vector3( ( float )( pt1.X - pt2.X ), ( float )( pt1.Y - pt2.Y ), ( float )( pt1.Z - pt2.Z ) );
		}

		public float	SqrDistanceTo( WorldPoint3 pt )
		{
			float diffX = ( float )( X - pt.X );
			float diffY = ( float )( Y - pt.Y );
			float diffZ = ( float )( Z - pt.Z );

			return ( diffX * diffX ) + ( diffY * diffY ) + ( diffZ * diffZ );
		}

		public float	DistanceTo( WorldPoint3 pt )
		{
			return ( float )System.Math.Sqrt( SqrDistanceTo( pt ) );
		}

		private long[]	m_Pt = new long[ 3 ] { 0, 0, 0 };
	}
}
