using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Rb.Log;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core
{
	/// <summary>
	/// Manages exporting the scene
	/// </summary>
	public class SceneExporter
	{
		/// <summary>
		/// Called prior to the runtime scene being exported
		/// </summary>
		public event EventHandler PreExport;

		/// <summary>
		/// Called after the runtime scene has been exported
		/// </summary>
		public event EventHandler PostExport;

		/// <summary>
		/// Gets the scene exporter singleton
		/// </summary>
		public static SceneExporter Instance
		{
			get { return ms_Singleton; }
		}

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
		/// Opens an exported scene from a given stream
		/// </summary>
		/// <param name="stream">Stream containing serialized Scene object</param>
		/// <returns>Returns the loaded scene object</returns>
		public static Scene Open( Stream stream )
		{
			IFormatter formatter = CreateFormatter( );
			object result = formatter.Deserialize( stream );
			return ( Scene )result;
		}
		
		/// <summary>
		/// Exports a scene to a scene file at a user-defined path
		/// </summary>
		/// <param name="scene">Scene to serialize</param>
		/// <returns>Returns true if the scene was exported, false if the user cancelled</returns>
		public bool ExportAs( Scene scene )
		{
			SaveFileDialog exportDialog = new SaveFileDialog( );
			exportDialog.Title = "Export To...";
			exportDialog.DefaultExt = "rtscene";
			exportDialog.Filter = "Runtime Scene File|*.rtscene|All Files|*.*";
			exportDialog.AddExtension = true;
			if ( exportDialog.ShowDialog( ) != DialogResult.OK )
			{
				return false;
			}

			LastExportPath = exportDialog.FileName;
			return ExportTo( LastExportPath, scene );
		}

		/// <summary>
		/// Exports a scene. Uses the last Export/open path, or (if it's empty), a user-defined path
		/// </summary>
		/// <param name="scene">Scene to serialize</param>
		public bool Export( Scene scene )
		{
			if ( LastExportPath == null )
			{
				return ExportAs( scene );
			}

			return ExportTo( LastExportPath, scene );
		}

		/// <summary>
		/// Exports a scene to a given scene file
		/// </summary>
		/// <param name="path">Scene file path</param>
		/// <param name="scene">Scene to serialize</param>
		/// <returns>Returns true if the scene was succesfully exported</returns>
		public bool ExportTo( string path, Scene scene )
		{
			bool success = false;
			try
			{
				if ( PreExport != null )
				{
					PreExport( this, null );
				}

				MemoryStream outStream = new MemoryStream( );

				IFormatter formatter = CreateFormatter( );
				formatter.Serialize( outStream, scene );

				using ( Stream fileStream = File.OpenWrite( path ) )
				{
					fileStream.Write( outStream.ToArray( ), 0, ( int )outStream.Length );
				}

				success = true;
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to export scene to \"{0}\"", path );
				ErrorMessageBox.Show( Properties.Resources.FailedToExportScene, path );
			}

			if ( PostExport != null )
			{
				PostExport( this, null );
			}

			return success;
		}

		#region Private stuff

		private readonly static SceneExporter ms_Singleton = new SceneExporter( );
		private string m_LastExportPath;

		private static IFormatter CreateFormatter( )
		{
			IFormatter formatter = new BinaryFormatter( null, new StreamingContext( ) );
			return formatter;
		}

		/// <summary>
		/// Private constructor forces singleton use only
		/// </summary>
		private SceneExporter( )
		{
		}

		#endregion
	}
}
