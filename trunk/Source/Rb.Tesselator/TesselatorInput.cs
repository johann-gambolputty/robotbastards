
using System.Collections.Generic;
using Rb.Core.Maths;

namespace Rb.Tesselator
{
	public class TesselatorInput
	{

		/// <summary>
		/// Sets the input polygon boundary
		/// </summary>
		public IEnumerable< Point2 > Polygon
		{
			get { return m_Polygon; }
			set { m_Polygon = value; }
		}

		/// <summary>
		/// Adds an internal contour
		/// </summary>
		public void AddContour( IEnumerable< Point2 > points )
		{
			m_Contours.Add( new Contour( points ) );
		}

		#region Internal members

		/// <summary>
		/// Internal contour class - holds the points of an internal contour
		/// </summary>
		internal class Contour
		{
			public Contour( IEnumerable< Point2 > points )
			{
				m_Points = points;
			}

			public IEnumerable< Point2 > Points
			{
				get { return m_Points; }
			}

			private readonly IEnumerable< Point2 > m_Points;
		}

		internal IEnumerable< Contour > Contours
		{
			get { return m_Contours; }
		}

		#endregion

		#region Private members

		private IEnumerable< Point2 > m_Polygon;
		private readonly List< Contour > m_Contours = new List< Contour >( );

		#endregion
	}
}
