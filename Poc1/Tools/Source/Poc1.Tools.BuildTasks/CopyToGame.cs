using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Poc1.Tools.BuildTasks
{
	public class CopyToGame : Task
	{
		[Required]
		public ITaskItem[] SourceFiles
		{
			get { return m_SourceFiles; }
			set { m_SourceFiles = value; }
		}

		[Output]
		public ITaskItem[] CopiedFiles
		{
			get { return m_CopiedFiles; }
		}

		public ITaskItem SkipIfUnchanged
		{
			set { m_SkipIfUnchanged = bool.Parse( value.ItemSpec ); }
		}

		public override bool Execute( )
		{
			List<ITaskItem> copiedFiles = new List<ITaskItem>( );

			foreach ( ITaskItem srcFile in m_SourceFiles )
			{
				bool isDesignPath;
				string srcPath = srcFile.ItemSpec;
				string dstPath = CreateDestinationPath( srcPath, out isDesignPath );
				if ( isDesignPath )
				{
					if ( !m_SkipIfUnchanged || FileRequiresUpdate( srcFile.ItemSpec, dstPath ) )
					{
						File.Copy( srcFile.ItemSpec, dstPath, true );
						copiedFiles.Add( new TaskItem( dstPath ) );
					}
				}
			}

			m_CopiedFiles = copiedFiles.ToArray( );
			return true;
		}

		private static bool FileRequiresUpdate( string srcPath, string dstPath )
		{
			DateTime srcTimestamp = File.GetLastWriteTime( srcPath );
			DateTime dstTimestamp = File.GetLastWriteTime( dstPath );
			return srcTimestamp > dstTimestamp; 
		}

		public static string CreateDestinationPath( string srcPath, out bool isDesignPath )
		{
			isDesignPath = srcPath.StartsWith( "Design" );
			if ( isDesignPath )
			{
				for ( int chIndex = 0; chIndex < srcPath.Length; ++chIndex )
				{
					if ( srcPath[ chIndex ] == '/' || srcPath[ chIndex ] == '\\' )
					{
						srcPath = srcPath.Remove( 0, chIndex );
						break;
					}
				}
				srcPath = "Game" + srcPath;

				string dstDir = Path.GetDirectoryName( srcPath );
				if ( !Directory.Exists( dstDir ) )
				{
					Directory.CreateDirectory( dstDir );
				}
			}
			return srcPath;
		}

		private ITaskItem[]	m_SourceFiles;
		private ITaskItem[] m_CopiedFiles;
		private bool m_SkipIfUnchanged;

	}
}
