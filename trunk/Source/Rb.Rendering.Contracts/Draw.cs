using System.Drawing;
using Rb.Rendering.Contracts.Objects;

namespace Rb.Rendering.Contracts
{
	/// <summary>
	/// Contains interfaces used by the <see cref="IDraw"/> interface to encapsulate rendering properties
	/// </summary>
	public class Draw
	{

		#region Pre-defined pens, brushes and surfaces

		/// <summary>
		/// Pre-defined pens
		/// </summary>
		public static class Pens
		{
			public static readonly IPen White	= Graphics.Draw.NewPen( Color.White );
			public static readonly IPen Red		= Graphics.Draw.NewPen( Color.Red );
			public static readonly IPen Green	= Graphics.Draw.NewPen( Color.Green );
			public static readonly IPen Blue	= Graphics.Draw.NewPen( Color.Blue );
			public static readonly IPen Black	= Graphics.Draw.NewPen( Color.Black );
		}
		
		/// <summary>
		/// Pre-defined brushes
		/// </summary>
		public static class Brushes
		{
			public static readonly IBrush White	= Graphics.Draw.NewBrush( Color.White );
			public static readonly IBrush Red	= Graphics.Draw.NewBrush( Color.Red );
			public static readonly IBrush Green	= Graphics.Draw.NewBrush( Color.Green );
			public static readonly IBrush Blue	= Graphics.Draw.NewBrush( Color.Blue );
			public static readonly IBrush Black	= Graphics.Draw.NewBrush( Color.Black );
		}
		
		/// <summary>
		/// Pre-defined surfaces
		/// </summary>
		public static class Surfaces
		{
			public static readonly ISurface White	= Graphics.Draw.NewSurface( Color.White );
			public static readonly ISurface Red		= Graphics.Draw.NewSurface( Color.Red );
			public static readonly ISurface Green	= Graphics.Draw.NewSurface( Color.Green );
			public static readonly ISurface Blue	= Graphics.Draw.NewSurface( Color.Blue );
			public static readonly ISurface Black	= Graphics.Draw.NewSurface( Color.Black );
		}

		#endregion

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
			/// Gets/sets the brush used to fill the surface faces. If null, faces aren't rendered
			/// </summary>
			IBrush FaceBrush
			{
				get; set;
			}

			/// <summary>
			/// Gets/sets the pen used to draw face edges. If null, wireframe isn't rendered
			/// </summary>
			bool EdgePen
			{
				get; set;
			}
		}
	}
}
