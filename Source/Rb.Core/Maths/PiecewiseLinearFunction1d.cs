
using System;
using System.Collections.Generic;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Abstract base class for piecewise linear functions
	/// </summary>
	[Serializable]
	public abstract class PiecewiseLinearFunction1d : IFunction1d
	{
		/// <summary>
		/// Returns a string representing this object
		/// </summary>
		public override string ToString( )
		{
			return "Piecewise Linear Function";
		}

		/// <summary>
		/// Returns the number of control points
		/// </summary>
		public int NumControlPoints
		{
			get { return m_ControlPoints.Count; }
		}

		/// <summary>
		/// Gets/sets an indexed control point
		/// </summary>
		public Point2 this[ int index ]
		{
			get { return m_ControlPoints[ index ]; }
			set
			{
				m_ControlPoints[ index ] = value;
				OnParametersChanged( );
			}
		}

		/// <summary>
		/// Gets the list of control points defining this function
		/// </summary>
		public IEnumerable<Point2> ControlPoints
		{
			get { return m_ControlPoints; }
		}

		/// <summary>
		/// Adds a control point to the end of the control point list
		/// </summary>
		public void AddControlPoint( Point2 cp )
		{
			m_ControlPoints.Add( cp );
			OnParametersChanged( );
		}

		/// <summary>
		/// Inserts a control point into the control point list
		/// </summary>
		public void InsertControlPoint( int index, Point2 cp )
		{
			m_ControlPoints.Insert( index, cp );
			OnParametersChanged( );
		}
		
		/// <summary>
		/// Removes a control point from the control point list
		/// </summary>
		public void RemoveControlPoint( int index )
		{
			m_ControlPoints.RemoveAt( index );
			OnParametersChanged( );
		}

		/// <summary>
		/// Finds the index of a control point that is within the distance to point pt
		/// </summary>
		public int FindControlPoint( Point2 pt, float distance )
		{
			float sqrDist = distance * distance;
			for ( int cpIndex = 0; cpIndex < m_ControlPoints.Count; ++cpIndex )
			{
				if ( pt.SqrDistanceTo( m_ControlPoints[ cpIndex ] ) < sqrDist )
				{
					return cpIndex;
				}
			}
			return -1;
		}

		#region IFunction1d Members

		/// <summary>
		/// Event, invoked when the parameters of this function are changed
		/// </summary>
		public event Action<IFunction1d> ParametersChanged
		{
			add { m_ParametersChanged += value; }
			remove { m_ParametersChanged -= value; }
		}

		/// <summary>
		/// Gets a value for this function at a specified point
		/// </summary>
		public abstract float GetValue( float t );

		#endregion

		#region Protected Members

		protected void OnParametersChanged( )
		{
			if ( m_ParametersChanged != null )
			{
				m_ParametersChanged( this );
			}
		}

		#endregion

		#region Private Members

		private readonly List<Point2> m_ControlPoints = new List<Point2>( );

		[NonSerialized]
		private Action<IFunction1d> m_ParametersChanged;

		#endregion
	}
}
