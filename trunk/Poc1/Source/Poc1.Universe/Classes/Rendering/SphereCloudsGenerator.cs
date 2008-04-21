using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using Poc1.Fast;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{

	//	SSE problems with noise generation:
	//	http://www.gamedev.net/community/forums/topic.asp?topic_id=469446

	public class SphereCloudsGenerator : IDisposable
	{
		public SphereCloudsGenerator( int res )
		{
			m_Resolution = res;

			CreateFaceBitmap( CubeMapFace.PositiveX );
			CreateFaceBitmap( CubeMapFace.NegativeX );
			CreateFaceBitmap( CubeMapFace.PositiveY );
			CreateFaceBitmap( CubeMapFace.NegativeY );
			CreateFaceBitmap( CubeMapFace.PositiveZ );
			CreateFaceBitmap( CubeMapFace.NegativeZ );

			for ( int textureIndex = 0; textureIndex < m_Textures.Length; ++textureIndex )
			{
				m_Textures[ textureIndex ] = Graphics.Factory.CreateCubeMapTexture( );
				m_Textures[ textureIndex ].Build
					(
						GetFaceBitmap( CubeMapFace.PositiveX ),
						GetFaceBitmap( CubeMapFace.NegativeX ),
						GetFaceBitmap( CubeMapFace.PositiveY ),
						GetFaceBitmap( CubeMapFace.NegativeY ),
						GetFaceBitmap( CubeMapFace.PositiveZ ),
						GetFaceBitmap( CubeMapFace.NegativeZ ),
						true
					);
			}

			m_UpdateThread = new Thread( UpdateThread );
			m_UpdateThread.Start( );
		}

		~SphereCloudsGenerator( )
		{
			Dispose( );
		}

		public float Blend
		{
			get { return m_Blend; }
		}

		public ICubeMapTexture CurrentCloudTexture
		{
			get { return m_Textures[ m_CurrentTexture ]; }
		}

		public ICubeMapTexture NextCloudTexture
		{
			get { return m_Textures[ ( m_CurrentTexture + 1 ) % m_Textures.Length ]; }
		}

		private ICubeMapTexture InProgressCloudTexture
		{
			get { return m_Textures[ ( m_CurrentTexture + 2 ) % m_Textures.Length ]; }
		}

		private Thread m_UpdateThread;
		private readonly AutoResetEvent m_CompleteEvent = new AutoResetEvent( false );
		private readonly AutoResetEvent m_UpdatedEvent = new AutoResetEvent( true );
		private volatile bool m_ExitThread = false;

		private void UpdateThread( )
		{
			while ( !m_ExitThread )
			{
				m_UpdatedEvent.WaitOne( );

				GameProfiles.Game.CloudGeneration.Begin( );
				m_XOffset = Utils.Wrap( m_XOffset + 0.01f, 0, Constants.TwoPi );
				m_ZOffset = Utils.Wrap( m_ZOffset + 0.015f, 0, Constants.TwoPi );

				//	Simple cloud coverage cycle
				m_CloudCoverage = Utils.Wrap( m_CloudCoverage + 0.01f, 0, Constants.TwoPi );

				//	TODO: AP: This should lock the unused texture's faces and write to them directly
				BuildFaceBitmap( CubeMapFace.PositiveX );
				BuildFaceBitmap( CubeMapFace.NegativeX );
				BuildFaceBitmap( CubeMapFace.PositiveY );
				BuildFaceBitmap( CubeMapFace.NegativeY );
				BuildFaceBitmap( CubeMapFace.PositiveZ );
				BuildFaceBitmap( CubeMapFace.NegativeZ );

				GetFaceBitmap(CubeMapFace.PositiveX).Save("test.bmp", ImageFormat.Bmp);

				GameProfiles.Game.CloudGeneration.End( );
				GameProfiles.Game.CloudGeneration.Reset( );

				m_CompleteEvent.Set( );
			}
		}

		public void Update( )
		{
			using ( GameProfiles.Game.Rendering.PlanetRendering.CloudRendering.CreateGuard( ) )
			{
				//	A very gradual blend between the 2 active cloud textures is required - any faster
				//	and there's a noticeable jump, when the new texture is completed
				m_Blend = Utils.Min( m_Blend + 0.02f, 1.0f );
				if ( m_Blend < 1.0f )
				{
					return;
				}
				if ( !m_CompleteEvent.WaitOne( 0, false) )
				{
					return;
				}
				InProgressCloudTexture.Build
					(
						GetFaceBitmap( CubeMapFace.PositiveX ),
						GetFaceBitmap( CubeMapFace.NegativeX ),
						GetFaceBitmap( CubeMapFace.PositiveY ),
						GetFaceBitmap( CubeMapFace.NegativeY ),
						GetFaceBitmap( CubeMapFace.PositiveZ ),
						GetFaceBitmap( CubeMapFace.NegativeZ ),
						true
					);
				m_CurrentTexture = ( m_CurrentTexture + 1 ) % m_Textures.Length;
				m_Blend = 0;
				m_UpdatedEvent.Set( );
			}
		}

		#region Private Members

		private float m_CloudCoverage;
		private float m_Blend;
		private int m_CurrentTexture = 0;
		private readonly ICubeMapTexture[] m_Textures = new ICubeMapTexture[ 3 ];
		private readonly Bitmap[] m_Faces = new Bitmap[ 6 ];
		private readonly int m_Resolution;

		private Bitmap GetFaceBitmap( CubeMapFace face )
		{
			return m_Faces[ ( int )face ];
		}

		private unsafe void BuildFaceBitmap( CubeMapFace face )
		{
			Bitmap bmp = GetFaceBitmap( face );
			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );
			GenerateSide( face, ( byte* )bmpData.Scan0, bmp.Width, bmp.Height, bmpData.Stride );
			bmp.UnlockBits( bmpData );
		}

		private void CreateFaceBitmap( CubeMapFace face )
		{
			PixelFormat format = PixelFormat.Format32bppArgb;
			Bitmap bmp = new Bitmap( m_Resolution, m_Resolution, format );
			m_Faces[ ( int )face ] = bmp;
			BuildFaceBitmap( face );
		}

		private readonly SphereCloudsBitmap m_Gen = new SphereCloudsBitmap( );
	//	private readonly Noise m_Noise = new Noise( );

		private unsafe void GenerateSide( CubeMapFace face, byte* pixels, int width, int height, int stride )
		{
			float xOffset = Functions.Sin( m_XOffset );
			float zOffset = Functions.Cos( m_ZOffset );
			float density = 0.3f + Functions.Cos( m_CloudCoverage ) * 0.2f;
			float cloudCut = density;
			float cloudBorder = cloudCut + 0.2f;
			m_Gen.Setup( xOffset, zOffset, cloudCut, cloudBorder );
			m_Gen.GenerateFace( face, PixelFormat.Format32bppArgb, width, height, stride, pixels );
			
			/*

			Fractals.Basis3dFunction basis = m_Noise.GetNoise;
			float incU = 2.0f / ( width - 1 );
			float incV = 2.0f / ( height - 1 );
			float v = -1;
			for ( int row = 0; row < height; ++row, v += incV )
			{
				float u = -1;
				byte* curPixel = pixels + row * stride;
				for ( int col = 0; col < width; ++col, u += incU )
				{
					float x, y, z;
					SphereTerrainGenerator.UvToXyz( u, v, face, out x, out y, out z );

					float val = Fractals.RidgedFractal( x + xOffset, y, z + zOffset, 1.8f, 8, 1.6f, basis );
					float alpha = 0;
					if ( val < cloudCut )
					{
						val = 0;
					}
					else
					{
						alpha = val < cloudBorder ? ( val - cloudCut ) / ( cloudBorder - cloudCut ) : 1.0f;
					}

					byte colour = ( byte )( val * 255.0f );
					curPixel[ 0 ] = colour;
					curPixel[ 1 ] = colour;
					curPixel[ 2 ] = colour;
					curPixel[ 3 ] = ( byte )( alpha * 255.0f );

					curPixel += 4;
				}
			}
			 */
		}

		private float m_XOffset = 0;
		private float m_ZOffset = 0;

		#endregion

		#region IDisposable Members

		public void Dispose( )
		{
			if ( m_UpdateThread != null )
			{
				m_ExitThread = true;
				if ( !m_UpdateThread.Join( 1000 ) )
				{
					m_UpdateThread.Abort( );
				}
				m_UpdateThread = null;
			}
		}

		#endregion
	}
}
