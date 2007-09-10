using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.EditModes;
using Rb.Core.Utils;
using Rb.Log;
using Rb.World;
using Poc0.LevelEditor.Rendering.OpenGl;

namespace Poc0.LevelEditor
{
	/// <summary>
	/// Helper class for opening and saving scenes
	/// </summary>
	class SceneSerializer
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
		public EditorScene Open(string path)
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

		#region Private stuff

		private string m_LastSavePath;

		private static ISurrogateSelector CreateSurrogateSelector( )
		{
			SurrogateSelector selector = new SurrogateSelector( );
			selector.AddSurrogate( typeof( OpenGlTileBlock2dRenderer ), new StreamingContext( ), new GraphicsSurrogate( ) );
			return selector;
		}

		private static IFormatter CreateFormatter( )
		{
			//	TODO: AP: Muesli formatter doesn't work!
			IFormatter formatter = new BinaryFormatter( CreateSurrogateSelector( ), new StreamingContext( ) );
			//IFormatter formatter = new Rb.Muesli.BinaryFormatter( CreateSurrogateSelector( ), new StreamingContext( ) );
			return formatter;
		}

		//	TODO: AP: This surrogate is ass
		/// <summary>
		/// Test class for remapping graphics types
		/// </summary>
		private class GraphicsSurrogate : ISerializationSurrogate
		{
			#region ISerializationSurrogate Members

			public void GetObjectData( object obj, SerializationInfo info, StreamingContext context )
			{
				TileBlockRenderer renderer = ( ( TileBlockRenderer )obj );
				info.AddValue( "grid", renderer.Grid );
			}

			public object SetObjectData( object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector )
			{
				TileGrid grid = ( TileGrid )info.GetValue( "grid", typeof( TileGrid ) );
				return new OpenGlTileBlock2dRenderer( grid, EditModeContext.Instance );
			}

			#endregion
		}

		#endregion
	}
}
