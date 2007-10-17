
using Rb.Core.Components;

namespace Rb.World.Entities
{
	/// <summary>
	/// XZ rotation request
	/// </summary>
	public class TurnRequest : Message
	{
		/// <summary>
		/// Sets up the rotation
		/// </summary>
		/// <param name="turnAmount">Rotation delta (radians around y axis)</param>
		public TurnRequest( float turnAmount )
		{
			m_Rotation = turnAmount;
		}

		/// <summary>
		/// Gets the rotation delta in radians around the y axis
		/// </summary>
		public float Rotation
		{
			get { return m_Rotation; }
		}

		private readonly float m_Rotation;
	}
}
