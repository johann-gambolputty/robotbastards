using System;
using RbEngine.Maths;

namespace RbEngine.Entities
{
	/// <summary>
	/// Requests entity movement on the XZ plane
	/// </summary>
	public class MovementXzRequest : MovementRequest
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
		/// Local or global movement flag
		/// </summary>
		public bool		Local
		{
			get { return m_Local; }
			set { m_Local = value; }
		}

		/// <summary>
		/// Sets up the movement request
		/// </summary>
		/// <param name="moveX">Movement on x axis</param>
		/// <param name="moveZ">Movement on z axis</param>
		/// <param name="local">If true, then movement is relative to the local coordinate system of the object being moved (false uses the global orthonormal basis)</param>
		public MovementXzRequest( float moveX, float moveZ, bool local )
		{
			m_MoveX		= moveX;
			m_MoveZ		= moveZ;
			m_Local		= local;
		}

		private float	m_MoveX;
		private float	m_MoveZ;
		private bool	m_Local;
	}
}
