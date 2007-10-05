using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.Tools.LevelEditor.Core.Selection;
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
			if ( m_EdgePen == null )
			{
				m_EdgePen = Graphics.Draw.NewPen( Color.Black, 4.0f );
			}
			if ( m_VertexMould == null )
			{
				m_VertexMould = Graphics.Draw.NewMould( Color.Red );
			}
		}

		/// <summary>
		/// Gets the mouse buttons used by this edit mode
		/// </summary>
		public override MouseButtons Buttons
		{
			get { return MouseButtons.Left | MouseButtons.Right; }
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
			Graphics.Draw.Lines( m_EdgePen, m_Points );

			if ( m_Points.Count > 0 )
			{
				Graphics.Draw.Line( m_EdgePen, m_Points[ m_Points.Count - 1 ], m_CursorPoint );
			}

			//	TODO: AP: reinstate (render as 3d camera facing circles, though)
			//	Render vertices
			//foreach ( Point2 point in m_Points )
			//{
			//	Graphics.Draw.Sphere( m_VertexMould, point, 3.0f, 5, 5 );
			//}
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Colour of polygon edges 
		/// </summary>
		protected Color EdgeColour
		{
			get { return m_EdgePen.Colour; }
			set { m_EdgePen.Colour = value; }
		}

		/// <summary>
		/// Colour of polygon vertices
		/// </summary>
		protected Color VertexColour
		{
			get { return m_VertexMould.Colour; }
			set { m_VertexMould.Colour = value; }
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

		private readonly Draw.IPen		m_EdgePen;
		private readonly Draw.IMould	m_VertexMould;
		private readonly List< Point3 >	m_Points = new List< Point3 >( );
		private Point3					m_CursorPoint;

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
		}

		/// <summary>
		/// Handles mouse movement
		/// </summary>
		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			IPicker picker = ( IPicker )sender;
			PickInfoCursor pick = picker.CreateCursorPickInfo( args.X, args.Y );
			if ( pick == null )
			{
				return;
			}
			if ( pick is IPickInfo3 )
			{
				m_CursorPoint = ( ( IPickInfo3 )pick ).PickPoint;
			}
			else if ( pick is IPickInfo2 )
			{
				Point2 pt = ( ( IPickInfo2 )pick ).PickPoint;
				m_CursorPoint = new Point3( pt.X, pt.Y, 0 );
			}
			else
			{
				throw new NotImplementedException( );
			}
		}

		/// <summary>
		/// Handles mouse clicks. Left clicks add a new vertex, right clicks close the polygon
		/// </summary>
		private void OnMouseClick( object sender, MouseEventArgs args )
		{
			if ( args.Button == MouseButtons.Left )
			{
				if ( IsOverFirstPoint( m_CursorPoint ) )
				{
					ClosePolygon( );
				}
				else
				{
					m_Points.Add( new Point3( m_CursorPoint ) );
				}
			}
			else if ( args.Button == MouseButtons.Right )
			{
				ClosePolygon( );
			}
		}

		#endregion
	}
	
	
	
}
