using System.Drawing.Imaging;
using System.IO;
using Rb.Core.Assets;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Information about... a wall
	/// </summary>
	public class WallData
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
		/// Gets/sets the default technique source for walls
		/// </summary>
		public static ISource DefaultTechniqueSource
		{
			get { return ms_DefaultTechniqueSource; }
			set { ms_DefaultTechniqueSource = value; }
		}

		/// <summary>
		/// Gets/sets the texture source for this node
		/// </summary>
		public AssetHandleT<Texture2d> Texture
		{
			get { return m_Texture; }
			set { m_Texture = value; }
		}

		/// <summary>
		/// Gets/sets the technque source for this node
		/// </summary>
		public AssetHandle Technique
		{
			get { return m_Technique; }
			set { m_Technique = value; }
		}

		static WallData( )
		{
			MemoryStream stream = new MemoryStream( );
			Properties.Resources.DefaultWallTexture.Save( stream, ImageFormat.Jpeg );
			ms_DefaultTextureSource = new StreamSource( stream, "DefaultWallTexture.jpeg" );

			//	TODO: AP: Argh argh bad :(
			ms_DefaultTechniqueSource = new Location( @"Graphics\Effects\perPixelTextured.cgfx" );
		}

		private AssetHandleT<Texture2d> m_Texture = new AssetHandleT<Texture2d>( ms_DefaultTextureSource );
		private AssetHandle m_Technique = new AssetHandle( ms_DefaultTechniqueSource );

		private static ISource ms_DefaultTextureSource;
		private static ISource ms_DefaultTechniqueSource;
	}
}
