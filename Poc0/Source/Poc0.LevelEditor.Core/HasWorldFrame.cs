using System;
using Poc0.Core;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	[Serializable]
	public class HasWorldFrame : IHasWorldFrame, ICloneable
	{
		public HasWorldFrame( )
		{
		}

		public HasWorldFrame( float x, float y )
		{
			m_Frame.Translation = new Point3( x, 0, y );
		}

		#region IHasWorldFrame Members

		public Matrix44 WorldFrame
		{
			get { return m_Frame; }
		}

		#endregion

		private readonly Matrix44 m_Frame = new Matrix44( );

		#region ICloneable Members

		/// <summary>
		/// Clones this object
		/// </summary>
		public object Clone( )
		{
			HasWorldFrame newFrame = new HasWorldFrame( );
			newFrame.WorldFrame.Copy( m_Frame );
			return newFrame;
		}

		#endregion
	}
}
