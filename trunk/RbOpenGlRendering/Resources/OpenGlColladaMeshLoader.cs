using System;
using System.Xml;
using System.Collections;
using Tao.OpenGl;

namespace RbOpenGlRendering.Resources
{
	//	TODO: This should probably be made generic - just load the COLLADA mesh structure into memory, then have a factory
	//	method that can convert that data into an API-specific mesh. That way, for physics mesh loaders, etc. this mesh
	//	loading code doesn't have to be copied and pasted.
	//	Saves RbCollada.Loader from scanning assemblies, also.
	//	Still needs a way to specify the particular API conversion in the resource loader (e.g. load COLLADA resource "crate.dae"
	//	using the physics mesh converter, or the OpenGL mesh converter, or whatever.
	//	That would be really nice in another way, because then the visible representation of, say, the physics mesh, can be
	//	created alongside it from the same data.
	//	Maybe something like this?
	//
	//	<modelSet name="root">
	//		<modelSet name="temporary">
	//			<resource path="crate.dae" discard="postLoad"> // Creates an in-memory collada object, discarded after main form is loaded
	//				<swapYZ/> //< swaps the y and z axis, because blender and other 3d apps have z as up (crazy)
	//			</resource>
	//		</modelSet>
	//		<modelSet name="graphics">
	//			<instance name="root/temporary/crate.dae" type="RenderableMesh"/> //< ...
	//		</modelSet>
	//		<modelSet name="physics">
	//			<instance name="root/temporary/crate.dae" type="PhysicsMesh"/> //< ...
	//		<modelSet/>
	//	</modelSet>
	//
	//	This has probably got a good solution along with the issue of invoking factory methods from XML
	//

	//	TODO: COLLADA provides a more generic way of storing and referencing data (it's not mesh specific). Maybe leverage that.
	//	(e.g. can MeshSource be a more generic DataSource object?)

	/// <summary>
	/// Object that loads COLLADA meshes
	/// </summary>
	/// <remarks>
	/// This is NOT a ResourceLoader class. The full COLLADA format contains a lot of data that is not relevant to rendering (physics, 
	/// animation, etc.). The full COLLADA format ResourceLoader is RbCollada.ColladaLoader, and it uses factory methods, like
	/// RenderFactory.CreateComposite(), to create API-specific loaders 
	/// </remarks>
	public class OpenGlColladaMeshLoader : RbCollada.SectionLoader
	{
		/// <summary>
		/// Returns the section that this section loader handles
		/// </summary>
		public override RbCollada.Section	Section
		{
			get
			{
				return RbCollada.Section.Mesh;
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
			private void		ReadTechniqueCommon( XmlReader reader )
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
			public int			Stride
			{
				get
				{
					return m_Stride;

				}
			}

			/// <summary>
			/// The source data
			/// </summary>
			public ArrayList	Data
			{
				get
				{
					return m_Data;
				}
			}

			/// <summary>
			/// Gets the type of data stored in the Data array
			/// </summary>
			public Type			DataType
			{
				get
				{
					return m_Data[ 0 ].GetType( );
				}
			}

			/// <summary>
			/// Source data array
			/// </summary>
			private ArrayList	m_Data;

			/// <summary>
			/// Element size
			/// </summary>
			private int			m_Stride;
		}

		/// <summary>
		/// Tags a MeshSource with a semantic
		/// </summary>
		public class MeshTaggedSource
		{
			/// <summary>
			/// Mesh source
			/// </summary>
			public MeshSource	Source
			{
				get
				{
					return m_Source;
				}
			}

			/// <summary>
			/// Mesh source semantic name
			/// </summary>
			public string		Semantic
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
				m_Semantic	= semantic;
				m_Source	= source;
			}

			private MeshSource	m_Source;
			private string		m_Semantic;
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
							string semantic		= reader.GetAttribute( "semantic" );
							string sourceName	= reader.GetAttribute( "source" ).Substring( 1 );

							MeshSource	source	= ( MeshSource )sourceTable[ sourceName ];

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
			public ArrayList	Sources
			{
				get
				{
					return m_Sources;
				}
			}

			private ArrayList	m_Sources = new ArrayList( );
		}

		/// <summary>
		/// Mesh triange information
		/// </summary>
		public class MeshPolygon
		{
			/// <summary>
			/// Polygon indices
			/// </summary>
			public int[]	Indices
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

			private int[]	m_Indices;
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
			public bool			StoredAsTriangles
			{
				get
				{
					return m_StoredAsTriangles;
				}
			}

			/// <summary>
			/// Polygon list
			/// </summary>
			public ArrayList	Polygons
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
			public int			VertexIndexOffset
			{
				get
				{
					return m_VertexIndexOffset;
				}
			}

