using System;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// A vertex used to define <see cref="LevelEdge"/> endpoints
	/// </summary>
	public class LevelVertex : ISelectable, IPickable, IMoveable3
	{
		/// <summary>
		/// Event, raised when the state of this vertex changes
		/// </summary>
		public event Action< LevelVertex > Changed;

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="pt">Initial position of the vertex</param>
		public LevelVertex( Point2 pt )
		{
			m_Position = pt;
			m_StartEdge = null;
			m_EndEdge = null;
		}

		/// <summary>
		/// Edge that this vertex begins
		/// </summary>
		public LevelEdge StartEdge
		{
			get { return m_StartEdge; }
			set { m_StartEdge = value; }
		}

		/// <summary>
		/// Edge that this vertex ends
		/// </summary>
		public LevelEdge EndEdge
		{
			get { return m_EndEdge; }
			set { m_EndEdge = value; }
		}

		/// <summary>
		/// The position of this vertex
		/// </summary>
		public Point2 Position
		{
			get { return m_Position; }
			set
			{
				bool changed = ( m_Position != value );
				m_Position = value;
				if ( ( changed ) && ( Changed != null ) )
				{
					Changed( this );
				}
			}
		}


		#region ISelectable Members

		/// <summary>
		/// Gets/sets the highlighted state of this object
		/// </summary>
		public bool Highlighted
		{
			get { return m_Highlighted; }
			set
			{
				bool changed = ( m_Highlighted != value );
				m_Highlighted = value;
				if ( changed && ( Changed != null ) )
				{
					Changed( this );
				}
			}
		}

		/// <summary>
		/// Gets/sets the selected state of this object
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set
			{
				bool changed = ( m_Selected != value );
				m_Selected = value;
				if ( changed && ( Changed != null ) )
				{
					Changed( this );
				}
			}
		}

		#endregion

		#region IPickable Members

		/// <summary>
		/// Creates the action appropriate for this object
		/// </summary>
		/// <param name="pick">Pick information</param>
		/// <returns>Returns a move action</returns>
		public IPickAction CreatePickAction( ILineIntersection pick )
		{
			return new MoveAction( new RayCastOptions( RayCastLayers.Grid ) );
		}

		/// <summary>
		/// Returns true if the specified action is a move action
		/// </summary>
		/// <param name="action">Action to check</param>
		/// <returns>true if action is a <see cref="MoveAction"/></returns>
		public bool SupportsPickAction( IPickAction action )
		{
			return action is MoveAction;
		}

		#endregion

		#region IMoveable3 Members

		/// <summary>
		/// Moves this object
		/// </summary>
		/// <param name="delta">Movement delta</param>
		public void Move( Vector3 delta )
		{
			m_Position.X += delta.X;
			m_Position.Y += delta.Z;

			if ( ( delta.SqrLength > 0.001f ) && ( Changed != null ) )
			{
				Changed( this );
			}
		}

		#endregion

		#region Private members

		private Point2		m_Position;
		private LevelEdge	m_StartEdge;
		private LevelEdge	m_EndEdge;
		private bool		m_Highlighted;
		private bool		m_Selected;

		#endregion
	}
}
