using System.Drawing;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Interfaces
{
	/// <summary>
	/// Contains interfaces used by the <see cref="IDraw"/> interface to encapsulate rendering properties
	/// </summary>
	/// <remarks>
	/// A whole bunch of predefined pens, brushes and surfaces can be found in <see cref="Graphics"/>.
	/// </remarks>
	public class Draw
	{
		/// <summary>
		/// Underlying state support for pens, brushes and surfaces
		/// </summary>
		public interface IState : IPass
		{
			/// <summary>
			/// Gets the underlying render state
			/// </summary>
			IRenderState State
			{
				get;
			}
		}

		/// <summary>
		/// Shape drawing pen interface
		/// </summary>
		public interface IPen : IState
		{
			/// <summary>
			/// Clones this pen
			/// </summary>
			IPen Clone( );

			/// <summary>
			/// The pen colour
			/// </summary>
			Color Colour
			{
				get; set;
			}

			/// <summary>
			/// Pen thickness
			/// </summary>
			float Thickness
			{
				get; set;
			}
		}

		/// <summary>
		/// Shape drawing brush interface
		/// </summary>
		public interface IBrush : IState
		{
			/// <summary>
			/// Clones this brush
			/// </summary>
			IBrush Clone( );

			/// <summary>
			/// The brush colour
			/// </summary>
			Color Colour
			{
				get; set;
			}

			/// <summary>
			/// Outline pen. If not null, then an outline is drawn around the rendered shape
			/// </summary>
			IPen OutlinePen
			{
				get; set;
			}
		}

		/// <summary>
		/// Shape drawing surface interface
		/// </summary>
		public interface ISurface : IState
		{
			/// <summary>
			/// Clones this surface
			/// </summary>
			ISurface Clone( );

			/// <summary>
			/// Gets/sets the brush used to fill the surface faces. If null, faces aren't rendered
			/// </summary>
			IBrush FaceBrush
			{
				get; set;
			}

			/// <summary>
			/// Gets/sets the pen used to draw face edges. If null, wireframe isn't rendered
			/// </summary>
			IPen EdgePen
			{
				get; set;
			}
		}
	}
}
