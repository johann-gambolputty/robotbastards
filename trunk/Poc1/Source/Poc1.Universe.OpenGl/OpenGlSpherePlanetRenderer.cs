using System.Drawing;
using Poc1.Universe.Classes;
using Poc1.Universe.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.OpenGl
{
	[RenderingLibraryType]
	public class OpenGlSpherePlanetRenderer : SpherePlanetRenderer
	{
		public OpenGlSpherePlanetRenderer( SpherePlanet planet ) :
			base( planet )
		{
			Draw.ISurface surface = Graphics.Draw.NewSurface( Color.Red, Color.Black );
			surface.FaceBrush.State.DepthTest = true;
			surface.FaceBrush.State.DepthWrite = true;
			surface.EdgePen.State.DepthTest = true;
			surface.EdgePen.State.DepthWrite = true;
			surface.EdgePen.State.DepthOffset = -1.0f;
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( surface, Point3.Origin, Planet.Radius, 20, 20 );
			m_Render = Graphics.Draw.StopCache( );
		}

		private readonly IRenderable m_Render;

		public override void Render( IRenderContext context )
		{
			float x = Planet.Transform.Position.X;
			float y = Planet.Transform.Position.Y;
			float z = Planet.Transform.Position.Z;

			Graphics.Renderer.PushTransform( Transform.LocalToWorld );
			Graphics.Renderer.Translate( Transform.LocalToWorld, x, y, z );

			m_Render.Render( context );

			foreach ( IPlanet moon in Planet.Moons )
			{
				moon.Render( context );
			}

			Graphics.Renderer.PopTransform( Transform.LocalToWorld );
		}
	}
}
