using Rb.Core.Components;
using Rb.Core.Maths;

namespace Poc0.Core
{
	/// <summary>
	/// Player start point
	/// </summary>
	public class PlayerStart : Component, IHasWorldFrame
	{
		/// <summary>
		/// Gets the world frame for this object
		/// </summary>
		public Matrix44 WorldFrame
		{
			get { return m_Frame; }
		}

		private readonly Matrix44 m_Frame = new Matrix44( );
	}
}
