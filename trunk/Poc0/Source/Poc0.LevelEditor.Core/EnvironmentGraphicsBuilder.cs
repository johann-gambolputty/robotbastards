using System.Collections.Generic;
using Poc0.Core.Environment;
using Rb.Core.Maths;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core
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

		private class Vertex
		{
			[VertexField( VertexField.Position )]
			public Point3 m_Point;
			
			[VertexField( VertexField.Normal )]
			public Vector3 m_Normal;

			public Vertex( Point3 pt, Vector3 normal )
			{
				m_Point = pt;
				m_Normal = normal;
			}
		}

		private static void CreateGroup( Csg.BspNode node, ICollection< Vertex > vertices )
		{
			if ( node == null )
			{
				return;
			}

			Vector3 planeNormal = new Vector3( node.Plane.Normal.X, 0, node.Plane.Normal.Y );

			vertices.Add( new Vertex( node.Quad[ 0 ], planeNormal ) );
			vertices.Add( new Vertex( node.Quad[ 1 ], planeNormal ) );
			vertices.Add( new Vertex( node.Quad[ 2 ], planeNormal ) );
			
			vertices.Add( new Vertex( node.Quad[ 2 ], planeNormal ) );
			vertices.Add( new Vertex( node.Quad[ 3 ], planeNormal ) );
			vertices.Add( new Vertex( node.Quad[ 0 ], planeNormal ) );

			CreateGroup( node.Behind, vertices );
			CreateGroup( node.InFront, vertices );
		}

		private static EnvironmentGraphicsData.CellGeometryGroup CreateGroup( Csg.BspNode node )
		{
			List< Vertex > vertices = new List< Vertex >( );
			CreateGroup( node, vertices );

			VertexBufferData vertexData = new VertexBufferData( vertices.Count );
			//vertexData.FromVertexCollection( vertices );
			float[] positions = vertexData.Add< float >( VertexField.Position, 3 );
			float[] normals = vertexData.Add< float >( VertexField.Normal, 3 );

			int positionIndex = 0;
			int normalIndex = 0;
			for ( int vertexIndex = 0; vertexIndex < vertices.Count; ++vertexIndex )
			{
				positions[ positionIndex++ ] = vertices[ vertexIndex ].m_Point.X;
				positions[ positionIndex++ ] = vertices[ vertexIndex ].m_Point.Y;
				positions[ positionIndex++ ] = vertices[ vertexIndex ].m_Point.Z;
				
				normals[ normalIndex++ ] = vertices[ vertexIndex ].m_Normal.X;
				normals[ normalIndex++ ] = vertices[ vertexIndex ].m_Normal.Y;
				normals[ normalIndex++ ] = vertices[ vertexIndex ].m_Normal.Z;
			}

			return new EnvironmentGraphicsData.CellGeometryGroup( vertexData, null );
		}

		/// <summary>
		/// Builds runtime environment graphics from level geometry
		/// </summary>
		/// <param name="geometry">Level geometry</param>
		/// <returns>Returns a new EnvironmentGraphics object</returns>
		public IRenderable Build(LevelGeometry geometry)
		{
			//	Check that there is some geometry to process
			if ( ( geometry.Csg == null ) || ( geometry.Csg.Root == null ) )
			{
				return null;
			}

			EnvironmentGraphicsData envGraphicsData = new EnvironmentGraphicsData( 1, 1 );

			EnvironmentGraphicsData.GridCell levelCell = new EnvironmentGraphicsData.GridCell( );

			EnvironmentGraphicsData.CellGeometryGroup group = CreateGroup( geometry.Csg.Root );
			levelCell.Groups.Add( group );

			envGraphicsData[ 0, 0 ] = levelCell;
			
			IEnvironmentGraphics envGraphics = Graphics.Factory.Create< IEnvironmentGraphics >( );
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

		private readonly float m_ChopSize;
	}
}
