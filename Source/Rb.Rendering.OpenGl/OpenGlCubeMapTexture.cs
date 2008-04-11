using System;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Implements ICubeMapTexture using OpenGL cube map extensions
	/// </summary>
	public class OpenGlCubeMapTexture : ICubeMapTexture
	{
		#region Private Members

		private TextureFormat m_Format = TextureFormat.Unknown;
		private readonly OpenGlTexture2d[] m_Textures = new OpenGlTexture2d[ 6 ];

		private const int NegativeX = 0;
		private const int PositiveX = 1;
		private const int NegativeY = 2;
		private const int PositiveY = 3;
		private const int NegativeZ = 4;
		private const int PositiveZ = 5;

		/// <summary>
		/// Disposes of all cube map textures
		/// </summary>
		private void DisposeTextures( )
		{
			foreach ( IDisposable disposable in m_Textures )
			{
				disposable.Dispose( );
			}
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Destroys this object
		/// </summary>
		public void Dispose( )
		{
			DisposeTextures( );
		}

		#endregion

		#region ITexture Members

		/// <summary>
		/// Gets the format of the texture
		/// </summary>
		public TextureFormat Format
		{
			get { return m_Format; }
		}

		/// <summary>
		/// Binds this texture
		/// </summary>
		/// <param name="unit">Texture unit to bind this texture to</param>
		public void Bind( int unit )
		{
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		public void Unbind( int unit )
		{
			throw new NotImplementedException( );
		}

		#endregion

		#region ICubeMapTexture Members

		/// <summary>
		/// Builds this cube map from 6 bitmaps
		/// </summary>
		/// <param name="posX">Positive X axis bitmap</param>
		/// <param name="negX">Negative X axis bitmap</param>
		/// <param name="posY">Positive Y axis bitmap</param>
		/// <param name="negY">Negative Y axis bitmap</param>
		/// <param name="posZ">Positive Z axis bitmap</param>
		/// <param name="negZ">Negative Z axis bitmap</param>
		public void Build( Bitmap posX, Bitmap negX, Bitmap posY, Bitmap negY, Bitmap posZ, Bitmap negZ )
		{
			throw new NotImplementedException( );
		}

		/// <summary>
		/// Builds this cube map from 6 textures
		/// </summary>
		/// <param name="posX">Positive X axis texture</param>
		/// <param name="negX">Negative X axis texture</param>
		/// <param name="posY">Positive Y axis texture</param>
		/// <param name="negY">Negative Y axis texture</param>
		/// <param name="posZ">Positive Z axis texture</param>
		/// <param name="negZ">Negative Z axis texture</param>
		public void Build( ITexture2d posX, ITexture2d negX, ITexture2d posY, ITexture2d negY, ITexture2d posZ, ITexture2d negZ )
		{
			if ( negX.Format != posX.Format )
			{
				throw new ArgumentException( "Negative X texture did not have the correct format " + posX.Format );
			}
			if ( posY.Format != posX.Format )
			{
				throw new ArgumentException( "Positive Y texture did not have the correct format " + posX.Format );
			}
			if ( negY.Format != posX.Format )
			{
				throw new ArgumentException( "Negative Y texture did not have the correct format " + posX.Format );
			}
			if ( posZ.Format != posX.Format )
			{
				throw new ArgumentException( "Positive Z texture did not have the correct format " + posX.Format );
			}
			if ( negZ.Format != posX.Format )
			{
				throw new ArgumentException( "Negative Z texture did not have the correct format " + posX.Format );
			}

			m_Textures[ PositiveX ] = posX;
			m_Textures[ NegativeX ] = negX;
			m_Textures[ PositiveY ] = posY;
			m_Textures[ NegativeY ] = negY;
			m_Textures[ PositiveZ ] = posZ;
			m_Textures[ NegativeZ ] = negZ;
		}

		/// <summary>
		/// Renders this cubemap texture to a series of bitmaps
		/// </summary>
		public Bitmap[] ToBitmaps( )
		{
			List<Bitmap> bitmaps = new List<Bitmap>( );
			foreach ( ITexture2d texture in m_Textures )
			{
				bitmaps.Add( texture.ToBitmap( ) );
			}
			return bitmaps.ToArray( );
		}

		#endregion
	}
}
