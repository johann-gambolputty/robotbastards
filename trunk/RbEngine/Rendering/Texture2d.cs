using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Resources;

namespace RbEngine.Rendering
{
	/// <summary>
	/// 2D texture data
	/// </summary>
	/// <seealso>ApplyTexture2d</seealso>
	public abstract class Texture2d : IAppliance
	{
		#region	Construction and setup

		/// <summary>
		/// Default constructor
		/// </summary>
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
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		public abstract void Create( int width, int height, PixelFormat format );

		#endregion

		#region	Public properties

		/// <summary>
		/// Gets the width of the texture
		/// </summary>
		public int	Width
		{
			get
			{
				return m_Width;
			}
		}

		/// <summary>
		/// Gets the height of the texture
		/// </summary>
		public int Height
		{
			get
			{
				return m_Height;
			}
		}

		/// <summary>
		/// Gets the format of the texture
		/// </summary>
		public PixelFormat	Format
		{
			get
			{
				return m_Format;
			}
		}

		#endregion

		#region	Loading

		/// <summary>
		/// Creates a texture from a resource, using the manifest resource stream
		/// </summary>
		public static Texture2d FromManifestResource( string name )
		{
			Texture2d texture = RenderFactory.Inst.NewTexture2d( );
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
			System.IO.Stream stream = AppDomainUtils.FindManifestResource( name );
			Load( new Bitmap( Image.FromStream( stream ) ) );
		}

		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		public abstract void Load( System.Drawing.Bitmap bmp );

		#endregion

		#region	Saving and conversion

		/// <summary>
		/// Generates an Image from the texture
		/// </summary>
		public abstract System.Drawing.Image ToImage( );

		/// <summary>
		/// Saves this texture to a file
		/// </summary>
		public void Save( string path )
		{
			ToImage( ).Save( path );
		}

		#endregion

		#region IAppliance Members

		/// <summary>
		/// Starts applying this texture
		/// </summary>
		public abstract void Begin( );

		/// <summary>
		/// Stops applying this texture
		/// </summary>
		public abstract void End( );

		#endregion

		#region	Protected stuff

		protected int			m_Width;
		protected int			m_Height;
		protected PixelFormat	m_Format;

		#endregion

	}
}
