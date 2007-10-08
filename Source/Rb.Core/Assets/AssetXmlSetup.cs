using System;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Rb.Log;

namespace Rb.Core.Assets
{
	public static class AssetXmlSetup
	{
		/// <summary>
		/// Sets up an <see cref="AssetManager"/> and a <see cref="LocationManagers"/> from an XML file
		/// </summary>
		/// <param name="uri">XML file location</param>
		/// <param name="manager">Manager to set up</param>
		/// <param name="locationManagers">Location managers to set up</param>
		public static void Setup( string uri, AssetManager manager, LocationManagers locationManagers )
		{
			XmlTextReader reader;
			try
			{
				//reader = XmlReader.Create( uri );
				reader = new XmlTextReader( uri );
				reader.WhitespaceHandling = WhitespaceHandling.Significant;
				reader.ReadStartElement( "assetManager" );
			}
			catch ( Exception ex )
			{
				AssetsLog.Exception( ex, "Failed to create XML reader" );
				return;
			}

			ReadLocationManagers( locationManagers, uri, reader );
			ReadLoaders( manager, uri, reader );
		}

		#region Private stuff
		
		/// <summary>
		/// Reads all location managers from an XML file
		/// </summary>
		/// <param name="managers">Location managers to store results</param>
		/// <param name="uri">XML uri</param>
		/// <param name="reader">XML reader</param>
		private static void ReadLocationManagers( LocationManagers managers, string uri, XmlReader reader )
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
						managers.Add( ReadLocationManager( reader ) );
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
			reader.Skip( );
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
