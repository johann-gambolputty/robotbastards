using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Rendering;

namespace Poc0.LevelEditor.Core.EditModes
{
	public abstract class Draw
	{
		#region Singleton

		/// <summary>
		/// Gets the class singleton
		/// </summary>
		public static Draw Instance
		{
			get { return ms_Instance; }
		}

		#endregion

		#region Construction

		/// <summary>
		/// Sets the class singleton (<see cref="Instance"/>)
		/// </summary>
		public Draw( )
		{
			ms_Instance = this;
		}

		#endregion

		#region Pens

		/// <summary>
		/// Pen, equivalent to <see cref="System.Drawing.Pen"/>, for the current graphics API
		/// </summary>
		public interface IPen
		{
			/// <summary>
			/// The pen colour
			/// </summary>
			Color Colour
			{
				get; set;
			}
		}

		/// <summary>
		/// Creates a new IPen from an existing <see cref="System.Drawing.Pen"/>
		/// </summary>
		/// <param name="pen">Source pen</param>
		/// <returns>New IPen</returns>
		public abstract IPen NewPen( Pen pen );

		/// <summary>
		/// Creates a new IPen, with a width of 1, drawing with the specified colour
		/// </summary>
		/// <param name="colour">Pen colour</param>
		/// <returns>New IPen</returns>
		public abstract IPen NewPen( Color colour );

		/// <summary>
		/// Creates a new IPen, with a specified width and colour
		/// </summary>
		/// <param name="colour">Pen colour</param>
		/// <param name="width">Pen width</param>
		/// <returns>New IPen</returns>
		public abstract IPen NewPen( Color colour, float width );

		#endregion

		#region Brushes

		/// <summary>
		/// Brush, equivalent to <see cref="System.Drawing.Brush"/>, for the current graphics API
		/// </summary>
		public interface IBrush
		{
			/// <summary>
			/// The brush colour
			/// </summary>
			Color Colour
			{
				get; set;
			}
		}

		/// <summary>
		/// Creates a new IBrush from an existing <see cref="System.Drawing.Brush"/>
		/// </summary>
		/// <param name="brush">Source brush</param>
		/// <returns>New IBrush</returns>
		public abstract IBrush NewBrush( Brush brush );

		#endregion

		#region Moulds

		/// <summary>
		/// Er... OK. A mould is to 3D solid rendering operations what a brush is to 2D drawing operations. See?
		/// </summary>
		public interface IMould
		{
			/// <summary>
			/// The colour of the mould
			/// </summary>
			Color Colour
			{
				get; set;
			}
		}

		/// <summary>
		/// Creates a new mould
		/// </summary>
		/// <param name="colour">Surface colour</param>
		/// <returns>Returns a new IMould</returns>
		public abstract IMould NewMould( Color colour );

		/// <summary>
		/// Creates a new mould
		/// </summary>
		/// <param name="wireframe">If true, the sphere is rendered in wireframe</param>
		/// <param name="colour">Surface colour</param>
		/// <returns>Returns a new IMould</returns>
		public abstract IMould NewMould( bool wireframe, Color colour );

		#endregion

		#region 2D

		#region Lines

		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		public void Line( IPen pen, Point start, Point end )
		{
			Line( pen, start.X, start.Y, end.X, end.Y );
		}
		
		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="startX">Line start X coordinate</param>
		/// <param name="startY">Line start Y coordinate</param>
		/// <param name="endX">Line end X coordinate</param>
		/// <param name="endY">Line end Y coordinate</param>
		public abstract void Line( IPen pen, int startX, int startY, int endX, int endY );

		/// <summary>
		/// Draws a series of lines connecting a list of points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		public void Lines( IPen pen, IEnumerable< Point > points )
		{
			Lines( pen, points, false );
		}

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="close">If true, then a line is drawn connecting the last point to the first</param>
		public void Lines( IPen pen, IEnumerable< Point > points, bool close )
		{
			IEnumerator< Point > pointPos = points.GetEnumerator( );
			if ( !pointPos.MoveNext( ) )
			{
				return;
			}

			Point firstPoint = pointPos.Current;
			Point lastPoint = firstPoint;

			while ( pointPos.MoveNext( ) )
			{
				Point curPoint = pointPos.Current;
				Line( pen, lastPoint, curPoint );
				lastPoint = curPoint;
			}

			if ( close )
			{
				Line( pen, lastPoint, firstPoint );
			}
		}

		#endregion

		#region Circles

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		public abstract void Circle( IPen pen, Point centre, float radius );

		#endregion

		#region Filled circles

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		public abstract void Circle( IBrush brush, Point centre, float radius );

		#endregion

		#region Rectangles

		#endregion

		#region Filled rectangles

		#endregion

		#region Polygons

		#endregion

		#region Filled polygons

		#endregion

		#endregion

		#region 3D

		#region Lines

		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		public abstract void Line( IPen pen, Point3 start, Point3 end );

