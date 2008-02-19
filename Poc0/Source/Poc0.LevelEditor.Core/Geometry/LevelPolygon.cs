
using System;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Polygon level geometry object
	/// </summary>
	[Serializable]
	public class LevelPolygon : ILevelGeometryObject, ISelectable, IPickable, IMoveable3, ISelectionModifier, IDelete
	{
		/// <summary>
		/// Event, raised when the state of this polygon changes
		/// </summary>
		public event Action< LevelPolygon > Changed;

		/// <summary>
		/// Sets up this polygon
		/// </summary>
		/// <param name="vertices">Polygon vertices</param>
		/// <param name="edges">Polygon edges</param>
		public LevelPolygon( LevelVertex[] vertices, LevelEdge[] edges )
		{
			m_Vertices = vertices;
			m_Edges = edges;
		}

		/// <summary>
		/// Gets/sets polygon vertices
		/// </summary>
		public LevelVertex[] Vertices
		{
			get { return m_Vertices; }
			set { m_Vertices = value; }
		}

		/// <summary>
		/// Gets/sets polygon edges
		/// </summary>
		public LevelEdge[] Edges
		{
			get { return m_Edges; }
			set { m_Edges = value; }
		}
		
		#region ILevelGeometryObject Members

		/// <summary>
		/// Adds this object to a level geometry instance
		/// </summary>
		public void AddToLevel( LevelGeometry level )
		{
			level.Add( this );
		}

		/// <summary>
		/// Removes this object from a level geometry instance
		/// </summary>
		public void RemoveFromLevel( LevelGeometry level )
		{
			level.Remove( this );
		}

		#endregion

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
			foreach ( LevelVertex vertex in m_Vertices )
			{
				vertex.Move( delta );
			}
		}

		#endregion

		#region Private members

		private bool 						m_Highlighted;
		private bool 						m_Selected;
		private LevelVertex[]				m_Vertices;
		private LevelEdge[]					m_Edges;
		private readonly ObstacleProperties	m_Properties = new ObstacleProperties( );

		#endregion


		#region ISelectionModifier Members

		/// <summary>
		/// Gets the obstacle properties
		/// </summary>
		public object SelectedObject
		{
			get { return m_Properties; }
		}

		#endregion

		#region IDelete Members

		/// <summary>
		/// Adds this polygon to the scene
		/// </summary>
		public void UnDelete( )
		{
			LevelGeometry geometry = LevelGeometry.FromScene( EditorState.Instance.CurrentScene );
			AddToLevel( geometry );
		}

		/// <summary>
		/// Removes this polygon from the scene
		/// </summary>
		public void Delete( )
		{
			LevelGeometry geometry = LevelGeometry.FromScene( EditorState.Instance.CurrentScene );
			RemoveFromLevel( geometry );
		}

		#endregion
	}

}
