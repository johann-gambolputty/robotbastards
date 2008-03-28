using Rb.Rendering.Interfaces;

namespace Rb.Rendering.OpenGl.Windows
{
	/// <summary>
	/// Windows implementation of OpenGlRenderFactory
	/// </summary>
	public class RenderFactory : OpenGlGraphicsFactory
	{
		/// <summary>
		/// Display setup creator
		/// </summary>
		public override IDisplaySetup CreateDisplaySetup( )
		{
			return new DisplaySetup( );
		}
	}
}
