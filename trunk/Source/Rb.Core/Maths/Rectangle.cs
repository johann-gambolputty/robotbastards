using System;

namespace Rb.Core.Maths
{
	public class Rectangle : IShape2
	{
		public Rectangle( )
		{
		}

		public Rectangle( float x, float y, float width, float height )
		{
			m_X = x;
			m_Y = y;
			m_Width = width;
			m_Height = height;
		}

		public float X
		{
			get { return m_X; }
			set { m_X = value; }
		}

		public float Y
		{
			get { return m_Y; }
			set { m_Y = value; }
		}

		public float Width
		{
			get { return m_Width; }
			set { m_Width = value; }
		}

		public float Height
		{
			get { return m_Height; }
			set { m_Height = value; }
		}

		public Point2 TopLeft
		{
			get { return new Point2( m_X, m_Y );}
			set
			{
				m_X = value.X;
				m_Y = value.Y;
			}
		}

		private float m_X;
		private float m_Y;
		private float m_Width;
		private float m_Height;

		#region IShape2 Members

		/// <summary>
		/// Returns true if this shape contains a given point
		/// </summary>
		public bool Contains( Point2 pt )
		{
			return ( pt.X >= m_X ) && ( pt.X <= ( m_X + m_Width ) ) && ( pt.Y >= m_Y ) && ( pt.Y <= ( m_Y + m_Height ) );
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
			Rectangle rect = ( Rectangle )shape;

			float x0 = m_X;
			float y0 = m_Y;
			float x1 = m_X + m_Width;
			float y1 = m_Y + m_Height;
			
			float x2 = rect.m_X;
			float y2 = rect.m_Y;
			float x3 = rect.m_X + rect.m_Width;
			float y3 = rect.m_Y + rect.m_Height;
			
			return ( ( !( ( x0 < x2 && x1 < x2 ) || ( x0 > x3 && x1 > x3 ) || ( y0 < y2 &&  y1 < y2 ) || ( y0 > y3  && y1 > y3 ) ) ) );
		}

		#endregion
	}
}
