using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering
{
	/// <summary>
	/// Simple implementation of the IRenderContext interface
	/// </summary>
	class RenderContext : IRenderContext
	{
		#region IRenderContext Members

		public ITechnique GlobalTechnique
		{
			get
			{
				throw new Exception( "The method or operation is not implemented." );
			}
			set
			{
				throw new Exception( "The method or operation is not implemented." );
			}
		}

		public void RenderInContext( ITechnique technique, IRenderable renderable )
		{
			if ( m_GlobalTechnique != null )
			{
			}
			else
			{
				if ( technique != null )
				{
					technique.Apply( renderable.Render );
				}
			}
		}

		#endregion


		#region Private stuff

		private ITechnique m_GlobalTechnique;

		#endregion
	}
}
