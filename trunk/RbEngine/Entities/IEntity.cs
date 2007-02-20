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
		Maths.Vector3	Position
		{
			get;
			set;
		}

		/// <summary>
		/// The entity facing
		/// </summary>
		Maths.Vector3	Facing
		{
			get;
			set;
		}
	}
}
