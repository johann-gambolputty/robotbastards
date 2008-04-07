using System.Drawing;
using Poc1.Universe.Classes;
using Poc1.Universe.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.OpenGl
{
	[RenderingLibraryType]
	public class OpenGlSpherePlanetRenderer : SpherePlanetRenderer
	{
		private static Draw.ISurface TempSurface;

		static OpenGlSpherePlanetRenderer( )
		{
			Draw.ISurface surface = Graphics.Draw.NewSurface( Color.Red, Color.Black );
			surface.FaceBrush.State.DepthTest = true;
			surface.FaceBrush.State.DepthWrite = true;
			surface.EdgePen.State.DepthTest = true;
			surface.EdgePen.State.DepthWrite = true;
			surface.EdgePen.State.DepthOffset = -1.0f;
			TempSurface = surface;
		}

		public OpenGlSpherePlanetRenderer( SpherePlanet planet ) :
			base( planet )
		{
			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( TempSurface, Point3.Origin, Planet.Radius, 20, 20 );
			m_Render = Graphics.Draw.StopCache( );
		}

		private delegate void UvToPointDelegate( float x, float y );

		private static void RenderCubeFace( int subdivisions, UvToPointDelegate uvToPoint )
		{
			float inc = 2.0f / subdivisions;
			float curY = -1;
			for ( int y = 0; y < subdivisions; ++y, curY += inc )
			{
				float curX = -1;
				for ( int x = 0; x < subdivisions; ++x, curX += inc )
				{
					uvToPoint( curX, curY );
					uvToPoint( curX + inc, curY );
					uvToPoint( curX + inc, curY + inc );
					uvToPoint( curX, curY + inc );
				}
			}
		}

		public void UvSpherePoint( float x, float y, float z )
		{
			float length = Planet.Radius / Functions.Sqrt( x * x + y * y + z * z );
			Gl.glVertex3f( x * length, y * length, z * length );
		}

		public void RenderSphere( int subdivisions )
		{
			Gl.glBegin( Gl.GL_QUADS );
			RenderCubeFace( subdivisions, delegate( float x, float y ) { UvSpherePoint( x, 1, y ); } );
			RenderCubeFace( subdivisions, delegate( float x, float y ) { UvSpherePoint( x, -1, y ); } );
			RenderCubeFace( subdivisions, delegate( float x, float y ) { UvSpherePoint( x, y, 1 ); } );
			RenderCubeFace( subdivisions, delegate( float x, float y ) { UvSpherePoint( x, y, -1 ); } );
			RenderCubeFace( subdivisions, delegate( float x, float y ) { UvSpherePoint( 1, x, y ); } );
			RenderCubeFace( subdivisions, delegate( float x, float y ) { UvSpherePoint( -1, x, y ); } );
			Gl.glEnd( );
		}

		public class Patch
		{
			public void IncreaseLod( )
			{
				
			}

			public void DecreaseLod( )
			{

			}

			private int m_Lod;
		}



		public override void Render( IRenderContext context )
		{
			float x = ( float )UniUnits.ToMetres( Planet.Transform.Position.X );
			float y = ( float )UniUnits.ToMetres( Planet.Transform.Position.Y );
			float z = ( float )UniUnits.ToMetres( Planet.Transform.Position.Z );

			Graphics.Renderer.PushTransform( Transform.LocalToWorld );
			Graphics.Renderer.Translate( Transform.LocalToWorld, x, y, z );

		//	m_Render.Render( context );
			TempSurface.FaceBrush.Begin( );
			RenderSphere( 2 );
			TempSurface.FaceBrush.End( );
			TempSurface.EdgePen.Begin( );
			RenderSphere( 2 );
			TempSurface.EdgePen.End( );

			Graphics.Renderer.PopTransform( Transform.LocalToWorld );

			foreach ( IPlanet moon in Planet.Moons )
			{
				moon.Render( context );
			}
		}

		private readonly static PlanetTerrainRenderer ms_TerrainRenderer = new PlanetTerrainRenderer( );
		private readonly IRenderable m_Render;
	}
}
