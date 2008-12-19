using System;
using System.Drawing;
using System.Drawing.Imaging;
using Poc1.Fast.Terrain;
using Poc1.Universe.Interfaces.Planets.Spherical.Models;
using Poc1.Universe.Planets.Models;
using Rb.Core.Maths;
using Rb.Core.Threading;
using Rb.Rendering.Interfaces.Objects;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.Universe.Planets.Spherical.Models
{
	/// <summary>
	/// Model for sphere planet's cloud cover
	/// </summary>
	public class SpherePlanetCloudModel : PlanetCloudModel, ISpherePlanetCloudModel
	{
		/// <summary>
		/// Default constructor. Builds low-res cloud textures, and kicks off the high-res generation thread
		/// </summary>
		public SpherePlanetCloudModel( IWorkItemQueue workQueue ) : this( workQueue, 512 )
		{
		}

		/// <summary>
		/// Default constructor. Builds low-res cloud textures, and kicks off the high-res generation thread
		/// </summary>
		public SpherePlanetCloudModel( IWorkItemQueue workQueue, int resolution )
		{
			m_CloudTexture = RbGraphics.Factory.CreateCubeMapTexture( );

			//	TODO: AP: Add a first pass that generates low-res bitmaps
			m_Resolution = resolution;
			CreateFaceBitmaps( resolution );

			DelegateWorkItem.Builder work = new DelegateWorkItem.Builder( );
			work.SetDoWork( GenerateBitmaps );
			work.SetWorkComplete( OnGenerateBitmapsComplete );
			workQueue.Enqueue( work.Build( "" ), null );
		}

		/// <summary>
		/// Gets/sets the resolution of the cloud maps
		/// </summary>
		public int Resolution
		{
			get { return m_Resolution; }
			set { m_Resolution = value; }
		}

		#region ISpherePlanetCloudModel Members

		/// <summary>
		/// Gets the cloud texture
		/// </summary>
		public ICubeMapTexture CloudTexture
		{
			get { return m_CloudTexture; }
		}

		#endregion

		#region Private Members

		private readonly SphereCloudsBitmap m_Gen = new SphereCloudsBitmap( );
		private int							m_Resolution = 512;
		private float 						m_XOffset = 0;
		private float 						m_ZOffset = 0;
		private readonly Bitmap[]			m_Faces = new Bitmap[ 6 ];
		private float						m_CloudCoverage;
		private readonly ICubeMapTexture	m_CloudTexture;

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
			GetFaceBitmap( CubeMapFace.PositiveX ).Save( "cloud +x.png" );
			GetFaceBitmap( CubeMapFace.NegativeX ).Save( "cloud -x.png" );
			GetFaceBitmap( CubeMapFace.PositiveY ).Save( "cloud +y.png" );
			GetFaceBitmap( CubeMapFace.NegativeY ).Save( "cloud -y.png" );
			GetFaceBitmap( CubeMapFace.PositiveZ ).Save( "cloud +z.png" );
			GetFaceBitmap( CubeMapFace.NegativeZ ).Save( "cloud -z.png" );

			m_CloudTexture.Build
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
			BuildFaceBitmap( CubeMapFace.PositiveX ); progress.UpdateProgress( 1 / 6.0f  );
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
