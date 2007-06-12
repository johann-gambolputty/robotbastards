using System;

namespace RbEngine.Entities
{
	/// <summary>
	/// Jump movement request
	/// </summary>
	public class JumpRequest : MovementXzRequest
	{
		/// <summary>
		/// Sets up jump movement
		/// </summary>
		public JumpRequest( float moveX, float moveZ, bool local ) :
				base( moveX, moveZ, local )
		{
		}
	}
}
