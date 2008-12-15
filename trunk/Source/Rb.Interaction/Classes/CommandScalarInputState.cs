using System;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Command input with a scalar value. Used for mouse wheel input
	/// </summary>
	[Serializable]
	public class CommandScalarInputState : ICommandInputState
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="lastValue">Last polled value</param>
		/// <param name="value">Current value</param>
		public CommandScalarInputState( float lastValue, float value )
		{
			m_Value = value;
			m_LastValue = lastValue;
		}

		/// <summary>
		/// Gets the previous value
		/// </summary>
		public float LastValue
		{
			get { return m_LastValue; }
		}

		/// <summary>
		/// Returns the difference between current and previous values
		/// </summary>
		public float Delta
		{
			get { return m_Value - m_LastValue; }
		}

		/// <summary>
		/// Gets the current value
		/// </summary>
		public float Value
		{
			get { return m_Value; }
		}

		/// <summary>
		/// Returns the curernt value as a string
		/// </summary>
		public override string ToString( )
		{
			return m_Value.ToString( );
		}

		#region Private Members

		private readonly float m_Value;
		private readonly float m_LastValue;

		#endregion
	}

}
