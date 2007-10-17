using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Stores two values, and provides means of interpolating between them
	/// </summary>
	[Serializable]
	public class FloatInterpolator
	{
		#region	Access

		/// <summary>
		/// The starting position
		/// </summary>
		public float Start
		{
			get { return m_Start; }
			set { m_Start = value; }
		}

		/// <summary>
		/// The current position
		/// </summary>
		/// <remarks>
		/// The current position can only be changed by calling UpdateCurrent()
		/// </remarks>
		public float Current
		{
			get { return m_Current; }
		}

		/// <summary>
		/// The end position
		/// </summary>
		public float End
		{
			get { return m_End; }
			set { m_End = value; }
		}

		/// <summary>
		/// Sets start, current and end points to a given position
		/// </summary>
		/// <param name="pt">Position</param>
		public void Set( float pt )
		{
			m_Current = pt;
			m_Start = pt;
			m_End = pt;
		}

		/// <summary>
		/// Calculates an intermediate point between the previous and next points
		/// </summary>
		/// <param name="t">Time, in the range [0..1]</param>
		/// <returns>Returns Previous, if t is 0, next if t is 1, and an intermediate position if t is inbetween 0 and 1</returns>
		public float UpdateCurrent( float t )
		{
			m_Current = ( Start + ( End - Start ) * ( t < 0 ? 0 : ( t > 1 ? 1 : t ) ) );
			return m_Current;
		}

		/// <summary>
		/// Calculates an intermediate point between the previous and next points
		/// </summary>
		/// <param name="time">Time, in the same units as LastStepTime, LastStepInterval (TinyTime units)</param>
		/// <returns>Returns Previous, if t is 0, Next if t is 1, and an intermediate position if t is inbetween 0 and 1</returns>
		public float UpdateCurrent( long time )
		{
			return UpdateCurrent( ( time - m_LastStepTime ) / ( float )m_LastStepInterval );
		}

		#endregion

		#region	Step

		/// <summary>
		/// Copies the end position in the start position, updates the current position
		/// </summary>
		public void Step( long curTime )
		{
			m_Start				= m_End;
			m_LastStepInterval	= ( curTime - m_LastStepTime );
			m_LastStepTime		= curTime;
			UpdateCurrent( curTime );
		}

		#endregion

		#region	Timing	

		/// <summary>
		/// The last time interval between Step() calls
		/// </summary>
		public long LastStepInterval
		{
			get { return m_LastStepInterval; }
		}

		/// <summary>
		/// The time that was passed to the last Step() call
		/// </summary>
		public long LastStepTime
		{
			get { return m_LastStepTime; }
		}

		#endregion

		#region	Private stuff

		private long	m_LastStepInterval;
		private long	m_LastStepTime;
		private float	m_Start;
		private float	m_End;
		private float	m_Current;

		#endregion

	}
}
