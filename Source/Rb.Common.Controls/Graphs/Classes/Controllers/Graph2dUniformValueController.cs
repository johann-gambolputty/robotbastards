using System;
using Rb.Common.Controls.Graphs.Classes.Sources;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Controllers
{
	/// <summary>
	/// Controller for the <see cref="Graph2dSourceUniformValue"/> type
	/// </summary>
	public class Graph2dUniformValueController : Graph2dControllerBase
	{
		/// <summary>
		/// Screen selection distance
		/// </summary>
		public const float ScreenSelectionTolerance = 3.0f;

		/// <summary>
		/// Called by the control owner when the left mouse button is pressed
		/// </summary>
		/// <param name="data">Graph data</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="dataX">Mouse X position in data space</param>
		/// <param name="dataY">Mouse Y position in data space</param>
		/// <param name="down">True if the button was pressed, false if the button was released</param>
		/// <returns>Returns true if the event was handled, and the control owner should perform no further processing</returns>
		public override bool OnMouseLeftButton( IGraph2dSource data, GraphTransform transform, float dataX, float dataY, bool down )
		{
			if ( !down || data.Disabled )
			{
				m_Moving = false;
				return false;
			}
			float selectionDistance = transform.ScreenToDataYScale * ScreenSelectionTolerance;

			Graph2dSourceUniformValue uniformData = (Graph2dSourceUniformValue)data;
			float value = uniformData.FixedAxis == Graph2dSourceUniformValue.Axis.X ? dataX : dataY;
			m_Moving = Math.Abs( value - uniformData.Value ) < selectionDistance;

			return false;
		}
		
		/// <summary>
		/// Called by the control owner when the mouse is moved
		/// </summary>
		/// <param name="data">Graph data</param>
		/// <param name="transform">Graph view transform</param>
		/// <param name="lastDataX">Previous mouse X position in data space</param>
		/// <param name="lastDataY">Previous mouse Y position in data space</param>
		/// <param name="dataX">Mouse X position in data space</param>
		/// <param name="dataY">Mouse Y position in data space</param>
		/// <returns>Returns true if the event was handled, and the control owner should perform no further processing</returns>
		public override bool OnMouseMove( IGraph2dSource data, GraphTransform transform, float lastDataX, float lastDataY, float dataX, float dataY )
		{
			if ( data.Disabled || !m_Moving )
			{
				return false;
			}

			Graph2dSourceUniformValue uniformData = ( Graph2dSourceUniformValue )data;
			if ( uniformData.FixedAxis == Graph2dSourceUniformValue.Axis.X )
			{
				uniformData.Value += dataX - lastDataX;
			}
			else
			{
				uniformData.Value += dataY - lastDataY;
			}

			return false;
		}

		#region Private Members

		private bool m_Moving;
		
		#endregion
	}
}
