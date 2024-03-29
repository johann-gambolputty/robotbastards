using System;
using System.Xml;
using System.Collections;
using Tao.OpenGl;
using Rb.ColladaLoader;

namespace Rb.Rendering.OpenGl.ColladaSectionLoaders
{
	/// <summary>
	/// Object that loads COLLADA meshes
	/// </summary>
	/// <remarks>
	/// This is NOT a ResourceStreamLoader class. The full COLLADA format contains a lot of data that is not relevant to rendering (physics, 
	/// animation, etc.). The full COLLADA format ResourceStreamLoader is Rb.ColladaLoader.Loader, and it uses factory methods, like
	/// RenderFactory.CreateComposite(), to create API-specific loaders 
	/// </remarks>
	public class MeshSectionLoader : SectionLoader
	{
		/// <summary>
		/// Returns the section that this section loader handles
		/// </summary>
		public override Section Section
		{
			get
			{
				return Section.Mesh;
			}
		}

		/// <summary>
		/// A source of data about the mesh
		/// </summary>
		public class MeshSource
		{
			/// <summary>
			/// Constructor. Parses the collada XML to create the source
			/// </summary>
			public MeshSource( XmlReader reader )
			{
				if ( reader.IsEmptyElement )
				{
					throw new ApplicationException( "Mesh source element was empty" );
				}
				while ( reader.Read( ) && ( reader.NodeType != XmlNodeType.EndElement ) )
				{
					if ( reader.NodeType == XmlNodeType.Element )
					{
						if ( reader.Name == "float_array" )
						{
							m_Data = new ArrayList( );
							m_Data.Capacity = int.Parse( reader.GetAttribute( "count" ) );

							reader.Read( );	//	Read to text data
							string[] dataElements = reader.Value.Split( new char[] { ' ' } );

							for ( int elementIndex = 0; elementIndex < dataElements.Length; ++elementIndex )
							{
								m_Data.Add( float.Parse( dataElements[ elementIndex ] ) );
							}

							reader.Read( );	// Read to end element
						}
						else if ( reader.Name == "technique_common" )
						{
							ReadTechniqueCommon( reader );
						}
						else
						{
							//	ReadPastElement( reader );
							throw new ApplicationException( string.Format( "Unknown element \"{0}\" encountered in COLLADA mesh source", reader.Name ) );
						}
					}
				}
			}

			/// <summary>
			/// Reads a "technique_common" element
			/// </summary>
			/// <param name="reader"></param>
			private void ReadTechniqueCommon( XmlReader reader )
			{
				if ( reader.IsEmptyElement )
				{
					return;
				}
				while ( reader.Read( ) && ( reader.NodeType != XmlNodeType.EndElement ) )
				{
					if ( reader.NodeType == XmlNodeType.Element )
					{
						if ( reader.Name == "accessor" )
						{
							//	We're just here for the stride...
							m_Stride = int.Parse( reader.GetAttribute( "stride" ) );
							ReadPastElement( reader );
						}
						else
						{
							throw new ApplicationException( string.Format( "Unknown element \"{0}\" encountered in COLLADA mesh source common technique", reader.Name ) );
						}
					}
				}
			}

			/// <summary>
			/// Data stride (distance between elements in the data array)
			/// </summary>
			public int Stride
			{
				get
				{
					return m_Stride;

				}
			}

			/// <summary>
			/// The source data
			/// </summary>
			public ArrayList Data
			{
				get
				{
					return m_Data;
				}
			}

			/// <summary>
			/// Gets the type of data stored in the Data array
			/// </summary>
			public Type DataType
			{
				get
				{
					return m_Data[ 0 ].GetType( );
				}
			}

			/// <summary>
			/// Source data array
			/// </summary>
			private ArrayList m_Data;

			/// <summary>
			/// Element size
			/// </summary>
			private int m_Stride;
		}

		/// <summary>
		/// Tags a MeshSource with a semantic
		/// </summary>
		public class MeshTaggedSource
		{
			/// <summary>
			/// Mesh source
			/// </summary>
			public MeshSource Source
			{
				get
				{
					return m_Source;
				}
			}

			/// <summary>
			/// Mesh source semantic name
			/// </summary>
			public string Semantic
			{
				get
				{
					return m_Semantic;
				}
			}

			/// <summary>
			/// Setup constructor
			/// </summary>
			public MeshTaggedSource( MeshSource source, string semantic )
			{
				m_Semantic = semantic;
				m_Source = source;
			}

			private MeshSource m_Source;
			private string m_Semantic;
		}

