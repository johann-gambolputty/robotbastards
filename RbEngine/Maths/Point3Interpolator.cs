using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Stores two points and provides a method for interpolating between them
	/// </summary>
	public class Point3Interpolator
	{
		/// <summary>
		/// The previous position
		/// </summary>
		public Point3	Previous
		{
			get 
			{
				return m_Previous; 
			}
		}

		/// <summary>
		/// The current position
		/// </summary>
		public Point3	Current
		{
			get
			{
				return m_Current;
			}
			set
			{
				m_Current = value;
			}
		}

		/// <summary>
		/// Calculates an intermediate point between the previous and current points
		/// </summary>
		/// <param name="t">Time, in the range [0..1]</param>
		/// <returns>Returns Previous, if t is 0, Current if t is 1, and an intermediate position if t is inbetween 0 and 1</returns>
		public Point3	Get( float t )
		{
			return ( Previous + ( Current - Previous ) * t );
		}

		/// <summary>
		/// Copies the current position in the previous position
		/// </summary>
		public void		Step( )
		{
			m_Previous	= m_Current;
			m_Current	= new Point3( m_Current );
		}

		private Point3	m_Previous	= new Point3( );
		private Point3	m_Current	= new Point3( );
	}
}
