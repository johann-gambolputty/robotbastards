using System;
using Rb.Core.Components;
using Rb.Rendering;
using Rb.World;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// Environment graphics
	/// </summary>
	[ Serializable, RenderingLibraryType ]
	public abstract class EnvironmentGraphics : IRenderable, ISceneObject
	{
		/// <summary>
		/// Sets/gets the root wall node
		/// </summary>
		public virtual WallNode Walls
		{
			get { return m_Root; }
			set { m_Root = value; }
		}

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public abstract void Render( IRenderContext context );

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		/// <param name="scene">Scene</param>
		public void SetSceneContext( Scene scene )
		{
			scene.Renderables.Add( this );
		}

		#endregion

		#region Private members

		private WallNode m_Root;

		#endregion
	}
}
