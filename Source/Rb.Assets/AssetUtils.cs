
using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Rb.Assets.Interfaces;
using Rb.Log;

namespace Rb.Assets
{
	/// <summary>
	/// Handy utility functions for working with assets
	/// </summary>
	public static class AssetUtils
	{
		#region Setup

		/// <summary>
		/// Sets up the <see cref="AssetManager"/> singleton and <see cref="Locations"/> singleton from an XML file
		/// </summary>
		public static void Setup( string uri )
		{
			XmlTextReader reader;
			try
			{
				reader = new XmlTextReader( uri );
				reader.WhitespaceHandling = WhitespaceHandling.Significant;
				reader.ReadStartElement( "assetManager" );
			}
			catch ( Exception ex )
			{
				AssetsLog.Exception( ex, "Failed to create XML reader" );
				return;
			}

			ReadLocationManagers( Locations.Instance, uri, reader );
			ReadLoaders( AssetManager.Instance, uri, reader );
		}

		#endregion

		#region Utilities
		
		/// <summary>
		/// Gets a file relative to a given source
		/// </summary>
		public static IFile GetRelativeFile( ISource source, string name )
		{
			if ( source is IFolder )
			{
				return ( ( IFolder )source ).GetFile( name );
			}
			return source.Location.ParentFolder.GetFile( name );
		}

		#endregion

		#region Private members

		/// <summary>
		/// Reads all location managers from an XML file
		/// </summary>
		/// <param name="managers">Location managers to store results</param>
		/// <param name="uri">XML uri</param>
		/// <param name="reader">XML reader</param>
		private static void ReadLocationManagers( Locations managers, string uri, XmlReader reader )
		{
			try
			{
				if ( reader.IsEmptyElement )
				{
					return;
				}

				reader.ReadStartElement( "locationManagers" );
				
				while ( reader.NodeType != XmlNodeType.EndElement )
				{
					if ( reader.NodeType != XmlNodeType.Element )
					{
						reader.Read( );
						continue;
					}
					
					if ( reader.Name == "locationManager" )
					{
						managers.Systems.Add( ReadLocationManager( reader ) );
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

		private static ILocationManager ReadLocationManager( XmlReader reader )
		{
			ILocationManager manager = ReadObject<ILocationManager>( reader );
			if ( reader.IsEmptyElement )
			{
				return manager;
			}
			if ( manager is IXmlSerializable )
			{
				( ( IXmlSerializable )manager ).ReadXml( reader );
			}
			reader.ReadEndElement( );

			return manager;
		}

		/// <summary>
		/// Reads all loaders from an XML file
		/// </summary>
		/// <param name="manager">Manager to set up</param>
		/// <param name="uri">XML uri</param>
		/// <param name="reader">XML reader</param>
		private static void ReadLoaders( AssetManager manager, string uri, XmlReader reader )
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
						manager.AddLoader( ReadLoader( reader ) );
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
		private static IAssetLoader ReadLoader( XmlReader reader )
		{
			IAssetLoader loader = ReadObject< IAssetLoader >( reader );

			if ( loader is IXmlSerializable )
			{
				( ( IXmlSerializable )loader ).ReadXml( reader );
			}
			else
			{
				reader.Skip( );
			}
			return loader;
		}

		private static T ReadObject< T >( XmlReader reader )
		{
			string typeName = reader.GetAttribute( "type" );
			string assemblyName = reader.GetAttribute( "assembly" );

			if ( string.IsNullOrEmpty( typeName ) )
			{
				throw new XmlException( string.Format( "<{0}> requires a \"type\" attribute", reader.Name ) );
			}
			if ( string.IsNullOrEmpty( assemblyName ) )
			{
				throw new XmlException( string.Format( "<{0}> requires an \"assembly\" attribute", reader.Name ) );
			}

			Type type = Assembly.Load( assemblyName ).GetType( typeName );

			object result = Activator.CreateInstance( type );
			return ( T )result;
		}

		#endregion
	}
}
