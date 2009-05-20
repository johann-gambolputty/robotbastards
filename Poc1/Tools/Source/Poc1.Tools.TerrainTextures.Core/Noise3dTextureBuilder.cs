using System;
using System.IO;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	/// Builds 3D noise textures
	/// </summary>
	public class Noise3dTextureBuilder
	{
		/// <summary>
		/// Builds a 3d noise texture
		/// </summary>
		/// <param name="parameters">Builder parameters</param>
		/// <returns>Returns a <see cref="Texture3dData"/> object containing the generated noise texture</returns>
		/// <exception cref="System.ArgumentNullException">Thrown if parameters is null</exception>
		/// <exception cref="System.IO.InvalidDataException">Thrown if parameters are invalid</exception>
		public static unsafe Texture3dData Build( Noise3dTextureBuilderParameters parameters )
		{
			Arguments.CheckNotNull( parameters, "parameters" );
			parameters.Validate( );

			Texture3dData data = new Texture3dData( parameters.Width, parameters.Height, parameters.Depth, parameters.Format );

			fixed( byte* voxels = data.Bytes )
			{
				GenerateNoise( data, parameters, voxels );
			}

			return data;
		}

		#region Private Members

		/// <summary>
		/// Generates a toroidally tiled noise
		/// </summary>
		private unsafe static void GenerateNoise( Texture3dData data, Noise3dTextureBuilderParameters parameters, byte* voxels )
		{
			SimpleNoise3d sNoise = new SimpleNoise3d();
			Noise noise = new Noise( );
			float nW	= parameters.NoiseWidth;
			float nH	= parameters.NoiseHeight;
			float nD	= parameters.NoiseDepth;
			float nX0	= parameters.NoiseX;
			float nY0	= parameters.NoiseY;
			float nZ0	= parameters.NoiseZ;
			float nXInc = nW / data.Width;
			float nYInc = nH / data.Height;
			float nZInc = nD / data.Depth;
			byte* voxel = voxels;
			int bytesPerVoxel = TextureFormatInfo.GetSizeInBytes( parameters.Format );
			if ( bytesPerVoxel != 3 && bytesPerVoxel != 4 )
			{

				throw new InvalidDataException( string.Format( "Unexpected voxel stride of {0} - expected either 3 or 4 from format {1}", bytesPerVoxel, parameters.Format ) );
			}

			//	Check hacked channel expectations...
			bool hasAlpha = TextureFormatInfo.HasAlphaChannel( parameters.Format );
			float nZ = nZ0;
			float maxNoise = float.MinValue;
			float minNoise = float.MaxValue;
			for ( int z = 0; z < parameters.Depth; ++z, nZ += nZInc )
			{
				float nY = nY0;
				for ( int y = 0; y < parameters.Height; ++y, nY += nYInc )
				{
					float nX = nX0;
					for ( int x = 0; x < parameters.Width; ++x, nX += nXInc )
					{
						switch ( parameters.GenerationType )
						{
							case NoiseGenerationType.Grayscale :
								{
									float n = ( 1.0f + sNoise.GetTilingNoise( nX, nY, nZ, nW, nH, nD ) ) / 2;

									maxNoise = Math.Max( maxNoise, n );
									minNoise = Math.Min( minNoise, n );

								//	float n = 0.5f + noise.GetNoise( nX, nY, nZ );
									voxel[ 0 ] = NoiseToByte( n );
									voxel[ 1 ] = voxel[ 0 ];
									voxel[ 2 ] = voxel[ 0 ];
									if ( hasAlpha )
									{
										voxel[ 3 ] = voxel[ 0 ];
									}
									break;
								}
							case NoiseGenerationType.MultiChannel :
								{
									float r = noise.GetNoise( nX, nY, nZ );
									float g = noise.GetNoise( nX + nW, nY, nZ );
									float b = noise.GetNoise( nX, nY + nH, nZ );
									voxel[ 0 ] = NoiseToByte( r );
									voxel[ 1 ] = NoiseToByte( g );
									voxel[ 2 ] = NoiseToByte( b );
									if ( hasAlpha )
									{
										float a = noise.GetNoise( nX, nY, nZ + parameters.NoiseDepth );
										voxel[ 3 ] = NoiseToByte( a );
									}
									break;
								}
							case NoiseGenerationType.TilingGrayscale :
							    {
									float n = GetTiledNoise( noise, nX, nY, nZ, nX0, nY0, nZ0, nW, nH, nD );
									voxel[ 0 ] = NoiseToByte( n );
									voxel[ 1 ] = voxel[ 0 ];
									voxel[ 2 ] = voxel[ 0 ];
									if ( hasAlpha )
									{
										voxel[ 3 ] = voxel[ 0 ];
									}
							        break;
							    }
							case NoiseGenerationType.TilingMultiChannel :
								{
									float r = GetTiledNoise( noise, nX, nY, nZ, nX0, nY0, nZ0, nW, nH, nD );
									float g = GetTiledNoise( noise, nX + nW, nY, nZ, nX0 + nW, nY0, nZ0, nW, nH, nD );
									float b = GetTiledNoise( noise, nX, nY + nH, nZ, nX0, nY0 + nH, nZ0, nW, nH, nD );
									voxel[ 0 ] = NoiseToByte( r );
									voxel[ 1 ] = NoiseToByte( g );
									voxel[ 2 ] = NoiseToByte( b );
									if ( hasAlpha )
									{
										float a = GetTiledNoise( noise, nX, nY, nZ + nD, nX0, nY0, nZ0 + nD, nW, nH, nD );
										voxel[ 3 ] = NoiseToByte( a );
									}
									break;
							    }
							case NoiseGenerationType.BigNoise :
							    {
									float real = 0.5f + GetTiledNoise( noise, nX, nY, nZ, nX0, nY0, nZ0, nW, nH, nD );
									float imag = 0.5f + GetTiledNoise( noise, nX + nW, nY + nH, nZ + nD, nX0, nY0, nZ0, nW, nH, nD );

								//	maxNoise = Math.Max( maxNoise, real );
								//	minNoise = Math.Min( minNoise, real );

									double a = Math.PI + Math.PI * real;

									voxel[ 0 ] = NoiseToByte( real );
									voxel[ 1 ] = NoiseToByte( imag );
									voxel[ 2 ] = ( byte )Utils.Clamp( ( Math.Sin( a ) + 1 ) * 127.5, 0, 255 );
									voxel[ 3 ] = ( byte )Utils.Clamp( ( Math.Cos( a ) + 1 ) * 127.5, 0, 255 );

							        break;
							    }
							default :
								throw new NotSupportedException( string.Format( "Noise generation type \"{0}\" is not supported yet", parameters.GenerationType ) );
						}
						
						voxel += bytesPerVoxel;
					}
				}
			}	
		}

		/// <summary>
		/// Gets a tiled noise value
		/// </summary>
		private static float GetTiledNoise( Noise noise, float x, float y, float z, float x0, float y0, float z0, float w, float h, float d )
		{
			float orgX = x - x0;
			float orgY = y - y0;
			float orgZ = z - z0;
			float tilX = x - w;
			float tilY = y - h;
			float tilZ = z - d;
			float invX = w - orgX;
			float invY = h - orgY;
			float invZ = d - orgZ;

			float nX0Y0Z0	= GetNoise( noise, w, h, d, x, y, z ) * invX * invY * invZ;
			float nX0Y1Z0	= GetNoise( noise, w, h, d, x, tilY, z ) * invX * orgY * invZ;
			float nX0Y0Z1	= GetNoise( noise, w, h, d, x, y, tilZ ) * invX * invY * orgZ;
			float nX0Y1Z1	= GetNoise( noise, w, h, d, x, tilY, tilZ ) * invX * orgY * orgZ;

			float nX1Y0Z0 = GetNoise( noise, w, h, d, tilX, y, z ) * orgX * invY * invZ;
			float nX1Y1Z0 = GetNoise( noise, w, h, d, tilX, tilY, z ) * orgX * orgY * invZ;
			float nX1Y0Z1 = GetNoise( noise, w, h, d, tilX, y, tilZ ) * orgX * invY * orgZ;
			float nX1Y1Z1 = GetNoise( noise, w, h, d, tilX, tilY, tilZ ) * orgX * orgY * orgZ;

			return ( nX0Y0Z0 + nX0Y1Z0 + nX0Y0Z1 + nX0Y1Z1 + nX1Y0Z0 + nX1Y1Z0 + nX1Y0Z1 + nX1Y1Z1 ) / ( w * h * d );
		}

		private static float GetNoise( Noise noise, float w, float h, float d, float x, float y, float z )
		{
			return noise.GetNoise( x % w, y % h, z % d );
		}

		/// <summary>
		/// Converts a floating point noise value into the range 0-255
		/// </summary>
		private static byte NoiseToByte( float n )
		{
			return ( byte )( Utils.Clamp( n, 0, 1 ) * 255 );
		}

		#endregion
	}
}
