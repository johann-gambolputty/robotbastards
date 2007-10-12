using System;
using System.Drawing;
using Rb.Rendering;
using Rb.World;

namespace Poc0.LevelEditor.Core.Rendering
{
	/// <summary>
	/// Renders a grid on the xz ground plane
	/// </summary>
	[Serializable, RenderingLibraryType]
	public abstract class GroundPlaneGrid : IRenderable, ISceneObject
	{
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
		public void AddedToScene( Scene scene )
		{
			scene.Renderables.Add( this );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene(Scene scene)
		{
			scene.Renderables.Remove( this );
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Gets the bitmap used for rendering a grid square
		/// </summary>
		protected static Bitmap GridSquareBitmap
		{
			get { return Properties.Resources.GridSquare; }
		}

		#endregion
	}
}
