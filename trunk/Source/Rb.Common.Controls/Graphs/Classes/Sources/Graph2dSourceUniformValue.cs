using Rb.Common.Controls.Graphs.Classes.Controllers;
using Rb.Common.Controls.Graphs.Classes.Renderers;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Sources
{
	public class Graph2dSourceUniformValue : Graph2dSourceAbstract
	{
		/// <summary>
		/// Axis enumeration
		/// </summary>
		public enum Axis
		{
			X,
			Y
		}

		public Graph2dSourceUniformValue( )
		{
		}

		public Graph2dSourceUniformValue( Axis axis, float value )
		{
			m_FixedAxis = axis;
			m_Value = value;
		}

		/// <summary>
		/// Gets/sets the uniform value
		/// </summary>
		public float Value
		{
			get { return m_Value; }
			set
			{
				bool changed = m_Value != value;
				m_Value = value;
				OnGraphChanged( changed );
			}
		}

		/// <summary>
		/// Gets/sets the fixed axis
		/// </summary>
		public Axis FixedAxis
		{
			get { return m_FixedAxis; }
			set { m_FixedAxis = value; }
		}
		
		/// <summary>
		/// Gets the display value of the graph, when the data cursor is at (x,y)
		/// </summary>
		public override string GetDisplayValueAt( float x, float y )
		{
			return Value.ToString( "G4" );
		}

		/// <summary>
		/// Creates a default renderer for this graph
		/// </summary>
		public override IGraph2dRenderer CreateRenderer( )
		{
			return new Graph2dUniformValueRenderer( );
		}

		/// <summary>
		/// Creates a default controller for this graph
		/// </summary>
		public override IGraph2dController CreateController( )
		{
			return new Graph2dUniformValueController( );
		}

		#region Private Members

		private float m_Value;
		private Axis m_FixedAxis = Axis.X;
		
		#endregion
	}
}
