using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Stores two points and provides a method for interpolating between them
	/// </summary>
	public class Point3Interpolator
	{
		#region	Access

		/// <summary>
		/// The previous position
		/// </summary>
		public Point3	Previous
		{
			get 
			{
				return m_Previous; 
			}
		}

		/// <summary>
		/// The next position
		/// </summary>
		public Point3	Next
		{
			get
			{
				return m_Next;
			}
			set
			{
				m_Next = value;
			}
		}

		/// <summary>
		/// Calculates an intermediate point between the previous and next points
		/// </summary>
		/// <param name="t">Time, in the range [0..1]</param>
		/// <returns>Returns Previous, if t is 0, next if t is 1, and an intermediate position if t is inbetween 0 and 1</returns>
		public Point3	Get( float t )
		{
			return ( Previous + ( Next - Previous ) * ( t < 0 ? 0 : ( t > 1 ? 1 : t ) ) );
		}

		/// <summary>
		/// Calculates an intermediate point between the previous and next points
		/// </summary>
		/// <param name="time">Time, in the same units as LastStepTime, LastStepInterval (TinyTime units)</param>
		/// <returns>Returns Previous, if t is 0, Next if t is 1, and an intermediate position if t is inbetween 0 and 1</returns>
		public Point3	Get( long time )
		{
			return Get( ( float )( time - m_LastStepTime ) / ( float )m_LastStepInterval );
		}

		#endregion

		#region	Step

		/// <summary>
		/// Copies the next position in the previous position
		/// </summary>
		public void		Step( long curTime )
		{
			m_Previous			= m_Next;
			m_Next				= new Point3( m_Next );
			m_LastStepInterval	= ( curTime - m_LastStepTime );
			m_LastStepTime		= curTime;
		}

		#endregion

		#region	Timing	

		/// <summary>
		/// The last time interval between Step() calls
		/// </summary>
		public long		LastStepInterval
		{
			get
			{
				return m_LastStepInterval;
			}
		}

		/// <summary>
		/// The time that was passed to the last Step() call
		/// </summary>
		public long		LastStepTime
		{
			get
			{
				return m_LastStepTime;
			}
		}

		#endregion

		#region	Private stuff

		private long	m_LastStepInterval;
		private long	m_LastStepTime;
		private Point3	m_Previous	= new Point3( );
		private Point3	m_Next		= new Point3( );

		#endregion

	}
}
