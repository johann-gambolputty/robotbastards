using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Poc1.Tools.TerrainTextures.Core;
using Poc1.Tools.Waves;

namespace Poc1.Tools.BuildTasks
{
	public class CreateWaveAnimation : Task
	{
		/// <summary>
		/// Task input files
		/// </summary>
		[Required]
		public ITaskItem SourceFile
		{
			get { return m_AnimationDefinitionFile; }
			set { m_AnimationDefinitionFile = value; }
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

			WaveAnimationParameters buildParams;
			using ( FileStream stream = new FileStream( definitionFile, FileMode.Open, FileAccess.Read ) )
			{
				buildParams = ( WaveAnimationParameters )s_BuildParametersSerializer.Deserialize( stream );
			}

			Bitmap[] animationBitmaps = new WaveAnimationGenerator( ).GenerateHeightmapSequence( buildParams );
			bmp.Save( OutputFile.ItemSpec, ImageFormat.Jpeg );

			return true;
		}

		#region Private Members

		private ITaskItem m_AnimationDefinitionFile;
		private ITaskItem m_OutputFile;

		private readonly static XmlSerializer s_BuildParametersSerializer = new XmlSerializer( typeof( WaveAnimationParameters ) );

		#endregion
	}
}
