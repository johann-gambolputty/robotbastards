using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	public class OpenGlDraw : DrawBase
	{
		#region GlDrawState class

		/// <summary>
		/// Base class for implementations of IPen, IBrush, andISurface
		/// </summary>
		private class GlDrawState
		{
			/// <summary>
			/// Initialises the render state
			/// </summary>
			public GlDrawState( )
			{
				m_State.DepthTest = false;
				m_State.DepthWrite = false;
			}

			/// <summary>
			/// Pushes the render state
			/// </summary>
			public void Begin( )
			{
				Graphics.Renderer.PushRenderState( m_State );
			}

			/// <summary>
			/// Pops the 
			/// </summary>
			public void End( )
			{
				Graphics.Renderer.PopRenderState( );
			}

			/// <summary>
			/// Gets the underlying render state
			/// </summary>
			public IRenderState State
			{
				get { return m_State; }
			}

			/// <summary>
			/// The pen colour
			/// </summary>
			public Color Colour
			{
				get { return m_State.Colour; }
				set { m_State.Colour = value; }
			}

			private readonly OpenGlRenderState m_State = new OpenGlRenderState( );
		}

		#endregion

		#region Pens

		private class GlPen : GlDrawState, IPen
		{
			public GlPen( )
			{
				State.FaceRenderMode = PolygonRenderMode.Lines;
			}

			/// <summary>
			/// Pen thickness
			/// </summary>
			public float Thickness
			{
				get { return State.LineWidth; }
				set { State.LineWidth = value; }
			}
		}

		/// <summary>
		/// Creates a new IPen, with a specified width and colour
		/// </summary>
		/// <param name="colour">Pen colour</param>
		/// <param name="width">Pen width</param>
		/// <returns>New IPen</returns>
		public override IPen NewPen( Color colour, float width )
		{
			GlPen newPen = new GlPen( );
			newPen.Thickness = width;
			newPen.Colour = colour;
			return newPen;
		}

		#endregion

		#region Brushes

		/// <summary>
		/// Brush, equivalent to <see cref="System.Drawing.Brush"/>, for the current graphics API
		/// </summary>
		private class GlBrush : GlDrawState, IBrush
		{
		
			/// <summary>
			/// Outline pen. If not null, then an outline is drawn around the rendered shape
			/// </summary>
			public IPen OutlinePen
			{
				get { return m_OutlinePen; }
				set { m_OutlinePen = value; }
			}
			
			private IPen m_OutlinePen;
		}
		
		/// <summary>
		/// Creates a new IBrush from an existing <see cref="System.Drawing.Brush"/>
		/// </summary>
		/// <param name="colour">Brush colour</param>
		/// <returns>New IBrush</returns>
		public override IBrush NewBrush( Color colour )
		{
			GlBrush newBrush = new GlBrush( );
			newBrush.Colour = colour;
			return newBrush;
		}

		/// <summary>
		/// Creates a new brush that fills with one colour and outlines with another
		/// </summary>
		/// <param name="colour">Fill colour</param>
		/// <param name="outlineColour">Outline colour</param>
		/// <returns>Returns the new brush</returns>
		public override IBrush NewBrush( Color colour, Color outlineColour )
		{
			GlBrush newBrush = new GlBrush( );
			newBrush.Colour = colour;
			newBrush.OutlinePen = NewPen( outlineColour );
			return newBrush;
		}

		#endregion

		#region Surfaces

		/// <summary>
		/// Er... OK. A surface is to 3D rendering operations what a brush is to 2D drawing operations. See?
		/// </summary>
		private class GlSurface : ISurface
		{
			/// <summary>
			/// Gets/sets the brush used to fill the surface faces. If null, faces aren't rendered
			/// </summary>
			public IBrush FaceBrush
			{
				get { return m_Brush; }
				set { m_Brush = value; }
			}

			/// <summary>
			/// Gets/sets the pen used to draw face edges. If null, wireframe isn't rendered
			/// </summary>
			public IPen EdgePen
			{
				get { return m_Pen; }
				set { m_Pen = value; }
			}

			private IBrush m_Brush;
			private IPen m_Pen;

			#region IState Members

			public IRenderState State
			{
				get { return m_Brush.State; }
			}

			#endregion

			#region IPass Members

			public void Begin( )
			{
				m_Brush.Begin( );
			}

			public void End( )
			{
				m_Brush.End( );
			}

			#endregion
		}

		/// <summary>
		/// Creates a new surface
		/// </summary>
		/// <param name="faceBrush">Brush used for filling in faces. If null, faces are rendered in wireframe only</param>
		/// <param name="edgePen">Pen used for drawing edges. If null, faces are rendered as filled only</param>
		/// <returns>Returns a new ISurface</returns>
		public override ISurface NewSurface( IBrush faceBrush, IPen edgePen )
		{
			GlSurface newSurface = new GlSurface( );
			newSurface.FaceBrush = faceBrush;
			newSurface.EdgePen = edgePen;
			return newSurface;
		}

		#endregion

		#region Caching

		private class DrawCache : IRenderable, IDisposable
		{
			public DrawCache( )
			{
				m_DisplayList = Gl.glGenLists( 1 );
				Gl.glNewList( m_DisplayList, Gl.GL_COMPILE );
			}

			public void Finish( )
			{
				Gl.glEndList( );
			}
			
			#region IRenderable Members

			public void Render( IRenderContext context )
			{
				Gl.glCallList( m_DisplayList );
			}

			#endregion
			
			#region IDisposable Members

			public void Dispose( )
			{
				Gl.glDeleteLists( m_DisplayList, 1 );
			}

			#endregion

			private readonly int m_DisplayList;
		}

		private DrawCache m_CurCache;

		/// <summary>
		/// Starts caching draw calls
		/// </summary>
		public override void StartCache( )
		{
			if ( m_CurCache != null )
			{
				throw new InvalidOperationException( "Made 2 StartCache() in a row - call StopCache() first" );
			}
			m_CurCache = new DrawCache( );
		}

		/// <summary>
		/// Stops caching draw calls
		/// </summary>
		/// <returns>Returns a renderable object, containing all draw operations since StartCachingOperations() was called</returns>
		public override IRenderable StopCache( )
		{
			if ( m_CurCache == null )
			{
				throw new InvalidOperationException( "Call StartCache() before calling StopCache()");
			}

			IRenderable result = m_CurCache;
			m_CurCache.Finish( );
			m_CurCache = null;

			return result;
		}

		#endregion

		#region 2D

		#region Lines

		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="startX">Line start X coordinate</param>
		/// <param name="startY">Line start Y coordinate</param>
		/// <param name="endX">Line end X coordinate</param>
		/// <param name="endY">Line end Y coordinate</param>
		public override void Line( IPen pen, float startX, float startY, float endX, float endY )
		{
			Begin( pen );
			
			Gl.glBegin( Gl.GL_LINES );

			Gl.glVertex2f( startX, startY );
			Gl.glVertex2f( endX, endY );

			Gl.glEnd( );

			End( pen );
		}

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="closed">If true, then a line is drawn connecting the last point to the first</param>
		public override void Lines( IPen pen, IEnumerable<Point2> points, bool closed )
		{
			Begin( pen );
			Gl.glBegin( Gl.GL_LINE_STRIP );

			IEnumerator<Point2> pointPos = points.GetEnumerator( );
			if ( pointPos.MoveNext( ) )
			{
				Point2 firstPoint = pointPos.Current;
				Gl.glVertex2f( firstPoint.X, firstPoint.Y );

				while ( pointPos.MoveNext( ) )
				{
					Gl.glVertex2f( pointPos.Current.X, pointPos.Current.Y );
				}

				if ( closed )
				{
					Gl.glVertex2f( firstPoint.X, firstPoint.Y );
				}
			}

			Gl.glEnd( );
			End( pen );
		}

		#endregion

		#region Circles

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		public override void Circle( IPen pen, float x, float y, float radius, int samples )
		{
			float angle = 0;
			float angleInc = Constants.TwoPi / samples;

			Begin( pen );

			Gl.glBegin( Gl.GL_LINE_STRIP );

			for ( int sampleCount = 0; sampleCount <= samples; ++sampleCount )
			{
				float sinAR = radius * Functions.Sin( angle );
				float cosAR = radius * Functions.Cos( angle );

				Gl.glVertex2f( x + sinAR, y + cosAR );

				angle += angleInc;
			}

			Gl.glEnd( );

			End( pen );
		}

		#endregion

		#region Filled circles

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		public override void Circle( IBrush brush, float x, float y, float radius, int samples )
		{
			float angle = 0;
			float angleInc = Constants.TwoPi / samples;

			Begin( brush );
			
			Gl.glBegin( Gl.GL_TRIANGLE_FAN );
			
			Gl.glVertex2f( x, y );
			
			for ( int sampleCount = 0; sampleCount <= samples; ++sampleCount )
			{
				float sinAR = radius * Functions.Sin( angle );
				float cosAR = radius * Functions.Cos( angle );
			
				Gl.glVertex2f( x + sinAR, y + cosAR );
			
				angle += angleInc;
			}
			
			Gl.glEnd( );
			End( brush );
			
			if ( brush.OutlinePen != null )
			{
				Circle( brush.OutlinePen, x, y, radius, samples );
			}
		}

		#endregion

		#region Rectangles
		
		/// <summary>
		/// Draws a rectangle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">The rectangle top left corner X coordinate</param>
		/// <param name="y">The rectangle top left corner Y coordinate</param>
		/// <param name="width">The rectangle width</param>
		/// <param name="height">The rectangle height</param>
		public override void Rectangle( IPen pen, float x, float y, float width, float height )
		{
			Begin( pen );

			Gl.glBegin( Gl.GL_LINE_STRIP );

			Gl.glVertex2f( x, y );
			Gl.glVertex2f( x + width, y );
			Gl.glVertex2f( x + width, y + height );
			Gl.glVertex2f( x, y + height );
			Gl.glVertex2f( x, y );

			Gl.glEnd( );

			End( pen );
		}

		#endregion

		#region Filled rectangles

		/// <summary>
		/// Draws a rectangle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">The rectangle top left corner X coordinate</param>
		/// <param name="y">The rectangle top left corner Y coordinate</param>
		/// <param name="width">The rectangle width</param>
		/// <param name="height">The rectangle height</param>
		public override void Rectangle( IBrush brush, float x, float y, float width, float height )
		{
			Begin( brush );

			Gl.glBegin( Gl.GL_QUADS );

			Gl.glVertex2f( x, y );
			Gl.glVertex2f( x + width, y );
			Gl.glVertex2f( x + width, y + height );
			Gl.glVertex2f( x, y + height );

			Gl.glEnd( );

			End( brush );
			
			if ( brush.OutlinePen != null )
			{
				Rectangle( brush.OutlinePen, x, y, width, height );
			}
		}

		#endregion

		#region Polygons

		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public override void Polygon( IPen pen, IEnumerable<Point2> points )
		{
			Lines( pen, points, true );
		}

		#endregion

		#region Filled polygons

		/// <summary>
		/// Draws a filled polygon
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public override void Polygon( IBrush brush, IEnumerable<Point2> points )
		{
			Begin( brush );
			
			Gl.glBegin( Gl.GL_POLYGON );
			
			IEnumerator< Point2 > pointPos = points.GetEnumerator( );
			while ( pointPos.MoveNext( ) )
			{
				Gl.glVertex2f( pointPos.Current.X, pointPos.Current.Y );
			}
			
			Gl.glEnd( );

			End( brush );
			if ( brush.OutlinePen != null )
			{
				Polygon( brush.OutlinePen, points );
			}
		} 

		#endregion

		#endregion

		#region 3D

		#region Lines

		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Line start X coordinate</param>
		/// <param name="y">Line start Y coordinate</param>
		/// <param name="z">Line start Z coordinate</param>
		/// <param name="endX">Line end X coordinate</param>
		/// <param name="endY">Line end Y coordinate</param>
		/// <param name="endZ">Line end Z coordinate</param>
		public override void Line( IPen pen, float x, float y, float z, float endX, float endY, float endZ )
		{
			Begin( pen );
			
			Gl.glBegin( Gl.GL_LINES );

			Gl.glVertex3f( x, y, z );
			Gl.glVertex3f( endX, endY, endZ );

			Gl.glEnd( );

			End( pen );
		}

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="closed">If true, then a line is drawn connecting the last point to the first</param>
		public override void Lines( IPen pen, IEnumerable<Point3> points, bool closed )
		{
			Begin( pen );
			Gl.glBegin( Gl.GL_LINE_STRIP );

			IEnumerator<Point3> pointPos = points.GetEnumerator( );
			if ( pointPos.MoveNext( ) )
			{
				Point3 firstPoint = pointPos.Current;
				Gl.glVertex3f( firstPoint.X, firstPoint.Y, firstPoint.Z );

				while ( pointPos.MoveNext( ) )
				{
					Gl.glVertex3f( pointPos.Current.X, pointPos.Current.Y, pointPos.Current.Z );
				}

				if ( closed )
				{
					Gl.glVertex3f( firstPoint.X, firstPoint.Y, firstPoint.Z );
				}
			}

			Gl.glEnd( );
			End( pen );
		}

		#endregion

		#region Circles

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		public override void Circle( IPen pen, float x, float y, float z, float radius, int samples )
		{

			//	TODO: AP: Always camera facing?
			float angle = 0;
			float angleInc = Constants.TwoPi / samples;

			Begin( pen );

			Gl.glBegin( Gl.GL_LINE_STRIP );

			for ( int sampleCount = 0; sampleCount <= samples; ++sampleCount )
			{
				float sinAR = radius * Functions.Sin( angle );
				float cosAR = radius * Functions.Cos( angle );

				Gl.glVertex3f( x + sinAR, y, z + cosAR );

				angle += angleInc;
			}

			Gl.glEnd( );
			End( pen );
		}

		#endregion

		#region Filled circles

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		public override void Circle( IBrush brush, float x, float y, float z, float radius, int samples )
		{
			//	TODO: AP: Always camera facing?
			float angle = 0;
			float angleInc = Constants.TwoPi / samples;

			Begin( brush );

			Gl.glBegin( Gl.GL_TRIANGLE_FAN );

			Gl.glVertex3f( x, y, z );

			for ( int sampleCount = 0; sampleCount <= samples; ++sampleCount )
			{
				float sinAR = radius * Functions.Sin( angle );
				float cosAR = radius * Functions.Cos( angle );

				Gl.glVertex3f( x + sinAR, y, z + cosAR );

				angle += angleInc;
			}

			Gl.glEnd( );
			End( brush );

			if ( brush.OutlinePen != null )
			{
				Circle( brush.OutlinePen, x, y, z, radius, samples );
			}
		}

		#endregion

		#region Rectangles

		/// <summary>
		/// Draws a rectangle, facing up the yaxis
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Rectangle centre X coordinate</param>
		/// <param name="y">Rectangle centre Y coordinate</param>
		/// <param name="z">Rectangle centre Z coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public override void Rectangle( IPen pen, float x, float y, float z, float width, float height )
		{
			float hWidth = width / 2;
			float hHeight = height / 2;

			Begin( pen );

			Gl.glBegin( Gl.GL_LINE_STRIP );

			Gl.glVertex3f( x - hWidth, y, z - hHeight );
			Gl.glVertex3f( x + hWidth, y, z - hHeight );
			Gl.glVertex3f( x + hWidth, y, z + hHeight );
			Gl.glVertex3f( x - hWidth, y, z + hHeight );
			Gl.glVertex3f( x - hWidth, y, z - hHeight );

			Gl.glEnd( );

			End( pen );
		}

		#endregion

		#region Filled Rectangles

		/// <summary>
		/// Draws a rectangle, facing up the yaxis
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Rectangle centre X coordinate</param>
		/// <param name="y">Rectangle centre Y coordinate</param>
		/// <param name="z">Rectangle centre Z coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public override void Rectangle( IBrush brush, float x, float y, float z, float width, float height )
		{
			float hWidth = width / 2;
			float hHeight = height / 2;

			Begin( brush );

			Gl.glBegin( Gl.GL_QUADS );

			Gl.glVertex3f( x - hWidth, y, z - hHeight );
			Gl.glVertex3f( x + hWidth, y, z - hHeight );
			Gl.glVertex3f( x + hWidth, y, z + hHeight );
			Gl.glVertex3f( x - hWidth, y, z + hHeight );

			Gl.glEnd( );

			End( brush );

			if ( brush.OutlinePen != null )
			{
				Rectangle( brush.OutlinePen, x, y, z, width, height );
			}
		}

		#endregion

		#region Polygons

		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public override void Polygon( IPen pen, IEnumerable<Point3> points )
		{
			Begin( pen );

			Gl.glBegin( Gl.GL_POLYGON );

			IEnumerator< Point3 > pointPos = points.GetEnumerator( );
			while ( pointPos.MoveNext( ) )
			{
				Gl.glVertex3f( pointPos.Current.X, pointPos.Current.Y, pointPos.Current.Z );
			}

			Gl.glEnd( );

			End( pen );
		}

		#endregion

		#region Filled Polygons

		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public override void Polygon( IBrush brush, IEnumerable<Point3> points )
		{
			Begin( brush );

			Gl.glBegin( Gl.GL_POLYGON );

			IEnumerator< Point3 > pointPos = points.GetEnumerator( );
			while ( pointPos.MoveNext( ) )
			{
				Gl.glVertex3f( pointPos.Current.X, pointPos.Current.Y, pointPos.Current.Z );
			}

			Gl.glEnd( );

			End( brush );
			if ( brush.OutlinePen != null )
			{
				Polygon( brush.OutlinePen, points );
			}
		}


		#endregion

		#region Axis aligned boxes

		/// <summary>
		/// Draws an axis aligned box
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="minX">Top left back x coordinate</param>
		/// <param name="minY">Top left back y coordinate</param>
		/// <param name="minZ">Top left back z coordinate</param>
		/// <param name="maxX">Bottom right front x coordinate</param>
		/// <param name="maxY">Bottom right front y coordinate</param>
		/// <param name="maxZ">Bottom right front z coordinate</param>
		public override void AlignedBox( IPen pen, float minX, float minY, float minZ, float maxX, float maxY, float maxZ )
		{
			Begin( pen );

			Gl.glBegin( Gl.GL_LINE_STRIP );

				//	-ve Z
				Gl.glVertex3f( minX, minY, minZ );
				Gl.glVertex3f( maxX, minY, minZ );
				Gl.glVertex3f( maxX, maxY, minZ );
				Gl.glVertex3f( minX, maxY, minZ );
				Gl.glVertex3f( minX, minY, minZ );
			
			Gl.glEnd( );
			
			Gl.glBegin( Gl.GL_LINE_STRIP );

				//	+ve Z
				Gl.glVertex3f( minX, minY, maxZ );
				Gl.glVertex3f( maxX, minY, maxZ );
				Gl.glVertex3f( maxX, maxY, maxZ );
				Gl.glVertex3f( minX, maxY, maxZ );
				Gl.glVertex3f( minX, minY, maxZ );
			
			Gl.glEnd( );

			Gl.glBegin( Gl.GL_LINE_STRIP );

				//	-ve X
				Gl.glVertex3f( minX, minY, minZ );
				Gl.glVertex3f( minX, minY, maxZ );
				Gl.glVertex3f( minX, maxY, maxZ );
				Gl.glVertex3f( minX, maxY, minZ );
				Gl.glVertex3f( minX, maxY, minZ );

			Gl.glEnd( );
			
			Gl.glBegin( Gl.GL_LINE_STRIP );

				//	+ve X
				Gl.glVertex3f( maxX, minY, minZ );
				Gl.glVertex3f( maxX, minY, maxZ );
				Gl.glVertex3f( maxX, maxY, maxZ );
				Gl.glVertex3f( maxX, maxY, minZ );
				Gl.glVertex3f( maxX, maxY, minZ );

			Gl.glEnd( );

			End( pen );
		}

		#endregion

		#region Spheres

		/// <summary>
		/// An ST sphere
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="centre">Centre of the sphere</param>
		/// <param name="radius">Sphere radius</param>
		/// <param name="sSamples">Number of longitudinal samples</param>
		/// <param name="tSamples">Number of latitudinal samples</param>
		public override void Sphere( ISurface surface, Point3 centre, float radius, int sSamples, int tSamples )
		{
			if ( surface == null )
			{
				RenderSphere( centre, radius, sSamples, tSamples );
				return;
			}
			if ( surface.FaceBrush != null )
			{
				surface.FaceBrush.Begin( );
				RenderSphere( centre, radius, sSamples, tSamples );
				surface.FaceBrush.End( );
			}
			if ( surface.EdgePen != null )
			{
				surface.EdgePen.Begin( );
				RenderSphere( centre, radius, sSamples, tSamples );
				surface.EdgePen.End( );
			}
		}

		private static void RenderSphere( Point3 centre, float radius, int sSamples, int tSamples )
		{
			float minT = 0;
			float maxT = Constants.Pi;
			float minS = 0;
			float maxS = Constants.TwoPi;

			//	Render the sphere as a series of strips
			float sIncrement = ( maxS - minS ) / ( sSamples );
			float tIncrement = ( maxT - minT ) / ( tSamples );

			float t = minT;

			Gl.glBegin( Gl.GL_QUADS );

			for ( int tCount = 0; tCount < tSamples; ++tCount )
			{
				float s = minS;
				for ( int SCount = 0; SCount < sSamples; ++SCount )
				{
					//	TODO: This is wasteful, because 2 positions are shared from the previous samples
					//	Cache an array of points on the first entry in this function, and read from
					//	it instead of calculating points on the fly.
					RenderSTVertex( s, t, centre, radius );
					RenderSTVertex( s, t + tIncrement, centre, radius );
					RenderSTVertex( s + sIncrement, t + tIncrement, centre, radius );
					RenderSTVertex( s + sIncrement, t, centre, radius );

					s += sIncrement;
				}
				t += tIncrement;
			}

			Gl.glEnd( );
			
		}

		private static void RenderSTVertex( float s, float t, Point3 c, float r )
		{
			float cosS = Functions.Cos( s );
			float cosT = Functions.Cos( t );

			float sinS = Functions.Sin( s );
			float sinT = Functions.Sin( t );
			
			float x = cosS * sinT;
			float y = cosT;
			float z = sinS * sinT;

			Gl.glNormal3f( x, y, z );
			Gl.glVertex3f( c.X + x * r, c.Y + y * r, c.Z+ z * r );
		}

		#endregion

		#region Cylinders

		/// <summary>
		/// Draws a cylinder. Cylinder is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Cylinder line start point</param>
		/// <param name="end">Cylinder line end point</param>
		/// <param name="radius">Cylinder radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the cylinder</param>
		public override void Cylinder( ISurface surface, Point3 start, Point3 end, float radius, int sSamples )
		{
			if ( surface == null )
			{
				RenderCylinder( start, end, radius, sSamples );
				return;
			}
			if ( surface.FaceBrush != null )
			{
				surface.FaceBrush.Begin( );
				RenderCylinder( start, end, radius, sSamples );
				surface.FaceBrush.End( );
			}

			if ( surface.EdgePen != null )
			{
				surface.EdgePen.Begin( );
				RenderCylinder( start, end, radius, sSamples );
				surface.EdgePen.End( );
			}
		}

		#region Private Cylinder Rendering

		private static void RenderSVertex( float angle, float y, float radius )
		{
			float sinAngle = Functions.Sin( angle );
			float cosAngle = Functions.Cos( angle );
			Gl.glNormal3f( sinAngle, 0, cosAngle );
			Gl.glVertex3f( sinAngle * radius, y, cosAngle * radius );	
		}
			
		private static void RenderYAxisCylinder( int subDivCount, float radius, float length )
		{
			float angleIncrement = Constants.TwoPi / subDivCount;
			float angle = 0;

			Gl.glBegin( Gl.GL_QUADS );

			for ( int subDiv = 0; subDiv < subDivCount; ++subDiv )
			{
				RenderSVertex( angle, 0, radius );
				RenderSVertex( angle, length, radius );
				RenderSVertex( angle+ angleIncrement, length, radius );
				RenderSVertex( angle + angleIncrement, 0, radius );

				angle += angleIncrement;
			}

			Gl.glEnd( );
		}

		private static void RenderCylinder( Point3 start, Point3 end, float radius, int sSamples )
		{
			Vector3 cylinderVec = end - start;
			float cylinderLength = cylinderVec.Length;
			cylinderVec /= cylinderLength;

			//	TODO: AP: Build Matrix44 and pre-multiply vertices
			Graphics.Renderer.PushTransform( TransformType.LocalToWorld );
			Graphics.Renderer.Translate( TransformType.LocalToWorld, start.X, start.Y, start.Z );

			Vector3 up = Vector3.YAxis;
			if ( up.Dot( cylinderVec ) < 0.9999f )
			{
				Vector3 rotateVec = Vector3.Cross( up, cylinderVec );
				float rotation = Functions.Acos( up.Dot( cylinderVec ) );
				Graphics.Renderer.RotateAroundAxis( TransformType.LocalToWorld, rotateVec, rotation );
			}
			else
			{
				throw new NotImplementedException( "Cylinder transform for cylinder axis == y axis not implemented" );
			}
			RenderYAxisCylinder( sSamples, radius, cylinderLength );

			RenderYAxisCylinderEndCap( sSamples, radius, 0, 1 );
			RenderYAxisCylinderEndCap( sSamples, radius, cylinderLength, -1 );

			Graphics.Renderer.PopTransform( TransformType.LocalToWorld );
		}
		
		private static void RenderYAxisCylinderEndCap( int subDivCount, float radius, float y, float dir )
		{
			float	AngleIncrement	= ( Constants.TwoPi / subDivCount ) * dir;
			float	Angle			= 0;
			float 	SinAngle		= Functions.Sin( Angle );
			float 	CosAngle		= Functions.Sin( Angle );

			Gl.glBegin( Gl.GL_TRIANGLE_FAN );

			Gl.glNormal3f( 0, dir, 0 );
			Gl.glVertex3f( SinAngle * radius, y, CosAngle * radius );

			for ( int SubDiv = 0; SubDiv < subDivCount; ++SubDiv )
			{
				Angle += AngleIncrement;
				SinAngle = Functions.Sin( Angle );
				CosAngle = Functions.Cos( Angle );

				Gl.glVertex3f( SinAngle * radius, y, CosAngle * radius );
			}

			Gl.glEnd( );
		}

		#endregion

		#endregion

		#region Capsules

		/// <summary>
		/// Draws a capsule. Capsule is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Capsule line start point</param>
		/// <param name="end">Capsule line end point</param>
		/// <param name="radius">Capsule radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the capsule</param>
		public override void Capsule( ISurface surface, Point3 start, Point3 end, float radius, int sSamples )
		{
			throw new NotImplementedException( );
		}

		#endregion

		#endregion

		#region Private Members

		private static void Begin( IPass pass )
		{
			if ( pass != null )
			{
				pass.Begin( );
			}
		}

		private static void End( IPass pass )
		{
			if ( pass != null )
			{
				pass.End( );
			}
		}

		#endregion
	}
}
