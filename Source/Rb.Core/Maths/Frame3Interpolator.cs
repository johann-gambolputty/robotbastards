using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Interpolator for position + orientation
	/// </summary>
	[Serializable]
	public class Frame3Interpolator
	{
		#region Position

		/// <summary>
		/// Gets the starting position
		/// </summary>
		public Point3 StartPosition
		{
			get { return m_StartPos; }
			set { m_StartPos = value; }
		}

		/// <summary>
		/// Gets the current position
		/// </summary>
		public Point3 CurrentPosition
		{
			get { return m_CurPos; }
			set { m_CurPos = value; }
		}

		/// <summary>
		/// Gets the ending position
		/// </summary>
		public Point3 EndPosition
		{
			get { return m_EndPos; }
			set { m_EndPos = value; }
		}

		#endregion

		#region Orientation

		//	TODO: AP: Move to quaternions

		/// <summary>
		/// Gets the starting angle
		/// </summary>
		public float StartAngle
		{
			get { return m_StartAngle; }
			set { m_StartAngle = value; }
		}


		/// <summary>
		/// Gets the current angle
		/// </summary>
		public float CurrentAngle
		{
			get { return m_CurAngle; }
			set { m_CurAngle = value; }
		}

		/// <summary>
		/// Gets the end angle
		/// </summary>
		public float EndAngle
		{
			get { return m_EndAngle; }
			set { m_EndAngle = value; }
		}

		#endregion

		#region Updates

		/// <summary>
		/// Forces the start, current and end positions to given values
		/// </summary>
		public void Set( Point3 pos )
		{
			m_StartPos = pos;
			m_CurPos = pos;
			m_EndPos = pos;
		}
		
		/// <summary>
		/// Forces the start, current and end angles to given values
		/// </summary>
		public void Set( float angle )
		{
			m_StartAngle = angle;
			m_CurAngle = angle;
			m_EndAngle = angle;
		}

		/// <summary>
		/// Forces the start, current and end positions and angles to given values
		/// </summary>
		public void Set( Point3 pos, float angle )
		{
			m_StartPos = pos;
			m_CurPos = pos;
			m_EndPos = pos;
			m_StartAngle = angle;
			m_CurAngle = angle;
			m_EndAngle = angle;
		}

		/// <summary>
		/// Updates the current angle and position
		/// </summary>
		/// <param name="t">Interpolation value [0-1]</param>
		public void UpdateCurrent( float t )
		{
			m_CurPos = ( m_StartPos + ( m_EndPos - m_StartPos ) * t );
			m_CurAngle = ( m_StartAngle + ( m_EndAngle - m_StartAngle ) * t );
		}

		/// <summary>
		/// Updates the current angle and position
		/// </summary>
		/// <param name="time">Update time</param>
		public void UpdateCurrent( long time )
		{
			if ( m_LastStepTime == 0 )
			{
				m_LastStepInterval = 1;
				m_LastStepTime = time;
			}
			UpdateCurrent( ( time - m_LastStepTime ) / ( float )m_LastStepInterval );
		}
		
		/// <summary>
		/// Copies the end position and angle in the start position and angle, updates the current position
		/// </summary>
		public void Step( long curTime )
		{
			m_StartPos			= m_EndPos;
			m_StartAngle		= m_EndAngle;
			m_LastStepInterval	= ( curTime - m_LastStepTime );
			m_LastStepTime		= curTime;
			
			//	TODO: AP: This effectively sets the current frame to the start frame
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

		#region Private stuff

		private Point3 m_StartPos;
		private float m_StartAngle;

		private Point3 m_CurPos;
		private float m_CurAngle;

		private Point3 m_EndPos;
		private float m_EndAngle;

		private long m_LastStepInterval;
		private long m_LastStepTime;

		#endregion
	}
}
