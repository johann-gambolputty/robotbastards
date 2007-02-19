using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// Entity interface
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// The entity position
		/// </summary>
		Vector3	Position
		{
			get;
			set;
		}

		/// <summary>
		/// The entity facing
		/// </summary>
		Vector3	Facing
		{
			get;
			set;
		}
	}
}
