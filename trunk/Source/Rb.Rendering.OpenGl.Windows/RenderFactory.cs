using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Rendering.OpenGl.Windows
{
	/// <summary>
	/// Windows implementation of OpenGlRenderFactory
	/// </summary>
	public class RenderFactory : Rb.Rendering.OpenGl.OpenGlRenderFactory
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
