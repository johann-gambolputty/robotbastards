using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// A TurnRequest is a message that requests that an entity turn around its local Y axis. 
	/// </summary>
	public class TurnRequest : RotationRequest
	{
		/// <summary>
		/// Gets or sets the turn request angle in degrees
		/// </summary>
		public float	AngleDegrees
		{
			get
			{
				return m_Angle;
			}
			set
			{
				m_Angle = value;
			}
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="angle">Turn angle, in degrees (0 means no orientation change)</param>
		public TurnRequest( float angle )
		{
			m_Angle = angle;
		}

		private float	m_Angle;
	}
}
