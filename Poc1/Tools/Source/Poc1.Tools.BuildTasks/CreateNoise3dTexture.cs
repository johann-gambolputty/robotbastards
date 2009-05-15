
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Poc1.Tools.TerrainTextures.Core;
using Rb.Rendering.Interfaces.Objects;
using Rb.TextureAssets;

namespace Poc1.Tools.BuildTasks
{
	/// <summary>
	/// Creates a 3-dimensional texture used for noise shaders
	/// </summary>
	public class CreateNoise3dTexture : Task
	{
		/// <summary>
		/// Task input files
		/// </summary>
		[Required]
		public ITaskItem SourceFile
		{
			get { return m_NoiseDefinitionFile; }
			set { m_NoiseDefinitionFile = value; }
		}

		/// <summary>
		/// Task output files
		/// </summary>
		[Required, Output]
		public ITaskItem OutputFile
		{
			get { return m_OutputFile; }
			set { m_OutputFile = value; }
		}

		/// <summary>
		/// Executes this task
		/// </summary>
		public override bool Execute( )
		{
			string definitionFile = SourceFile.ItemSpec;

			Noise3dTextureBuilderParameters buildParams;
			using ( FileStream stream = new FileStream( definitionFile, FileMode.Open, FileAccess.Read ) )
			{
				buildParams = ( Noise3dTextureBuilderParameters )s_BuildParametersSerializer.Deserialize( stream );
			}
			buildParams.Validate( );

			string outputDirectory = Path.GetDirectoryName( OutputFile.ItemSpec );
			if ( !Directory.Exists( outputDirectory ) )
			{
				Directory.CreateDirectory( outputDirectory );
			}

			Texture3dData texData = Noise3dTextureBuilder.Build( buildParams );
			using ( FileStream stream = new FileStream( OutputFile.ItemSpec, FileMode.Create, FileAccess.Write ) )
			{
				TextureWriter.WriteTextureToStream( new Texture3dData[] { texData }, stream );
			}

			//	TODO: AP: REMOVEME
			//	Write slices to sample bitmaps
			string baseBitmapDir = Path.GetDirectoryName( OutputFile.ItemSpec ) + "\\tmp\\";
			string baseBitmapPath = baseBitmapDir + "slice";
			if ( !Directory.Exists( baseBitmapDir ) )
			{
				Directory.CreateDirectory( baseBitmapDir );
			}
			Bitmap bmp = new Bitmap( texData.Width * 2, texData.Height * 2 );
			int xStride = TextureFormatInfo.GetSizeInBytes( texData.Format );
			int yStride = xStride * texData.Width;
			int zStride = yStride * texData.Height;
			for ( int d = 0; d < texData.Depth; ++d  )
			{
				//	Copy slice to bitmap
				int zOffset = d * zStride;
				for ( int y = 0; y < texData.Height * 2; ++y )
				{
					int yOffset = zOffset + ( y % texData.Height ) * yStride;
					for ( int x = 0; x < texData.Width* 2; ++x )
					{
						int offset = yOffset + ( x % texData.Width ) * xStride;
						byte val = texData.Bytes[ offset ];
						bmp.SetPixel( x, y, Color.FromArgb( val, val, val ) );
					}
				}

				bmp.Save( baseBitmapPath + d.ToString( "D3" ) + ".png" );
			}

			return true;
		}

		#region Private Members

		private ITaskItem m_NoiseDefinitionFile;
		private ITaskItem m_OutputFile;

		private readonly static XmlSerializer s_BuildParametersSerializer = new XmlSerializer( typeof( Noise3dTextureBuilderParameters ) );

		#endregion
	}
}
