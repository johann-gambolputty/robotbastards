using System.Collections.Generic;
using Poc0.Core.Objects;
using Rb.Core.Maths;

namespace Poc0.Core.Environment
{
	/// <summary>
	/// AI path planner
	/// </summary>
	public class Navigator : INavigator
	{
		/// <summary>
		/// Path node
		/// </summary>
		public class Node
		{
			public Node InFront
			{
				get { return m_InFront; }
				set { m_InFront = value; }
			}

			public Node Behind
			{
				get { return m_Behind; }
				set { m_Behind = value; }
			}

			public Plane2 Plane
			{
				get { return m_Plane;  }
			}

			public Region GetRegion( float x, float y )
			{
				if ( m_Plane.ClassifyPoint( x, y, 0.0001f ) == PlaneClassification.Behind )
				{
					return ( m_Behind == null ) ? null : m_Behind.GetRegion( x, y );
				}

				return ( m_InFront == null ) ? m_Region : m_InFront.GetRegion( x, y );
			}

			private Plane2 m_Plane;
			private Region m_Region;
			private Node m_InFront;
			private Node m_Behind;
		}

		/// <summary>
		/// Connects two regions
		/// </summary>
		public class RegionConnector
		{
			public RegionConnector( Region region0, Region region1 )
			{
				m_Region0 = region0;
				m_Region1 = region1;
			}

			private readonly Region m_Region0;
			private readonly Region m_Region1;
		}

		/// <summary>
		/// Leaf node region
		/// </summary>
		public class Region
		{
			public Region( RegionConnector[] connectors )
			{
				m_Connectors = connectors;
			}

			private RegionConnector[] m_Connectors;
		}


		#region INavigator Members

		/// <summary>
		/// Creates a path between two points
		/// </summary>
		public IPath CreatePath( Point3 start, Point3 end )
		{
			return null;
		}

		#endregion

	}
}
