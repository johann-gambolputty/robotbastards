using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Rb.World;

namespace Poc0.LevelEditor
{
	class SceneExporter
	{
		/// <summary>
		/// Access to the last export path
		/// </summary>
		public string LastExportPath
		{
			get { return m_LastExportPath; }
			set
			{
				m_LastExportPath = value;
				if ( LastExportPathChanged != null )
				{
					LastExportPathChanged( m_LastExportPath );
				}
			}
		}

		/// <summary>
		/// Event, invoked when the last export path changes
		/// </summary>
		public event Action< string > LastExportPathChanged;
		
		/// <summary>
		/// Exports a scene to a scene file at a user-defined path
		/// </summary>
		/// <param name="scene">Scene to serialize</param>
		public void ExportAs( Scene scene )
		{
			SaveFileDialog exportDialog = new SaveFileDialog( );
			exportDialog.Title = "Export To...";
			exportDialog.DefaultExt = "runtimeScene";
			exportDialog.AddExtension = true;
			if ( exportDialog.ShowDialog( ) != DialogResult.OK )
			{
				return;
			}

			LastExportPath = exportDialog.FileName;
			ExportTo( LastExportPath, scene );
		}

		/// <summary>
		/// Exports a scene. Uses the last Export/open path, or (if it's empty), a user-defined path
		/// </summary>
		/// <param name="scene">Scene to serialize</param>
		public void Export( Scene scene )
		{
			if ( LastExportPath == null )
			{
				ExportAs( scene );
			}
			else
			{
				ExportTo( LastExportPath, scene );
			}
		}

		/// <summary>
		/// Exports a scene to a given scene file
		/// </summary>
		/// <param name="path">Scene file path</param>
		/// <param name="scene">Scene to serialize</param>
		public void ExportTo( string path, Scene scene )
		{
			//try 
			//{
			//    MemoryStream outStream = new MemoryStream( );

			//    IFormatter formatter = CreateFormatter( );
			//    formatter.Serialize( outStream, scene );

			//    using ( Stream fileStream = File.OpenWrite( path ) )
			//    {
			//        fileStream.Write( outStream.ToArray( ), 0, ( int )outStream.Length );
			//    }
			//}
			//catch ( Exception ex )
			//{
			//    string msg = string.Format( Properties.Resources.FailedToExportScene, path );
			//    AppLog.Error( msg );
			//    ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
			//    MessageBox.Show( msg, Properties.Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
			//    return;
			//}
		}
		private string m_LastExportPath;
	}
}
