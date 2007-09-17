using System;
using Rb.Core.Assets;
using Rb.Rendering;
using Rb.World;

namespace Poc0.Core
{
	/// <summary>
	/// Class encapsulating graphics required by standard entity
	/// </summary>
	/// <remarks>
	/// A handy wrapper that makes it much easier to edit entity graphics in the editor
	/// </remarks>
	[Serializable]
	public class EntityGraphics : IRenderable
	{
		/// <summary>
		/// Location of the graphics asset used to display the entity
		/// </summary>
		public ISource GraphicsLocation
		{
			get { return m_Asset.Source; }
			set { m_Asset.Source = value; }
		}

		#region IRenderable Members

		/// <summary>
		/// Renders entity graphics
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			m_Lights.Begin( );

			m_Asset.Asset.Render( context );

			m_Lights.End( );
		}

		#endregion

		private readonly LightMeter m_Lights = new LightMeter( );
		private readonly RenderableAssetHandle m_Asset = new RenderableAssetHandle( );
	}
}
