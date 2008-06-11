
using System;
using System.Collections.Generic;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Abstract base class for piecewise linear functions
	/// </summary>
	public abstract class PiecewiseLinearFunction1d : IFunction1d
	{
		#region ControlPoint Struct

		/// <summary>
		/// Function control point
		/// </summary>
		[Serializable]
		public struct ControlPoint
		{
			/// <summary>
			/// Setup construction
			/// </summary>
			/// <param name="position">Control point position (normalized)</param>
			/// <param name="value">Control point value (normalized)</param>
			public ControlPoint( float position, float value )
			{
				m_Position = position;
				m_Value = value;
			}

			/// <summary>
			/// Sets/gets the normalized value of this control point
			/// </summary>
			public float Value
			{
				get { return m_Value; }
				set { m_Value = value < 0 ? 0 : ( value > 1 ? 1 : value ); }
			}

			/// <summary>
			/// Sets/gets the normalized position of this control point
			/// </summary>
			public float Position
			{
				get { return m_Position; }
				set { m_Position = value < 0 ? 0 : ( value > 1 ? 1 : value ); }
			}

			#region Private Members

			private float m_Position;
			private float m_Value;

			#endregion
		}
		 
		#endregion

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
		public ControlPoint this[ int index ]
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
		public IEnumerable<ControlPoint> ControlPoints
		{
			get { return m_ControlPoints; }
		}

		/// <summary>
		/// Adds a control point to the end of the control point list
		/// </summary>
		public void AddControlPoint( ControlPoint cp )
		{
			m_ControlPoints.Add( cp );
			OnParametersChanged( );
		}

		/// <summary>
		/// Inserts a control point into the control point list
		/// </summary>
		public void InsertControlPoint( int index, ControlPoint cp )
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

		#region IFunction1d Members

		/// <summary>
		/// Event, invoked when the parameters of this function are changed
		/// </summary>
		public event Action<IFunction1d> ParametersChanged;

		/// <summary>
		/// Gets a value for this function at a specified point
		/// </summary>
		public abstract float GetValue( float t );

		#endregion

		#region Protected Members

		protected void OnParametersChanged( )
		{
			if ( ParametersChanged != null )
			{
				ParametersChanged( this );
			}
		}

		#endregion

		#region Private Members

		private readonly List<ControlPoint> m_ControlPoints = new List<ControlPoint>( ); 
		
		#endregion
	}
}
