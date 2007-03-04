using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// Base class for messages requesting movement
	/// </summary>
	public abstract class MovementRequest : Components.Message
	{
		/// <summary>
		/// Gets the distance covered by the movement
		/// </summary>
		public abstract float	Distance
		{
			get;
		}
	}
}
