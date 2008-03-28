
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.OpenGl.Cg
{
	public class CgEffectDataSource : IEffectDataSource
	{
		#region IEffectDataSource Members

		public bool Bind( IEffectParameter parameter )
		{
			throw new System.Exception( "The method or operation is not implemented." );
		}

		public object Value
		{
			get
			{
				throw new System.Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new System.Exception( "The method or operation is not implemented." );
			}
		}

		public void Apply( IEffectParameter parameter )
		{
			throw new System.Exception( "The method or operation is not implemented." );
		}

		#endregion
	}
}
