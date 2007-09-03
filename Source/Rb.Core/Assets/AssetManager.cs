using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Rb.Core.Utils;
using Rb.Log;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Singleton that manages asset loading
	/// </summary>
	public class AssetManager
	{
		/// <summary>
		/// Gets the AssetManager singleton
		/// </summary>
		public static AssetManager Instance
		{
			get { return ms_Singleton;  }
		}

		//	TODO: AP: Asynchronous asset loading

		#region Loaders

		/// <summary>
		/// Adds an asset loader
		/// </summary>
		/// <param name="loader">Loader to add</param>
		public void AddLoader( IAssetLoader loader )
		{
			m_Loaders.Add( loader );
		}

		/// <summary>
		/// Adds a bunch of loaders from an XML file
		/// </summary>
		public void AddLoadersFromXml( string uri )
		{
			XmlReader reader;
			try
			{
				reader = XmlReader.Create( uri );
			}
			catch ( Exception ex )
			{
				AssetsLog.Error( "Failed to create XML reader" );
				ExceptionUtils.ToLog( AssetsLog.GetSource( Severity.Error ), ex );
				return;
			}

			ReadLoaders( uri, reader );
		}

		/// <summary>
		/// Finds an <see cref="IAssetLoader"/> that can load a specified source
		/// </summary>
		public IAssetLoader FindLoaderForAsset( ISource source )
		{
			foreach ( IAssetLoader loader in m_Loaders )
			{
				if ( loader.CanLoad( source ) )
				{
					return loader;
				}
			}
			return null;
		}

		#endregion

		#region Synchronous asset loading

		/// <summary>
		/// Creates a load state that can load an asset from a given source
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Asset loading parameters</param>
		/// <returns>Returns a new LoadState that can load the asset at source</returns>
		public LoadState CreateLoadState( ISource source, LoadParameters parameters )
		{
			IAssetLoader loader = FindLoaderForAsset( source );

			return new LoadState( loader, source, parameters );
		}

		/// <summary>
		/// Loads an asset at a given location path
		/// </summary>
		/// <param name="path">Location path</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( string path )
		{
			return Load( new Location( path ) );
		}

		/// <summary>
		/// Loads an asset at a given location path, with specified parameters
		/// </summary>
		/// <param name="path">Location path</param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( string path, LoadParameters parameters )
		{
			return Load( new Location( path ), parameters );
		}

		/// <summary>
		/// Loads an asset from a given source
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( ISource source )
		{
			return Load( source, null );
		}
		
		/// <summary>
		/// Loads an asset at a given location, with specified parameters
		/// </summary>
		/// <param name="source">Asset source</param>
		/// <param name="parameters">Loading parameters</param>
		/// <returns>Returns the loaded asset</returns>
		public object Load( ISource source, LoadParameters parameters )
		{
			if ( !source.IsValid )
			{
				throw new ArgumentException( string.Format( "Invalid asset source {0}", source ) );
			}

			foreach ( IAssetLoader loader in m_Loaders )
			{
				if ( loader.CanLoad( source ) )
				{
					LoadState loadState = new LoadState( loader, source, parameters );
					return loadState.Load( );
				}
			}

			throw new ArgumentException( string.Format( "No loader could load asset {0}", source ) );
		}

		#endregion

		#region Private stuff

		private readonly List< IAssetLoader >	m_Loaders		= new List< IAssetLoader >( );
		private static readonly AssetManager	ms_Singleton	= new AssetManager( );

		/// <summary>
		/// Reads all loaders from an XML file
		/// </summary>
		/// <param name="uri">XML uri</param>
		/// <param name="reader">XML reader</param>
		private void ReadLoaders( string uri, XmlReader reader )
		{
			try
			{
				if ( reader.IsEmptyElement )
				{
					return;
				}

				reader.ReadStartElement( "loaders" );
				
				while ( reader.NodeType != XmlNodeType.EndElement )
				{
					if ( reader.NodeType != XmlNodeType.Element )
					{
						reader.Read( );
						continue;
					}
					
					if ( reader.Name == "loader" )
					{
						ReadLoader( reader );
					}
					else
					{
						throw new XmlException( string.Format( "Unexpected element <{0}>", reader.Name ) );
					}
				}

				reader.ReadEndElement( );
			}
			catch ( Exception ex )
			{
				string msg = "Failed to read AssetManager loaders";
				Entry logEntry = new Entry( AssetsLog.GetSource( Severity.Error ), msg );
				IXmlLineInfo lineInfo = ( IXmlLineInfo )reader;
				logEntry.Locate( uri, lineInfo.LineNumber, lineInfo.LinePosition, "" );

				Source.HandleEntry( logEntry );
				ExceptionUtils.ToLog( AssetsLog.GetSource( Severity.Error ), ex );
				return;
			}
			
		}
		
		/// <summary>
		/// Reads a type from XML, creates an instance of the type, and adds the instance to the loader list
		/// </summary>
		private void ReadLoader( XmlReader reader )
		{
			string type = reader.GetAttribute( "type" );
			string assembly = reader.GetAttribute( "assembly" );

			if ( string.IsNullOrEmpty( type ) )
			{
				throw new XmlException( "<loader> requires a \"type\" attribute" );
			}
			if ( string.IsNullOrEmpty( assembly ) )
			{
				throw new XmlException( "<loader> requires an \"assembly\" attribute" );
			}

			Type loaderType = Assembly.Load( assembly ).GetType( type );

			IAssetLoader loader = ( IAssetLoader )Activator.CreateInstance( loaderType );
			AddLoader( loader );
		}

		#endregion

	}
}
