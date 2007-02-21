using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// A TurnRequest is a message that requests that an entity turn around its local Y axis. 
	/// </summary>
	public class TurnRequest : Components.Message
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
		/// <param name="entity">Entity to apply the turn to</param>
		/// <param name="angle">Turn angle, in degrees (0 means no orientation change)</param>
		public TurnRequest( Entity3 entity, float angle )
		{
		}

		private Entity3	m_Entity;
		private float	m_Angle;
	}
}
