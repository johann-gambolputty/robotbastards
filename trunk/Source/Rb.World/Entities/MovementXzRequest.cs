using System;
using Rb.Core.Maths;

namespace Rb.World.Entities
{
	/// <summary>
	/// XZ movement request
	/// </summary>
	public class MovementXzRequest : MovementRequest
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="deltaX">X movement</param>
		/// <param name="deltaZ">Z movement</param>
		public MovementXzRequest( float deltaX, float deltaZ )
		{
			m_X = deltaX;	
			m_Z = deltaZ;
		}

		/// <summary>
		/// Gets the movement along the local X axis
		/// </summary>
		public float DeltaX
		{
			get { return m_X;  }
		}

		/// <summary>
		/// Gets the movement along the local Z axis
		/// </summary>
		public float DeltaZ
		{
			get { return m_Z; }
		}

		/// <summary>
		/// Returns the distance covered by the move
		/// </summary>
		public override float Distance
		{
			get
			{
				return Functions.Sqrt( m_X * m_X + m_Z * m_Z );
			}
		}

		private readonly float m_X;
		private readonly float m_Z;
	}
}
