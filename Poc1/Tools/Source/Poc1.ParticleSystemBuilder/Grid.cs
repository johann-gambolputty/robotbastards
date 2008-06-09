using System.Drawing;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.ParticleSystemBuilder
{
	public class Grid : IRenderable
	{
		/// <summary>
		/// Initializes the grid
		/// </summary>
		/// <param name="size">World size of the grid</param>
		/// <param name="res">Grid resolution</param>
		public Grid( float size, int res )
		{
			float min = -size / 2;
			float max = size / 2;

			Draw.IBrush brush = Graphics.Draw.NewBrush( Color.FromArgb( 180, Color.DarkBlue ) );
			brush.State.Blend = true;
			brush.State.SourceBlend = BlendFactor.SrcAlpha;
			brush.State.DestinationBlend = BlendFactor.OneMinusSrcAlpha;
			brush.State.DepthTest = true;
			brush.State.DepthWrite = true;
			brush.State.DepthOffset = 1.0f;

			Draw.IPen pen = Graphics.Draw.NewPen( Color.FromArgb( 100, Color.White ) );
			pen.State.Blend = true;
			pen.State.SourceBlend = BlendFactor.SrcAlpha;
			pen.State.DestinationBlend = BlendFactor.OneMinusSrcAlpha;
			pen.State.DepthTest = true;
			pen.State.DepthWrite = true;

			Graphics.Draw.StartCache( );
			Graphics.Draw.Rectangle( brush, 0, 0, 0, size, size );

			float inc = size / ( res - 1 );
			float fX = min;
			for ( int x = 0; x < res; ++x, fX += inc )
			{
			    Graphics.Draw.Line( pen, fX, 0, min, fX, 0, max );
			}

			float fY = min;
			for ( int y = 0; y < res; ++y, fY += inc )
			{
			    Graphics.Draw.Line( pen, min, 0, fY, max, 0, fY );
			}
			m_Grid = Graphics.Draw.StopCache( );
		}

		#region IRenderable Members

		private readonly IRenderable m_Grid;

		/// <summary>
		/// Renders the grid
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			m_Grid.Render( context );
		}

		#endregion
	}
}
