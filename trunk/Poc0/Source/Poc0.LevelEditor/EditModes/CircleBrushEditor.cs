using System;
using System.Drawing;
using System.Windows.Forms;
using Poc0.LevelEditor.Core;
using Poc0.LevelEditor.Core.Geometry;
using Poc0.LevelEditor.Properties;
using Rb.Core.Maths;
using Rb.Log;
using Rb.Rendering;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.EditModes
{
	public class CircleBrushEditor : IEditor
	{
		/// <summary>
		/// Initialises this object
		/// </summary>
		public CircleBrushEditor( int numEdges )
		{
			m_EdgeCount = numEdges;
			m_DrawEdge = Graphics.Draw.NewPen( Color.White, 2.0f );
			m_DrawVertex = Graphics.Draw.NewBrush( Color.Red, Color.DarkRed );
		}

		/// <summary>
		/// Gets/sets the number of edges used to subdivide the circumferences of circle
		/// </summary>
		public int EdgeCount
		{
			get { return m_EdgeCount; }
			set { m_EdgeCount = value; }
		}

		#region IEditor members

		/// <summary>
		/// Gets a string describing the usage of this editor
		/// </summary>
		public string Description
		{
			get
			{ 
				string addPoint	= ResourceHelper.MouseButtonName( MouseButtons.Right );
				string clear	= Keys.Escape.ToString( );

				return string.Format( Resources.CircleBrushInputs, addPoint, clear );
			}
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
		
		/// <summary>
		/// Renders this edit mode
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			float angle = m_AngleStart;
			float angleIncrement = Constants.TwoPi / EdgeCount;
			float lastX = m_CentrePoint.X + Functions.Sin( angle - angleIncrement ) * m_Radius;
			float lastY = m_CentrePoint.Z + Functions.Cos( angle - angleIncrement ) * m_Radius;
			for ( int edgeIndex = 0; edgeIndex < EdgeCount; ++edgeIndex )
			{
				float x = m_CentrePoint.X + Functions.Sin( angle ) * m_Radius;
				float y = m_CentrePoint.Z + Functions.Cos( angle ) * m_Radius;

				Graphics.Draw.Line( m_DrawEdge, lastX, m_CentrePoint.Y, lastY, x, m_CentrePoint.Y, y );

				lastX = x;
				lastY = y;
				angle += angleIncrement;
				if ( angle > Constants.TwoPi )
				{
					angle -= Constants.TwoPi;
				}
			}
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Colour of circle edges 
		/// </summary>
		protected Color EdgeColour
		{
			get { return m_DrawEdge.Colour; }
			set { m_DrawEdge.Colour = value; }
		}

		/// <summary>
		/// Colour of circle vertices
		/// </summary>
		protected Color VertexColour
		{
			get { return m_DrawVertex.Colour; }
			set { m_DrawVertex.Colour = value; }
		}

		#endregion

		#region Private members

		private readonly Draw.IPen		m_DrawEdge;
		private readonly Draw.IBrush	m_DrawVertex;
		private Point3					m_CentrePoint;
		private float					m_Radius = 3.0f;
		private float					m_AngleStart = 0.0f;
		private int						m_EdgeCount;
		private bool					m_DefiningRadius;


		private static readonly RayCastOptions ms_PickOptions = new RayCastOptions( RayCastLayers.Grid );

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
			Point3 mousePoint = ( ( Line3Intersection )pick ).IntersectionPosition;
			if ( !m_DefiningRadius )
			{
				m_CentrePoint = mousePoint;
			}
			else
			{
				m_Radius = ( ( Line3Intersection )pick ).IntersectionPosition.DistanceTo( m_CentrePoint );

				Vector3 dirToMouse = mousePoint - m_CentrePoint;
				m_AngleStart = Functions.Atan2( dirToMouse.X, dirToMouse.Z );
			}
		}

		/// <summary>
		/// Handles mouse clicks. Right clicks starts defining the circle radius
		/// </summary>
		private void OnMouseClick( object sender, MouseEventArgs args )
		{
			if ( args.Button == MouseButtons.Right )
			{
				if ( !m_DefiningRadius )
				{
					m_DefiningRadius = true;
				}
				else
				{
					AddCircleToLevelGeometry( LevelGeometry.FromCurrentScene( ));
				}
			}
		}

		private void OnKeyDown( object sender, KeyEventArgs args )
		{
			if ( args.KeyCode == Keys.Escape )
			{
				m_DefiningRadius = false;
			}
			else if ( args.KeyValue == '=' )
			{
				++EdgeCount;
			}
			else if ( args.KeyValue == '-' )
			{
				if ( EdgeCount > 3 )
				{
					--EdgeCount;
				}
			}
			else if ( args.KeyCode == Keys.Return )
			{
				AddCircleToLevelGeometry( LevelGeometry.FromCurrentScene( ) );
			}
		}

		private void AddCircleToLevelGeometry( LevelGeometry geometry )
		{
			try
			{
				Point2[] points2 = new Point2[ EdgeCount ];

				float angle = m_AngleStart;
				float angleIncrement = Constants.TwoPi / EdgeCount;
				for ( int ptIndex = 0; ptIndex < EdgeCount; ++ptIndex )
				{
					float x = m_CentrePoint.X + Functions.Sin( angle ) * m_Radius;
					float y = m_CentrePoint.Z + Functions.Cos( angle ) * m_Radius;

					points2[ ptIndex ] = new Point2( x, y );

					angle += angleIncrement;
					if ( angle > Constants.TwoPi )
					{
						angle -= Constants.TwoPi;
					}
				}

				UiPolygon brush = new UiPolygon( "", points2 );
				geometry.Add( brush, false, false );
				AppLog.Info( "Combined brush with current level geometry" );
			}
			catch ( Exception ex )
			{
				AppLog.Exception( ex, "Failed to combine circle brush with current level geometry" );
				ErrorMessageBox.Show( Properties.Resources.FailedToCombineCsgBrush );
			}

			m_DefiningRadius = false;
		}

		#endregion
	}
}