			/// <summary>
			/// Index data stride
			/// </summary>
			public int			IndexStride
			{
				get
				{
					return m_IndexStride;
				}
			}

			private bool		m_StoredAsTriangles;
			private ArrayList	m_Polygons = new ArrayList( );
			private int			m_VertexIndexOffset;	//	TODO: Bodge (should work as semantics, again)
			private int			m_IndexStride;

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
						string[] indexStrings = reader.Value.Split( new char[]{ ' ' } );

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
		public override object			LoadSection( XmlReader reader )
		{
			Hashtable		sources		= new Hashtable( );
			MeshVertices	vertices	= null;
			MeshPolygons	polygons	= null;

			while ( reader.Read( ) && reader.NodeType != XmlNodeType.EndElement )
			{
				if ( reader.NodeType != XmlNodeType.Element )
				{
					continue;
				}

				switch ( reader.Name )
				{
					case "source" :
					{
						sources[ reader.GetAttribute( "id" ) ] = new MeshSource( reader );
						break;
					}

					case "vertices" :
					{
						vertices = new MeshVertices( reader, sources );
						break;
					}

					case "polygons" :
					{
						polygons = new MeshPolygons( reader, false, false );
						break;
					}
						
					case "triangles" :
					{
						polygons = new MeshPolygons( reader, true, false );
						break;
					}

					default		:
					{
						ReadPastElement( reader );
						break;
					}
				}

			}

			return BuildMesh( vertices, polygons );
		}

		/*
		/// <summary>
		/// COLLADA OpenGL mesh test class
		/// </summary>
		private class TestMesh : RbEngine.Rendering.IRender
		{

			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="vertices">COLLADA vertex information</param>
			/// <param name="polygons">COLLADA polygon information</param>
			public TestMesh( MeshVertices vertices, MeshPolygons polygons )
			{
				BuildVertexBufferObjects( vertices );
				BuildIndexBuffer( polygons );
			}

			private void BuildIndexBuffer( MeshPolygons polygons )
			{
				if ( polygons.StoredAsTriangles )
				{
					m_IndexBuffer = new int[ polygons.Polygons.Count * 3 ];

					int curIndex = 0;
					foreach ( MeshPolygon curPoly in polygons.Polygons )
					{
						m_IndexBuffer[ curIndex++ ] = curPoly.Indices[ 0 ];
						m_IndexBuffer[ curIndex++ ] = curPoly.Indices[ 1 ];
						m_IndexBuffer[ curIndex++ ] = curPoly.Indices[ 2 ];
					}

					m_PrimitiveType = Gl.GL_TRIANGLES;
				}
				else
				{
					throw new ApplicationException( "sorry, non-triange index buffers not implemented yet" );
				}
			}

			private void BuildVertexBufferObjects( MeshVertices vertices )
			{
				m_VertexBufferObjects = new VertexBufferObject[ vertices.Sources.Count ];
				int vboIndex = 0;

				foreach ( MeshTaggedSource curSource in vertices.Sources )
				{
					VertexBufferObject curVbo;

					//	Convert the source semantic to an opengl client state
					//	Swap y and z elements
					//	TODO: Should be a resource flag, at the very least
					bool swapYz = false;
					switch ( curSource.Semantic )
					{
						case "POSITION" :	curVbo.ClientState = Gl.GL_VERTEX_ARRAY; swapYz = true;	break;
						case "NORMAL"	:	curVbo.ClientState = Gl.GL_NORMAL_ARRAY; swapYz = true;	break;
						case "COLOR"	:	curVbo.ClientState = Gl.GL_COLOR_ARRAY;					break;
						default			:
							throw new ApplicationException( string.Format( "Unknown mesh semantic \"{0}\"", curSource.Semantic ) );
					}

					int numObjects = curSource.Source.Data.Count / curSource.Source.Stride;

					//	Store the stride (note, for GL this has a slightly different meaning, I think. TODO: Check COLLADA spec. to make sure)
					curVbo.Stride		= 0;
					curVbo.NumElements	= curSource.Source.Stride;

					//	Generate a VBO
					Gl.glGenBuffersARB( 1, out curVbo.Handle );

					//	Bind and fill the VBO
					Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, curVbo.Handle );

					Type dataType = curSource.Source.DataType;
					switch ( Type.GetTypeCode( dataType ) )
					{
						case TypeCode.Single :
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

							curVbo.ElementType = Gl.GL_FLOAT;
							Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * curVbo.NumElements ) * numObjects, bufferData, Gl.GL_STATIC_DRAW_ARB );
							break;
						}

						case TypeCode.Int32 :
						{
							curVbo.ElementType = Gl.GL_INT;
							Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 4 * curVbo.NumElements ) * numObjects, ( int[] )curSource.Source.Data.ToArray( dataType ), Gl.GL_STATIC_DRAW_ARB );
							break;
						}

						case TypeCode.Byte :
						{
							curVbo.ElementType = Gl.GL_UNSIGNED_BYTE;
							Gl.glBufferDataARB( Gl.GL_ARRAY_BUFFER_ARB, ( 1 * curVbo.NumElements ) * numObjects, ( byte[] )curSource.Source.Data.ToArray( dataType ), Gl.GL_STATIC_DRAW_ARB );
							break;
						}

						default :
						{
							throw new ApplicationException( string.Format( "Unhandled vertex source type \"{0}\"", curSource.Source.DataType.Name ) );
						}
					}

					 m_VertexBufferObjects[ vboIndex++ ] = curVbo;
				}
			}

			private struct VertexBufferObject
			{
				public int		Handle;
				public int		ClientState;
				public int		Stride;
				public int		NumElements;
				public int		ElementType;
			}

			private VertexBufferObject[]	m_VertexBufferObjects;
			private int[]					m_IndexBuffer;
			private int						m_PrimitiveType;

			#region IRender Members

			/// <summary>
			/// Renders this mesh
			/// </summary>
			public void Render()
			{
				//	TODO: This is a bit rubbish - disables all client states, enables them as needed
				//	Can they be enabled, bound, then disabled?
				Gl.glDisableClientState( Gl.GL_VERTEX_ARRAY );
				Gl.glDisableClientState( Gl.GL_COLOR_ARRAY );
				Gl.glDisableClientState( Gl.GL_NORMAL_ARRAY );
				Gl.glDisableClientState( Gl.GL_TEXTURE_COORD_ARRAY );

				for ( int vboIndex = 0; vboIndex < m_VertexBufferObjects.Length; ++vboIndex )
				{
					VertexBufferObject curVbo = m_VertexBufferObjects[ vboIndex ];

					Gl.glBindBufferARB( Gl.GL_ARRAY_BUFFER_ARB, curVbo.Handle );
					Gl.glEnableClientState( curVbo.ClientState );

					float[] nullArray = null;
					switch ( curVbo.ClientState )
					{
						case Gl.GL_VERTEX_ARRAY :
							Gl.glVertexPointer( curVbo.NumElements, curVbo.ElementType, 0, nullArray );
							break;

						case Gl.GL_NORMAL_ARRAY :
							Gl.glNormalPointer( curVbo.ElementType, 0, nullArray );
							break;

						case Gl.GL_COLOR_ARRAY :
							Gl.glColorPointer( curVbo.NumElements, curVbo.ElementType, 0, nullArray );
							break;
					}
				}

				Gl.glDrawElements( m_PrimitiveType, m_IndexBuffer.Length, Gl.GL_UNSIGNED_INT, m_IndexBuffer );
			}

			#endregion
		}
		*/

