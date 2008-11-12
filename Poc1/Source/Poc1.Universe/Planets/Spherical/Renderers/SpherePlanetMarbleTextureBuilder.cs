using System;
using System.Drawing;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Rb.Core.Threading;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Planets.Spherical.Renderers
{
	/// <summary>
	/// Builds textures for sphere planet marble renderers
	/// </summary>
	public class SpherePlanetMarbleTextureBuilder : ISpherePlanetMarbleTextureBuilder
	{
		#region ISpherePlanetMarbleTextureBuilder Members

		/// <summary>
		/// Adds the request to build a texture for the specified planet onto a build queue
		/// </summary>
		public void QueueBuild( IWorkItemQueue queue, ISpherePlanet planet, Action<ITexture> onComplete )
		{
			SourceSinkWorkItem.Builder<Bitmap[]> sourceSink = new SourceSinkWorkItem.Builder<Bitmap[]>( );
			sourceSink.SetSource( CreateTextureBitmaps, planet );
			sourceSink.SetSink( FinishBuild, onComplete );

			queue.Enqueue( sourceSink.Build( "Build Marble Texture" ) );
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
			faceBitmaps[ ( int )CubeMapFace.PositiveX ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.PositiveX, width, height ); progressMonitor.UpdateProgress( 1 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.NegativeX ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.NegativeX, width, height ); progressMonitor.UpdateProgress( 2 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.PositiveY ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.PositiveY, width, height ); progressMonitor.UpdateProgress( 3 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.NegativeY ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.NegativeY, width, height ); progressMonitor.UpdateProgress( 4 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.PositiveZ ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.PositiveZ, width, height ); progressMonitor.UpdateProgress( 5 / 6.0f );
			faceBitmaps[ ( int )CubeMapFace.NegativeZ ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.NegativeZ, width, height );
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
