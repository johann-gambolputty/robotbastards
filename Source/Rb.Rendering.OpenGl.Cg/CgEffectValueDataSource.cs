using System;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.OpenGl.Cg
{
	class CgEffectValueDataSource<T> : IEffectValueDataSource<T>
	{
		private T m_Value;

		#region IEffectValueDataSource<T> Members

		public T Value
		{
			get { return m_Value; }
			set { m_Value = value; }
		}

		#endregion

		#region IEffectDataSource Members

		public void Bind( IEffectParameter parameter )
		{
			IntPtr param = ( ( CgEffectParameter )parameter ).Parameter;
		}

		public void Apply( IEffectParameter parameter )
		{
			throw new System.Exception( "The method or operation is not implemented." );
		}

		#endregion
	}
}
