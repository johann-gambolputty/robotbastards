using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Interface for scene objects that can be rendered
	/// </summary>
	public interface ISceneRenderable
	{
		/// <summary>
		/// The list of IAppliance objects that must be applied at the beginning of Render()
		/// </summary>
		Rendering.ApplianceList PreRenderList
		{
			get;
		}

		/// <summary>
		/// Renders the object
		/// </summary>
		/// <param name="renderTime">Frame time (in TinyTime ticks)</param>
		void Render( long renderTime );
	}
}
