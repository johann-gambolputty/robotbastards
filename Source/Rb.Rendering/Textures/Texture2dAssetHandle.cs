using System;
using System.Drawing;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Textures
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
		/// <param name="trackChangesToSource">If true, then changes to the texture source are tracked</param>
		public Texture2dAssetHandle( ISource source, bool trackChangesToSource ) :
			base( source, trackChangesToSource )
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
		/// Creates the texture from a single texture data object
		/// </summary>
		/// <param name="data">Texture data used to create the texture</param>
		/// <param name="generateMipMaps">Mipmap generation flag</param>
		public void Create( Texture2dData data, bool generateMipMaps )
		{
			throw new InvalidOperationException( "Can't call Create(Texture2dData, bool) on an asset-backed texture" );
		}

		/// <summary>
		/// Creates the texture from an array of texture data objects, that specify decreasing mipmap levels
		/// </summary>
		/// <param name="data">Texture data used to create the texture and its mipmaps</param>
		public void Create( Texture2dData[] data )
		{
			throw new InvalidOperationException( "Can't call Create(Texture2dData[]) on an asset-backed texture" );
		}

		/// <summary>
		/// Gets texture data from this texture
		/// </summary>
		/// <param name="getMipMaps">If true, texture data for all mipmap levels are retrieved</param>
		/// <returns>
		/// Returns texture data extracted from this texture. If getMipMaps is false, only one <see cref="Texture2dData"/>
		/// object is returned. Otherwise, the array contains a <see cref="Texture2dData"/> object for each mipmap
		/// level.
		/// </returns>
		public Texture2dData[] ToTextureData( bool getMipMaps )
		{
			 return Asset.ToTextureData( getMipMaps );
		}

		/// <summary>
		/// Creates the texture from a single bitmap
		/// </summary>
		/// <param name="bmp">Source bitmap</param>
		/// <param name="generateMipMaps">Mipmap generation flag</param>
		public void Create( Bitmap bmp, bool generateMipMaps )
		{
			throw new InvalidOperationException( "Can't call Create(Bitmap, bool) on an asset-backed texture" );	
		}

		/// <summary>
		/// Creates the texture from an array of bitmaps
		/// </summary>
		/// <param name="bitmaps">Source bitmap data</param>
		public void Create( Bitmap[] bitmaps )
		{
			throw new InvalidOperationException( "Can't call Create(Bitmap[]) on an asset-backed texture" );	
		}

		/// <summary>
		/// Converts this texture to a bitmap
		/// </summary>
		/// <param name="getMipMaps">If true, an array of bitmaps are returned, one for each mipmap level</param>
		/// <returns>
		/// Returns an array of bitmaps. If getMipMaps is false, only one bitmap is returned. If
		/// getMipMaps is true, one bitmap is returned for each mipmap level.
		/// </returns>
		public Bitmap[] ToBitmap( bool getMipMaps )
		{
			return Asset.ToBitmap( getMipMaps );
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

		#region IDisposable Members

		/// <summary>
		/// Disposes of the underlying texture asset
		/// </summary>
		public void Dispose( )
		{
			Asset.Dispose( );
		}

		#endregion
	}
}
