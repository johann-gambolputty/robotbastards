using System;
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
		#region Default wall data settings

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
		public static ISource DefaultEffectSource
		{
			get { return ms_DefaultEffectSource; }
			set { ms_DefaultEffectSource = value; }
		}

		#endregion

		#region Public members

		/// <summary>
		/// Event, invoked when wall data is altered
		/// </summary>
		public event EventHandler WallDataChanged;

		/// <summary>
		/// Gets/sets the texture source for this node
		/// </summary>
		public ITexture2d Texture
		{
			get { return m_Texture; }
			set
			{
				m_Texture = value;
				if ( WallDataChanged != null )
				{
					WallDataChanged( this, null );
				}
			}
		}

		/// <summary>
		/// Gets/sets the technque source for this node
		/// </summary>
		public ITechnique Technique
		{
			get { return m_Technique; }
			set
			{
				m_Technique = value;
				if ( WallDataChanged != null )
				{
					WallDataChanged( this, null );
				}
			}
		}

		#endregion

		#region Private members

		static WallData( )
		{
			MemoryStream stream = new MemoryStream( );
			Properties.Resources.DefaultWallTexture.Save( stream, ImageFormat.Jpeg );
			ms_DefaultTextureSource = new StreamSource( stream, "DefaultWallTexture.jpeg" );

			//	TODO: AP: Argh argh bad :(
			ms_DefaultEffectSource = new Location( @"Graphics\Effects\perPixelTextured.cgfx" );
		}
		
		/// <summary>
		/// Creates a texture asset handle from the default texture source
		/// </summary>
		private static ITexture2d CreateDefaultTexture( )
		{
			//	Note: Both methods work. Texture asset handle is used if the default source is changed to
			//	an external file
		//	LoadParameters loadParams = new LoadParameters( );
		//	loadParams.Properties.Add( "generateMipMaps", true );
		//	return ( ITexture2d )AssetManager.Instance.Load( ms_DefaultTextureSource, loadParams );
			Texture2dAssetHandle handle = new Texture2dAssetHandle( ms_DefaultTextureSource );
			handle.LoadParameters = new LoadParameters( );

			//	Mip maps yes please thank you
			handle.LoadParameters.Properties.Add( "generateMipMaps", true );
			return handle;
		}

		/// <summary>
		/// Creates an effect asset handle from the default effect source
		/// </summary>
		private static ITechnique CreateDefaultTechnique( )
		{
			EffectAssetHandle handle = new EffectAssetHandle( ms_DefaultEffectSource );
			return new TechniqueSelector( handle );
		}

		private ITexture2d m_Texture = CreateDefaultTexture( );
		private ITechnique m_Technique = CreateDefaultTechnique( );

		private static ISource ms_DefaultTextureSource;
		private static ISource ms_DefaultEffectSource;

		#endregion
	}
}
