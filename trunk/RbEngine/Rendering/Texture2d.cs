using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Resources;
using Tao.OpenGl;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Summary description for Texture2d.
	/// </summary>
	public class Texture2d
	{
		#region	Construction and setup

		public Texture2d( )
		{
		}

		/// <summary>
		/// Loads the texture from a bitmap file
		/// </summary>
		public Texture2d( string path )
		{
			Load( path );
		}

		/// <summary>
		/// Creates a texture from a resource, using the manifest resource stream
		/// </summary>
		public static Texture2d FromManifestResource( string name )
		{
			Texture2d texture = new Texture2d( );
			texture.LoadManifestResource( name );
			return texture;
		}

		/// <summary>
		/// Loads the texture from a bitmap file
		/// </summary>
		public void Load( string path )
		{
			Image img = Image.FromFile( path, true );
			Bitmap bmp = new Bitmap( img );

			//	Dispose() img immediately - while it remains active, Image objects lock their source files
			img.Dispose( );

			//	Load the bitmap
			Load( bmp );
		}

		/// <summary>
		/// Loads the texture from a resource in this assembly's manifest resources
		/// </summary>
		public void LoadManifestResource( string name )
		{
			Load( new Bitmap( Image.FromStream( GetType( ).Assembly.GetManifestResourceStream( name ) ) ) );
		}

		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		public void Load( System.Drawing.Bitmap bmp )
		{
			//	Get the GL format of the bitmap, possibly converting it in the process
			int glFormat = 0;
			int glType = 0;
			bmp = CheckBmpFormat( bmp, out glFormat, out glType );

			//	Lock the bitmap, and create the texture
			BitmapData bmpData = bmp.LockBits( new Rectangle( 0, 0, bmp.Width, bmp.Height ), ImageLockMode.ReadOnly, bmp.PixelFormat );
			Create( bmpData.Width, bmpData.Height, glFormat, glType, bmpData.Scan0 );
			bmp.UnlockBits( bmpData );
		}

		/// <summary>
		/// Creates the texture
		/// </summary>
		private void Create( int width, int height, int glFormat, int glType, IntPtr bytes )
		{
			//	Generate a texture name
			Gl.glGenTextures( 1, out m_TextureHandle );
			Gl.glBindTexture( Gl.GL_TEXTURE_2D, m_TextureHandle );

			Gl.glTexImage2D( Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, width, height, 0, glFormat, glType, bytes );
		}

		/// <summary>
		/// Checks the format of a Bitmap. If it's not compatible with any OpenGL formats, it's converted to a format that is
		/// </summary>
		private Bitmap CheckBmpFormat( Bitmap bmp, out int glFormat, out int glType )
		{
			//	Handle direct mappings to GL texture image formats
			switch ( bmp.PixelFormat )
			{
				case System.Drawing.Imaging.PixelFormat.Alpha					:	break;
				case System.Drawing.Imaging.PixelFormat.Canonical				:	break;
				case System.Drawing.Imaging.PixelFormat.DontCare				:	break;
				case System.Drawing.Imaging.PixelFormat.Extended				:	break;
				case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555		:	break;
				case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale	:	break;
				case System.Drawing.Imaging.PixelFormat.Format16bppRgb555 		:	break;
				case System.Drawing.Imaging.PixelFormat.Format16bppRgb565 		:	break;
				case System.Drawing.Imaging.PixelFormat.Format1bppIndexed 		:	break;
				case System.Drawing.Imaging.PixelFormat.Format24bppRgb			:
				{
					glFormat = Gl.GL_BGR_EXT;
					glType = Gl.GL_UNSIGNED_INT;
					return bmp;
				}
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb			:	break;
				case System.Drawing.Imaging.PixelFormat.Format32bppPArgb		:	break;
				case System.Drawing.Imaging.PixelFormat.Format32bppRgb			:	break;
				case System.Drawing.Imaging.PixelFormat.Format48bppRgb			:	break;
				case System.Drawing.Imaging.PixelFormat.Format4bppIndexed		:	break;
				case System.Drawing.Imaging.PixelFormat.Format64bppArgb			:	break;
				case System.Drawing.Imaging.PixelFormat.Format64bppPArgb		:	break;
				case System.Drawing.Imaging.PixelFormat.Format8bppIndexed		:	break;
				case System.Drawing.Imaging.PixelFormat.Gdi						:	break;
				case System.Drawing.Imaging.PixelFormat.Indexed					:	break;
				case System.Drawing.Imaging.PixelFormat.Max						:	break;
				case System.Drawing.Imaging.PixelFormat.PAlpha					:	break;
			}

			//	Unhandled format. Convert the bitmap into a more manageable form whose GL format we know
			bmp = bmp.Clone( new System.Drawing.Rectangle( 0, 0, bmp.Width, bmp.Height ), System.Drawing.Imaging.PixelFormat.Format24bppRgb );

			glFormat = Gl.GL_BGR_EXT;
			glType = Gl.GL_UNSIGNED_BYTE;

			return bmp;
		}

		#endregion

		#region	Public properties

		/// <summary>
		/// Returns the internal texture handle
		/// </summary>
		public int	TextureHandle
		{
			get
			{
				return m_TextureHandle;
			}
		}

		#endregion

		#region	Private stuff

		private int	m_TextureHandle = -1;

		#endregion
	}
}