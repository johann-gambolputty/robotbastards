using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Resource management
	/// </summary>
	public class ResourceManager
    {
        #region Singleton

        /// <summary>
        /// Resource manager singleton
        /// </summary>
		static public ResourceManager Instance
		{
			get { return ms_Singleton; }
        }

        #endregion

        #region Caching

        /// <summary>
        /// Clears out the contents of the resource caches
        /// </summary>
        public void ClearResourceCaches( )
        {
            foreach ( ResourceLoader loader in m_StreamLoaders )
            {
                loader.Cache.Clear( );
            }

            foreach ( ResourceLoader loader in m_DirectoryLoaders )
            {
                loader.Cache.Clear( );
            }
        }

        #endregion

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
		public void AddLoader( ResourceStreamLoader loader )
		{
			m_StreamLoaders.Add( loader );
		}

		/// <summary>
		/// Adds a loader that can read resource directory
		/// </summary>
		/// <param name="loader"> Loader to add </param>
		public void AddLoader( ResourceDirectoryLoader loader )
		{
			m_DirectoryLoaders.Add( loader );
		}

		/// <summary>
		/// Sets up the manager from an XML file
		/// </summary>
		/// <param name="xmlSetupFilePath">Setup file path</param>
		public void Setup( string xmlSetupFilePath )
		{
			System.Xml.XmlDocument doc = new System.Xml.XmlDocument( );
			doc.Load( xmlSetupFilePath );

			Setup( ( System.Xml.XmlElement )doc.SelectSingleNode( "/resourceManager" ) );
		}

		/// <summary>
		/// Sets up the manager from an XML definition
		/// </summary>
		/// <param name="element"> Element that created this manager </param>
		public void Setup( XmlElement element )
		{
			//	Load providers
			XmlNodeList providerNodes = element.SelectNodes( "providers/provider" );
			foreach ( XmlNode curProviderNode in providerNodes )
			{
				if ( curProviderNode is XmlElement )
                {
                    ResourceProvider provider = CreateInstance< ResourceProvider >( curProviderNode, "resource provider" );
                    if ( provider != null )
                    {
                        provider.Setup( ( XmlElement )curProviderNode );
                        AddProvider( provider );
                    }
				}
			}

			//	Load loaders
			foreach ( XmlNode curLoaderNode in element.SelectNodes( "loaders/loader" ) )
			{
				if ( curLoaderNode is XmlElement )
                {
                    ResourceLoader loader = CreateInstance < ResourceLoader >( curLoaderNode, "resource loader" );
                    if ( loader != null )
					{
						AddLoader( loader );
					}
				}
			}
		}

		#endregion

		#region	Loading

        /// <summary>
        /// Finds a ResourceProvider that can open a named stream
        /// </summary>
        /// <param name="path">Stream path</param>
        /// <returns>Returns a resource provider that can load path</returns>
        /// <exception cref="ApplicationException">Thrown if no provider can be found that supports the specified path</exception>
        public System.IO.Stream OpenStream( ref string path )
        {
			//	path was just a directory - see if there's a provider that recognises it
			foreach ( ResourceProvider provider in m_Providers )
			{
                System.IO.Stream stream = provider.Open( ref path );
                if ( stream != null )
                {
					return stream;
                }
            }
			throw new ApplicationException( string.Format( "Could not find provider that could open resource stream \"{0}\"", path ) );
		}

        /// <summary>
        /// Returns a ResourceProvider that can create a stream from a given path
        /// </summary>
        /// <param name="path">Stream path</param>
        /// <returns>Returns a resource provider that can load path</returns>
        /// <exception cref="ApplicationException">Thrown if no provider can be found that supports the specified path</exception>
        public ResourceProvider FindDirectoryProvider( ref string path )
        {
			//	path was just a directory - see if there's a provider that recognises it
			foreach ( ResourceProvider provider in m_Providers )
			{
				if ( provider.DirectoryExists( ref path ) )
				{
				    return provider;
				}
            }
			throw new ApplicationException( string.Format( "Could not find provider that could open resource directory \"{0}\"", path ) );
		}

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
			return Load( path, null );
		}

		/// <summary>
		/// Finds a provider that can load a stream from a path, then finds a loader that can read the stream and read it into a resource
		/// </summary>
		/// <param name="path"> Resource path </param>
		/// <returns>Returns resource</returns>
        /// <param name="parameters">Load parameters</param>
		/// <remarks>
		/// Throws a System.ApplicationException if the path could not be opened by any provider, or if there were no loaders that could read the opened stream
		/// </remarks>
		public Object Load( string path, LoadParameters parameters )
		{
			if ( System.IO.Path.GetExtension( path ) == string.Empty )
			{
                ResourceProvider provider = FindDirectoryProvider( ref path );

				foreach ( ResourceDirectoryLoader loader in m_DirectoryLoaders )
                {
                    if ( !loader.CanLoadDirectory( provider, path ) )
                    {
                        continue;
                    }

				    ResourcesLog.Info( "Loading \"{0}\"", path );

                    //	TODO: AP: Needs to handle Load when resource is not null
                    object result = loader.Cache.Find( path );
                    if ( result != null )
                    {
                        ResourcesLog.Info( "Retrieved \"{0}\" from directory resource cache", path );
                        return result;
                    }

                    bool canCache;
                    if ( parameters == null )
                    {
                        result = loader.Load( provider, path, out canCache );
                    }
                    else
                    {
                        result = loader.Load( provider, path, out canCache, parameters );
                    }

                    //  Cache the result
                    if ( canCache && ( result != null ) )
                    {
                        ResourcesLog.Verbose( "Adding \"{0}\" to resource cache", path );
                        loader.Cache.Add( path, result );
                    }
                    return result;
                }

				throw new ApplicationException( string.Format( "Could not find a loader that could load directory resource \"{0}\"", path ) );
			}

            System.IO.Stream input = OpenStream( ref path );
            
			foreach( ResourceStreamLoader loader in m_StreamLoaders )
			{
				if ( !loader.CanLoadStream( path, input ) )
				{
				    continue;
				}
                
			    ResourcesLog.Info( "Loading \"{0}\"", path );

                object result = loader.Cache.Find( path );
                if ( result != null )
                {
                    ResourcesLog.Info( "Retrieved \"{0}\" from stream resource cache", path );
                    return result;
                }

                bool canCache;
                if ( parameters == null )
                {
                    result = loader.Load( input, path, out canCache );
                }
                else
                {
                    result = loader.Load( input, path, out canCache, parameters );
                }

                if ( ( result != null ) && ( canCache ) )
                {
                    ResourcesLog.Verbose( "Adding \"{0}\" to resource cache", path );
                    loader.Cache.Add( path, result );
                }
                return result;
            }

			throw new ApplicationException( string.Format( "Could not find loader that could open stream \"{0}\"", path ) );
		}

		#endregion


		#region	Private stuff

		private ArrayList               m_Providers = new ArrayList( );
		private ArrayList               m_StreamLoaders = new ArrayList( );
		private ArrayList               m_DirectoryLoaders = new ArrayList( );
        private static ResourceManager  ms_Singleton = new ResourceManager( );

		/// <summary>
		/// Adds a loader that can read resource streams
		/// </summary>
		/// <param name="loader"> Loader to add </param>
		private void AddLoader( ResourceLoader loader )
		{
			if ( loader is ResourceStreamLoader )
			{
				m_StreamLoaders.Add( loader );
			}
			else
			{
				m_DirectoryLoaders.Add( ( ResourceDirectoryLoader )loader );
			}
		}
        
        /// <summary>
        /// Creates an object instance from information stored in an XML element
        /// </summary>
        /// <param name="node">XML node. Attribute "type" specifies the type to instance, optional attribute "assembly" specified the assembly to load</param>
        /// <param name="context">Name of the context in which the instance is being created. Used for log output</param>
        /// <returns>New instance</returns>
        private static T CreateInstance< T >( XmlNode node, string context )
        {
            string          typeName            = node.Attributes[ "type" ].Value;
            XmlAttribute    assemblyAttribute   = node.Attributes[ "assembly" ];

            object result = null;
            if ( assemblyAttribute != null )
            {
                result = AppDomain.CurrentDomain.Load( assemblyAttribute.Value ).CreateInstance( typeName );
                if ( result == null )
                {
                    ResourcesLog.Error("Unable to create instance for {0} type \"{1}\" from assembly \"{2}\"", context, typeName, assemblyAttribute.Value);
                }
                else
                {
                    ResourcesLog.Info( "Successfully created {0} type \"{1}\" from assembly \"{2}\"", context, typeName, assemblyAttribute.Value );
                }
            }
            else
            {
                result = Utils.AppDomainUtils.CreateInstance( typeName );
                if ( result == null )
                {
                    ResourcesLog.Error( "Unable to create instance for {0} type \"{1}\" from current app domain", context, typeName );
                }
                else
                {
                    ResourcesLog.Info( "Successfully created {0} type \"{1}\" from current app domain", context, typeName );
                }
            }

            if ( !( result is T ) )
            {
                ResourcesLog.Error( "Created instance of {0} type \"{1}\" but could not cast it to \"{2}\"", context, typeName, typeof( T ) );
                return default( T );
            }

            return ( T )result;
        }

		#endregion

	}
}
