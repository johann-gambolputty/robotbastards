using System.Collections.Generic;

namespace Rb.NiceControls.Graph
{
	/// <summary>
	/// Interface for piecewise linear graphs, defined by a set of control points
	/// </summary>
	public interface IPiecewiseGraph : IGraph
	{
		/// <summary>
		/// Returns true if control points cannot be added or removed from the graph
		/// </summary>
		bool FixedControlPointCount
		{
			get;
		}

		/// <summary>
		/// The control points defining the function
		/// </summary>
		List<ControlPoint> ControlPoints
		{
			get;
		}
	}
}
