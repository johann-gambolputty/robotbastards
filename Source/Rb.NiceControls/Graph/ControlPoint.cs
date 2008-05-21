using System;

namespace Rb.NiceControls.Graph
{
	/// <summary>
	/// A control point, used to define a point in a distribution
	/// </summary>
	[Serializable]
	public class ControlPoint
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
}
