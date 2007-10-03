using System;
using System.Collections;
using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	public class MoveAction : IPickAction
	{
		#region IPickAction Members

		/// <summary>
		/// Adds objects to this action
		/// </summary>
		/// <param name="objects">Objects to add</param>
		public void AddObjects( IEnumerable objects )
		{
			foreach ( object obj in objects )
			{
				if ( obj is IMoveable3 )
				{
					m_Movers.Add( ( IMoveable3 )obj );
				}
			}
		}

		/// <summary>
		/// Called when the pick info changes
		/// </summary>
		/// <param name="lastPick">Last pick information</param>
		/// <param name="curPick">Current pick information</param>
		public void PickChanged( PickInfoCursor lastPick,  PickInfoCursor curPick )
		{
			if ( lastPick is PickInfoRay3 )
			{
				PickInfoRay3 lastPick3 = ( PickInfoRay3 )lastPick;
				PickInfoRay3 curPick3 = ( PickInfoRay3 )curPick;

				Vector3 delta = curPick3.Intersection.IntersectionPosition - lastPick3.Intersection.IntersectionPosition;
				if ( delta.SqrLength > 0 )
				{
					m_Delta += delta;
					MoveObjects( delta );
				}
			}
			else
			{
				throw new NotImplementedException( string.Format( "Unhandled pick type \"{0}\"", lastPick.GetType( ) ) );
			}
		}

		/// <summary>
		/// Returns true if PickChanged() was called with arguments that altered the states of the attached objects
		/// </summary>
		public bool HasModifiedObjects
		{
			get { return m_Modified; }
		}

		#endregion

		#region IAction Members

		/// <summary>
		/// Applies the stored movement delta to all objects
		/// </summary>
		public void Undo( )
		{
			MoveObjects( m_Delta );
		}

		/// <summary>
		/// Applies the inverse of the stored movement delta to all objects
		/// </summary>
		public void Redo( )
		{
			MoveObjects( -m_Delta );
		}

		#endregion

		#region Private members

		private Vector3 m_Delta;
		private readonly List< IMoveable3 > m_Movers = new List< IMoveable3 >( );
		private bool m_Modified;
		
		/// <summary>
		/// Moves all stored objects by the specified delta
		/// </summary>
		/// <param name="delta">Movement delta</param>
		private void MoveObjects( Vector3 delta )
		{
			m_Modified = true;
			foreach ( IMoveable3 moveable in m_Movers )
			{
				moveable.Move( delta );
			}
		}

		#endregion
	}
}
