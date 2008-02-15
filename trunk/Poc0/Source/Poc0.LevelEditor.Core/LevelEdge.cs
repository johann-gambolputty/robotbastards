using System;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Poc0.LevelEditor.Core
{
	public class LevelEdge : ISelectable, IPickable, IMoveable3
	{
		/// <summary>
		/// Event, raised when the state of this edge changes
		/// </summary>
		public event Action< LevelEdge > Changed;

		public LevelEdge( LevelVertex start, LevelVertex end, LevelPolygon owner, bool doubleSided )
		{
			m_Start = start;
			m_End = end;
			m_Owner = owner;
			m_DoubleSided = doubleSided;

			m_Start.StartEdge = this;
			m_End.EndEdge = this;
		}

		public bool IsDoubleSided
		{
			get { return m_DoubleSided; }
		}

		public LevelPolygon Polygon
		{
			get { return m_Owner; }
		}

		public LevelVertex Start
		{
			get { return m_Start; }
			set { m_Start = value; }
		}

		public LevelVertex End
		{
			get { return m_End; }
			set { m_End = value; }
		}

		public LevelEdge PreviousEdge
		{
			get { return m_PrevEdge; }
			set { m_PrevEdge = value; }
		}

		public LevelEdge NextEdge
		{
			get { return m_NextEdge; }
			set { m_NextEdge = value; }
		}

		public static void LinkEdges( LevelEdge edge0, LevelEdge edge1 )
		{
			edge0.NextEdge = edge1;
			edge1.PreviousEdge = edge0;
		}

		public float SqrDistanceTo( Point2 pt )
		{
			Vector2 lineVec = m_End.Position - m_Start.Position;
			float sqrLength = lineVec.SqrLength;
			float t = 0.0f;

			if ( sqrLength != 0.0f )
			{
				t = Utils.Clamp( ( pt - m_Start.Position ).Dot( lineVec ) / sqrLength, 0.0f, 1.0f );
			}

			Point2 closestPt = m_Start.Position + ( lineVec * t );
			return closestPt.SqrDistanceTo( pt );
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
			m_Start.Move( delta );
			m_End.Move( delta );
		}

		#endregion
		#region Private members

		private LevelEdge 				m_PrevEdge;
		private LevelEdge 				m_NextEdge;
		private LevelVertex				m_Start;
		private LevelVertex				m_End;
		private readonly bool			m_DoubleSided;
		private readonly LevelPolygon	m_Owner;
		private bool 					m_Highlighted;
		private bool 					m_Selected;

		#endregion
	}
}
