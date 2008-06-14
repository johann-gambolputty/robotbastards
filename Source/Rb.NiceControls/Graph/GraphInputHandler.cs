using System;
using System.Drawing;
using Rb.Core.Maths;
using Rectangle=System.Drawing.Rectangle;

namespace Rb.NiceControls.Graph
{
	/// <summary>
	/// Abstract base class for graph input handlers
	/// </summary>
	/// <remarks>
	/// Implements reusable Render() method. Has some handy static utility
	/// functions.
	/// </remarks>
	public class GraphInputHandler : IGraphInputHandler
	{
		/// <summary>
		/// Sets the function to be controller by this handler
		/// </summary>
		public GraphInputHandler( IFunction1d function )
		{
			if ( function == null )
			{
				throw new ArgumentNullException( "function" );
			}
			m_Function = function;
		}

		/// <summary>
		/// Gets the function being controlled by this handler
		/// </summary>
		public IFunction1d Function
		{
			get { return m_Function; }
		}

		/// <summary>
		/// Creates a graph input handler for a given function
		/// </summary>
		public static IGraphInputHandler CreateHandlerForFunction( IFunction1d function )
		{
			if ( function is PiecewiseLinearFunction1d )
			{
				return new PiecewiseGraphInputHandler( ( PiecewiseLinearFunction1d )function );
			}
			return new GraphInputHandler( function );
		}

		#region IGraphInputHandler Members

		public virtual void Attach( IGraphControl control )
		{
		}

		public virtual void Detach( IGraphControl control )
		{
		}

		public virtual void Render( Rectangle bounds, Graphics graphics )
		{
			float yOffset = bounds.Bottom;
			float graphHeight = bounds.Height;
			using ( Pen graphPen = new Pen( Color.Red, 1.5f ) )
			{
				float y0 = yOffset - m_Function.GetValue( 0 ) * graphHeight;
				float t = 0;
				float tInc = 1.0f / ( bounds.Width - 1 );
				for ( int sample = 0; sample < bounds.Width; ++sample, t += tInc )
				{
					float y1 = yOffset - m_Function.GetValue( t ) * graphHeight;
					float x0 = sample + bounds.Left;
					float x1 = x0 + 1;
					graphics.DrawLine( graphPen, x0, y0, x1, y1 );
					y0 = y1;
				}
			}

		}

		#endregion

		#region Private Members

		private readonly IFunction1d m_Function;

		#endregion
	}
}
