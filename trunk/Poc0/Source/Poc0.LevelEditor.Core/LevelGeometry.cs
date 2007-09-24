using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Level geometry. Combines brushes to create the walkable geometry of a level
	/// </summary>
	[Serializable]
	public class LevelGeometry : IRenderable
	{
		/// <summary>
		/// CSG operations
		/// </summary>
		public enum Csg
		{
			Union,
			Intersection,
			Complement
		}

		/// <summary>
		/// Renders the level geometry
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Geometry != null )
			{
				Render( m_Geometry );
			}
		}

		/// <summary>
		/// Combines a brush with the current geometry set
		/// </summary>
		/// <param name="csg">CSG operation</param>
		/// <param name="brush">Brush to add to the level geometry</param>
		public void Combine( Csg csg, GeometryBrush brush )
		{
			BspNode brushBsp = Build( brush );
			if ( m_Geometry == null )
			{
				if ( csg == Csg.Union )
				{
					m_Geometry = brushBsp;
				}
				return;
			}
			m_Geometry = Combine( csg, m_Geometry, brushBsp );
		}


		private BspNode m_Geometry;

		[NonSerialized]
		private Draw.IPen m_DrawEdge;

		[NonSerialized]
		private Draw.IBrush m_DrawVertex;

		private void Render( BspNode node )
		{
			if ( m_DrawEdge == null )
			{
				m_DrawEdge = Graphics.Draw.NewPen( Color.Red, 2.0f );
			}
			if ( m_DrawVertex == null )
			{
				m_DrawVertex = Graphics.Draw.NewBrush( Color.Red );
			}

			Graphics.Draw.Line( m_DrawEdge, node.Edge.P0, node.Edge.P1 );
			Graphics.Draw.Circle( m_DrawVertex, node.Edge.P0.X, node.Edge.P0.Y, 3.0f );
			Graphics.Draw.Circle( m_DrawVertex, node.Edge.P1.X, node.Edge.P1.Y, 3.0f );
			if ( node.Behind != null )
			{
				Render( node.Behind );
			}
			if ( node.InFront != null )
			{
				Render( node.InFront );
			}
		}

		private static BspNode Combine( Csg csg, BspNode set0, BspNode set1 )
		{
			return set0;
		}

		/// <summary>
		/// Currently using a BSP tree for our CSG needs
		/// </summary>
		[Serializable]
		private class BspNode
		{
			public BspNode( Edge edge )
			{
				m_Edge = edge;
			}

			public BspNode Behind
			{
				get { return m_Behind; }
				set { m_Behind = value; }
			}

			public BspNode InFront
			{
				get { return m_InFront; }
				set { m_InFront = value; }
			}

			public Edge Edge
			{
				get { return m_Edge; }
			}

			private BspNode m_Behind;
			private BspNode m_InFront;
			private readonly Edge m_Edge;
		}

		[Serializable]
		private class Edge
		{
			public Edge( Point2 p0, Point2 p1 )
			{
				m_P0 = p0;
				m_P1 = p1;
				m_Plane = new Plane2( p0, p1 );
			}

			public Point2 P0
			{
				get { return m_P0; }
			}

			public Point2 P1
			{
				get { return m_P1; }
			}

			public Plane2 Plane
			{
				get { return m_Plane; }
			}

			private readonly Point2 m_P0;
			private readonly Point2 m_P1;
			private readonly Plane2 m_Plane;
		}

		private BspNode Build( IList< Edge > edges )
		{
			Edge splitter = edges[ 0 ]; // TODO: AP: Choose better spliiter
			BspNode splitNode = new BspNode( splitter );
			if ( edges.Count == 1 )
			{
				return splitNode;
			}

			List< Edge > leftEdges = new List< Edge >( );
			List< Edge > rightEdges = new List< Edge >( );
			for ( int edgeIndex = 1; edgeIndex < edges.Count; ++edgeIndex )
			{
				ClassifyEdge( splitter.Plane, edges[ edgeIndex ], leftEdges, rightEdges );
			}

			if ( leftEdges.Count > 0 )
			{
				splitNode.Behind = Build( leftEdges );
			}
			
			if ( rightEdges.Count > 0 )
			{
				splitNode.InFront = Build( rightEdges );
			}

			return splitNode;
		}

		private static void ClassifyEdge( Plane2 splitter, Edge edge, ICollection< Edge > leftEdges, ICollection< Edge > rightEdges )
		{
			bool p0Behind = splitter.GetSignedDistanceTo( edge.P0 ) < -0.01f;
			bool p1Behind = splitter.GetSignedDistanceTo( edge.P1 ) < -0.01f;

			if ( p0Behind == p1Behind )
			{
				if ( p0Behind )
				{
					leftEdges.Add( edge );
				}
				else
				{
					rightEdges.Add( edge );
				}
			}
			else
			{
				Line2Intersection intersection = Intersections2.GetLinePlaneIntersection( edge.P0, edge.P1, splitter );
				leftEdges.Add( new Edge( edge.P0, intersection.IntersectionPosition ) );
				leftEdges.Add( new Edge( intersection.IntersectionPosition, edge.P1 ) );
			}
		}

		private BspNode Build( GeometryBrush brush )
		{
			Point2[] points = brush.Points;

			List< Edge > edges = new List< Edge >( points.Length );

			int lastPtIndex = points.Length - 1;
			for ( int ptIndex = 0; ptIndex < points.Length; lastPtIndex = ptIndex++ )
			{
				edges.Add( new Edge( points[ lastPtIndex ], points[ ptIndex ] ) );
			}

			return Build( edges );
		}
	}
}
