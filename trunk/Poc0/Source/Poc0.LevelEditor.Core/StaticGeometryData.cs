using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Rb.Core.Assets;
using Rb.Rendering;
using Rb.Rendering.Textures;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Properties of a region of static geometry
	/// </summary>
	[Serializable]
	public class StaticGeometryData
	{
		#region Creation

		/// <summary>
		/// Creates default geometry data for walls
		/// </summary>
		public static StaticGeometryData CreateDefaultWallData( )
		{
			StaticGeometryData data = new StaticGeometryData( );

			data.Texture = CreateDefaultTexture( "DefaultWallTexture" );
		//	data.Technique = CreateDefaultTechnique( @"Graphics\Effects\perPixelTexturedShadowed.cgfx" );
			data.Technique = CreateDefaultTechnique( @"Graphics\Effects\DefaultWall.cgfx" );

			return data;
		}
		
		/// <summary>
		/// Creates default geometry data for floors
		/// </summary>
		public static StaticGeometryData CreateDefaultFloorData( )
		{
			StaticGeometryData data = new StaticGeometryData( );
			
			data.Texture = CreateDefaultTexture( "DefaultFloorTexture" );
		//	data.Technique = CreateDefaultTechnique( @"Graphics\Effects\perPixelTexturedShadowed.cgfx" );
			data.Technique = CreateDefaultTechnique( @"Graphics\Effects\DefaultFloor.cgfx" );
			return data;
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
		/// Gets/sets the technque source for this geometry
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

		#region Private stuff

		private ITexture2d m_Texture;
		private ITechnique m_Technique;
		
		/// <summary>
		/// Creates a <see cref="StreamSource"/> from a bitmap
		/// </summary>
		private static ITexture2d CreateDefaultTexture( string name )
		{
			Bitmap bmp = ( Bitmap )Properties.Resources.ResourceManager.GetObject( name, Properties.Resources.Culture );

			MemoryStream stream = new MemoryStream( );
			bmp.Save( stream, ImageFormat.Jpeg );
			StreamSource streamSrc = new StreamSource( stream, name + ".jpeg" ); // NOTE: AP: Requires extension for dumb loaders

			Texture2dAssetHandle texture = new Texture2dAssetHandle( streamSrc, true );
			
			//	Mip maps yes please thank you
			texture.LoadParameters = new LoadParameters( );
			texture.LoadParameters.Properties.Add( "generateMipMaps", true );
			return texture;
		}

		/// <summary>
		/// Creates an effect asset handle from the default effect source
		/// </summary>
		private static ITechnique CreateDefaultTechnique( string location )
		{
			EffectAssetHandle handle = new EffectAssetHandle( new Location( location ), true );
			return new TechniqueSelector( handle );
		}

		#endregion

	}
}
