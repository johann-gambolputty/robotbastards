using System;
using System.Collections.Generic;

namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	/// Piecewise linear function
	/// </summary>
	[Serializable]
	public class LinearDistribution : IDistribution
	{
		/// <summary>
		/// Function control points
		/// </summary>
		public IList<ControlPoint> ControlPoints
		{
			get { return m_ControlPoints; }
		}

		/// <summary>
		/// Samples the distribution function
		/// </summary>
		/// <param name="t">Normalized position</param>
		/// <returns>Function value at t</returns>
		public float Sample( float t )
		{
			if ( m_ControlPoints.Count == 0 )
			{
				return 0;
			}
			if ( t <= m_ControlPoints[ 0 ].Position )
			{
				return m_ControlPoints[ 0 ].Value;
			}
			for ( int i = 0; i < m_ControlPoints.Count - 1; ++i )
			{
				if ( t <= m_ControlPoints[ i + 1 ].Position )
				{
					float minLocalT = m_ControlPoints[ i ].Position;
					float maxLocalT = m_ControlPoints[ i + 1 ].Position;

					float localT = ( t - minLocalT ) / ( maxLocalT - minLocalT );
					return ( m_ControlPoints[ i ].Value * ( 1 - localT ) + m_ControlPoints[ i + 1 ].Value * localT );
				}
			}
			return m_ControlPoints[ m_ControlPoints.Count - 1 ].Value;
		}

		#region Private Members

		private readonly IList<ControlPoint> m_ControlPoints = new List<ControlPoint>( );

		#endregion
	}
}
