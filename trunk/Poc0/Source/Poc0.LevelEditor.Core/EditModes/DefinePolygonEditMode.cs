using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.Core.EditModes
{
	/// <summary>
	/// Base class edit mode for defining polygonal areas
	/// </summary>
	public class DefinePolygonEditMode : EditMode, IRenderable
	{
		/// <summary>
		/// Raised when a polygon is closed
		/// </summary>
		public event Action< Point3[] > PolygonClosed;

		/// <summary>
		/// Initialises this object
		/// </summary>
		public DefinePolygonEditMode( )
		{
			m_DrawEdge = Graphics.Draw.NewPen( Color.White, 2.0f );
			m_DrawVertex = Graphics.Draw.NewBrush( Color.Red, Color.DarkRed );
		}

		/// <summary>
		/// Gets the mouse buttons used by this edit mode
		/// </summary>
		public override MouseButtons Buttons
		{
			get { return MouseButtons.Right; }
		}

		/// <summary>
		/// Binds to the specified control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		protected override void BindToControl( Control control )
		{
			control.MouseMove += OnMouseMove;
			control.MouseClick += OnMouseClick;
			control.KeyDown += OnKeyDown;
		}

		/// <summary>
		/// Unbinds to the specified control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		protected override void UnbindFromControl( Control control )
		{
			control.MouseMove -= OnMouseMove;
			control.MouseClick -= OnMouseClick;
			control.KeyDown -= OnKeyDown;
		}

		/// <summary>
		/// Starts the edit mode
		/// </summary>
		public override void Start( )
		{
			EditorState.Instance.CurrentScene.Renderables.Add( this );
			base.Start( );
		}

		/// <summary>
		/// Stops the edit mode
		/// </summary>
		public override void Stop( )
		{
			EditorState.Instance.CurrentScene.Renderables.Remove( this );
			base.Stop( );
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

			//RenderFont font = RenderFonts.GetDefaultFont( DefaultFont.Debug );
			//font.DrawText( 0, 0, Color.Black, "X: {0:F2} Y: {1:F2} Z: {2:F2}", m_CursorPoint.X, m_CursorPoint.Y, m_CursorPoint.Z );
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
		/// Called when a polygon is closed
		/// </summary>
		/// <param name="polygon">Polygon points</param>
		protected virtual void OnPolygonClosed( Point3[] polygon )
		{
		}

		#endregion

		#region Private members

		private readonly Draw.IPen		m_DrawEdge;
		private readonly Draw.IBrush	m_DrawVertex;
		private readonly List< Point3 >	m_Points = new List< Point3 >( );
		private Point3					m_CursorPoint;

		private static readonly RayCastOptions ms_PickOptions = new RayCastOptions( RayCastLayers.Grid );

		/// <summary>
		/// Point distance tolerance - the polygon is closed when the user adds a vertex this close to the first vertex
		/// </summary>
		private const float				CloseTolerance = 0.2f;

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
		/// Closes off the current polygon
		/// </summary>
		private void ClosePolygon( )
		{
			Point3[] points = m_Points.ToArray( );
			OnPolygonClosed( points );
			if ( PolygonClosed != null )
			{
				PolygonClosed( points );
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
				ClosePolygon( );
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
			if ( args.Button == MouseButtons.Right )
			{
				if ( IsOverFirstPoint( m_CursorPoint ) )
				{
					ClosePolygon( );
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
