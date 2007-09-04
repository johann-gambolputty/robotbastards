using System;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Rendering;

namespace Poc0.Core
{
	/// <summary>
	/// Entity
	/// </summary>
	[Serializable]
	public class Entity : Component, IHasWorldFrame, INamed
	{
		public IRenderable Graphics
		{
			get { return m_Graphics;  }
			set { m_Graphics = value; }
		}

		private IRenderable m_Graphics;

		#region IHasWorldFrame Members

		/// <summary>
		/// Gets the world frame for this object
		/// </summary>
		public Matrix44 WorldFrame
		{
			get { return m_Frame; }
		}

		#endregion

		#region INamed Members

		/// <summary>
		/// Name of the entity
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion

		#region Private members

		private readonly Matrix44 m_Frame = new Matrix44( );
		private string m_Name = "Bob";

		#endregion
	}
}
