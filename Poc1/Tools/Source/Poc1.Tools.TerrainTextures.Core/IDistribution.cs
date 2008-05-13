using System.Collections.Generic;

namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	/// Interface for a distribution function
	/// </summary>
	public interface IDistribution
	{
		/// <summary>
		/// Gets the list of control points that parameterize the distribution function
		/// </summary>
		IList<ControlPoint> ControlPoints
		{
			get;
		}

		/// <summary>
		/// Samples the distribution function
		/// </summary>
		/// <param name="t">Normalized position</param>
		/// <returns>Function value at t</returns>
		float Sample( float t );
	}
}
