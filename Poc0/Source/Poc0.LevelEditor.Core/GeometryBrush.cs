using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// A brush for editing level geometry
	/// </summary>
	public class GeometryBrush
	{
		/// <summary>
		/// A named polygonal brush
		/// </summary>
		/// <param name="name">Brush name</param>
		/// <param name="points">Brush polygon points</param>
		public GeometryBrush( string name, Point2[] points )
		{
			m_Name = name;
			m_Points = points;
		}

		/// <summary>
		/// Gets the name of this brush
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the points of this brush's polygon
		/// </summary>
		public Point2[] Points
		{
			get { return m_Points; }
		}

		private readonly string m_Name;
		private readonly Point2[] m_Points;
	}
}
