using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	public class OpenGlDraw : Draw
	{
		/// <summary>
		/// Base class for implementations of IPen, IBrush, and IMould
		/// </summary>
		private class GlDrawState
		{
			/// <summary>
			/// Initialises the render state
			/// </summary>
			public GlDrawState( )
			{
				m_State.DisableCap( RenderStateFlag.DepthTest | RenderStateFlag.DepthWrite );
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
			public RenderState State
			{
				get { return m_State; }
			}

			/// <summary>
			/// The pen colour
			/// </summary>
			public Color Colour
			{
				get { return m_Colour; }
				set
				{
					m_Colour = value;
					m_State.SetColour( value );
				}
			}

			private readonly OpenGlRenderState m_State = new OpenGlRenderState( );
			private Color m_Colour = Color.Black;
		}

		#region Pens

		private class GlPen : GlDrawState, IPen
		{
			/// <summary>
			/// Pen thickness
			/// </summary>
			public float Thickness
			{
				get { return m_Thickness; }
				set
				{
					m_Thickness = value;
					State.SetLineWidth( value );
				}
			}

			private float m_Thickness = 1.0f;
		}

		/// <summary>
		/// Creates a new IPen from an existing <see cref="System.Drawing.Pen"/>
		/// </summary>
		/// <param name="pen">Source pen</param>
		/// <returns>New IPen</returns>
		public override IPen NewPen( Pen pen )
		{
			GlPen newPen = new GlPen( );
			newPen.Thickness = pen.Width;
			newPen.Colour = pen.Color;
			return newPen;
		}

		/// <summary>
		/// Creates a new IPen, with a width of 1, drawing with the specified colour
		/// </summary>
		/// <param name="colour">Pen colour</param>
		/// <returns>New IPen</returns>
		public override IPen NewPen( Color colour )
		{
			GlPen newPen = new GlPen( );
			newPen.Colour = colour;
			return newPen;
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
		/// <param name="brush">Source brush</param>
		/// <returns>New IBrush</returns>
		public override IBrush NewBrush( Brush brush )
		{
			GlBrush newBrush = new GlBrush( );
			newBrush.Colour = ( ( SolidBrush )brush ).Color; // TODO: AP: Better handling of source brush type
			return newBrush;
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

		#region Moulds

		/// <summary>
		/// Er... OK. A mould is to 3D solid rendering operations what a brush is to 2D drawing operations. See?
		/// </summary>
		private class Mould : GlDrawState, IMould
		{
			/// <summary>
			/// Wireframe flag
			/// </summary>
			public bool Wireframe
			{
				get { return m_Wireframe; }
				set
				{
					m_Wireframe = value;
					State.SetPolygonRenderingMode( value ? PolygonRenderMode.Lines : PolygonRenderMode.Fill );
				}
			}

			private bool m_Wireframe;
		}

		/// <summary>
		/// Creates a new mould
		/// </summary>
		/// <param name="colour">Surface colour</param>
		/// <returns>Returns a new IMould</returns>
		public override IMould NewMould( Color colour )
		{
			Mould newMould = new Mould( );
			newMould.Colour = colour;
			return newMould;
		}

		/// <summary>
		/// Creates a new mould
		/// </summary>
		/// <param name="wireframe">If true, the sphere is rendered in wireframe</param>
		/// <param name="colour">Surface colour</param>
		/// <returns>Returns a new IMould</returns>
		public override IMould NewMould( bool wireframe, Color colour )
		{
			Mould newMould = new Mould( );
			newMould.Colour = colour;
			newMould.Wireframe = wireframe;
			return newMould;
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
			pen.Begin( );
			
			Gl.glBegin( Gl.GL_LINES );

			Gl.glVertex2f( startX, startY );
			Gl.glVertex2f( endX, endY );

			Gl.glEnd( );

			pen.End( );
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

			pen.Begin( );

			Gl.glBegin( Gl.GL_LINE_STRIP );

			for ( int sampleCount = 0; sampleCount <= samples; ++sampleCount )
			{
				float sinAR = radius * Functions.Sin( angle );
				float cosAR = radius * Functions.Cos( angle );

				Gl.glVertex2f( x + sinAR, y + cosAR );

				angle += angleInc;
			}

			Gl.glEnd( );

			pen.End( );
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

			brush.Begin( );
			
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
			brush.End( );
			
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
			pen.Begin( );

			Gl.glBegin( Gl.GL_LINE_STRIP );

			Gl.glVertex2f( x, y );
			Gl.glVertex2f( x + width, y );
			Gl.glVertex2f( x + width, y + height );
			Gl.glVertex2f( x, y + height );
			Gl.glVertex2f( x, y );

			Gl.glEnd( );

			pen.End( );
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
			brush.Begin( );

			Gl.glBegin( Gl.GL_QUADS );

			Gl.glVertex2f( x, y );
			Gl.glVertex2f( x + width, y );
			Gl.glVertex2f( x + width, y + height );
			Gl.glVertex2f( x, y + height );

			Gl.glEnd( );

			brush.End( );
			
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
		public override void Polygon( IPen pen, IEnumerable< Point2 > points )
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
		public override void Polygon( IBrush brush, IEnumerable< Point2 > points )
		{
			brush.Begin( );
			
			Gl.glBegin( Gl.GL_POLYGON );
			
			IEnumerator< Point2 > pointPos = points.GetEnumerator( );
			while ( pointPos.MoveNext( ) )
			{
				Gl.glVertex2f( pointPos.Current.X, pointPos.Current.Y );
			}
			
			Gl.glEnd( );

			brush.End( );
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
			pen.Begin( );
			
			Gl.glBegin( Gl.GL_LINES );

			Gl.glVertex3f( x, y, z );
			Gl.glVertex3f( endX, endY, endZ );

			Gl.glEnd( );

			pen.End( );
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

			pen.Begin( );

			Gl.glBegin( Gl.GL_LINE_STRIP );

			for ( int sampleCount = 0; sampleCount <= samples; ++sampleCount )
			{
				float sinAR = radius * Functions.Sin( angle );
				float cosAR = radius * Functions.Cos( angle );

				Gl.glVertex3f( x + sinAR, y, z + cosAR );

				angle += angleInc;
			}

			Gl.glEnd( );
			pen.End( );
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

			brush.Begin( );

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
			brush.End( );

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

			pen.Begin( );

			Gl.glBegin( Gl.GL_LINE_STRIP );

			Gl.glVertex3f( x - hWidth, y, z - hHeight );
			Gl.glVertex3f( x + hWidth, y, z - hHeight );
			Gl.glVertex3f( x + hWidth, y, z + hHeight );
			Gl.glVertex3f( x - hWidth, y, z + hHeight );
			Gl.glVertex3f( x - hWidth, y, z - hHeight );

			Gl.glEnd( );

			pen.End( );
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

			brush.Begin( );

			Gl.glBegin( Gl.GL_QUADS );

			Gl.glVertex3f( x - hWidth, y, z - hHeight );
			Gl.glVertex3f( x + hWidth, y, z - hHeight );
			Gl.glVertex3f( x + hWidth, y, z + hHeight );
			Gl.glVertex3f( x - hWidth, y, z + hHeight );

			Gl.glEnd( );

			brush.End( );

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
		public override void Polygon( IPen pen, IEnumerable< Point3 > points )
		{
			pen.Begin( );

			Gl.glBegin( Gl.GL_POLYGON );

			IEnumerator< Point3 > pointPos = points.GetEnumerator( );
			while ( pointPos.MoveNext( ) )
			{
				Gl.glVertex3f( pointPos.Current.X, pointPos.Current.Y, pointPos.Current.Z );
			}

			Gl.glEnd( );

			pen.End( );
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
			brush.Begin( );

			Gl.glBegin( Gl.GL_POLYGON );

			IEnumerator< Point3 > pointPos = points.GetEnumerator( );
			while ( pointPos.MoveNext( ) )
			{
				Gl.glVertex3f( pointPos.Current.X, pointPos.Current.Y, pointPos.Current.Z );
			}

			Gl.glEnd( );


			brush.End( );
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
			pen.Begin( );

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

			pen.End( );
		}

		#endregion

		#region Spheres

		/// <summary>
		/// An ST sphere
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="centre">Centre of the sphere</param>
		/// <param name="radius">Sphere radius</param>
		/// <param name="sSamples">Number of longitudinal samples</param>
		/// <param name="tSamples">Number of latitudinal samples</param>
		public override void Sphere( IMould mould, Point3 centre, float radius, int sSamples, int tSamples )
		{
			mould.Begin( );

			float minT = 0;
			float maxT	= Constants.Pi;
			float minS	= 0;
			float maxS	= Constants.TwoPi;

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

			mould.End( );
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

			Gl.glVertex3f( c.X + x * r, c.Y + y * r, c.Z+ z * r );
		}

		#endregion

		#region Cylinders


		/// <summary>
		/// Draws a cylinder. Cylinder is defined by a line and a radius value
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="start">Cylinder line start point</param>
		/// <param name="end">Cylinder line end point</param>
		/// <param name="radius">Cylinder radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the cylinder</param>
		public override void Cylinder( IMould mould, Point3 start, Point3 end, float radius, int sSamples )
		{
			throw new NotImplementedException( );
		}

		#endregion

		#region Capsules

		/// <summary>
		/// Draws a capsule. Capsule is defined by a line and a radius value
		/// </summary>
		/// <param name="mould">Drawing parameters</param>
		/// <param name="start">Capsule line start point</param>
		/// <param name="end">Capsule line end point</param>
		/// <param name="radius">Capsule radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the capsule</param>
		public override void Capsule( IMould mould, Point3 start, Point3 end, float radius, int sSamples )
		{
			throw new NotImplementedException( );
		}

		#endregion

		#endregion
	}
}
