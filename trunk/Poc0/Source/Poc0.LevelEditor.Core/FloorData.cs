using System.Drawing.Imaging;
using System.IO;
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
		public ITexture2d Texture
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
		private static ITexture2d CreateDefaultTexture( )
		{
			Texture2dAssetHandle handle = new Texture2dAssetHandle( ms_DefaultTextureSource );

			handle.LoadParameters = new LoadParameters( );

			//	Mip maps yes please thank you
			handle.LoadParameters.Properties.Add( "generateMipMaps", true );
			return handle;
		}

		private ITexture2d m_Texture = CreateDefaultTexture( );
		private static ISource ms_DefaultTextureSource;
	}
}
