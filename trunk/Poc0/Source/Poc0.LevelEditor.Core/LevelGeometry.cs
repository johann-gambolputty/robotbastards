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
			IList< Edge > set0Edges = Flatten( set0 );
			IList< Edge > set1Edges = Flatten( set1 );
			IList< Edge > allEdges = new List< Edge >( );
			Clip( csg == Csg.Intersection || csg == Csg.Complement, set0, set1Edges, allEdges );
			Clip( csg == Csg.Intersection, set1, set0Edges, allEdges );

			return Build( allEdges );
		}

		private static IList< Edge > Flatten( BspNode root )
		{
			List< Edge > edges = new List< Edge >( );
			Flatten( root, edges );
			return edges;
		}

		private static void Flatten( BspNode root, IList< Edge > edges )
		{
			edges.Add( root.Edge );
			if ( root.Behind != null )
			{
				Flatten( root.Behind, edges );
			}
			if ( root.InFront != null )
			{
				Flatten( root.InFront, edges );
			}
		}

		private static void Clip( bool keepInsideEdges, BspNode node, IList< Edge > edges, IList< Edge > outputEdges )
		{
			IList< Edge > leftEdges = ( node.Behind != null ) ? new List< Edge >( ) : ( keepInsideEdges ? null : outputEdges );
			IList< Edge > rightEdges = ( node.InFront != null ) ? new List< Edge >( ) : ( keepInsideEdges ? outputEdges : null );

			foreach ( Edge edge in edges )
			{
				ClassifyEdge( node.Plane, edge, leftEdges, rightEdges );
			}
			if ( node.Behind != null )
			{
				Clip( keepInsideEdges, node.Behind, leftEdges, outputEdges );
			}
			if ( node.InFront != null )
			{
				Clip( keepInsideEdges, node.InFront, rightEdges, outputEdges );
			}
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

			public Plane2 Plane
			{
				get { return Edge.Plane; }
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

		private static BspNode Build( IList< Edge > edges )
		{
			Edge splitter = edges[ 0 ]; // TODO: AP: Choose better splitter
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

		private const float OnPlaneTolerance = 0.001f;

		private static bool GetEdgeClassification( PlaneClassification p0Class, PlaneClassification p1Class, out PlaneClassification edgeClass )
		{
			if ( p0Class == p1Class )
			{
				edgeClass = p0Class;
				return true;
			}
			if ( p0Class == PlaneClassification.On )
			{
				edgeClass = p1Class;
				return true;
			}
			if ( p1Class == PlaneClassification.On )
			{
				edgeClass = p0Class;
				return true;
			}
			edgeClass = PlaneClassification.On;
			return false;
		}

		private static void ClassifyEdge( Plane2 splitter, Edge edge, ICollection< Edge > leftEdges, ICollection< Edge > rightEdges )
		{
			PlaneClassification p0Class = splitter.ClassifyPoint( edge.P0, OnPlaneTolerance );
			PlaneClassification p1Class = splitter.ClassifyPoint( edge.P1, OnPlaneTolerance );
			PlaneClassification edgeClass;

			if ( GetEdgeClassification( p0Class, p1Class, out edgeClass ) )
			{
				if ( edgeClass == PlaneClassification.Behind )
				{
					if ( leftEdges != null )
					{
						leftEdges.Add( edge );
					}
				}
				else if ( rightEdges != null )
				{
					rightEdges.Add( edge );
				}
			}
			else
			{
				Line2Intersection intersection = Intersections2.GetLinePlaneIntersection( edge.P0, edge.P1, splitter );
				
				if ( p0Class != PlaneClassification.Behind )
				{
					Utils.Swap( ref leftEdges, ref rightEdges );
				}
				if ( leftEdges != null )
				{
					leftEdges.Add( new Edge( edge.P0, intersection.IntersectionPosition ) );	
				}
				if ( rightEdges != null )
				{
					rightEdges.Add( new Edge( intersection.IntersectionPosition, edge.P1 ) );	
				}
			}
		}

		private static BspNode Build( GeometryBrush brush )
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
