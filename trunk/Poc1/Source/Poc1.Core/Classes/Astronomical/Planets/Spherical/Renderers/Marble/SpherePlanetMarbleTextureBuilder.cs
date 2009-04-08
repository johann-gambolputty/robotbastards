using System;
using System.Drawing;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Renderers;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Renderers.Marble;
using Rb.Core.Threading;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers.Marble
{
	/// <summary>
	/// Builds textures for sphere planet marble renderers
	/// </summary>
	public class SpherePlanetMarbleTextureBuilder : ISpherePlanetMarbleTextureBuilder
	{
		#region ISpherePlanetMarbleTextureBuilder Members

		/// <summary>
		/// Returns true if the marble texture needs to be rebuilt
		/// </summary>
		public bool RequiresRebuild( ISpherePlanet planet )
		{
			return false;
		}

		/// <summary>
		/// Adds the request to build a texture for the specified planet onto a build queue
		/// </summary>
		public void QueueBuild( IWorkItemQueue queue, ISpherePlanet planet, Action<ITexture> onComplete )
		{
			SourceSinkWorkItem.Builder<Bitmap[]> sourceSink = new SourceSinkWorkItem.Builder<Bitmap[]>( );
			sourceSink.SetSource( CreateTextureBitmaps, planet );
			sourceSink.SetSink( FinishBuild, onComplete );

			queue.Enqueue( sourceSink.Build( "Build Marble Texture" ), null );
		}

		/// <summary>
		/// Builds a texture. Blocking call.
		/// </summary>
		public ITexture Build( ISpherePlanet planet, IProgressMonitor progressMonitor )
		{
			return CreateTexture( CreateTextureBitmaps( progressMonitor, planet ) );
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Creates the texture from the specified bitmaps
		/// </summary>
		private static ITexture CreateTexture( Bitmap[] faceBitmaps )
		{
			ICubeMapTexture texture = Rb.Rendering.Graphics.Factory.CreateCubeMapTexture( );
			texture.Build
				(
					faceBitmaps[ ( int )CubeMapFace.PositiveX ], faceBitmaps[ ( int )CubeMapFace.NegativeX ],
					faceBitmaps[ ( int )CubeMapFace.PositiveY ], faceBitmaps[ ( int )CubeMapFace.NegativeY ],
					faceBitmaps[ ( int )CubeMapFace.PositiveZ ], faceBitmaps[ ( int )CubeMapFace.NegativeZ ],
					true
				);
			return texture;
		}

		/// <summary>
		/// Creates bitmaps for the texture
		/// </summary>
		private static Bitmap[] CreateTextureBitmaps( IProgressMonitor progressMonitor, ISpherePlanet planet )
		{
			Bitmap[] faceBitmaps = new Bitmap[ 6 ];
			int width = 256;
			int height = 256;
			progressMonitor.UpdateProgress( 0 );

			ISpherePlanetTerrainRenderer renderer = planet.Renderer.GetRenderer<ISpherePlanetTerrainRenderer>( );
			if ( renderer == null )
			{
				throw new InvalidOperationException( "Expected a valid ISpherePlanetTerrainRenderer to be available" );
			}

			faceBitmaps[ ( int )CubeMapFace.PositiveX ] = renderer.CreateMarbleTextureFace( CubeMapFace.PositiveX, width, height ); progressMonitor.UpdateProgress( 1 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.NegativeX ] = renderer.CreateMarbleTextureFace( CubeMapFace.NegativeX, width, height ); progressMonitor.UpdateProgress( 2 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.PositiveY ] = renderer.CreateMarbleTextureFace( CubeMapFace.PositiveY, width, height ); progressMonitor.UpdateProgress( 3 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.NegativeY ] = renderer.CreateMarbleTextureFace( CubeMapFace.NegativeY, width, height ); progressMonitor.UpdateProgress( 4 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.PositiveZ ] = renderer.CreateMarbleTextureFace( CubeMapFace.PositiveZ, width, height ); progressMonitor.UpdateProgress( 5 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.NegativeZ ] = renderer.CreateMarbleTextureFace( CubeMapFace.NegativeZ, width, height );
			progressMonitor.UpdateProgress( 1 );

			foreach ( object cubeMapFace in Enum.GetValues( typeof( CubeMapFace ) ) )
			{
				faceBitmaps[ ( int )cubeMapFace ].Save( "PlanetCubeMap" + cubeMapFace + ".png" );
			}

			return faceBitmaps;
		}

		/// <summary>
		/// Creates a texture from the supplied bitmaps
		/// </summary>
		private static void FinishBuild( Bitmap[] faceBitmaps, Action<ITexture> onComplete )
		{
			onComplete( CreateTexture( faceBitmaps ) );
		}

		#endregion
	}
}
