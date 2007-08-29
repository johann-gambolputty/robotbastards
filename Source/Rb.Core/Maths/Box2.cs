using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Object-aligned box
	/// </summary>
	public class Box2 : IShape2
	{
		/// <summary>
		/// Default constructor - axis-aligned unit cube at the origin
		/// </summary>
		public Box2( )
		{
			Set( -0.5f, -0.5f, 1, 1 );
		}

		/// <summary>
		/// Axis-aligned rectangle setup
		/// </summary>
		/// <param name="x">Top left corner x coordinate</param>
		/// <param name="y">Top left corner y coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public Box2( float x, float y, float width, float height )
		{
			Set( x, y, width, height );
		}

		/// <summary>
		/// Axis-aligned rectangle setup
		/// </summary>
		/// <param name="min">Top left corner position</param>
		/// <param name="max">Bottom right corner position</param>
		public Box2( Point2 min, Point2 max )
		{
			Set( min, max );
		}

		/// <summary>
		/// Object-aligned box setup
		/// </summary>
		/// <remarks>
		/// Up to the caller to ensure that edges are orthogonal
		/// </remarks>
		/// <param name="tl">Box top left corner</param>
		/// <param name="tr">Box top right corner</param>
		/// <param name="bl">Box bottom left corner</param>
		/// <param name="br">Box bottom right corner</param>
		public Box2( Point2 tl, Point2 tr, Point2 bl, Point2 br )
		{
			Set( tl, tr, bl, br );
		}

		/// <summary>
		/// Axis-aligned rectangle setup
		/// </summary>
		/// <param name="x">Top left corner x coordinate</param>
		/// <param name="y">Top left corner y coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public void Set( float x, float y, float width, float height )
		{
			m_Origin = new Point2( x + ( width / 2 ), y + ( height / 2 ) );
			m_Axis[ 0 ] = new Vector2( 1, 0 );
			m_Axis[ 1 ] = new Vector2( 0, 1 );
			m_HalfDims[ 0 ] = width / 2;
			m_HalfDims[ 1 ] = height / 2;
		}

		/// <summary>
		/// Axis-aligned rectangle setup
		/// </summary>
		/// <param name="min">Top left corner position</param>
		/// <param name="max">Bottom right corner position</param>
		public void Set( Point2 min, Point2 max )
		{
			Set( min.X, min.Y, max.X - min.X, max.Y - min.Y );
		}

		/// <summary>
		/// Object-aligned box setup
		/// </summary>
		/// <remarks>
		/// Up to the caller to ensure that edges are orthogonal
		/// </remarks>
		/// <param name="tl">Box top left corner</param>
		/// <param name="tr">Box top right corner</param>
		/// <param name="bl">Box bottom left corner</param>
		/// <param name="br">Box bottom right corner</param>
		public void Set( Point2 tl, Point2 tr, Point2 bl, Point2 br )
		{
			Vector2 topEdge = tr - tl;
			Vector2 leftEdge = bl - tl;

			float xDim = topEdge.Length;
			float yDim = leftEdge.Length;

			m_HalfDims[ 0 ] = xDim;
			m_HalfDims[ 1 ] = yDim;

			m_Origin = tl + ( topEdge / 2 ) + ( leftEdge / 2 );
			m_Axis[ 0 ] = topEdge / xDim;
			m_Axis[ 1 ] = leftEdge / yDim;
		}

		private readonly float[]	m_HalfDims = new float[ 2 ];
		private readonly Vector2[]	m_Axis = new Vector2[ 2 ];
		private Point2				m_Origin;

		#region IShape2 Members

		/// <summary>
		/// Returns true if this shape contains a given point
		/// </summary>
		public bool Contains( Point2 pt )
		{
			throw new Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// Gets the distance from this shape to a point
		/// </summary>
		public float Distance( Point2 pt )
		{
			throw new Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// Gets the distance from this shape to another shape
		/// </summary>
		public float Distance( IShape2 shape )
		{
			throw new Exception("The method or operation is not implemented.");
		}

		/// <summary>
		/// Returns true if this shape and another shape overlap
		/// </summary>
		public bool Overlaps( IShape2 shape )
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

	}
}
