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
	public abstract class Texture2d
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
			Load( new Bitmap( Image.FromStream( GetType( ).Assembly.GetManifestResourceStream( name ) ) ) );
		}

		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		public abstract void Load( System.Drawing.Bitmap bmp );

		#endregion

	}
}
