using System;
using System.Collections;
using System.Xml;
using System.Reflection;

namespace RbCollada
{

	//	TODO: I haven't come up with a very good solution to returning the results of multiple sections (they're added to an array, which is
	//	returned as the Load() result. In RbXmlLoader, list results are caught and list elements are added as seperate resources). But that
	//	means all resources are added to a single ModelSet (which isn't the end of the world, but still...)
	//	I guess the best solution is to not use COLLADA files in builds where this could be important. Bah.

	/// <summary>
	/// ResourceStreamLoader that loads a COLLADA file
	/// </summary>
	public class Loader : RbEngine.Resources.ResourceStreamLoader
	{
		/// <summary>
		/// COLLADA loader
		/// </summary>
		public Loader( )
		{
			//	Run through all existing types. Store the ones that implement the ISectionLoader interface
			foreach ( Assembly curAssembly in AppDomain.CurrentDomain.GetAssemblies( ) )
			{
				FindColladaLoadersInAssembly( curAssembly );
			}
			AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler( OnAssemblyLoad );
		}


		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <returns> The loaded resource </returns>
		public override Object Load( System.IO.Stream input, string inputSource )
		{
			XmlTextReader reader = new XmlTextReader( input );
			ArrayList results = new ArrayList( );

			try
			{
				while ( reader.Read( ) )
				{
					if ( reader.NodeType == XmlNodeType.Element )
					{
						if ( reader.Name != "COLLADA" )
						{
							throw new ApplicationException( "Expected root element to be \"COLLADA\"" );
						}
						else
						{
							ParseCOLLADA( reader, results );
						}
					}
				}
			}
			catch ( XmlException e )
			{
				string msg = string.Format( "Invalid COLLADA XML file \"{0}\"\n{0}({1},{2}): {3}", inputSource, e.LineNumber, e.LinePosition, e.Message );
				throw new ApplicationException( msg, e );																			
			}
			catch ( Exception e )
			{
				string msg = string.Format( "Failed to load COLLADA XML file \"{0}\"\n{0}({1},{2}): {3}", inputSource, reader.LineNumber, reader.LinePosition, e.Message );
				throw new ApplicationException( msg, e );
			}

			return results;
		}

		/// <summary>
		/// Parses the root section ("COLLADA") of a collada file
		/// </summary>
		private void ParseCOLLADA( XmlReader reader, ArrayList results )
		{
			while ( reader.Read( ) )
			{
				if ( reader.NodeType == XmlNodeType.Element )
				{
					if ( reader.Name == "library_geometries" )
					{
						ParseLibraryGeometries( reader, results );
					}
					else
					{
						SectionLoader.ReadPastElement( reader );
					}
				}
			}
		}

		/// <summary>
		/// Parses the "library_geometries" section
		/// </summary>
		private void ParseLibraryGeometries( XmlReader reader, ArrayList results )
		{
			if ( !reader.IsEmptyElement )
			{
				while ( reader.Read( ) && ( reader.NodeType != XmlNodeType.EndElement ) )
				{
					if ( reader.NodeType == XmlNodeType.Element )
					{
						if ( reader.Name == "geometry" )
						{
							ParseGeometry( reader, results );
						}
						else
						{
							SectionLoader.ReadPastElement( reader );
						}
					}
				}
			}
		}

		/// <summary>
		/// Parses the "geometry" section
		/// </summary>
		private void ParseGeometry( XmlReader reader, ArrayList results )
		{
			string geometryName = reader.GetAttribute( "name" );
			if ( !reader.IsEmptyElement )
			{
				while ( reader.Read( ) && ( reader.NodeType != XmlNodeType.EndElement ) )
				{
					if ( reader.NodeType == XmlNodeType.Element )
					{
						if ( reader.Name == "mesh" )
						{
							SectionLoader loader = m_Loaders[ ( int )Section.Mesh ];
							if ( loader != null )
							{
								//	Load the mesh
								object newMesh = loader.LoadSection( reader );

								//	Add it to the results array
								results.Add( newMesh );

								//	Name the mesh
								if ( newMesh is RbEngine.Components.INamedObject )
								{
									( ( RbEngine.Components.INamedObject )newMesh ).Name = geometryName;
								}
							}
						}
						else
						{
							SectionLoader.ReadPastElement( reader );
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns true if this loader can load the specified stream
		/// </summary>
		/// <param name="path"> Stream path (contains extension that the loader can check)</param>
		/// <param name="input"> Input stream (file types can be identified by peeking at header bytes) </param>
		/// <returns> Returns true if the stream can </returns>
		/// <remarks>
		/// path can be null, in which case, the loader must be able to identify the resource type by checking the content in input (e.g. by peeking
		/// at the header bytes).
		/// </remarks>
		public override bool CanLoadStream( string path, System.IO.Stream input )
		{
			return path.EndsWith( ".dae" );
		}

		private SectionLoader[]	m_Loaders = new SectionLoader[ ( int )Section.NumSections ];

		/// <summary>
		/// Adds a new ISectionLoader object to m_Loaders
		/// </summary>
		private void AddLoader( SectionLoader loader )
		{
			if ( m_Loaders[ ( int )loader.Section ] != null )
			{
				throw new ApplicationException( string.Format( "Two collada loaders (\"{0}\" and \"{1}\") tried to provide implementation for the same section", loader.GetType( ).Name, m_Loaders[ ( int )loader.Section ].GetType( ).Name ) );
			}

			RbEngine.Output.WriteLineCall( RbEngine.Output.ResourceInfo, "Adding COLLADA section loader type \"{0}\"", loader.GetType( ).Name );
			m_Loaders[ ( int )loader.Section ] = loader;
		}

		/// <summary>
		/// Finds and instances all ISectionLoader implementing classes in an assembly
		/// </summary>
		private void FindColladaLoadersInAssembly( Assembly assembly )
		{
			foreach ( Type curType in assembly.GetTypes( ) )
			{
				if ( curType.IsSubclassOf( typeof( SectionLoader ) ) )
				{
					//	Create and add an instance of the loader type
					AddLoader( ( SectionLoader )Activator.CreateInstance( curType ) );
				}
			}
		}

		private void OnAssemblyLoad( object sender, AssemblyLoadEventArgs args )
		{
			FindColladaLoadersInAssembly( args.LoadedAssembly );
		}
	}
}
