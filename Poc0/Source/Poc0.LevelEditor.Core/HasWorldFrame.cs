using System;
using Poc0.Core;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	public class HasWorldFrame : IHasWorldFrame
	{
		#region IHasWorldFrame Members

		public Matrix44 WorldFrame
		{
			get { return m_Frame; }
		}

		#endregion

		private readonly Matrix44 m_Frame = new Matrix44( );
	}
}
