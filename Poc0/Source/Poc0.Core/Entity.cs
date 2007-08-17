using System;
using System.Collections.Generic;
using System.Text;

namespace Poc0.Core
{
	/// <summary>
	/// Entity
	/// </summary>
	public class Entity : IHasWorldFrame
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
