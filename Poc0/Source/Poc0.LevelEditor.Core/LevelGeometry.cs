using System;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Level geometry. Combines brushes to create the walkable geometry of a level
	/// </summary>
	[Serializable]
	public class LevelGeometry : IRenderable
	{
		#region Public members

		/// <summary>
		/// Default constructor
		/// </summary>
		public LevelGeometry( )
		{
			m_Csg.GeometryChanged += OnGeometryChanged;
		}

		/// <summary>
		/// Gets the CSG handler for this level
		/// </summary>
		public Csg Csg
		{
			get { return m_Csg; }
		}

		/// <summary>
		/// Flag to enable/disable contour rendering
		/// </summary>
		public bool RenderContours
		{
			get { return m_RenderContours; }
			set
			{
				m_RenderContours = value;
				DestroyRenderable( );
			}
		}

		/// <summary>
		/// Renders the level geometry
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( m_Renderable == null )
			{
				BuildRenderable( );
			}
			m_Renderable.Render( context );
		}

		#endregion

		#region Private members

		private readonly Csg m_Csg = new Csg( );

		private bool m_RenderContours = false;

		private IRenderable m_Renderable;

		[NonSerialized]
		private Draw.IPen m_DrawEdge;

		[NonSerialized]
		private Draw.IBrush m_DrawVertex;

		private void DestroyRenderable( )
		{
			IDisposable dispose = m_Renderable as IDisposable;
			if ( dispose != null )
			{
				dispose.Dispose( );
			}
			m_Renderable = null;
		}

		private void BuildRenderable( )
		{
			if ( m_DrawEdge == null )
			{
				m_DrawEdge = Graphics.Draw.NewPen( Color.Red, 2.0f );
			}

			if ( m_DrawVertex == null )
			{
				m_DrawVertex = Graphics.Draw.NewBrush( Color.Wheat, Color.Black );
			}

			Graphics.Draw.StartCache( );

			if ( !m_RenderContours )
			{
				if ( m_Csg.Root != null )
				{
					Render( m_Csg.Root );
				}
			}
			else
			{
				if ( m_Csg.Contours != null )
				{
					Color[] contourColours = new Color[] { Color.Red, Color.Green, Color.Blue, Color.OrangeRed };
					for ( int contour = 0; contour < m_Csg.Contours.Length; ++contour )
					{
						Csg.Edge firstEdge = m_Csg.Contours[ contour ];
						Csg.Edge edge = firstEdge;
						do
						{
							m_DrawEdge.Colour = contourColours[ contour % contourColours.Length ];
							DrawEdge( edge );
							edge = edge.NextEdge;
						} while ( ( edge != null ) && ( edge != firstEdge ) );
					}
				}
			}

			m_Renderable = Graphics.Draw.StopCache( );
		}

		/// <summary>
		/// Draws a CSG edge
		/// </summary>
		private void DrawEdge( Csg.Edge edge )
		{
			Graphics.Draw.Line( m_DrawEdge, edge.P0, edge.P1 );
			
			Vector2 vec = ( edge.P1 - edge.P0 );
			Point2 mid = edge.P0 + vec / 2;
			Graphics.Draw.Line( m_DrawEdge, mid, mid + vec.MakePerpNormal( ) * 4.0f );
			
			Graphics.Draw.Circle( m_DrawVertex, edge.P0.X, edge.P0.Y, 2.0f );
			Graphics.Draw.Circle( m_DrawVertex, edge.P1.X, edge.P1.Y, 2.0f );
		}

		/// <summary>
		/// Renders the specified BSP node
		/// </summary>
		private void Render( Csg.BspNode node )
		{
			DrawEdge( node.Edge );

			if ( node.ConvexRegion != null )
			{
				Graphics.Draw.Polygon( m_DrawVertex, node.ConvexRegion );
			}

			if ( node.Behind != null )
			{
				Render( node.Behind );
			}
			if ( node.InFront != null )
			{
				Render( node.InFront );
			}
		}

		/// <summary>
		/// Called when level geometry has changed
		/// </summary>
		private void OnGeometryChanged( object sender, EventArgs args )
		{
			DestroyRenderable( );
		}

		#endregion

	}
}
