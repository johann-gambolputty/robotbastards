using System;
using System.Drawing;
using Poc1.Universe.Interfaces.Planets.Spherical;
using Poc1.Universe.Interfaces.Planets.Spherical.Renderers;
using Rb.Core.Threading;
using Rb.Core.Utils;
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
		public void QueueBuild( ISpherePlanet planet, Action<ITexture> onComplete )
		{
			SourceSinkWorkItem.Builder sourceSink = new SourceSinkWorkItem.Builder( );
			sourceSink.SetSource<Bitmap[], ISpherePlanet>( CreateTextureBitmaps, planet );
			sourceSink.SetSink<Action<ITexture>, Bitmap[]>( FinishBuild, onComplete );

			ExtendedThreadPool.Instance.Enqueue( sourceSink.Build( ) );
		}

		/// <summary>
		/// Builds a texture. Blocking call.
		/// </summary>
		public ITexture Build( ISpherePlanet planet )
		{
			return CreateTexture( CreateTextureBitmaps( planet ) );
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
		private static Bitmap[] CreateTextureBitmaps( ISpherePlanet planet )
		{
			Bitmap[] faceBitmaps = new Bitmap[ 6 ];
			int width = 256;
			int height = 256;
			faceBitmaps[ ( int )CubeMapFace.PositiveX ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.PositiveX, width, height );
			faceBitmaps[ ( int )CubeMapFace.NegativeX ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.NegativeX, width, height );
			faceBitmaps[ ( int )CubeMapFace.PositiveY ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.PositiveY, width, height );
			faceBitmaps[ ( int )CubeMapFace.NegativeY ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.NegativeY, width, height );
			faceBitmaps[ ( int )CubeMapFace.PositiveZ ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.PositiveZ, width, height );
			faceBitmaps[ ( int )CubeMapFace.NegativeZ ] = planet.SphereTerrainModel.CreateMarbleTextureFace( CubeMapFace.NegativeZ, width, height );

			foreach ( object cubeMapFace in Enum.GetValues( typeof( CubeMapFace ) ) )
			{
				faceBitmaps[ ( int )cubeMapFace ].Save( "PlanetCubeMap" + cubeMapFace + ".png" );
			}

			return faceBitmaps;
		}

		/// <summary>
		/// Creates a texture from the supplied bitmaps
		/// </summary>
		private static void FinishBuild( Action<ITexture> onComplete, Bitmap[] faceBitmaps )
		{
			onComplete( CreateTexture( faceBitmaps ) );
		}

		#endregion
	}
}
