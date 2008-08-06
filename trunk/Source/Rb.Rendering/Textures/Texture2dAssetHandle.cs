using System;
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
		/// Creates the texture from a texture data model
		/// </summary>
		/// <param name="data">Texture data</param>
		public void Create( Texture2dData data )
		{
			Asset.Create( data );
		}

		/// <summary>
		/// Creates an empty texture
		/// </summary>
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
