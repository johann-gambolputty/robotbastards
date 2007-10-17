using System;
using System.Drawing;
using Poc0.Core.Objects;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.Rendering;
using Rb.World;
using Rb.World.Services;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// For changing the position of a game object
	/// </summary>
	[Serializable]
	public class PositionEditor : IRay3Intersector, IObjectEditor, IPickable, IMoveable3, IRenderable, ISelectable
	{
		#region Public methods

		/// <summary>
		/// Sets up this editor
		/// </summary>
		/// <param name="hasPosition">Positioning interface of the game object</param>
		/// <param name="pick">Position to place the new object at</param>
		public PositionEditor( IHasPosition hasPosition, ILineIntersection pick )
		{
			m_HasPosition = hasPosition;
			m_HasPosition.PositionChanged += OnPositionChanged;

			Position = ( ( Line3Intersection )pick ).IntersectionPosition;

			IRayCastService rayCaster = EditorState.Instance.CurrentScene.GetService< IRayCastService >( );
			rayCaster.AddIntersector( RayCastLayers.Entity, this );
		}

		/// <summary>
		/// Gets the position of the game object
		/// </summary>
		public Point3 Position
		{
			get { return m_HasPosition.Position; }
			set
			{
				m_HasPosition.Position = value;
				if ( ObjectChanged != null )
				{
					ObjectChanged( this, null );
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
		/// Moves the game object
		/// </summary>
		/// <param name="delta">Movement delta</param>
		public void Move( Vector3 delta )
		{
			Position += delta;
		}

		#endregion

		#region IObjectEditor Members

		/// <summary>
		/// Called when this object changes
		/// </summary>
		public event EventHandler ObjectChanged;

		/// <summary>
		/// Gets the underlying game object
		/// </summary>
		public object Instance
		{
			get { return m_HasPosition; }
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
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
			Graphics.Draw.Circle( drawProps, m_HasPosition.Position.X, m_HasPosition.Position.Y + 0.1f, m_HasPosition.Position.Z, Radius, 12 );
		}

		#endregion

		#region ISelectable Members

		/// <summary>
		/// Highlight flag
		/// </summary>
		public bool Highlighted
		{
			get { return m_Highlight; }
			set { m_Highlight = value; }
		}

		/// <summary>
		/// Selection flag
		/// </summary>
		public bool Selected
		{
			get { return m_Selected; }
			set { m_Selected = value; }
		}

		#endregion

		#region IRay3Intersector Members

		/// <summary>
		/// Checks if a ray intersects this object
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>true if the ray intersects this object</returns>
		public bool TestIntersection( Ray3 ray )
		{
			Line3Intersection pick = m_Plane.GetIntersection( ray );
			return pick == null ? false : pick.IntersectionPosition.DistanceTo( m_HasPosition.Position ) > Radius;
		}

		/// <summary>
		/// Checks if a ray intersects this object, returning information about the intersection if it does
		/// </summary>
		/// <param name="ray">Ray to check</param>
		/// <returns>Intersection information. If no intersection takes place, this method returns null</returns>
		public Line3Intersection GetIntersection( Ray3 ray )
		{
			Line3Intersection pick = m_Plane.GetIntersection( ray );
			if ( ( pick == null ) || ( pick.IntersectionPosition.DistanceTo( m_HasPosition.Position ) > Radius ) )
			{
			    return null;
			}
			pick.IntersectedObject = this;
			return pick;
		}

		#endregion

		#region Private members

		private readonly IHasPosition m_HasPosition;
		private Plane3 m_Plane;
		private const float Radius = 1.0f;
		private static readonly Draw.IBrush m_DrawUnselected;
		private static readonly Draw.IBrush m_DrawHighlight;
		private static readonly Draw.IBrush m_DrawSelected;
		private bool m_Highlight;
		private bool m_Selected;
		
		static PositionEditor( )
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

		/// <summary>
		/// Called when the position of the game object changes
		/// </summary>
		private void OnPositionChanged( object obj, Point3 oldPos, Point3 newPos )
		{
			m_Plane = new Plane3( m_HasPosition.Position + new Vector3( 0, 0.1f, 0 ), Vector3.YAxis );
		}

		#endregion
	}
}
