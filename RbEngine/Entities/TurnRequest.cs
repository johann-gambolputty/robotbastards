using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// A TurnRequest is an action request that turns the agent around its local Y axis. 
	/// </summary>
	public class TurnRequest : ActionRequest
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
		/// <param name="entity">Entity to apply the </param>
		/// <param name="angle"></param>
		public TurnRequest( Entity3 entity, float angle )
		{
		}

		private Entity3	m_Entity;
		private float	m_Angle;
	}
}
