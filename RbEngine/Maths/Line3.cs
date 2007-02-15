using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Summary description for Line3.
	/// </summary>
	public class Line3
	{
		#region	Construction and setup

		/// <summary>
		/// Zero length line at the origin
		/// </summary>
		public Line3( )
		{
			m_Start	= new Vector3( );
			m_End	= new Vector3( );
		}

		/// <summary>
		/// Sets the start and end points of the line
		/// </summary>
		public Line3( Vector3 start, Vector3 end )
		{
			Set( start, end );
		}

		/// <summary>
		/// Sets the start and end points of the line
		/// </summary>
		public void	Set( Vector3 start, Vector3 end )
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
		public float	GetSqrDistanceToPoint( Vector3 pt )
		{
			Vector3	lineVec		= End - Start;
			float	sqrLength	= lineVec.SqrLength;
			float	t			= 0.0f;

			if ( sqrLength != 0.0f )
			{
				t = Utils.Clamp( ( pt - Start ).Dot( lineVec ) / sqrLength, 0.0f, 1.0f );
			}

			Vector3 closestPt = Start + ( lineVec * t );
			return closestPt.SqrDistanceTo( pt );
		}

		/// <summary>
		/// Returns the distance from this line to a point
		/// </summary>
		/// <param name="pt"> Point to test </param>
		/// <returns> Distance to pt </returns>
		public float	GetDistanceToPoint( Vector3 pt )
		{
			return ( float )System.Math.Sqrt( GetSqrDistanceToPoint( pt ) );
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Start position on the line
		/// </summary>
		public Vector3	Start
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
		public Vector3	End
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

		Vector3	m_Start;
		Vector3	m_End;

		#endregion
	}
}
