
using System;
using System.Collections;
using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	/// <summary>
	/// Action to turn objects to face a point. Works with the <see cref="IOrientable3"/> interface
	/// </summary>
	public class FaceAction : IPickAction
	{
		/// <summary>
		/// Sets up this action
		/// </summary>
		/// <param name="options">Pick raycast options</param>
		public FaceAction( RayCastOptions options )
		{
			m_PickOptions = options;
		}

		#region IPickAction Members

		/// <summary>
		/// Gets this action's pick raycast options
		/// </summary>
		public RayCastOptions PickOptions
		{
			get { return m_PickOptions; }
		}

		/// <summary>
		/// Adds objects to this action
		/// </summary>
		/// <param name="objects">Objects to add</param>
		public void AddObjects( IEnumerable objects )
		{
			foreach ( object obj in objects )
			{
				IOrientable3 orientable = obj as IOrientable3;
				if ( orientable != null )
				{
					m_Objects.Add( orientable );
					m_OriginalOrientations.Add( orientable.Orientation );
				}
			}
		}

		/// <summary>
		/// Called when the pick info changes
		/// </summary>
		/// <param name="lastPick">Last pick information</param>
		/// <param name="curPick">Current pick information</param>
		public void PickChanged( ILineIntersection lastPick,  ILineIntersection curPick )
		{
			if ( lastPick is Line3Intersection )
			{
				Line3Intersection lastPick3 = ( Line3Intersection )lastPick;
				Line3Intersection curPick3 = ( Line3Intersection )curPick;

				Vector3 delta = curPick3.IntersectionPosition - lastPick3.IntersectionPosition;
				if ( delta.SqrLength > 0 )
				{
					m_InputPos = curPick3.IntersectionPosition;
					TurnObjects( m_InputPos );
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
		/// Turns all objects back to their original orientations
		/// </summary>
		public void Undo( )
		{
			RestoreObjectOrientations( );
		}

		/// <summary>
		/// Turns all objects to face the stored position
		/// </summary>
		public void Redo( )
		{
			TurnObjects( m_InputPos );
		}

		#endregion

		#region Private members

		private readonly RayCastOptions			m_PickOptions;
		private Point3							m_InputPos;
		private readonly List< IOrientable3 >	m_Objects = new List< IOrientable3 >( );
		private readonly List< float >			m_OriginalOrientations = new List< float >( );
		private bool							m_Modified;
		
		/// <summary>
		/// Moves all stored objects by the specified delta
		/// </summary>
		/// <param name="inputPos">Input position that defines the delta over time</param>
		private void TurnObjects( Point3 inputPos )
		{
			m_Modified = true;
			foreach ( IOrientable3 orientable in m_Objects )
			{
				orientable.Face( inputPos );
			}
		}

		/// <summary>
		/// Restores all objects to their original orientations
		/// </summary>
		private void RestoreObjectOrientations( )
		{
			for ( int index = 0; index < m_Objects.Count; ++index )
			{
				m_Objects[ index ].Orientation = m_OriginalOrientations[ index ];
			}
		}

		#endregion
	}
}
