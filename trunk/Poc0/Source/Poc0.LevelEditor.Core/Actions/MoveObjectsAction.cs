using System.Collections.Generic;
using Poc0.Core;
using Rb.Core.Components;
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
			List< IHasPosition > frameObjects = new List< IHasPosition >( );

			foreach ( object obj in objects )
			{
				frameObjects.Add( Parent.GetType< IHasPosition >( obj ) );
			}

			m_Objects = objects;
			m_FrameObjects = frameObjects.ToArray( );

			ApplyDelta( delta );
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
			MoveObjects( delta );
			m_Delta += delta;
		}

		#region IAction Members

		/// <summary>
		/// Moves all the objects back to their original positions
		/// </summary>
		public void Undo( )
		{
			MoveObjects( m_Delta );
		}

		/// <summary>
		/// Reapplies movement to all objects
		/// </summary>
		public void Redo( )
		{
			MoveObjects( m_Delta * -1 );
		}

		#endregion

		private Vector3 m_Delta;
		private readonly object[] m_Objects;
		private readonly IHasPosition[] m_FrameObjects;

		private void MoveObjects( Vector3 delta )
		{
			foreach ( IHasPosition frame in m_FrameObjects )
			{
				frame.Position += delta;
			}
			foreach ( ObjectEditState editState in m_Objects )
			{
			    editState.OnObjectChanged( );
			}
		}
	}
}
