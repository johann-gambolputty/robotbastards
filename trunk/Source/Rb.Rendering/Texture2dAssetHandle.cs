using System;
using Rb.Core.Assets;

namespace Rb.Rendering
{
	/// <summary>
	/// An implementation of <see cref="ITexture2d"/> that wraps up a texture asset
	/// </summary>
	[Serializable]
	public class Texture2dAssetHandle : AssetHandleT< ITexture2d >, ITexture2d
	{
		#region Construction
		
		/// <summary>
		/// No source - set <see cref="AssetHandle.Source"/> prior to accessing <see cref="AssetHandle.Asset"/>
		/// </summary>
		public Texture2dAssetHandle( )
		{
		}

		/// <summary>
		/// Sets the source of the asset. Does not load the asset until <see cref="AssetHandle.Asset"/> is first accessed
		/// </summary>
		/// <param name="source">Asset source</param>
		public Texture2dAssetHandle( ISource source ) :
			base( source )
		{
		}

		/// <summary>
		/// Sets the source of the asset
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="loadImmediately">If true, the asset is loaded in this constructor. Otherwise, the
		/// asset is loaded on-demand when <see cref="AssetHandle.Asset"/> is first accessed</param>
		public Texture2dAssetHandle( ISource source, bool loadImmediately ) :
			base( source, loadImmediately )
		{
		}

		#endregion

		#region ITexture2d Members

		/// <summary>
		/// Gets the width of the texture
		/// </summary>
		public int Width
		{
			get { return Asset.Width; }
		}

		/// <summary>
		/// Gets the height of the texture
		/// </summary>
		public int Height
		{
			get { return Asset.Height; }
		}

		/// <summary>
		/// Gets the format of the texture
		/// </summary>
		public TextureFormat Format
		{
			get { return Asset.Format; }
		}

		/// <summary>
		/// Creates an empty texture
		/// </summary>
		/// <param name="width">Width of the texture in pixels</param>
		/// <param name="height">Height of the texture in pixels</param>
		/// <param name="format">Format of the texture</param>
		public void Create( int width, int height, TextureFormat format )
		{
			Asset.Create( width, height, format );
		}

		/// <summary>
		/// Loads the texture from bitmap data
		/// </summary>
		public void Load( System.Drawing.Bitmap bmp, bool generateMipMap )
		{
			Asset.Load( bmp, generateMipMap );
		}

		/// <summary>
		/// Converts this texture to a bitmap
		/// </summary>
		/// <returns>Bitmap</returns>
		public System.Drawing.Bitmap ToBitmap( )
		{
			return Asset.ToBitmap( );
		}
		
		/// <summary>
		/// Binds this texture
		/// </summary>
		/// <param name="unit">Texture unit to bind this texture to</param>
		public void Bind( int unit )
		{
			Asset.Bind( unit );
		}
		
		/// <summary>
		/// Unbinds this texture
		/// </summary>
		/// <param name="unit">Texture unit that this texture is bound to</param>
		public void Unbind( int unit )
		{
			Asset.Unbind( unit );
		}


		#endregion
	}
}
