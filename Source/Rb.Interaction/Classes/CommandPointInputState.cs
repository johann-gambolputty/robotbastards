using System;
using Rb.Interaction.Interfaces;
using Rb.Core.Maths;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Command input with a point value. Used for mouse cursor input. Point domain is unit square centered on (0.5,0.5)
	/// </summary>
	[Serializable]
	public class CommandPointInputState : ICommandInputState
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="lastPos">Last position</param>
		/// <param name="curPos">Current position</param>
		public CommandPointInputState( Point2 lastPos, Point2 curPos )
		{
			m_LastPos = lastPos;
			m_CurPos = curPos;
		}

		/// <summary>
		/// Gets the last position
		/// </summary>
		public Point2 LastPos
		{
			get { return m_LastPos; }
		}


		/// <summary>
		/// Returns the difference between current and previous values
		/// </summary>
		public Vector2 Delta
		{
			get { return m_CurPos - m_LastPos; }
		}


		/// <summary>
		/// Gets the current position
		/// </summary>
		public Point2 CurPos
		{
			get { return m_CurPos; }
		}

		/// <summary>
		/// Converts to string
		/// </summary>
		public override string ToString( )
		{
			return CurPos.ToString( );
		}

		#region Private Members

		private readonly Point2 m_LastPos;
		private readonly Point2 m_CurPos;

		#endregion
	}

}
