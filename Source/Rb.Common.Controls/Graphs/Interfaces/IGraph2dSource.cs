
using System;

namespace Rb.Common.Controls.Graphs.Interfaces
{


	/// <summary>
	/// Graph data interface
	/// </summary>
	public interface IGraph2dSource
	{
		/// <summary>
		/// Graph changed event
		/// </summary>
		event EventHandler GraphChanged;

		/// <summary>
		/// Gets/sets the graph disabled flag
		/// </summary>
		bool Disabled
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the highlight state of the graph
		/// </summary>
		bool Highlighted
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the selected state of the graph
		/// </summary>
		bool Selected
		{
			get; set;
		}

		/// <summary>
		/// Minimum X value
		/// </summary>
		float MinimumX
		{
			get;
		}

		/// <summary>
		/// Maximum X value
		/// </summary>
		float MaximumX
		{
			get;
		}

		/// <summary>
		/// Minimum Y value
		/// </summary>
		float MinimumY
		{
			get;
		}

		/// <summary>
		/// Maximum Y value
		/// </summary>
		float MaximumY
		{
			get;
		}

		/// <summary>
		/// Checks if a point in data space hits the graph
		/// </summary>
		bool IsHit( float x, float y, float tolerance );

		/// <summary>
		/// Gets the display value of the graph, when the data cursor is at (x,y)
		/// </summary>
		string GetDisplayValueAt( float x, float y );

		/// <summary>
		/// Raises the GraphChanged event
		/// </summary>
		void OnGraphChanged( );

		/// <summary>
		/// Creates a controller for this graph.
		/// </summary>
		IGraph2dController CreateController( );

		/// <summary>
		/// Creates a renderer for this graph
		/// </summary>
		IGraph2dRenderer CreateRenderer( );
	}
}
