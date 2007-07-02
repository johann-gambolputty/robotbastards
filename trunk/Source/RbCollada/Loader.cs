using System;
using System.Collections;
using System.Xml;
using System.Reflection;
using Rb.Core.Components;

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
			ArrayList results = new ArrayList( );

            return LoadResource( input, inputSource, results );
		}

		/// <summary>
		/// Loads a COLLADA resource into an array
		/// </summary>
		/// <exception cref="ApplicationException">Thrown if resource is not an IList or IParentObject</exception>
		public override Object Load( System.IO.Stream input, string inputSource, RbEngine.Resources.LoadParameters parameters )
		{
			return LoadResource( input, inputSource, parameters == null ? new System.Collections.ArrayList( ) : parameters.Target );
		}

		/// <summary>
		/// Loads a COLLADA resource into an array
		/// </summary>
		/// <param name="input"></param>
		/// <param name="inputSource"></param>
		/// <param name="resource"></param>
		/// <returns></returns>
		public Object LoadResource( System.IO.Stream input, string inputSource, Object resource )
		{
			IList			listResults		= resource as IList;
			IParentObject	parentResults	= resource as IParentObject;
			if ( ( listResults == null ) && ( parentResults == null ) )
			{
				throw new ApplicationException( string.Format( "Failed to load \"{0}\": The COLLADA loader can only load into an IList or IParentObject", inputSource ) );
			}

			//	This is a bit of a cheat. We'll just load everything into an array, then at the end, if parentResults is not null, all the stored objects in the list
			//	are transferred to the parent object
			if ( listResults == null )
			{
				listResults = new ArrayList( );
			}

			XmlTextReader reader = new XmlTextReader( input );
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
							ParseCOLLADA( reader, listResults );
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

			//	Transfer list to parent
			if ( parentResults != null )
			{
				foreach ( Object obj in listResults )
				{
					parentResults.AddChild( obj );
				}
			}

			return resource;
		}


		/// <summary>
		/// Parses the root section ("COLLADA") of a collada file
		/// </summary>
		private void ParseCOLLADA( XmlReader reader, IList results )
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
		private void ParseLibraryGeometries( XmlReader reader, IList results )
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
		private void ParseGeometry( XmlReader reader, IList results )
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
								if ( newMesh is INamedObject )
								{
									( ( INamedObject )newMesh ).Name = geometryName;
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
