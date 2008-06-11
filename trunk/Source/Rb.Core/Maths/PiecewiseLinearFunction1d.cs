
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

		/// <summary>
		/// Gets the list of control points defining this function
		/// </summary>
		public List<ControlPoint> ControlPoints
		{
			get { return m_ControlPoints; }
		}

		#region IFunction1d Members

		/// <summary>
		/// Gets a value for this function at a specified point
		/// </summary>
		public abstract float GetValue( float t );

		#endregion

		#region Private Members

		private readonly List<ControlPoint> m_ControlPoints = new List<ControlPoint>( ); 
		
		#endregion
	}
}
