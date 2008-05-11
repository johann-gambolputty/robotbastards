using System.Collections.Generic;

namespace Poc1.Tools.TerrainTextures.Core
{
	public interface IDistribution
	{
		IList<ControlPoint> ControlPoints
		{
			get;
		}

		float Sample( float t );
	}
}
