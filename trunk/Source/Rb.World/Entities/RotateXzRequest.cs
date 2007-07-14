
using Rb.Core.Components;

namespace Rb.World.Entities
{
	/// <summary>
	/// XZ rotation request
	/// </summary>
	public class RotateXzRequest : Message
	{
		/// <summary>
		/// Sets up the rotation
		/// </summary>
		/// <param name="rotation">Rotation (radians around y axis)</param>
		public RotateXzRequest( float rotation )
		{
			m_Rotation = rotation;
		}

		/// <summary>
		/// Gets the rotation in radians around the y axis
		/// </summary>
		public float Rotation
		{
			get { return m_Rotation; }
		}

		private float m_Rotation;
	}
}
