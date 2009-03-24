using System;
using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Core.Classes.Profiling;
using Poc1.Fast.Terrain;
using Rb.Core.Maths;
using Rb.Core.Threading;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.Core.Classes.Astronomical.Planets.Spherical.Renderers
{
	/// <summary>
	/// Builds cloud cube maps
	/// </summary>
	public unsafe class CloudCubeMapTextureBuilder
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public CloudCubeMapTextureBuilder( ) : this( ExtendedThreadPool.Instance )
		{
		}
		
		/// <summary>
		/// Setup constructor. Builds low-res cloud textures, and kicks off the high-res generation thread
		/// </summary>
		public CloudCubeMapTextureBuilder( IWorkItemQueue workQueue ) : this( workQueue, 512 )
		{
		}

		/// <summary>
		/// Default constructor. Builds low-res cloud textures, and kicks off the high-res generation thread
		/// </summary>
		public CloudCubeMapTextureBuilder( IWorkItemQueue workQueue, int resolution )
		{
			Arguments.CheckNotNull( workQueue, "workQueue" );
			for ( int cloudTextureIndex = 0; cloudTextureIndex < m_CloudTextures.Length; ++cloudTextureIndex )
			{
				m_CloudTextures[ cloudTextureIndex ] = RbGraphics.Factory.CreateCubeMapTexture( );
			}

			//	TODO: AP: Add a first pass that generates low-res bitmaps
			m_WorkQueue = workQueue;
			m_Resolution = resolution;
			CreateFaceBitmaps( resolution );

			AddWorkItem( );
		}

		/// <summary>
		/// Gets the current cloud cube map texture
		/// </summary>
		public ICubeMapTexture CurrentTexture
		{
			get { return m_CloudTextures[ m_CloudTextureIndex ]; }
		}

		/// <summary>
		/// Gets the current cloud cube map texture
		/// </summary>
		public ICubeMapTexture NextTexture
		{
			get { return m_CloudTextures[ ( m_CloudTextureIndex + 1 ) % m_CloudTextures.Length ]; }
		}

		/// <summary>
		/// Advances to the next current/next texture pair
		/// </summary>
		public void Advance( )
		{
			//	TODO: AP: If the next texture pair isn't ready, do something... something...
			m_CloudTextureIndex = ( m_CloudTextureIndex + 1 ) % m_CloudTextures.Length;
			AddWorkItem( );
		}

		#region Private Members

		private readonly IWorkItemQueue		m_WorkQueue;
		private readonly SphereCloudsBitmap m_Gen = new SphereCloudsBitmap( );
		private int							m_Resolution = 512;
		private float						m_XOffset = 0;
		private float						m_ZOffset = 0;
		private readonly Bitmap[]			m_Faces = new Bitmap[ 6 ];
		private float						m_CloudCoverage;
		private readonly ICubeMapTexture[]	m_CloudTextures = new ICubeMapTexture[ 3 ];
		private int							m_CloudTextureIndex;
		private int							m_GenerationCount;

		/// <summary>
		/// Gets the current cloud cube map texture
		/// </summary>
		private ICubeMapTexture BuildTexture
		{
			get
			{
				if ( m_GenerationCount == 0 )
				{
					return m_CloudTextures[ 0 ];
				}
				if ( m_GenerationCount == 1 )
				{
					return m_CloudTextures[ 1 ];	
				}
				return m_CloudTextures[ ( m_CloudTextureIndex + 2 ) % m_CloudTextures.Length ];
			}
		}


		/// <summary>
		/// Adds a work item to build the current texture
		/// </summary>
		private void AddWorkItem( )
		{
			DelegateWorkItem.Builder work = new DelegateWorkItem.Builder( );
			work.SetDoWork( GenerateBitmaps );
			work.SetWorkComplete( OnGenerateBitmapsComplete );
			m_WorkQueue.Enqueue( work.Build( "Cloud generation work item" ), null );
		}

		/// <summary>
		/// Gets the bitmap used to generate the texture for a specific cube map face
		/// </summary>
		private Bitmap GetFaceBitmap( CubeMapFace face )
		{
			return m_Faces[ ( int )face ];
		}

		/// <summary>
		/// Builds a bitmap for a specific cube map face
		/// </summary>
		private unsafe void BuildFaceBitmap( CubeMapFace face )
		{
			Bitmap bmp = GetFaceBitmap( face );
			BitmapData bmpData = bmp.LockBits( new System.Drawing.Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.WriteOnly, bmp.PixelFormat );

			float xOffset = Functions.Sin( m_XOffset );
			float zOffset = Functions.Cos( m_ZOffset );
			float density = 0.3f + Functions.Cos( m_CloudCoverage ) * 0.1f;
			float cloudCut = density;
			float cloudBorder = cloudCut + 0.2f;
			m_Gen.Setup( xOffset, zOffset, cloudCut, cloudBorder );
			m_Gen.GenerateFace( face, PixelFormat.Format32bppArgb, bmp.Width, bmp.Height, bmpData.Stride, ( byte* )bmpData.Scan0 );

			bmp.UnlockBits( bmpData );
		}

		/// <summary>
		/// Creates bitmaps for each cube map face
		/// </summary>
		private void CreateFaceBitmaps( int resolution )
		{
			foreach ( object face in Enum.GetValues( typeof( CubeMapFace ) ) )
			{
				PixelFormat format = PixelFormat.Format32bppArgb;
				Bitmap bmp = new Bitmap( m_Resolution, resolution, format );
				m_Faces[ ( int )face ] = bmp;
			}
		}

		/// <summary>
		/// Invoked when GenerateBitmaps() has completed
		/// </summary>
		private void OnGenerateBitmapsComplete( )
		{
			//GetFaceBitmap( CubeMapFace.PositiveX ).Save( "cloud +x.png" );
			//GetFaceBitmap( CubeMapFace.NegativeX ).Save( "cloud -x.png" );
			//GetFaceBitmap( CubeMapFace.PositiveY ).Save( "cloud +y.png" );
			//GetFaceBitmap( CubeMapFace.NegativeY ).Save( "cloud -y.png" );
			//GetFaceBitmap( CubeMapFace.PositiveZ ).Save( "cloud +z.png" );
			//GetFaceBitmap( CubeMapFace.NegativeZ ).Save( "cloud -z.png" );

			BuildTexture.Build
			(
				GetFaceBitmap( CubeMapFace.PositiveX ),
				GetFaceBitmap( CubeMapFace.NegativeX ),
				GetFaceBitmap( CubeMapFace.PositiveY ),
				GetFaceBitmap( CubeMapFace.NegativeY ),
				GetFaceBitmap( CubeMapFace.PositiveZ ),
				GetFaceBitmap( CubeMapFace.NegativeZ ),
				true
			);
			if ( m_GenerationCount == 0 )
			{
				m_CloudTextures[ 2 ] = m_CloudTextures[ 1 ] = m_CloudTextures[ 0 ];
			}
			else if ( m_GenerationCount == 1 )
			{
				m_CloudTextures[ 2 ] = m_CloudTextures[ 1 ];
			}

			++m_GenerationCount;
		}

		/// <summary>
		/// Called to generate cloud bitmaps
		/// </summary>
		private void GenerateBitmaps( IProgressMonitor progress )
		{
			GameProfiles.Game.CloudGeneration.Begin( );
			m_XOffset = Utils.Wrap( m_XOffset + 0.002f, 0, Constants.TwoPi );
			m_ZOffset = Utils.Wrap( m_ZOffset + 0.0025f, 0, Constants.TwoPi );

			//	Simple cloud coverage cycle
			m_CloudCoverage = Utils.Wrap( m_CloudCoverage + 0.01f, 0, Constants.TwoPi );

			progress.UpdateProgress( 0 );
			BuildFaceBitmap( CubeMapFace.PositiveX ); progress.UpdateProgress( 1 / 6.0f );
			BuildFaceBitmap( CubeMapFace.NegativeX ); progress.UpdateProgress( 2 / 6.0f );
			BuildFaceBitmap( CubeMapFace.PositiveY ); progress.UpdateProgress( 3 / 6.0f );
			BuildFaceBitmap( CubeMapFace.NegativeY ); progress.UpdateProgress( 4 / 6.0f );
			BuildFaceBitmap( CubeMapFace.PositiveZ ); progress.UpdateProgress( 5 / 6.0f );
			BuildFaceBitmap( CubeMapFace.NegativeZ );
			progress.UpdateProgress( 1 );

			GameProfiles.Game.CloudGeneration.End( );
			GameProfiles.Game.CloudGeneration.Reset( );
		}

		#endregion
	}
}