		/// <summary>
		/// Builds an OpenGL mesh
		/// </summary>
		/// <param name="vertices"></param>
		/// <param name="polygons"></param>
		/// <returns></returns>
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
				mesh.CreateGroupIndexBuffer( 0, indexBuffer, Gl.GL_TRIANGLES );
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
					case "POSITION" :	clientState = Gl.GL_VERTEX_ARRAY; swapYz = true;	break;
					case "NORMAL"	:	clientState = Gl.GL_NORMAL_ARRAY; swapYz = true;	break;
					case "COLOR"	:	clientState = Gl.GL_COLOR_ARRAY;					break;
					default			:
						throw new ApplicationException( string.Format( "Unknown mesh semantic \"{0}\"", curSource.Semantic ) );
				}

				//	Store the stride (note, for GL this has a slightly different meaning, I think. TODO: Check COLLADA spec. to make sure)
				int		numObjects	= curSource.Source.Data.Count / curSource.Source.Stride;
				short	stride		= 0;
				short	numElements	= ( short )curSource.Source.Stride;

				Type dataType = curSource.Source.DataType;
				switch ( Type.GetTypeCode( dataType ) )
				{
					case TypeCode.Single :
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
						mesh.SetupVertexBuffer( vboIndex, numObjects, clientState, stride, numElements, Gl.GL_STATIC_DRAW_ARB, bufferData );
						break;
					}

					case TypeCode.Int32 :
					{
						int[] bufferData = ( int[] )curSource.Source.Data.ToArray( dataType );	
						mesh.SetupVertexBuffer( vboIndex, numObjects, clientState, stride, numElements, Gl.GL_STATIC_DRAW_ARB, bufferData );
						break;
					}

					case TypeCode.Byte :
					{
						byte[] bufferData = ( byte[] )curSource.Source.Data.ToArray( dataType );
						mesh.SetupVertexBuffer( vboIndex, numObjects, clientState, stride, numElements, Gl.GL_STATIC_DRAW_ARB, bufferData );
						break;
					}

					default :
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
