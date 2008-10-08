using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.Tools.BuildTasks
{
	public class CreateNoiseBitmap : Task
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
		[Required]
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

			NoiseBitmapBuilderParameters buildParams;
			using ( FileStream stream = new FileStream( definitionFile, FileMode.Open, FileAccess.Read ) )
			{
				buildParams = ( NoiseBitmapBuilderParameters )s_BuildParametersSerializer.Deserialize( stream );
			}

			Bitmap bmp = NoiseBitmapBuilder.Build( buildParams );
			bmp.Save( OutputFile.ItemSpec, ImageFormat.Jpeg );

			if ( !string.IsNullOrEmpty( buildParams.TestFilePath ) )
			{
				using ( Bitmap testBmp = new Bitmap( bmp.Width * 2, bmp.Height * 2, PixelFormat.Format24bppRgb ) )
				{
					using ( Graphics graphics = Graphics.FromImage( testBmp ) )
					{
						graphics.DrawImage( bmp, 0, 0 );
						graphics.DrawImage( bmp, bmp.Width, 0 );
						graphics.DrawImage( bmp, 0, bmp.Height );
						graphics.DrawImage( bmp, bmp.Width, bmp.Height );
					}
					testBmp.Save( buildParams.TestFilePath );
				}
			}
			return true;
		}

		#region Private Members

		private ITaskItem m_NoiseDefinitionFile;
		private ITaskItem m_OutputFile;

		private readonly static XmlSerializer s_BuildParametersSerializer = new XmlSerializer( typeof( NoiseBitmapBuilderParameters ) );

		#endregion
	}
}
