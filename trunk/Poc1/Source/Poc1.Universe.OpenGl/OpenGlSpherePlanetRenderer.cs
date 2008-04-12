using System.Drawing;
using Poc1.Universe.Classes;
using Poc1.Universe.Classes.Cameras;
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

		static OpenGlSpherePlanetRenderer( )
		{
			Draw.ISurface surface = Graphics.Draw.NewSurface( Color.Red, Color.Black );
			surface.FaceBrush.State.DepthTest = false;
			surface.FaceBrush.State.DepthWrite = false;
			surface.FaceBrush.State.CullBackFaces = true;
			surface.EdgePen.State.DepthTest = false;
			surface.EdgePen.State.DepthWrite = false;
			surface.EdgePen.State.CullBackFaces = true;
			TempSurface = surface;
		}

		public OpenGlSpherePlanetRenderer( SpherePlanet planet ) :
			base( planet )
		{
		}

		private delegate void UvToPointDelegate( float x, float y );

		private static void RenderCubeFace( int subdivisions, bool cw, UvToPointDelegate uvToPoint )
		{
			float inc = 2.0f / subdivisions;
			float curY = -1;
			for ( int y = 0; y < subdivisions; ++y, curY += inc )
			{
				float curX = -1;
				for ( int x = 0; x < subdivisions; ++x, curX += inc )
				{
					uvToPoint( curX, curY );
					if ( cw )
					{
						uvToPoint( curX + inc, curY );
						uvToPoint( curX + inc, curY + inc );
						uvToPoint( curX, curY + inc );
					}
					else
					{
						uvToPoint( curX, curY + inc );
						uvToPoint( curX + inc, curY + inc );
						uvToPoint( curX + inc, curY );
					}
				}
			}
		}

		private float PlanetRenderRadius
		{
			get { return UniCamera.ToAstroRenderUnits( Planet.Radius ); }
		}

		public void UvSpherePoint( float x, float y, float z )
		{
			float length = PlanetRenderRadius / Functions.Sqrt( x * x + y * y + z * z );
			Gl.glVertex3f( x * length, y * length, z * length );
		}

		public void RenderSphere( int subdivisions )
		{
			Gl.glBegin( Gl.GL_QUADS );
			RenderCubeFace( subdivisions, false, delegate( float x, float y ) { UvSpherePoint( x, 1, y ); } );
			RenderCubeFace( subdivisions, true, delegate( float x, float y ) { UvSpherePoint( x, -1, y ); } );
			RenderCubeFace( subdivisions, true, delegate( float x, float y ) { UvSpherePoint( x, y, 1 ); } );
			RenderCubeFace( subdivisions, false, delegate( float x, float y ) { UvSpherePoint( x, y, -1 ); } );
			RenderCubeFace( subdivisions, true, delegate( float x, float y ) { UvSpherePoint( 1, x, y ); } );
			RenderCubeFace( subdivisions, false, delegate( float x, float y ) { UvSpherePoint( -1, x, y ); } );
			Gl.glEnd( );
		}

		public override void Render( IRenderContext context )
		{
			UniCamera.PushAstroRenderTransform( TransformType.LocalToWorld, Planet.Transform );

			TempSurface.FaceBrush.Begin( );
			RenderSphere( 4 );
			TempSurface.FaceBrush.End( );
			TempSurface.EdgePen.Begin( );
			RenderSphere( 4 );
			TempSurface.EdgePen.End( );

			if ( Planet.EnableTerrainRendering )
			{
				ms_TerrainRenderer.Render( context, PlanetRenderRadius );
			}

			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
		}

		private readonly static Draw.ISurface TempSurface;
		private readonly static PlanetTerrainRenderer ms_TerrainRenderer = new PlanetTerrainRenderer( );
	}
}
