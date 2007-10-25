using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Rb.Core.Assets;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Information about... a floor
	/// </summary>
	public class FloorData
	{
		/// <summary>
		/// Gets/sets the default texture source for walls
		/// </summary>
		public static ISource DefaultTextureSource
		{
			get { return ms_DefaultTextureSource; }
			set { ms_DefaultTextureSource = value; }
		}

		/// <summary>
		/// Gets/sets the texture source
		/// </summary>
		public AssetHandleT<Texture2d> Texture
		{
			get { return m_Texture; }
			set { m_Texture = value; }
		}

		/// <summary>
		/// Static initializer
		/// </summary>
		static FloorData( )
		{
			MemoryStream stream = new MemoryStream( );
			Properties.Resources.DefaultFloorTexture.Save( stream, ImageFormat.Jpeg );
			ms_DefaultTextureSource = new StreamSource( stream, "DefaultFloorTexture.jpeg" );
		}

		/// <summary>
		/// Creates a texture asset handle from the default texture source
		/// </summary>
		private static AssetHandleT<Texture2d> CreateDefaultTextureHandle( )
		{
			AssetHandleT<Texture2d> handle = new AssetHandleT<Texture2d>( ms_DefaultTextureSource );
			handle.LoadParameters = new LoadParameters( );

			//	Mip maps yes please thank you
			handle.LoadParameters.Properties.Add( "generateMipMaps", true );
			return handle;
		}

		private AssetHandleT<Texture2d> m_Texture = CreateDefaultTextureHandle( );
		private static ISource ms_DefaultTextureSource;
	}
}
