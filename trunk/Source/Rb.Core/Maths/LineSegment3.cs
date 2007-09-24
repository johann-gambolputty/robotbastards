using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Summary description for Line3.
	/// </summary>
	public class LineSegment3
	{
		#region	Construction and setup

		/// <summary>
		/// Zero length line at the origin
		/// </summary>
		public LineSegment3( )
		{
			m_Start	= new Point3( );
			m_End	= new Point3( );
		}

		/// <summary>
		/// Sets the start and end points of the line
		/// </summary>
		public LineSegment3( Point3 start, Point3 end )
		{
			Set( start, end );
		}

		/// <summary>
		/// Sets the start and end points of the line
		/// </summary>
		public void	Set( Point3 start, Point3 end )
		{
			m_Start = start;
			m_End	= end;
		}

		#endregion

		#region	Operations

		/// <summary>
		/// Returns the squared distance from this line to a point
		/// </summary>
		/// <param name="pt"> Point to test </param>
		/// <returns> Squared distance to pt </returns>
		public float GetSqrDistanceToPoint( Point3 pt )
		{
			Vector3	lineVec		= End - Start;
			float	sqrLength	= lineVec.SqrLength;
			float	t			= 0.0f;

			if ( sqrLength != 0.0f )
			{
				t = Utils.Clamp( ( pt - Start ).Dot( lineVec ) / sqrLength, 0.0f, 1.0f );
			}

			Point3 closestPt = Start + ( lineVec * t );
			return closestPt.SqrDistanceTo( pt );
		}

		/// <summary>
		/// Returns the distance from this line to a point
		/// </summary>
		/// <param name="pt"> Point to test </param>
		/// <returns> Distance to pt </returns>
		public float GetDistanceToPoint( Point3 pt )
		{
			return ( float )Math.Sqrt( GetSqrDistanceToPoint( pt ) );
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Start position on the line
		/// </summary>
		public Point3	Start
		{
			get
			{
				return m_Start;
			}
			set
			{
				m_Start = value;
			}
		}

		/// <summary>
		/// End position on the line
		/// </summary>
		public Point3	End
		{
			get
			{
				return m_End;
			}
			set
			{
				m_End = value;
			}
		}

		/// <summary>
		/// Length of the line
		/// </summary>
		public float	Length
		{
			get
			{
				return m_Start.DistanceTo( m_End );
			}
		}

		#endregion

		#region	Private stuff

		Point3	m_Start;
		Point3	m_End;

		#endregion
	}
}
