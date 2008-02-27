using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Rb.Log;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core
{
	/// <summary>
	/// Helper class for opening and saving scenes
	/// </summary>
	public class SceneSerializer
	{
		/// <summary>
		/// Called prior to the scene being serialized
		/// </summary>
		public event EventHandler PreSerialize;

		/// <summary>
		/// Called after the scene has been serialized
		/// </summary>
		public event EventHandler PostSerialize;

		/// <summary>
		/// Gets the scene serializer singleton
		/// </summary>
		public static SceneSerializer Instance
		{
			get { return ms_Singleton; }
		}

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
				//	Stop all the current edit modes - they sometimes attach themselves to the
				//	scene to be rendered
				foreach ( IEditMode mode in EditorState.Instance.ActiveEditModes )
				{
					mode.Stop( );
				}

				if ( PreSerialize != null )
				{
					PreSerialize( this, null );
				}

				MemoryStream outStream = new MemoryStream( );

				IFormatter formatter = CreateFormatter( );
				formatter.Serialize( outStream, scene );

				using ( Stream fileStream = File.OpenWrite( path ) )
				{
					fileStream.Write( outStream.ToArray( ), 0, ( int )outStream.Length );
				}

				//	Restart all edit modes
				foreach ( IEditMode mode in EditorState.Instance.ActiveEditModes )
				{
					mode.Start( );
				}
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to save scene to \"{0}\"", path );
				ErrorMessageBox.Show( Properties.Resources.SaveSceneFailed, path );
			}

			if ( PostSerialize != null )
			{
				PostSerialize( this, null );
			}
		}

		/// <summary>
		/// Opens a scene file
		/// </summary>
		/// <param name="path">Path to the scene file</param>
		/// <returns>Returns the deserialized scene, or null if the open failed</returns>
		public Scene Open( string path )
		{
			Scene scene;
			try
			{
				using ( Stream fileStream = File.OpenRead( path ) )
				{
					IFormatter formatter = CreateFormatter( );
					scene = ( Scene )formatter.Deserialize( fileStream );
				}
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to open scene \"{0}\"", path );
				ErrorMessageBox.Show( Properties.Resources.OpenSceneFailed, path );
				return null;
			}

			LastSavePath = path;

			return scene;
		}

		/// <summary>
		/// Opens a scene file at a user-defined path
		/// </summary>
		/// <returns>Returns the deserialized scene, or null if the open failed</returns>
		public Scene Open( )
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
		//	IFormatter formatter = new Muesli.BinaryFormatter( );
			return formatter;
		}

		#endregion

		#region Private stuff

		private readonly static SceneSerializer ms_Singleton = new SceneSerializer( );
		private string m_LastSavePath;

		/// <summary>
		/// Private constructor forces singleton use only
		/// </summary>
		private SceneSerializer( )
		{
		}

		#endregion
	}
}