		/// <summary>
		/// Mesh vertex information. Stores a set of MeshTaggedSource objects that represent named vertex data streams
		/// </summary>
		public class MeshVertices
		{
			/// <summary>
			/// Constructor. Parses the collada XML to create the vertex data
			/// </summary>
			public MeshVertices( XmlReader reader, Hashtable sourceTable )
			{
				while ( reader.Read( ) && ( reader.NodeType != XmlNodeType.EndElement ) )
				{
					if ( reader.NodeType == XmlNodeType.Element )
					{
						if ( reader.Name == "input" )
						{
							string semantic = reader.GetAttribute( "semantic" );
							string sourceName = reader.GetAttribute( "source" ).Substring( 1 );

							MeshSource source = ( MeshSource )sourceTable[ sourceName ];

							m_Sources.Add( new MeshTaggedSource( source, semantic ) );
						}
						else
						{
							ReadPastElement( reader );
						}
					}
				}
			}

			/// <summary>
			/// A list of MeshTaggedSource objects representing the named vertex data streams
			/// </summary>
			public ArrayList Sources
			{
				get
				{
					return m_Sources;
				}
			}

			private ArrayList m_Sources = new ArrayList( );
		}

		/// <summary>
		/// Mesh triange information
		/// </summary>
		public class MeshPolygon
		{
			/// <summary>
			/// Polygon indices
			/// </summary>
			public int[] Indices
			{
				get
				{
					return m_Indices;
				}
			}

			/// <summary>
			/// Setup constructor
			/// </summary>
			public MeshPolygon( int[] indices )
			{
				m_Indices = indices;
			}

			private int[] m_Indices;
		}

		/// <summary>
		/// Mesh polygon list information
		/// </summary>
		public class MeshPolygons
		{
			/// <summary>
			/// Constructor. Parses the collada XML to create the vertex data
			/// </summary>
			public MeshPolygons( XmlReader reader, bool triangles, bool convertToTriangles )
			{
				m_StoredAsTriangles = triangles || convertToTriangles;
				if ( triangles )
				{
					LoadTriangles( reader );
				}
				else
				{
					LoadPolygons( reader, convertToTriangles );
				}
			}

			/// <summary>
			/// true if polygons stored in the Polygons list are all triangles
			/// </summary>
			public bool StoredAsTriangles
			{
				get
				{
					return m_StoredAsTriangles;
				}
			}

			/// <summary>
			/// Polygon list
			/// </summary>
			public ArrayList Polygons
			{
				get
				{
					return m_Polygons;
				}
			}

			/// <summary>
			/// Vertex data offset
			/// </summary>
			//	TODO: Bodge (should work as semantics, again)
			public int VertexIndexOffset
			{
				get
				{
					return m_VertexIndexOffset;
				}
			}

			/// <summary>
			/// Index data stride
			/// </summary>
			public int IndexStride
			{
				get
				{
					return m_IndexStride;
				}
			}

			private bool m_StoredAsTriangles;
			private ArrayList m_Polygons = new ArrayList( );
			private int m_VertexIndexOffset;	//	TODO: Bodge (should work as semantics, again)
			private int m_IndexStride;

			/// <summary>
			/// Loads a triangle array
			/// </summary>
			private void LoadTriangles( XmlReader reader )
			{
				m_Polygons.Capacity = int.Parse( reader.GetAttribute( "count" ) );

				while ( reader.Read( ) && ( reader.NodeType != XmlNodeType.EndElement ) )
				{
					if ( reader.NodeType != XmlNodeType.Element )
					{
						continue;
					}
					if ( reader.Name == "input" )
					{
						//	TODO: This just assumes that the vertex data for the mesh is used as input...
						if ( reader.GetAttribute( "semantic" ) == "VERTEX" )
						{
							m_VertexIndexOffset = int.Parse( reader.GetAttribute( "offset" ) );
						}
						++m_IndexStride;
						ReadPastElement( reader );
					}
					else if ( reader.Name == "p" )
					{
						reader.Read( );
						string[] indexStrings = reader.Value.Split( new char[] { ' ' } );

						int[] indexes = new int[ 3 * IndexStride ];
						for ( int stringIndex = 0; stringIndex < indexStrings.Length; )
						{
							//	TODO: Bodge
							for ( int curIndex = 0; curIndex < 3 * IndexStride; ++curIndex )
							{
								indexes[ curIndex ] = int.Parse( indexStrings[ stringIndex++ ] );
							}

							m_Polygons.Add( new MeshPolygon( new int[ 3 ] { indexes[ VertexIndexOffset ], indexes[ VertexIndexOffset + IndexStride ], indexes[ VertexIndexOffset + IndexStride * 2 ] } ) );
						}
						reader.Read( );
					}
				}
			}

			/// <summary>
			/// Loads a polygon array
			/// </summary>
			private void LoadPolygons( XmlReader reader, bool convertToTriangles )
			{
				throw new ApplicationException( string.Format( "<{0}> COLLADA element is not yet handled, sorry", reader.Name ) );
			}
		}

