
using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// 2D line segment
	/// </summary>
	[Serializable]
	public class LineSegment2
	{
		#region	Construction and setup

		/// <summary>
		/// Zero length line at the origin
		/// </summary>
		public LineSegment2( )
		{
			m_Start	= new Point2( );
			m_End	= new Point2( );
		}

		/// <summary>
		/// Sets the start and end points of the line
		/// </summary>
		public LineSegment2( Point2 start, Point2 end )
		{
			Set( start, end );
		}

		/// <summary>
		/// Sets the start and end points of the line
		/// </summary>
		public void	Set( Point2 start, Point2 end )
		{
			m_Start = start;
			m_End	= end;
		}

		#endregion

		#region Static operations

		/// <summary>
		/// Gets a point on the line
		/// </summary>
		/// <param name="start">Start point of the line</param>
		/// <param name="end">End point of the line</param>
		/// <param name="t">Fraction along the line (0==start, 1==end)</param>
		/// <returns>Returns the evaluated point on the line</returns>
		public static Point2 GetPointOnLine( Point2 start, Point2 end, float t )
		{
			return start + ( end - start ) * t;
		}
		
		/// <summary>
		/// Returns a time value on the line that minimises the distance to pt
		/// </summary>
		public static float GetClosestTimeOnLine( Point2 start, Point2 end, Point2 pt )
		{
			Vector2 lineVec = end - start;
			float sqrLength = lineVec.SqrLength;
			float t = 0.0f;

			if ( sqrLength != 0.0f )
			{
				t = Utils.Clamp( ( pt - start).Dot( lineVec ) / sqrLength, 0.0f, 1.0f );
			}

			return t;
		}

		#endregion

		#region	Operations

		/// <summary>
		/// Gets a point on the line
		/// </summary>
		/// <param name="t">Fraction along the line (0==start, 1==end)</param>
		/// <returns>Returns the evaluated point on the line</returns>
		public Point2 GetPointOnLine( float t )
		{
			return GetPointOnLine( Start, End, t );
		}


		/// <summary>
		/// Returns a time value on the line that minimises the distance to pt
		/// </summary>
		public float GetClosestTimeOnLine( Point2 pt )
		{
			return GetClosestTimeOnLine( Start, End, pt );
		}

		/// <summary>
		/// Returns a point on the line that minimises the distance to pt
		/// </summary>
		public Point2 GetClosestPointOnLine( Point2 pt )
		{
			Point2 closestPt = GetPointOnLine( GetClosestTimeOnLine( pt ) );
			return closestPt;
		}

		/// <summary>
		/// Returns the squared distance from this line to a point
		/// </summary>
		/// <param name="pt"> Point to test </param>
		/// <returns> Squared distance to pt </returns>
		public float GetSqrDistanceToPoint( Point2 pt )
		{
			return GetClosestPointOnLine( pt ).SqrDistanceTo( pt );
		}

		/// <summary>
		/// Returns the distance from this line to a point
		/// </summary>
		/// <param name="pt"> Point to test </param>
		/// <returns> Distance to pt </returns>
		public float GetDistanceToPoint( Point2 pt )
		{
			return GetClosestPointOnLine( pt ).DistanceTo( pt );
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Start position on the line
		/// </summary>
		public Point2 Start
		{
			get { return m_Start; }
			set { m_Start = value; }
		}

		/// <summary>
		/// End position on the line
		/// </summary>
		public Point2 End
		{
			get { return m_End; }
			set { m_End = value; }
		}

		/// <summary>
		/// Length of the line
		/// </summary>
		public float Length
		{
			get
			{
				return m_Start.DistanceTo( m_End );
			}
		}

		#endregion

		#region	Private stuff

		private Point2 m_Start;
		private Point2 m_End;

		#endregion
	}
}
