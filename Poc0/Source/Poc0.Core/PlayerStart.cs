using System;
using System.Collections.Generic;
using System.Text;

namespace Poc0.Core
{
	/// <summary>
	/// Player start point
	/// </summary>
	public class PlayerStart : IHasWorldFrame
	{
		/// <summary>
		/// Gets the world frame for this object
		/// </summary>
		public Frame WorldFrame
		{
			get { return m_Frame; }
		}

		private readonly Frame m_Frame = new Frame( );
	}
}
