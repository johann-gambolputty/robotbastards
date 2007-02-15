using System;
using System.Collections;
using System.Diagnostics;

namespace RbEngine.Resources
{
	/// <summary>
	/// Summary description for ResourceManager.
	/// </summary>
	public class ResourceManager
	{

		static public ResourceManager Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		static ResourceManager ms_Singleton = new ResourceManager( );

		#region	Setup

		/// <summary>
		/// Adds a provider that can open streams for resources
		/// </summary>
		/// <param name="provider"> Provider to add </param>
		public void	AddProvider( ResourceProvider provider )
		{
			m_Providers.Add( provider );
		}

		/// <summary>
		/// Adds a loader that can read resource streams
		/// </summary>
		/// <param name="loader"> Loader to add </param>
		public void AddLoader( ResourceLoader loader )
		{
			m_Loaders.Add( loader );
		}

		/// <summary>
		/// Sets up the manager from an XML definition
		/// </summary>
		/// <param name="element"> Element that created this manager </param>
		public void Setup( System.Xml.XmlElement element )
		{
			//	Load providers
			System.Xml.XmlNodeList providerNodes = element.SelectNodes( "providers/provider" );
			foreach ( System.Xml.XmlNode curProviderNode in providerNodes )
			{
				if ( curProviderNode is System.Xml.XmlElement )
				{
					string providerTypeName = curProviderNode.Attributes[ "type" ].Value;
					Type providerType = GetType( ).Assembly.GetType( providerTypeName );
					if ( providerType == null )
					{
						Trace.WriteLineIf( Switches.Errors.Enabled, String.Format( "Unable to find type for provider \"{0}\"", providerTypeName ) );
					}
					else
					{
						Debug.WriteLineIf( Switches.Info.Enabled, String.Format( "Creating resource provider \"{0}\"", providerTypeName ) );
						ResourceProvider newProvider = ( ResourceProvider )System.Activator.CreateInstance( providerType );

						newProvider.Setup( ( System.Xml.XmlElement ) curProviderNode );

                        AddProvider( newProvider );
					}
				}
			}

			//	Load loaders
			foreach ( System.Xml.XmlNode curLoaderNode in element.SelectNodes( "loaders/loader" ) )
			{
				if ( curLoaderNode is System.Xml.XmlElement )
				{
					string loaderTypeName = curLoaderNode.Attributes[ "type" ].Value;
					ResourceLoader loader = ( ResourceLoader )GetType( ).Assembly.CreateInstance( loaderTypeName );
					if ( loader == null )
					{
						Trace.WriteLineIf( Switches.Errors.Enabled, String.Format( "Unable to create instance for resource loader type \"{0}\"", loaderTypeName ) );
					}
					else
					{
						Debug.WriteLineIf( Switches.Info.Enabled, String.Format( "Creating resource loader \"{0}\"", loaderTypeName ) );
						AddLoader( loader );
					}
				}
			}
		}

		#endregion

		/// <summary>
		/// Finds a provider that can load a stream from a path, then finds a loader that can read the stream and turn it into a resource
		/// </summary>
		/// <param name="path"> Resource path </param>
		/// <returns> The loaded resource </returns>
		/// <remarks>
		/// Throws a System.ApplicationException if the path could not be opened by any provider, or if there were no loaders that could read the opened stream
		/// </remarks>
		public Object Load( string path )
		{
			foreach ( ResourceProvider provider in m_Providers )
			{
				System.IO.Stream input = provider.Open( ref path );
				if ( input != null )
				{
					foreach( ResourceLoader loader in m_Loaders )
					{
						if ( loader.CanLoadStream( path, input ) )
						{
							return loader.Load( input, path );
						}
					}
					throw new System.ApplicationException( String.Format( "Could not find loader that could open stream {0}", path ) );
				}
			}

			throw new System.ApplicationException( String.Format( "Could not open resource stream {0}", path ) );
		}

		private ArrayList m_Providers = new ArrayList( );
		private ArrayList m_Loaders = new ArrayList( );

	}
}
