using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Rb.Core.Utils;
using Rb.Log;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core
{
	/// <summary>
	/// Helper class for opening and saving scenes
	/// </summary>
	public class SceneSerializer
	{
		/// <summary>
		/// Access to the last save path
		/// </summary>
		public string LastSavePath
		{
			get { return m_LastSavePath; }
			set
			{
				m_LastSavePath = value;
				if ( LastSavePathChanged != null )
				{
					LastSavePathChanged( m_LastSavePath );
				}
			}
		}

		/// <summary>
		/// Event, invoked when the last save path changes
		/// </summary>
		public event Action< string > LastSavePathChanged;

		/// <summary>
		/// Saves a scene to a scene file at a user-defined path
		/// </summary>
		/// <param name="scene">Scene to serialize</param>
		public void SaveAs( Scene scene )
		{
			SaveFileDialog saveDialog = new SaveFileDialog( );
			saveDialog.DefaultExt = "scene";
			saveDialog.AddExtension = true;
			if ( saveDialog.ShowDialog( ) != DialogResult.OK )
			{
				return;
			}

			LastSavePath = saveDialog.FileName;
			SaveTo( LastSavePath, scene );
		}

		/// <summary>
		/// Saves a scene. Uses the last save/open path, or (if it's empty), a user-defined path
		/// </summary>
		/// <param name="scene">Scene to serialize</param>
		public void Save( Scene scene )
		{
			if ( LastSavePath == null )
			{
				SaveAs( scene );
			}
			else
			{
				SaveTo( LastSavePath, scene );
			}
		}

		/// <summary>
		/// Saves a scene to a given scene file
		/// </summary>
		/// <param name="path">Scene file path</param>
		/// <param name="scene">Scene to serialize</param>
		public void SaveTo( string path, Scene scene )
		{
			try 
			{
				MemoryStream outStream = new MemoryStream( );

				IFormatter formatter = CreateFormatter( );
				formatter.Serialize( outStream, scene );

				using ( Stream fileStream = File.OpenWrite( path ) )
				{
					fileStream.Write( outStream.ToArray( ), 0, ( int )outStream.Length );
				}
			}
			catch ( Exception ex )
			{
				string msg = string.Format( Properties.Resources.FailedToSaveScene, path );
				AppLog.Error( msg );
				ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
				MessageBox.Show( msg, Properties.Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}
		}

		/// <summary>
		/// Opens a scene file
		/// </summary>
		/// <param name="path">Path to the scene file</param>
		/// <returns>Returns the deserialized scene, or null if the open failed</returns>
		public EditorScene Open( string path )
		{
			EditorScene scene;
			try
			{
				using ( Stream fileStream = File.OpenRead( path ) )
				{
					IFormatter formatter = CreateFormatter( );
					scene = ( EditorScene )formatter.Deserialize( fileStream );
				}
			}
			catch ( Exception ex )
			{
				string msg = string.Format( Properties.Resources.FailedToOpenScene, path );
				AppLog.Error( msg );
				ExceptionUtils.ToLog( AppLog.GetSource( Severity.Error ), ex );
				MessageBox.Show( msg, Properties.Resources.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error );
				return null;
			}

			LastSavePath = path;

			return scene;
		}

		/// <summary>
		/// Opens a scene file at a user-defined path
		/// </summary>
		/// <returns>Returns the deserialized scene, or null if the open failed</returns>
		public EditorScene Open( )
		{
			OpenFileDialog openDialog = new OpenFileDialog( );
			openDialog.Filter = "Scene Files (*.scene)|*.scene|All Files (*.*)|*.*";
			if ( openDialog.ShowDialog( ) != DialogResult.OK )
			{
				return null;
			}

			return Open( openDialog.FileName );
		}

		#region Protected stuff

		/// <summary>
		/// Creates the formatter that is used to serialize the scene
		/// </summary>
		protected virtual IFormatter CreateFormatter( )
		{
			IFormatter formatter = new BinaryFormatter( null, new StreamingContext( ) );
			return formatter;
		}

		#endregion

		#region Private stuff

		private string m_LastSavePath;

		#endregion
	}
}