		/// <summary>
		/// Loads a COLLADA section
		/// </summary>
		/// <param name="reader">XML reader</param>
		public override object LoadSection( XmlReader reader )
		{
			Hashtable sources = new Hashtable( );
			MeshVertices vertices = null;
			MeshPolygons polygons = null;

			while ( reader.Read( ) && reader.NodeType != XmlNodeType.EndElement )
			{
				if ( reader.NodeType != XmlNodeType.Element )
				{
					continue;
				}

				switch ( reader.Name )
				{
					case "source":
						{
							sources[ reader.GetAttribute( "id" ) ] = new MeshSource( reader );
							break;
						}

					case "vertices":
						{
							vertices = new MeshVertices( reader, sources );
							break;
						}

					case "polygons":
						{
							polygons = new MeshPolygons( reader, false, false );
							break;
						}

					case "triangles":
						{
							polygons = new MeshPolygons( reader, true, false );
							break;
						}

					default:
						{
							ReadPastElement( reader );
							break;
						}
				}

			}

			return BuildMesh( vertices, polygons );
		}

		/// <summary>
		/// Builds an OpenGL mesh
		/// </summary>
		private object BuildMesh( MeshVertices vertices, MeshPolygons polygons )
		{
			OpenGlMesh mesh = new OpenGlMesh( );

			//	Set up the index buffer in the mesh
			if ( polygons.StoredAsTriangles )
			{
				int[] indexBuffer = new int[ polygons.Polygons.Count * 3 ];

				int curIndex = 0;
				foreach ( MeshPolygon curPoly in polygons.Polygons )
				{
					indexBuffer[ curIndex++ ] = curPoly.Indices[ 0 ];
					indexBuffer[ curIndex++ ] = curPoly.Indices[ 1 ];
					indexBuffer[ curIndex++ ] = curPoly.Indices[ 2 ];
				}

				mesh.CreateGroups( 1 );
				mesh.SetGroup( 0, new OpenGlIndexedGroup( Gl.GL_TRIANGLES, indexBuffer ) );
			}
			else
			{
				throw new ApplicationException( "sorry, non-triange index buffers not implemented yet" );
			}

			mesh.CreateVertexBuffers( vertices.Sources.Count );
			int vboIndex = 0;

			foreach ( MeshTaggedSource curSource in vertices.Sources )
			{
				//	Convert the source semantic to an opengl client state
				//	Swap y and z elements
				//	TODO: Should be a resource flag, at the very least
				bool swapYz = false;
				int clientState = 0;
				switch ( curSource.Semantic )
				{
					case "POSITION": clientState = Gl.GL_VERTEX_ARRAY; swapYz = true; break;
					case "NORMAL": clientState = Gl.GL_NORMAL_ARRAY; swapYz = true; break;
					case "COLOR": clientState = Gl.GL_COLOR_ARRAY; break;
					default:
						throw new ApplicationException( string.Format( "Unknown mesh semantic \"{0}\"", curSource.Semantic ) );
				}

				//	Store the stride (note, for GL this has a slightly different meaning, I think. TODO: Check COLLADA spec. to make sure)
				int numObjects = curSource.Source.Data.Count / curSource.Source.Stride;
				short stride = 0;
				short numElements = ( short )curSource.Source.Stride;

				Type dataType = curSource.Source.DataType;
				switch ( Type.GetTypeCode( dataType ) )
				{
					case TypeCode.Single:
						{
							//	HACK: Swap y and z coordinates - in blender (the 3d editor I'm currently using), z is up, in the engine, y is up.
							float[] bufferData = ( float[] )curSource.Source.Data.ToArray( dataType );
							if ( swapYz )
							{
								for ( int index = 0; index < bufferData.Length; index += 3 )
								{
									float tmp = bufferData[ index + 1 ];
									bufferData[ index + 1 ] = bufferData[ index + 2 ];
									bufferData[ index + 2 ] = tmp;
								}
							}
							mesh.SetVertexBuffer( vboIndex, new OpenGlVertexBuffer( numObjects, 0, clientState, stride, numElements, Gl.GL_STATIC_DRAW_ARB, bufferData ) );
							break;
						}

					case TypeCode.Int32:
						{
							int[] bufferData = ( int[] )curSource.Source.Data.ToArray( dataType );
							mesh.SetVertexBuffer( vboIndex, new OpenGlVertexBuffer( numObjects, 0, clientState, stride, numElements, Gl.GL_STATIC_DRAW_ARB, bufferData ) );
							break;
						}

					case TypeCode.Byte:
						{
							byte[] bufferData = ( byte[] )curSource.Source.Data.ToArray( dataType );
							mesh.SetVertexBuffer( vboIndex, new OpenGlVertexBuffer( numObjects, 0, clientState, stride, numElements, Gl.GL_STATIC_DRAW_ARB, bufferData ) );
							break;
						}

					default:
						{
							throw new ApplicationException( string.Format( "Unhandled vertex source type \"{0}\"", curSource.Source.DataType.Name ) );
						}
				}

				++vboIndex;
			}

			return mesh;
		}
	}
}
