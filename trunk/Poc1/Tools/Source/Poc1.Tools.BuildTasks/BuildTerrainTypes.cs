using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Poc1.Tools.TerrainTextures.Core;

namespace Poc1.Tools.BuildTasks
{
	/// <summary>
	/// Builds a TTS class
	/// </summary>
	public class BuildTerrainTypes : Task
	{
		/// <summary>
		/// Task source files
		/// </summary>
		[Required]
		public ITaskItem[] SourceFiles
		{
			get { return m_SourceFiles; }
			set { m_SourceFiles = value; }
		}

		/// <summary>
		/// Output directory for the exported terrain types
		/// </summary>
		[Required]
		public ITaskItem OutputDirectory
		{
			get { return m_OutputDirectory; }
			set { m_OutputDirectory = value; }
		}

		/// <summary>
		/// Gets the outputs of this task
		/// </summary>
		[Output]
		public ITaskItem[] Outputs
		{
			get { return m_Outputs; }
		}

		public ITaskItem SkipIfUnchanged
		{
			set { m_SkipIfUnchanged = bool.Parse( value.ItemSpec ); }
		}

		/// <summary>
		/// Executes this task
		/// </summary>
		public override bool Execute( )
		{
			string dstDir = OutputDirectory.ItemSpec;

			List<ITaskItem> outputs = new List<ITaskItem>( );
			foreach ( ITaskItem srcFile in m_SourceFiles )
			{
				TerrainTypeList set = TerrainTypeList.Load( srcFile.ItemSpec );

				string baseName = Path.GetFileNameWithoutExtension( srcFile.ItemSpec );

				string[] setOutputs = set.GetExportOutputs( dstDir, baseName );
				bool skip = false;
				DateTime srcTimestamp = File.GetLastWriteTime( srcFile.ItemSpec );
				if ( m_SkipIfUnchanged )
				{
					skip = true;
					foreach ( string setOutput in setOutputs )
					{
						skip &= ( File.Exists( setOutput ) && ( File.GetLastWriteTime( setOutput ) >= srcTimestamp ) );
					}
				}
				if ( skip )
				{
					Log.LogMessage( MessageImportance.Normal, "Skipping terrain set \"{0}\" - outputs are up to date", srcFile.ItemSpec );
					continue;
				}
				if ( !Directory.Exists( dstDir ) )
				{
					Directory.CreateDirectory( dstDir );
				}

				set.Export( dstDir, baseName );

				foreach ( string path in setOutputs )
				{
					outputs.Add( new TaskItem( path ) );
				}
			}

			m_Outputs = outputs.ToArray( );

			return true;
		}

		#region Private Members

		private ITaskItem[] m_Outputs;
		private ITaskItem[] m_SourceFiles;
		private ITaskItem m_OutputDirectory;
		private bool m_SkipIfUnchanged;


		private static string s_ProbeDir;

		static BuildTerrainTypes( )
		{
			//	Poc1.Tools.TerrainTextures.Core and dependencies are not visible from
			//	msbuild's location, so try to load them from the same directory as this assembly
			//	was loaded from.
			string loc = typeof( BuildTerrainTypes ).Assembly.Location;
			s_ProbeDir = Path.GetDirectoryName( loc );
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}

		private static Assembly CurrentDomain_AssemblyResolve( object sender, ResolveEventArgs args )
		{
			string name = new AssemblyName( args.Name ).Name + ".dll";
			string path = Path.Combine( s_ProbeDir, name );
			return Assembly.LoadFrom( path );
		}

		#endregion
	}
}
