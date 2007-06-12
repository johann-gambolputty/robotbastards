using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Delegate used by ITechnique.Apply()
	/// </summary>
	public delegate void TechniqueRenderDelegate( );

	/// <summary>
	/// A technique is an object that modifies the rendering state, renders something, then cleans up the rendering state
	/// </summary>
	public interface ITechnique
	{
		/// <summary>
		/// Applies this technique
		/// </summary>
		void Apply( TechniqueRenderDelegate render );
	}
}