		/// <summary>
		/// Draws a series of lines connecting a list of points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		public void Lines( IPen pen, IEnumerable< Point3 > points )
		{
			Lines( pen, points, false );
		}

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="close">If true, then a line is drawn connecting the last point to the first</param>
		public void Lines( IPen pen, IEnumerable< Point3 > points, bool close )
		{
			IEnumerator< Point3 > pointPos = points.GetEnumerator( );
			if ( !pointPos.MoveNext( ) )
			{
				return;
			}

			Point3 firstPoint = pointPos.Current;
			Point3 lastPoint = firstPoint;

			while ( pointPos.MoveNext( ) )
			{
				Point3 curPoint = pointPos.Current;
				Line( pen, lastPoint, curPoint );
				lastPoint = curPoint;
			}

			if ( close )
			{
				Line( pen, lastPoint, firstPoint );
			}
		}

		#endregion

		#region Spheres

		/// <summary>
		/// Draws a sphere
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="centre">Centre of the sphere</param>
		/// <param name="radius">Sphere radius</param>
		public void Sphere( IMould mould, Point3 centre, float radius )
		{
			Sphere(  mould, centre, radius, 10, 10 );
		}

		/// <summary>
		/// An ST sphere
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="centre">Centre of the sphere</param>
		/// <param name="radius">Sphere radius</param>
		/// <param name="sSamples">Number of longitudinal samples</param>
		/// <param name="tSamples">Number of latitudinal samples</param>
		public abstract void Sphere( IMould mould, Point3 centre, float radius, int sSamples, int tSamples );

		#endregion

		#region Cylinders

		/// <summary>
		/// Draws a cylinder. Cylinder is defined by a line and a radius value
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="start">Cylinder line start point</param>
		/// <param name="end">Cylinder line end point</param>
		/// <param name="radius">Cylinder radius</param>
		public void Cylinder( IMould mould, Point3 start, Point3 end, float radius )
		{
			Cylinder( mould, start, end, radius, 10 );
		}

		/// <summary>
		/// Draws a cylinder. Cylinder is defined by a line and a radius value
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="start">Cylinder line start point</param>
		/// <param name="end">Cylinder line end point</param>
		/// <param name="radius">Cylinder radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the cylinder</param>
		public abstract void Cylinder( IMould mould, Point3 start, Point3 end, float radius, int sSamples );

		#endregion

		#region Capsules
		
		/// <summary>
		/// Draws a capsule. Capsule is defined by a line and a radius value
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="start">Capsule line start point</param>
		/// <param name="end">Capsule line end point</param>
		/// <param name="radius">Capsule radius</param>
		public void Capsule( IMould mould, Point3 start, Point3 end, float radius )
		{
			Capsule( mould, start, end, radius, 10 );
		}

		/// <summary>
		/// Draws a capsule. Capsule is defined by a line and a radius value
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="start">Capsule line start point</param>
		/// <param name="end">Capsule line end point</param>
		/// <param name="radius">Capsule radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the capsule</param>
		public abstract void Capsule( IMould mould, Point3 start, Point3 end, float radius, int sSamples );

		#endregion

		#endregion

		#region Private members

		private static Draw ms_Instance;

		#endregion
	}

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
		/// Starts the edit mode
		/// </summary>
		public override void Start( )
		{
			if ( m_EdgePen == null )
			{
				m_EdgePen = Draw.Instance.NewPen( Pens.Black );
			}
			if ( m_VertexMould == null )
			{
				m_VertexMould = Draw.Instance.NewMould( Color.Red );
			}

			foreach ( Control control in Controls )
			{
				control.MouseClick += OnMouseClick;
				control.KeyDown += OnKeyDown;
			}
			EditModeContext.Instance.Scene.Renderables.Add( this );
		}

		/// <summary>
		/// Stops the edit mode
		/// </summary>
		public override void Stop( )
		{
			foreach ( Control control in Controls )
			{
				control.MouseClick -= OnMouseClick;
			}
			EditModeContext.Instance.Scene.Renderables.Remove( this );
		}

		#region IRenderable Implementation

		private Draw.IPen m_EdgePen;
		private Draw.IMould m_VertexMould;

		/// <summary>
		/// Renders this edit mode
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			//	Render lines
			Draw.Instance.Lines( m_EdgePen, m_Points );

			//	Render vertices
			foreach ( Point3 point in m_Points )
			{
				Draw.Instance.Sphere( m_VertexMould, point, 3.0f, 5, 5 );
			}
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

		private const float CloseTolerance = 0.2f;	
		private readonly List< Point3 > m_Points = new List< Point3 >( );

		/// <summary>
		/// Returns true if a point is close to the first point in the polygon
		/// </summary>
		/// <param name="pt">Point to test</param>
		/// <returns>Returns true if pt is over the first point in the defined polygon</returns>
		private bool IsOverFirstPoint( Point3 pt )
		{
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
		/// Handles mouse clicks. Left clicks add a new vertex, right clicks close the polygon
		/// </summary>
		private void OnMouseClick( object sender, MouseEventArgs args )
		{
			//ITilePicker picker = ( ITilePicker )sender;
			if ( args.Button == MouseButtons.Left )
			{
				//	TODO: AP: Get camera, transform args.Location to world space
				Point3 pt = Point3.Origin; //picker.CursorToWorld( args.X, args.Y );
				if ( IsOverFirstPoint( pt ) )
				{
					ClosePolygon( );
				}
				else
				{
					m_Points.Add( pt );
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
