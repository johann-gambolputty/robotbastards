using System;
using RbEngine.Maths;

namespace RbEngine.Entities
{
	/// <summary>
	/// Requests entity movement on the XZ plane
	/// </summary>
	public class MovementXzRequest : Components.Message
	{
		/// <summary>
		/// X movement component
		/// </summary>
		public float	MoveX
		{
			get { return m_MoveX; }
			set { m_MoveX = value; }
		}

		/// <summary>
		/// Z movement component
		/// </summary>
		public float	MoveZ
		{
			get { return m_MoveZ; }
			set { m_MoveZ = value; }
		}

		/// <summary>
		/// Sets up the movement request
		/// </summary>
		/// <param name="entity">Entity to be moved</param>
		/// <param name="moveX">Movement on x axis</param>
		/// <param name="moveZ">Movement on z axis</param>
		public MovementXzRequest( Entity3 entity, float moveX, float moveZ )
		{
			m_Entity	= entity;
			m_MoveX		= moveX;
			m_MoveZ		= moveZ;
		}

		private Entity3	m_Entity;
		private float	m_MoveX;
		private float	m_MoveZ;
	}
}
