using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// Requests entity movement
	/// </summary>
	public class MovementRequest : ActionRequest
	{
		/// <summary>
		/// Sets up the movement request
		/// </summary>
		/// <param name="entity">Entity to be moved</param>
		/// <param name="pt">Position for the moomin to move to</param>
		public MovementRequest( IEntity entity, Maths.WorldPoint3 pt )
		{
			m_Entity = entity;
			m_Point = pt;
		}

		/// <summary>
		/// Sets the entity to the specified position
		/// </summary>
		public override void		Commit( )
		{
			m_Entity.Position = m_Point;
		}

		private IEntity				m_Entity;
		private Maths.WorldPoint3	m_Point;
	}
}
