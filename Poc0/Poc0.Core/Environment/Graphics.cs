using Rb.Rendering;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// Environment graphics
	/// </summary>
	class Graphics : IRenderable
	{
		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			throw new System.Exception( "The method or operation is not implemented." );
		}

		#endregion
	}
}
