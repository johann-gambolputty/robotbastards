using System;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Graphics=Rb.Rendering.Graphics;
using Rb.World.Services;

namespace Poc0.LevelEditor.Core.Objects
{
	/// <summary>
	/// For changing the position of a game object
	/// </summary>
	[Serializable]
	public class PositionModifier : IRay3Intersector, IPickable, IMoveable3, IRenderable, ISelectable, ISelectionModifier
	{
		#region Public members

		/// <summary>
		/// Sets up this modifier
		/// </summary>
		/// <param name="owner">Positioning interface of the game object</param>
		/// <param name="pos">Position to place the new object at</param>
		public PositionModifier( object owner, Point3 pos )
		{
			m_Owner = owner;
			Position = pos;

			IRayCastService rayCaster = EditorState.Instance.CurrentScene.GetService< IRayCastService >( );
			rayCaster.AddIntersector( RayCastLayers.Entity, this );
		}

		/// <summary>
		/// Event, invoked when the position changes
		/// </summary>
		public event EventHandler Changed;

		/// <summary>
		/// Gets/setse the position of the associated placeable object
		/// </summary>
		public Point3 Position
		{
			get { return m_Pos; }
			set
			{
				m_Pos = value;
				m_Plane.Set( m_Pos + new Vector3( 0, 0.1f, 0 ), Vector3.YAxis );

				OnChanged( );
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
			//	TODO: AP: Correct move action parameters
			return new MoveAction( new RayCastOptions( RayCastLayers.StaticGeometry ) );
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
		/// <param name="inputPos">Input position that defines the delta over time</param>
		public void Move( Vector3 delta, Point3 inputPos )
		{
			Position += delta;
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public virtual void Render( IRenderContext context )
		{
			Draw.IBrush drawProps = m_DrawUnselected;
			if ( Selected )
			{
				drawProps = m_DrawSelected;
			}
			else if ( Highlighted )
			{
				drawProps = m_DrawHighlight;
			}
			Graphics.Draw.Circle( drawProps, Position.X, Position.Y + 0.1f, Position.Z, Radius, 12 );
		}

		#endregion

		#region ISelectable Members

		/// <summary>
		/// Highlight flag
		/// </summary>
		public bool Highlighted
		{
			get
			{
				if ( m_Owner is ISelectable )
				{
					return ( ( ISelectable )m_Owner ).Highlighted;
				}
				return m_Highlight;
			}
			set
			{
				if ( m_Owner is ISelectable )
				{
					( ( ISelectable )m_Owner ).Highlighted = value;
				}
				else
				{
					m_Highlight = value;
				}
			}
		}

		/// <summary>
		/// Selection flag
		/// </summary>
		public bool Selected
		{
			get
			{
				if ( m_Owner is ISelectable )
				{
					return ( ( ISelectable )m_Owner ).Selected;
				}
				return m_Selected;
			}
			set
			{
				if ( m_Owner is ISelectable )
				{
					( ( ISelectable )m_Owner ).Selected = value;
				}
				else
				{
					m_Selected = value;
				}
			}
		}

		#endregion

		#region IRay3Intersector Members

		/// <summary>
		/// Checks if a ray intersects this object
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>true if the ray intersects this object</returns>
		public virtual bool TestIntersection( Ray3 ray )
		{
			Line3Intersection pick = m_Plane.GetIntersection( ray );
			return pick == null ? false : pick.IntersectionPosition.DistanceTo( Position ) > Radius;
		}

		/// <summary>
		/// Checks if a ray intersects this object, returning information about the intersection if it does
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>Intersection information. If no intersection takes place, this method returns null</returns>
		public virtual Line3Intersection GetIntersection( Ray3 ray )
		{
			Line3Intersection pick = m_Plane.GetIntersection( ray );
			if ( ( pick == null ) || ( pick.IntersectionPosition.DistanceTo( Position ) > Radius ) )
			{
			    return null;
			}
			pick.IntersectedObject = this;
			return pick;
		}

		#endregion
		
		#region ISelectionModifier Members

		/// <summary>
		/// Gets the actual object to select, if this is selected
		/// </summary>
		public object SelectedObject
		{
			get { return m_Owner; }
		}

		#endregion

		#region Protected members
		
		/// <summary>
		/// Invokes the Changed event
		/// </summary>
		protected void OnChanged( )
		{
			if ( Changed != null )
			{
				Changed( this, null );
			}
		}

		/// <summary>
		/// Gets the plane defined by the current position and the up vector
		/// </summary>
		protected Plane3 Plane
		{
			get { return m_Plane; }
		}

		#endregion

		#region Private members

		private readonly object m_Owner;
		private readonly Plane3 m_Plane = new Plane3( Point3.Origin, Vector3.YAxis );
		private Point3 m_Pos = new Point3( 0, 0, 0 );
		private const float Radius = 1.0f;
		private static readonly Draw.IBrush m_DrawUnselected;
		private static readonly Draw.IBrush m_DrawHighlight;
		private static readonly Draw.IBrush m_DrawSelected;
		private bool m_Highlight;
		private bool m_Selected;

		static PositionModifier( )
		{
			m_DrawUnselected = Graphics.Draw.NewBrush( Color.Black, Color.PaleGoldenrod );
			m_DrawHighlight = Graphics.Draw.NewBrush( Color.DarkSalmon, Color.Goldenrod );
			m_DrawSelected = Graphics.Draw.NewBrush( Color.Red, Color.Orange );

			m_DrawUnselected.State.EnableCap( RenderStateFlag.DepthTest | RenderStateFlag.DepthWrite );
			m_DrawUnselected.OutlinePen.State.EnableCap( RenderStateFlag.DepthTest | RenderStateFlag.DepthWrite );
			
			m_DrawHighlight.State.EnableCap( RenderStateFlag.DepthTest | RenderStateFlag.DepthWrite );
			m_DrawHighlight.OutlinePen.State.EnableCap( RenderStateFlag.DepthTest | RenderStateFlag.DepthWrite );
			
			m_DrawSelected.State.EnableCap( RenderStateFlag.DepthTest | RenderStateFlag.DepthWrite );
			m_DrawSelected.OutlinePen.State.EnableCap( RenderStateFlag.DepthTest | RenderStateFlag.DepthWrite );
		}

		#endregion
	}

}
