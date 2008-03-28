using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.EditModes
{
	public abstract class EdgeListEditor : IEditor
	{
		/// <summary>
		/// Delegate, used by the <see cref="EdgeListFinished"/> event
		/// </summary>
		/// <param name="points">Edge list points</param>
		/// <param name="loop">If true, then the edge list is a closed loop</param>
		public delegate void EdgeListFinishedDelegate( Point3[] points, bool loop );

		/// <summary>
		/// Raised when the current edge list is finished (by pressing return or closing the loop)
		/// </summary>
		public event EdgeListFinishedDelegate EdgeListFinished;

		/// <summary>
		/// Initialises this object
		/// </summary>
		public EdgeListEditor( )
		{
			m_DrawEdge = Graphics.Draw.NewPen( Color.White, 2.0f );
			m_DrawVertex = Graphics.Draw.NewBrush( Color.Red, Color.DarkRed );
		}

		/// <summary>
		/// Gets a string describing the usage of this editor
		/// </summary>
		public abstract string Description
		{
			get;
		}

		/// <summary>
		/// Binds to the specified control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		public void BindToControl( Control control )
		{
			control.MouseMove += OnMouseMove;
			control.MouseClick += OnMouseClick;
			control.KeyDown += OnKeyDown;
		}

		/// <summary>
		/// Unbinds to the specified control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		public void UnbindFromControl( Control control )
		{
			control.MouseMove -= OnMouseMove;
			control.MouseClick -= OnMouseClick;
			control.KeyDown -= OnKeyDown;
		}

		#region IRenderable Implementation

		/// <summary>
		/// Renders this edit mode
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			//	Render lines
			Graphics.Draw.Lines( m_DrawEdge, m_Points );

			if ( m_Points.Count > 0 )
			{
				Graphics.Draw.Line( m_DrawEdge, m_Points[ m_Points.Count - 1 ], m_CursorPoint );
			}
			
			//	Render vertices
			foreach ( Point3 point in m_Points )
			{
				Graphics.Draw.Circle( m_DrawVertex, point, 0.5f );
			}
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Colour of polygon edges 
		/// </summary>
		protected Color EdgeColour
		{
			get { return m_DrawEdge.Colour; }
			set { m_DrawEdge.Colour = value; }
		}

		/// <summary>
		/// Colour of polygon vertices
		/// </summary>
		protected Color VertexColour
		{
			get { return m_DrawVertex.Colour; }
			set { m_DrawVertex.Colour = value; }
		}

		/// <summary>
		/// Called when a edge list is finished
		/// </summary>
		protected virtual void OnFinished( Point3[] points, bool loop )
		{
		}

		#endregion

		#region Private members

		private readonly Draw.IPen		m_DrawEdge;
		private readonly Draw.IBrush	m_DrawVertex;
		private readonly List< Point3 >	m_Points = new List< Point3 >( );
		private Point3					m_CursorPoint;

		protected static MouseButtons	ms_AddPointButton = MouseButtons.Right;

		private static readonly RayCastOptions ms_PickOptions = new RayCastOptions( RayCastLayers.Grid );

		/// <summary>
		/// Point distance tolerance - the edge list is closed when the user adds a vertex this close to the first vertex
		/// </summary>
		private const float CloseTolerance = 0.2f;

		/// <summary>
		/// Returns true if a point is close to the first point in the polygon
		/// </summary>
		/// <param name="pt">Point to test</param>
		/// <returns>Returns true if pt is over the first point in the defined polygon</returns>
		private bool IsOverFirstPoint( Point3 pt )
		{
			if ( m_Points.Count == 0 )
			{
				return false;
			}
			return ( m_Points.Count > 0 ) && ( m_Points[ 0 ].DistanceTo( pt ) < CloseTolerance );
		}
		
		/// <summary>
		/// Finishes the current edge list
		/// </summary>
		private void Finish( bool loop )
		{
			Point3[] points = m_Points.ToArray( );
			OnFinished( points, loop );
			if ( EdgeListFinished != null )
			{
				EdgeListFinished( points, loop );
			}
			m_Points.Clear( );
		}

		/// <summary>
		/// Handles key down events. Pressing escape chucks away the current polygon
		/// </summary>
		private void OnKeyDown( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == Keys.Escape )
			{
				m_Points.Clear( );
			}
			else if ( args.KeyCode == Keys.Return )
			{
				Finish( false );
			}
		}

		/// <summary>
		/// Handles mouse movement
		/// </summary>
		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			IPicker picker = ( IPicker )sender;
			ILineIntersection pick = picker.FirstPick( args.X, args.Y, ms_PickOptions );
			if ( pick == null )
			{
				return;
			}
			m_CursorPoint = ( ( Line3Intersection )pick ).IntersectionPosition;
		}

		/// <summary>
		/// Handles mouse clicks. Left clicks add a new vertex, right clicks close the polygon
		/// </summary>
		private void OnMouseClick( object sender, MouseEventArgs args )
		{
			if ( args.Button == ms_AddPointButton )
			{
				if ( IsOverFirstPoint( m_CursorPoint ) )
				{
					Finish( true );
				}
				else
				{
					m_Points.Add( m_CursorPoint );
				}
			}
		}

		#endregion
	}
}
