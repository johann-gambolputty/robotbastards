using System.IO;
using System.Xml.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
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
	//	[Required]
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

			//bool dependenciesAreUpToDate = true;
			//DateTime inputTimestamp = File.GetLastWriteTime( SourceFile.ItemSpec );
			//for ( int frame = 0; frame < buildParams.Frames; ++frame )
			//{
			//    string filename = Path.Combine( OutputDirectory.ItemSpec, frame + ".png" );
			//    if ( !File.Exists( filename ) || File.GetLastWriteTime( filename ) < inputTimestamp )
			//    {
			//        dependenciesAreUpToDate = false;
			//        break;
			//    }
			//}
			//if ( dependenciesAreUpToDate )
			//{
			//    Log.LogMessage( "All dependencies for wave animation \"{0}\" are up to date", SourceFile.ItemSpec );
			//    return true;
			//}

		//	Bitmap[] animationBitmaps = new WaveAnimationGenerator( ).GenerateHeightmapSequence( buildParams );

			//if ( !Directory.Exists( OutputDirectory.ItemSpec ) )
			//{
			//    Directory.CreateDirectory( OutputDirectory.ItemSpec );
			//}

			//for ( int frame = 0; frame < animationBitmaps.Length; ++frame )
			//{
			//    animationBitmaps[ frame ].Save( Path.Combine( OutputDirectory.ItemSpec, frame + ".png" ), ImageFormat.Png );
			//}
			Log.LogMessage( "Generating wave animation as " + ( buildParams.StoreHeights ? "heightmaps" : "bump maps" ) );
			WaveAnimation animation = new WaveAnimationGenerator( ).GenerateSequence( buildParams, null );
			animation.Save( OutputFile.ItemSpec );
		//	animation.Frames[0].Save( OutputFile.ItemSpec + ".0.png" );

			return true;
		}

		#region Private Members

		private ITaskItem m_AnimationDefinitionFile;
		private ITaskItem m_OutputFile;

		private readonly static XmlSerializer s_BuildParametersSerializer = new XmlSerializer( typeof( WaveAnimationParameters ) );

		#endregion
	}
}
