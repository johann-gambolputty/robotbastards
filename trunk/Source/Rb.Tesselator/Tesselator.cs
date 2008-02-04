using System;
using System.Collections.Generic;
using Rb.Core.Maths;
using Tao.OpenGl;

namespace Rb.Tesselator
{
	/// <summary>
	/// Tesselates arbitrary polygons into convex polygons
	/// </summary>
	public class Tesselator
	{
		/// <summary>
		/// Ordering of indices in a polygon list (<see cref="PolygonList"/>)
		/// </summary>
		public enum Order
		{
			TriList,
			TriStrip,
			TriFan
		}

		/// <summary>
		/// Stores indices into <see cref="PolygonLists.Points"/>, that define 1 or more polygons
		/// </summary>
		public class PolygonList
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public PolygonList( Order order, int[] indices )
			{
				m_Order = order;
				m_Indices = indices;
			}

			/// <summary>
			/// Ordering of indices in the index list
			/// </summary>
			public Order Order
			{
				get { return m_Order; }
			}

			/// <summary>
			/// Polygon indices
			/// </summary>
			public int[] Indices
			{
				get { return m_Indices; }
			}

			/// <summary>
			/// Gets the number of primitives (tris or quads) defined by this list
			/// </summary>
			public int NumPrimitives
			{
				get { return GetNumPrimitives( m_Order, m_Indices.Length ); }
			}

			private readonly Order m_Order;
			private readonly int[] m_Indices;
		}

		/// <summary>
		/// Output polygons class
		/// </summary>
		public class PolygonLists
		{
			/// <summary>
			/// Setup constructor
			/// </summary>
			public PolygonLists( PolygonList[] lists, Point2[] points )
			{
				m_Lists = lists;
				m_Points = points;
			}

			/// <summary>
			/// Gets the array of output triangles
			/// </summary>
			public PolygonList[] Lists
			{
				get { return m_Lists; }
			}

			/// <summary>
			/// Gets the array of output points that the polygons index
			/// </summary>
			public Point2[] Points
			{
				get { return m_Points; }
			}

			private readonly PolygonList[] m_Lists;
			private readonly Point2[] m_Points;
		}

		/// <summary>
		/// Tesselates the current polygon and contour set
		/// </summary>
		public static PolygonLists Tesselate( TesselatorInput input )
		{
			if ( input.Polygon == null )
			{
				throw new InvalidOperationException( "SetPolygon() must be called prior to Tesselate()" );
			}

			Tesselator obj = new Tesselator();
			Glu.GLUtesselator tess = obj.BuildTesselator( );
			try
			{
				Glu.gluTessBeginPolygon( tess, IntPtr.Zero );

					foreach ( Point2 pt in input.Polygon )
					{
						obj.Vertex( tess, pt );
					}

					foreach ( TesselatorInput.Contour contour in input.Contours )
					{
						Glu.gluTessBeginContour( tess );
						foreach ( Point2 pt in contour.Points )
						{
							obj.Vertex( tess, pt );
						}
						Glu.gluTessEndContour( tess );
					}

				Glu.gluTessEndPolygon( tess );
			}
			finally
			{
				Glu.gluDeleteTess( tess );
			}

			return new PolygonLists( obj.m_Lists.ToArray( ), obj.m_Points.ToArray( ) );
		}
		

		/// <summary>
		/// Determines the number of primitives, for a number of vertices with a given ordering
		/// </summary>
		public static int GetNumPrimitives( Order order, int numVertices )
		{
			switch ( order )
			{
				case Order.TriList	: return numVertices / 3;
				case Order.TriFan	: return ( numVertices - 2 );
				case Order.TriStrip	: return ( numVertices - 2 );
			}
			throw new ArgumentException( "Unknown order type " + order, "order" );
		}


		#region Private members

		/// <summary>
		/// Private constructor - only static methods are available
		/// </summary>
		private Tesselator( )
		{
		}

		/// <summary>
		/// Generates a vertex in the tesselator
		/// </summary>
		private void Vertex( Glu.GLUtesselator tess, Point2 pt )
		{
			ms_Pt[ 0 ] = pt.X;
			ms_Pt[ 1 ] = 0;
			ms_Pt[ 2 ] = pt.Y;

			int index = m_Points.Count;
			m_Points.Add( pt );
			Glu.gluTessVertex( tess, ms_Pt, new IntPtr( index ) );
		}

		private readonly List< Point2 > m_Points = new List< Point2 >( );
		private static readonly double[] ms_Pt = new double[ 3 ];

		/// <summary>
		/// Builds the tesselator object
		/// </summary>
		private Glu.GLUtesselator BuildTesselator( )
		{
			Glu.GLUtesselator tess = Glu.gluNewTess( );

			Glu.TessBeginCallback begin = BeginCallback;
			Glu.TessVertexCallback vertex = VertexCallback;
			Glu.TessCombineCallback combine = CombineCallback;
			Glu.TessEndCallback end = EndCallback;

			Glu.gluTessCallback( tess, Glu.GLU_TESS_BEGIN, begin );
			Glu.gluTessCallback( tess, Glu.GLU_TESS_VERTEX, vertex );
			Glu.gluTessCallback( tess, Glu.GLU_TESS_COMBINE, combine );
			Glu.gluTessCallback( tess, Glu.GLU_TESS_END, end );

			return tess;
		}

		private Order m_CurrentOrder;
		private readonly List< int > m_CurrentIndices = new List< int >( );
		private readonly List< PolygonList > m_Lists = new List< PolygonList >( );

		private void BeginCallback( int type )
		{
			switch ( type )
			{
				case Gl.GL_TRIANGLES		: m_CurrentOrder = Order.TriList; break;
				case Gl.GL_TRIANGLE_STRIP	: m_CurrentOrder = Order.TriStrip; break;
				case Gl.GL_TRIANGLE_FAN		: m_CurrentOrder = Order.TriFan; break;
				default :
					throw new ArgumentException( string.Format( "Unhandled GL primitive type {0}", type ) );
			}
		}

		private void VertexCallback( IntPtr data )
		{
			m_CurrentIndices.Add( data.ToInt32( ) );
		}

		private void CombineCallback( double[] pt, IntPtr[] data, float[] weights, IntPtr[] outputData )
		{
			int index = m_Points.Count;
			m_Points.Add( new Point2( ( float )pt[ 0 ], ( float )pt[ 2 ] ) );
			outputData[ 0 ] = new IntPtr( index );
		}

		private void EndCallback( )
		{
			m_Lists.Add( new PolygonList( m_CurrentOrder, m_CurrentIndices.ToArray( ) ) );
			m_CurrentIndices.Clear( );
		}

		#endregion
	}

}
