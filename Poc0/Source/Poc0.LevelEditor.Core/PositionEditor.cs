using System;
using System.Drawing;
using Poc0.Core;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.Rendering;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// For changing the position of a game object
	/// </summary>
	public class PositionEditor : IObjectEditor, IPickable, IMoveable3, IRenderable, ISelectable
	{
		/// <summary>
		/// Sets up this editor
		/// </summary>
		/// <param name="hasPosition">Positioning interface of the game object</param>
		/// <param name="pick">Position to place the new object at</param>
		public PositionEditor( IHasPosition hasPosition, PickInfoCursor pick )
		{
			m_HasPosition = hasPosition;
			m_HasPosition.Position = ( ( IPickInfo3 )pick ).PickPoint;
		}

		#region IPickable Members

		/// <summary>
		/// Checks if a pick test is within this object
		/// </summary>
		/// <param name="pick">Pick information</param>
		/// <returns>Returns a pickable object (this) if pick test passes, otherwise, null</returns>
		public IPickable TestPick( IPickInfo pick )
		{
			if ( ( ( IPickInfo3 )pick ).PickPoint.DistanceTo( m_HasPosition.Position ) < 1.0f )
			{
				return this;
			}
			return null;
		}

		/// <summary>
		/// Creates the action appropriate for this object
		/// </summary>
		/// <param name="pick">Pick information</param>
		/// <returns>Returns a move action</returns>
		public IPickAction CreatePickAction( IPickInfo pick )
		{
			return new MoveAction( );
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

		private readonly IHasPosition m_HasPosition;

		#region IMoveable3 Members

		/// <summary>
		/// Moves the game object
		/// </summary>
		/// <param name="delta">Movement delta</param>
		public void Move( Vector3 delta )
		{
			m_HasPosition.Position += delta;
			if ( ObjectChanged != null )
			{
				ObjectChanged( this, null );
			}
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
			Draw.IBrush pen = m_DrawUnselected;
			if ( Selected )
			{
				pen = m_DrawSelected;
			}
			else if ( Highlighted )
			{
				pen = m_DrawHighlight;
			}
			Graphics.Draw.Circle( pen, m_HasPosition.Position.X, m_HasPosition.Position.Y, m_HasPosition.Position.Z, 1.0f, 12 );
		}

		private readonly Draw.IBrush m_DrawUnselected = Graphics.Draw.NewBrush( Color.Black, Color.PaleGoldenrod );
		private readonly Draw.IBrush m_DrawHighlight = Graphics.Draw.NewBrush( Color.DarkSalmon, Color.Goldenrod );
		private readonly Draw.IBrush m_DrawSelected = Graphics.Draw.NewBrush( Color.Red, Color.Orange );
		private bool m_Highlight;
		private bool m_Selected;


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
	}
}
