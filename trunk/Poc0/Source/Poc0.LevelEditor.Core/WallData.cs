using System.Drawing.Imaging;
using System.IO;
using Rb.Core.Assets;

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
		public ISource TextureSource
		{
			get { return m_TextureSource; }
			set { m_TextureSource = value; }
		}

		/// <summary>
		/// Gets/sets the technque source for this node
		/// </summary>
		public ISource TechniqueSource
		{
			get { return m_TechniqueSource; }
			set { m_TechniqueSource = value; }
		}

		static WallData( )
		{
			MemoryStream stream = new MemoryStream( );
			Properties.Resources.DefaultWallTexture.Save( stream, ImageFormat.Jpeg );
			ms_DefaultTextureSource = new StreamSource( stream, "DefaultWallTexture" );

			//	TODO: AP: Argh argh bad :(
			ms_DefaultTechniqueSource = new Location( @"Graphics\Effects\perPixelTextured.cgfx" );
		}

		private ISource m_TextureSource = ms_DefaultTextureSource;
		private ISource m_TechniqueSource = ms_DefaultTechniqueSource;

		private static ISource ms_DefaultTextureSource;
		private static ISource ms_DefaultTechniqueSource;
	}
}
