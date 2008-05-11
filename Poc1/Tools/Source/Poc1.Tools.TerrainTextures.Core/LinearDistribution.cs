
using System.Collections.Generic;

namespace Poc1.Tools.TerrainTextures.Core
{
	public class LinearDistribution : IDistribution
	{
		public IList<ControlPoint> ControlPoints
		{
			get { return m_ControlPoints; }
		}
		public float Sample( IList<ControlPoint> points, float t )
		{
			if ( t <= points[ 0 ].Position )
			{
				return points[ 0 ].Value;
			}
			for ( int i = 0; i < points.Count - 1; ++i )
			{
				if ( t <= points[ i + 1 ].Position )
				{
					float minLocalT = points[ i ].Position;
					float maxLocalT = points[ i + 1 ].Position;

					float localT = ( t - minLocalT ) / ( maxLocalT - minLocalT );
					return ( points[ i ].Value * ( 1 - localT ) + points[ i + 1 ].Value * localT );
				}
			}
			return points[ points.Count - 1 ].Value;
		}

		private IList<ControlPoint> ControlPoints = new List<COntrol
	}
}
