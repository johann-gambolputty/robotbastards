using System.Collections.Generic;
using Poc0.Core.Environment;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Textures;
using Rectangle=Rb.Core.Maths.Rectangle;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Builds runtime environment graphics from level geometry
	/// </summary>
	/// <remarks>
	/// At the moment, this just chops up level geometry into a regular grid, and creates an optimised
	/// mesh for each grid square. This is because the 2d bsp approach does not allow Y overlaps (e.g.
	/// no tunnels or bridges), and the camera looks down on the player at all times.
	/// If the camera was first person, then a portal-based approach might be better.
	/// (http://www.gamedev.net/reference/articles/article1891.asp)
	/// Note: at the moment, just creates a giant vertex buffer :-/
	/// </remarks>
	internal class EnvironmentGraphicsBuilder
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="chopSize">Grid chop size</param>
		public EnvironmentGraphicsBuilder( float chopSize )
		{
			m_ChopSize = chopSize;
		}

		/// <summary>
		/// Static geometry vertex
		/// </summary>
		private class Vertex
		{
			/// <summary>
			/// Vertex position
			/// </summary>
			[VertexField( VertexField.Position )]
			public Point3 m_Point;
			
			/// <summary>
			/// Vertex normal
			/// </summary>
			[VertexField( VertexField.Normal )]
			public Vector3 m_Normal;

			/// <summary>
			/// Texture 0 coordinate
			/// </summary>
			[VertexField( VertexField.Texture0 )]
			public Point2 m_Texture0;

			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="pt">Vertex position</param>
			/// <param name="normal">Vertex normal</param>
			/// <param name="texture0">Vertex texture unit 0 coordinate</param>
			public Vertex( Point3 pt, Vector3 normal, Point2 texture0 )
			{
				m_Point = pt;
				m_Normal = normal;
				m_Texture0 = texture0;
			}
		}

		/// <summary>
		/// Group material properties
		/// </summary>
		private class GroupMaterial
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="technique">Group technique</param>
			/// <param name="textures">Group textures</param>
			public GroupMaterial( ITechnique technique, ITexture2d[] textures )
			{
				m_Technique = technique;
				m_Textures = textures;
			}

			/// <summary>
			/// Gets the group rendering technique
			/// </summary>
			public ITechnique Technique
			{
				get { return m_Technique;}
			}

			/// <summary>
			/// Gets the group textures
			/// </summary>
			public ITexture2d[] Textures
			{
				get { return m_Textures; }
			}

			private readonly ITechnique		m_Technique;
			private readonly ITexture2d[]	m_Textures;
		}

		/// <summary>
		/// Builds a list of <see cref="GroupBuilder"/> objects
		/// </summary>
		private class GroupListBuilder
		{
			/// <summary>
			/// Gets a group for a given material
			/// </summary>
			/// <param name="material">Group material</param>
			/// <returns>Returns an existing unretired group that matches the specified material, or a new group</returns>
			public GroupBuilder GetGroup( GroupMaterial material )
			{
				foreach ( GroupBuilder group in m_Groups )
				{
					if ( group.Material == material )
					{
						return group;
					}
				}
				GroupBuilder newGroup = new GroupBuilder( material );
				m_Groups.Add( newGroup );
				return newGroup;
			}

			/// <summary>
			/// Gets the list of groups
			/// </summary>
			public IList< GroupBuilder > Groups
			{
				get { return m_Groups; }
			}

			private readonly List< GroupBuilder > m_Groups = new List< GroupBuilder >( );
		}

		/// <summary>
		/// Builds <see cref="EnvironmentGraphicsData.CellGeometryGroup"/> objects
		/// </summary>
		private class GroupBuilder
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="material">Group material</param>
			public GroupBuilder( GroupMaterial material )
			{
				m_Material = material;
			}

			/// <summary>
			/// Gets the group material
			/// </summary>
			public GroupMaterial Material
			{
				get { return m_Material; }
			}

			/// <summary>
			/// Gets the group indices
			/// </summary>
			public ICollection<int> Indices
			{
				get { return m_Indices; }
			}

			/// <summary>
			/// Creates a cell geometry group
			/// </summary>
			/// <returns>New cell geometry group</returns>
			public EnvironmentGraphicsData.CellGeometryGroup Create( )
			{
				ITechnique technique = m_Material.Technique;

				ITexture2d[] textures = new ITexture2d[ m_Material.Textures.Length ];

				for ( int textureIndex = 0; textureIndex < textures.Length; ++textureIndex )
				{
					textures[ textureIndex ] = m_Material.Textures[ textureIndex ];
				}

				EnvironmentGraphicsData.CellGeometryGroup group = new EnvironmentGraphicsData.CellGeometryGroup( m_Indices.ToArray( ), technique, textures );
				return group;
			}


			private readonly GroupMaterial m_Material;
			private readonly List< int > m_Indices = new List< int >( );
		}

		private const float FloorUScale = 0.1f;
		private const float FloorVScale = 0.1f;

		private static Vertex FloorVertex( float x, float y, float z )
		{
			float u = x * FloorUScale;
			float v = z * FloorVScale;
			return new Vertex( new Point3( x, y, z ), new Vector3( 0, 1, 0 ), new Point2( u, v ) );
		}


		private static EnvironmentGraphicsData.CellGeometryGroup[] CreateGroups( GroupListBuilder builder )
		{
			EnvironmentGraphicsData.CellGeometryGroup[] groups = new EnvironmentGraphicsData.CellGeometryGroup[ builder.Groups.Count ];

			for ( int groupIndex = 0; groupIndex < groups.Length; ++groupIndex )
			{
				groups[ groupIndex ] = builder.Groups[ groupIndex ].Create( );
			}

			return groups;
		}

		private static void GenerateConvexPolygonTriIndices( LevelGeometryTesselator.Polygon poly, int[] indexMap, GroupListBuilder builder, GroupMaterial material )
		{
			GroupBuilder group = builder.GetGroup( material );
			ICollection< int > indices = group.Indices;

			int baseIndex = indexMap[ poly.Edges[0].StartIndex ];
			for ( int index = 1; index < poly.Edges.Length - 1; ++index )
			{
				indices.Add( baseIndex );
				indices.Add( indexMap[ poly.Edges[ index ].StartIndex ] );
				indices.Add( indexMap[ poly.Edges[ index ].EndIndex ] );
			}
		}

		/// <summary>
		/// Builds runtime environment graphics from level geometry
		/// </summary>
		/// <param name="envGraphics">Environment graphics to build</param>
		/// <param name="geometry">Level geometry</param>
		/// <returns>Returns a new <see cref="IEnvironmentGraphics"/> object</returns>
		public IEnvironmentGraphics Build( IEnvironmentGraphics envGraphics, LevelGeometry geometry )
		{
			//	Create environment graphics

			Csg2.Node root = Csg2.Build( geometry.ObstaclePolygons );
			List< LevelGeometryTesselator.Polygon > floorPolys = new List< LevelGeometryTesselator.Polygon >( );
			List< LevelGeometryTesselator.Polygon > obstaclePolys = new List< LevelGeometryTesselator.Polygon >( );

			LevelGeometryTesselator tess = new LevelGeometryTesselator( );
			LevelGeometryTesselator.Polygon poly = tess.CreateBoundingPolygon( -100, -100, 100, 100 );

			LevelGeometryTesselator.AddPolygonDelegate addFloorPoly =
				delegate( LevelGeometryTesselator.Polygon floorPoly, Csg2.Node node )
				{
					floorPolys.Add( floorPoly );
				};
			
			LevelGeometryTesselator.AddPolygonDelegate addObstaclePoly =
				delegate( LevelGeometryTesselator.Polygon obstaclePoly, Csg2.Node node )
				{
					obstaclePolys.Add( obstaclePoly );
				};

			tess.BuildConvexRegions( root, poly, addFloorPoly, addObstaclePoly, true );

			GroupListBuilder groupsBuilder = new GroupListBuilder( );

			Point2[] flatPoints = tess.Points.ToArray( );

			//    //	Get the texture source for the wall
			//    ITexture2d textureSource = node.Edge.WallData.Texture;
			//    ITechnique techniqueSource = node.Edge.WallData.Technique;

			//    GroupMaterial material = new GroupMaterial( techniqueSource, new ITexture2d[] { textureSource } );
			//    GroupBuilder group = builder.GetGroup( material );
			StaticGeometryData defaultFloor = StaticGeometryData.CreateDefaultFloorData( );
			StaticGeometryData defaultWall = StaticGeometryData.CreateDefaultWallData( );
			GroupMaterial defaultFloorMaterial = new GroupMaterial
				(
					defaultFloor.Technique, new ITexture2d[] { defaultFloor.Texture }
				);
			GroupMaterial defaultWallMaterial = new GroupMaterial
				(
					defaultWall.Technique, new ITexture2d[] { defaultWall.Texture }
				);
			GroupMaterial defaultObstacleMaterial = new GroupMaterial
				(
					defaultFloor.Technique, new ITexture2d[] { defaultFloor.Texture }
				);


			int[] floorIndexMap = new int[ flatPoints.Length ];
			int[] roofIndexMap = new int[ flatPoints.Length ];

			for ( int index = 0; index < flatPoints.Length; ++index )
			{
				floorIndexMap[ index ] = -1;
				roofIndexMap[ index ] = -1;
			}

			List< Vertex > vertices = new List< Vertex >( flatPoints.Length * 2 );

			foreach ( LevelGeometryTesselator.Polygon floorPoly in floorPolys )
			{
				for ( int edgeIndex = 0; edgeIndex < floorPoly.Edges.Length; ++edgeIndex )
				{
					int pIndex = floorPoly.Edges[ edgeIndex ].StartIndex;
					if ( floorIndexMap[ pIndex ] == -1 )
					{
						Point2 srcPt = flatPoints[ pIndex ];
						floorIndexMap[ pIndex ] = AddVertex( vertices, FloorVertex( srcPt.X, 0, srcPt.Y ) );
					}
				}
				GenerateConvexPolygonTriIndices( floorPoly, floorIndexMap, groupsBuilder, defaultFloorMaterial );
			}

			foreach ( LevelGeometryTesselator.Polygon obstaclePoly in obstaclePolys )
			{
				//	Duplicate floor points at obstacle height, add wall polys
				for ( int edgeIndex = 0; edgeIndex < obstaclePoly.Edges.Length; ++edgeIndex )
				{
					int pIndex = obstaclePoly.Edges[ edgeIndex ].StartIndex;
					if ( roofIndexMap[ pIndex ] == -1 )
					{
						Point2 srcPt = flatPoints[ pIndex ];
						roofIndexMap[ pIndex ] = AddVertex( vertices, FloorVertex( srcPt.X, 6, srcPt.Y ) );
					}
				}
				//	Generate wall polys
				for ( int edgeIndex = 0; edgeIndex < obstaclePoly.Edges.Length; ++edgeIndex )
				{
					int pIndex = obstaclePoly.Edges[ edgeIndex ].StartIndex;
					int nextPIndex = obstaclePoly.Edges[ edgeIndex ].EndIndex;

					if ( ( floorIndexMap[ pIndex ] == -1 ) || ( floorIndexMap[ nextPIndex ] == -1 ) )
					{
						//	No equivalent floor vertices - ignore
						continue;
					}

					int v0 = AddVertex( vertices, null );
					int v1 = AddVertex( vertices, null );
					int v2 = AddVertex( vertices, null );
					int v3 = AddVertex( vertices, null );
					

					GroupBuilder group = groupsBuilder.GetGroup( defaultWallMaterial );
					group.Indices.Add( floorIndex );
					group.Indices.Add( nextFloorIndex );
					group.Indices.Add( roofIndex );

					group.Indices.Add( nextFloorIndex );
					group.Indices.Add( nextRoofIndex );
					group.Indices.Add( roofIndex );
				}

				GenerateConvexPolygonTriIndices( obstaclePoly, roofIndexMap, groupsBuilder, defaultObstacleMaterial );
			}

			VertexBufferData buffer = VertexBufferData.FromVertexCollection( vertices );
			EnvironmentGraphicsData.GridCell cell = new EnvironmentGraphicsData.GridCell( buffer );
			
			foreach ( GroupBuilder group in groupsBuilder.Groups )
			{
				cell.Groups.Add( group.Create( ) );
			}

			EnvironmentGraphicsData data = new EnvironmentGraphicsData( 1, 1 );
			data[ 0, 0 ] = cell;

			envGraphics.Build( data );

			return envGraphics;

			/*
			EnvironmentGraphicsData envGraphicsData = new EnvironmentGraphicsData( 1, 1 );

			EnvironmentGraphicsData.GridCell levelCell = new EnvironmentGraphicsData.GridCell( );

			EnvironmentGraphicsData.CellGeometryGroup[] groups = CreateGroups( geometry.Csg.Root );
			levelCell.Groups.AddRange( groups );

			envGraphicsData[ 0, 0 ] = levelCell;
			
			envGraphics.Build( envGraphicsData );

			return envGraphics;

			/*
			//	Determine x/z bounds of the geometry
			Rectangle levelBounds = GetLevelBounds( geometry.Csg.Root );

			int xDivisions = ( int )( levelBounds.Width / m_ChopSize ) + 1;
			int yDivisions = ( int )( levelBounds.Height / m_ChopSize ) + 1;

			float y = levelBounds.Y;
			
			TriBuilder builder = new TriBuilder( );

			for ( int yDiv = 0; yDiv < yDivisions; ++yDiv, y += m_ChopSize )
			{
				float x = levelBounds.X;
				for ( int xDiv = 0; xDiv < xDivisions; ++xDiv, x += m_ChopSize )
				{
					ClippetyClip( geometry.Csg.Root, x, y, x + m_ChopSize, y + m_ChopSize, builder );
				}
			}

			return null;
			*/
		}

		private static Vertex WallVertex( Point3 pt, Vector3 normal )
		{
			return new Vertex( pt, normal, );
		}

		private static int AddVertex( ICollection< Vertex > vertices, Vertex vertex )
		{
			int index = vertices.Count;
			vertices.Add( vertex );
			return index;
		}

		private class TriBuilder
		{
			public class Tri
			{
				public Tri( Point3 p0, Point3 p1, Point3 p2 )
				{
					m_P0 = p0;
					m_P1 = p1;
					m_P2 = p2;
				}

				public Point3 P0
				{
					get { return m_P0; }
				}

				public Point3 P1
				{
					get { return m_P1; }
				}

				public Point3 P2
				{
					get { return m_P2; }
				}

				private readonly Point3 m_P0;
				private readonly Point3 m_P1;
				private readonly Point3 m_P2;
			}

			public List< Tri > Tris
			{
				get { return m_Tris; }
			}

			public void AddQuad( Point3 p0, Point3 p1, Point3 p2, Point3 p3 )
			{
				m_Tris.Add( new Tri( p0, p1, p2 ) );
				m_Tris.Add( new Tri( p2, p3, p0 ) );
			}

			private readonly List< Tri > m_Tris = new List< Tri >( );
		}

		private const byte MinXCode = 0x1;
		private const byte MaxXCode = 0x2;
		private const byte MinYCode = 0x4;
		private const byte MaxYCode = 0x8;

		private static byte ClipCode( Point3 pt, float x, float y, float mX, float mY )
		{
			byte code = 0;
			if ( pt.X < x )
			{
				code |= MinXCode;
			}
			else if ( pt.X > mX )
			{
				code |= MaxXCode;
			}
			if ( pt.Z < y )
			{
				code |= MinYCode;
			}
			else if ( pt.Z > mY )
			{
				code |= MaxYCode;
			}
			return code;
		}

		private static void ClipNode( Point3[] quad, float x, float y, float mX, float mY, TriBuilder builder )
		{
			//	Loop endlessly (will keep clipping until quad is trivially accepted or rejected)
			while ( true )
			{
				byte startClip = ClipCode( quad[ 0 ], x, y, mX, mY );
				byte endClip = ClipCode( quad[ 1 ], x, y, mX, mY );

				if ( ( startClip | endClip ) == 0 )
				{
					//	Both points in chop rect - trivially accept
					builder.AddQuad( quad[ 0 ], quad[ 1 ], quad[ 2 ], quad[ 3 ]);
					return;
				}
				else if ( ( startClip & endClip ) != 0 )
				{
					//	Both points on the same side outside the chop rect - trivially reject
					return;
				}

				//	One or both points outside the chop rect - clip one of them to the chop rect and start again
				if ( startClip != 0 )
				{
					ClipQuad( true, quad, startClip, x, y, mX, mY );
				}
				else if ( endClip != 0 )
				{
					ClipQuad( false, quad, endClip, x, y, mX, mY );	
				}
			}
		}


		/// <summary>
		/// Clips a quad against 4 planes, defined by a rectangle on the x-z plane
		/// </summary>
		private static void ClipQuad( bool left, Point3[] quad, byte outcode, float x, float y, float mX, float mY )
		{
			//	x = x0 + 1/slope * (y - y0)
			//	y = y0 + slope * (x-x0)
			float t;
			float outX		= 0;
			float outZ		= 0;
			float lowOutY	= 0;
			float highOutY	= 0;
			float x0 		= quad[ 0 ].X;
			float x1 		= quad[ 1 ].X;
			float lowY0		= quad[ 0 ].Y;
			float lowY1		= quad[ 1 ].Y;
			float highY0	= quad[ 3 ].Y;
			float highY1	= quad[ 2 ].Y;
			float z0 		= quad[ 0 ].Z;
			float z1 		= quad[ 1 ].Z;
			
			if ( ( outcode & MinYCode ) != 0 )
			{
				t = ( mY - z0 ) / ( z1 - z0 );
				outX = x0 + ( x1 - x0 ) * t;
				lowOutY = lowY0 + ( lowY1 - lowY0 ) * t;
				highOutY = highY0 + ( highY1 - highY0 ) * t;
				outZ = y;
			}
			else if ( ( outcode & MaxYCode ) != 0 )
			{
				t = ( y - z0 ) / ( z1 - z0 );
				outX = x0 + ( x1 - x0 ) * t;
				lowOutY = lowY0 + ( lowY1 - lowY0 ) * t;
				highOutY = highY0 + ( highY1 - highY0 ) * t;
				outZ = mY;
			}
			if ( ( outcode & MinXCode ) != 0 )
			{
				t = ( x - x0 ) / ( x1 - x0 );
				outX = x;
				lowOutY = lowY0 + ( lowY1 - lowY0 ) * t;
				highOutY = highY0 + ( highY1 - highY0 ) * t;
				outZ = z0 + ( z1 - z0 ) * t;
			}
			else if ( ( outcode & MaxXCode ) != 0 )
			{
				t = ( mX - x0 ) / ( x1 - x0 );
				outX = mX;
				lowOutY = lowY0 + ( lowY1 - lowY0 ) * t;
				highOutY = highY0 + ( highY1 - highY0 ) * t;
				outZ = z0 + ( z1 - z0 ) * t;
			}

			if ( left )
			{
				quad[ 0 ].Set( outX, lowOutY, outZ );
				quad[ 3 ].Set( outX, highOutY, outZ );
			}
			else
			{
				quad[ 1 ].Set( outX, lowOutY, outZ );
				quad[ 2 ].Set( outX, highOutY, outZ );
			}
		}

		/// <summary>
		/// Clips a BSP node, returns a triangle list
		/// </summary>
		private static void ClippetyClip( Csg.BspNode node, float x, float y, float mX, float mY, TriBuilder builder )
		{
			if ( node == null )
			{
				return;
			}
			ClipNode( node.Quad, x, y, mX, mY, builder );

			ClippetyClip( node.InFront, x, y, mX, mY, builder );
			ClippetyClip( node.Behind, x, y, mX, mY, builder );
		}

		/// <summary>
		/// Gets the bounding box for a level
		/// </summary>
		/// <param name="node">Root node</param>
		/// <returns>Returns the level bounding rectangle</returns>
		private Rectangle GetLevelBounds( Csg.BspNode node )
		{
			Point2 minPt = new Point2( float.MaxValue, float.MaxValue );
			Point2 maxPt = new Point2( float.MinValue, float.MinValue );
			GetLevelBounds( node, minPt, maxPt );

			return new Rectangle( minPt, maxPt );
		}

		/// <summary>
		/// Gets the bounding rectangle for a node and its children
		/// </summary>
		/// <param name="node">Node to check</param>
		/// <param name="min">Current minimum bounds</param>
		/// <param name="max">Current maximum bounds</param>
		private void GetLevelBounds( Csg.BspNode node, Point2 min, Point2 max )
		{
			if ( node == null )
			{
				return;
			}

			min.X = Utils.Min( Utils.Min( node.Edge.P0.X, min.X ), node.Edge.P1.X );
			min.Y = Utils.Min( Utils.Min( node.Edge.P0.Y, min.Y ), node.Edge.P1.Y );
			
			max.X = Utils.Max( Utils.Max( node.Edge.P0.X, max.X ), node.Edge.P1.X );
			max.Y = Utils.Max( Utils.Max( node.Edge.P0.Y, max.Y ), node.Edge.P1.Y );

			GetLevelBounds( node.InFront, min, max );
			GetLevelBounds( node.Behind, min, max );
		}

		//private static void CreateFloorGroup( Csg.BspNode node, GroupListBuilder builder )
		//{
		//    if ( node.ConvexRegion == null )
		//    {
		//        return;
		//    }
			
		//    //	Get the texture source for the floor
		//    ITexture2d textureSource = node.FloorData.Texture;
		//    ITechnique techniqueSource = node.FloorData.Technique;
			
		//    GroupMaterial material = new GroupMaterial( techniqueSource, new ITexture2d[] { textureSource } );
		//    GroupBuilder group = builder.GetGroup( material );

		//    float height = 0;
		//    Point2[] points = null; // node.ConvexRegion;
		//    Point2 basePos = points[ 0 ];
		//    ICollection<Vertex> vertices = group.Vertices;
		//    for ( int vertexIndex = 1; vertexIndex < points.Length - 1; ++vertexIndex )
		//    {
		//        vertices.Add( FloorVertex( basePos.X, height, basePos.Y ) );

		//        Point2 pt = points[ vertexIndex ];
		//        vertices.Add( FloorVertex( pt.X, height, pt.Y ) );
				
		//        pt = points[ vertexIndex + 1 ];
		//        vertices.Add( FloorVertex( pt.X, height, pt.Y ) );
		//    }
		//}

		//private static void CreateWallGroup( Csg.BspNode node, GroupListBuilder builder )
		//{
		//    Vector3 planeNormal = new Vector3( node.Plane.Normal.X, 0, node.Plane.Normal.Y );

		//    //	Get the texture source for the wall
		//    ITexture2d textureSource = node.Edge.WallData.Texture;
		//    ITechnique techniqueSource = node.Edge.WallData.Technique;

		//    GroupMaterial material = new GroupMaterial( techniqueSource, new ITexture2d[] { textureSource } );
		//    GroupBuilder group = builder.GetGroup( material );

		//    //	TODO: AP: Split quad up into grid depending on texture size
		//    //	OR: Give groups a single technique + texture, remove the texture packer, repeat the UVs
		//    float texWidth = node.Quad[ 0 ].DistanceTo( node.Quad[ 1 ] ) / 5.0f;
		//    float texHeight = node.Quad[ 0 ].DistanceTo( node.Quad[ 3 ] ) / 5.0f;
		//    Point2 uvBl = new Point2( 0, 0 );
		//    Point2 uvBr = new Point2( texWidth, 0 );
		//    Point2 uvTr = new Point2( texWidth, texHeight );
		//    Point2 uvTl = new Point2( 0, texHeight );

		//    ICollection<Vertex> vertices = group.Vertices;
		//    vertices.Add( new Vertex( node.Quad[ 0 ], planeNormal, uvBl ) );
		//    vertices.Add( new Vertex( node.Quad[ 1 ], planeNormal, uvBr ) );
		//    vertices.Add( new Vertex( node.Quad[ 2 ], planeNormal, uvTr ) );

		//    vertices.Add( new Vertex( node.Quad[ 2 ], planeNormal, uvTr ) );
		//    vertices.Add( new Vertex( node.Quad[ 3 ], planeNormal, uvTl ) );
		//    vertices.Add( new Vertex( node.Quad[ 0 ], planeNormal, uvBl ) );
		//}

		private readonly float m_ChopSize;
	}
}
