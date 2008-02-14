using System;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Polygon level geometry object
	/// </summary>
	public class LevelPolygon : ILevelGeometryObject
	{
		/// <summary>
		/// Sets up this polygon
		/// </summary>
		/// <param name="vertices">Polygon vertices</param>
		/// <param name="edges">Polygon edges</param>
		public LevelPolygon( LevelVertex[] vertices, LevelEdge[] edges )
		{
			m_Vertices = vertices;
			m_Edges = edges;
		}

		/// <summary>
		/// Gets/sets polygon vertices
		/// </summary>
		public LevelVertex[] Vertices
		{
			get { return m_Vertices; }
			set { m_Vertices = value; }
		}

		/// <summary>
		/// Gets/sets polygon edges
		/// </summary>
		public LevelEdge[] Edges
		{
			get { return m_Edges; }
			set { m_Edges = value; }
		}
		
		#region ILevelGeometryObject Members

		/// <summary>
		/// Adds this object to a level geometry instance
		/// </summary>
		public void AddToLevel( LevelGeometry level )
		{
			level.Add( this );
		}

		/// <summary>
		/// Removes this object from a level geometry instance
		/// </summary>
		public void RemoveFromLevel( LevelGeometry level )
		{
			level.Remove( this );
		}

		#endregion

		#region Private members

		private LevelVertex[]	m_Vertices;
		private LevelEdge[]		m_Edges;

		#endregion

	}

}
