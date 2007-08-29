using System;
using System.Collections.Generic;
using System.Text;
using Poc0.Core;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core.Actions
{
	/// <summary>
	/// Moves a set of objects by a given delta
	/// </summary>
	public class MoveObjectsAction : IAction
	{
		/// <summary>
		/// Moves the objects
		/// </summary>
		/// <param name="objects">Objects to move</param>
		/// <param name="delta">Movement delta</param>
		public MoveObjectsAction( object[] objects, Vector3 delta )
		{
			foreach ( IHasWorldFrame frame in objects )
			{
				frame.WorldFrame.Translation += delta;
			}
			m_Delta = delta;
			m_Objects = objects;
		}

		/// <summary>
		/// Returns the distance moved
		/// </summary>
		public float MovementDistance
		{
			get { return m_Delta.Length; }
		}

		/// <summary>
		/// Applies an extra movement to the stored objects
		/// </summary>
		/// <param name="delta">Extra movement vector</param>
		public void ApplyDelta( Vector3 delta )
		{
			foreach ( IHasWorldFrame frame in m_Objects )
			{
				frame.WorldFrame.Translation += delta;
			}
			m_Delta += delta;
		}

		#region IAction Members

		/// <summary>
		/// Moves all the objects back to their original positions
		/// </summary>
		public void Undo( )
		{
			foreach ( IHasWorldFrame frame in m_Objects )
			{
				frame.WorldFrame.Translation -= m_Delta;
			}
		}

		/// <summary>
		/// Reapplies movement to all objects
		/// </summary>
		public void Redo( )
		{
			foreach ( IHasWorldFrame frame in m_Objects )
			{
				frame.WorldFrame.Translation += m_Delta;
			}
		}

		#endregion

		private Vector3 m_Delta;
		private readonly object[] m_Objects;

	}
}
