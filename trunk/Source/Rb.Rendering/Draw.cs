using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;

namespace Rb.Rendering
{
	/// <summary>
	/// Draws various shapes, in 2d or 3d
	/// </summary>
	/// <remarks>
	/// All draw operations are affected by the current state of the rendering pipeline
	/// </remarks>
	public abstract class Draw
	{
		public static class Pens
		{
			public static IPen White	= Graphics.Draw.NewPen( Color.White );
			public static IPen Red		= Graphics.Draw.NewPen( Color.Red );
			public static IPen Blue		= Graphics.Draw.NewPen( Color.Blue );
			public static IPen Green	= Graphics.Draw.NewPen( Color.Green );
			public static IPen Black	= Graphics.Draw.NewPen( Color.Black );
		}

		#region Pens

		/// <summary>
		/// Pen, equivalent to <see cref="System.Drawing.Pen"/>, for the current graphics API
		/// </summary>
		public interface IPen : IPass
		{
			/// <summary>
			/// The pen colour
			/// </summary>
			Color Colour
			{
				get; set;
			}

			/// <summary>
			/// Pen thickness
			/// </summary>
			float Thickness
			{
				get;
				set;
			}

			/// <summary>
			/// Gets the underlying render state
			/// </summary>
			RenderState State
			{
				get;
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
		public interface IBrush : IPass
		{
			/// <summary>
			/// The brush colour
			/// </summary>
			Color Colour
			{
				get; set;
			}

			/// <summary>
			/// Outline pen. If not null, then an outline is drawn around the rendered shape
			/// </summary>
			IPen OutlinePen
			{
				get; set;
			}

			/// <summary>
			/// Gets the underlying render state
			/// </summary>
			RenderState State
			{
				get;
			}
		}

		/// <summary>
		/// Creates a new IBrush from an existing <see cref="System.Drawing.Brush"/>
		/// </summary>
		/// <param name="brush">Source brush</param>
		/// <returns>New IBrush</returns>
		public abstract IBrush NewBrush( Brush brush );
		
		/// <summary>
		/// Creates a new colour IBrush
		/// </summary>
		/// <param name="colour">Brush colour</param>
		/// <returns>New IBrush</returns>
		public abstract IBrush NewBrush( Color colour );

		/// <summary>
		/// Creates a new colour IBrush
		/// </summary>
		/// <param name="colour">Brush colour</param>
		/// <param name="outlineColour">Brush outline colour</param>
		/// <returns>New IBrush</returns>
		public abstract IBrush NewBrush( Color colour, Color outlineColour );

		#endregion

		#region Moulds

		/// <summary>
		/// Er... OK. A mould is to 3D solid rendering operations what a brush is to 2D drawing operations. See?
		/// </summary>
		public interface IMould : IPass
		{
			/// <summary>
			/// The colour of the mould
			/// </summary>
			Color Colour
			{
				get; set;
			}

			/// <summary>
			/// Wireframe flag
			/// </summary>
			bool Wireframe
			{
				get; set;
			}

			/// <summary>
			/// Gets the underlying render state
			/// </summary>
			RenderState State
			{
				get;
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

		#region Caching

		/// <summary>
		/// Starts caching draw calls
		/// </summary>
		public abstract void StartCache( );

		/// <summary>
		/// Stops caching draw calls
		/// </summary>
		/// <returns>Returns a renderable object, containing all draw operations since StartCachingOperations() was called</returns>
		public abstract IRenderable StopCache( );

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
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		public void Line( IPen pen, Point2 start, Point2 end )
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
		public abstract void Line( IPen pen, float startX, float startY, float endX, float endY );

		/// <summary>
		/// Draws a series of lines connecting a list of points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		public void Lines( IPen pen, IEnumerable< Point2 > points )
		{
			Lines( pen, points, false );
		}

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="close">If true, then a line is drawn connecting the last point to the first</param>
		public void Lines( IPen pen, IEnumerable< Point2 > points, bool close )
		{
			IEnumerator< Point2 > pointPos = points.GetEnumerator( );
			if ( pointPos.MoveNext( ) )
			{
				Point2 firstPoint = pointPos.Current;
				Point2 lastPoint = firstPoint;

				while ( pointPos.MoveNext( ) )
				{
					Point2 curPoint = pointPos.Current;
					Line( pen, lastPoint, curPoint );
					lastPoint = curPoint;
				}

				if ( close )
				{
					Line( pen, lastPoint, firstPoint );
				}
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
		public void Circle( IPen pen, Point centre, float radius )
		{
			Circle( pen, centre.X, centre.Y, radius, 10 );
		}

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		public abstract void Circle( IPen pen, float x, float y, float radius, int samples );

		#endregion

		#region Filled circles

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		public void Circle( IBrush brush, Point centre, float radius )
		{
			Circle( brush, centre.X, centre.Y, radius, 10 );
		}
		
		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="radius">Circle radius</param>
		public void Circle( IBrush brush, float x, float y, float radius )
		{
			Circle( brush, x, y, radius, 10 );
		}

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		public abstract void Circle( IBrush brush, float x, float y, float radius, int samples );

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
		public abstract void Rectangle( IPen pen, float x, float y, float width, float height );

		#endregion

		#region Filled rectangles
		
		/// <summary>
		/// Draws a filled rectangle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">The rectangle top left corner X coordinate</param>
		/// <param name="y">The rectangle top left corner Y coordinate</param>
		/// <param name="width">The rectangle width</param>
		/// <param name="height">The rectangle height</param>
		public abstract void Rectangle( IBrush brush, float x, float y, float width, float height );

		#endregion

		#region Polygons
		
		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public abstract void Polygon( IPen pen, IEnumerable< Point2 > points );
		
		#endregion

		#region Filled polygons

		/// <summary>
		/// Draws a filled polygon
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public abstract void Polygon( IBrush brush, IEnumerable< Point2 > points );

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
		public abstract void Line( IPen pen, float x, float y, float z, float endX, float endY, float endZ );

		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		public void Line( IPen pen, Point3 start, Point3 end )
		{
			Line( pen, start.X, start.Y, start.Z, end.X, end.Y, end.Z );
		}

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

		#region Circles

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		public void Circle( IPen pen, Point3 centre, float radius )
		{
			Circle( pen, centre.X, centre.Y, centre.Z, radius, 10 );
		}

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		public void Circle( IPen pen, float x, float y, float z, float radius )
		{
			Circle( pen, x, y, radius, 10 );
		}

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		public abstract void Circle( IPen pen, float x, float y, float z, float radius, int samples );

		#endregion

		#region Filled circles

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		public void Circle( IBrush brush, Point3 centre, float radius )
		{
			Circle( brush, centre.X, centre.Y, centre.Z, radius, 10 );
		}
		
		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		public void Circle( IBrush brush, float x, float y, float z, float radius )
		{
			Circle( brush, x, y, z, radius, 10 );
		}

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		public abstract void Circle( IBrush brush, float x, float y, float z, float radius, int samples );

		#endregion

		#region Rectangles

		/// <summary>
		/// Draws a rectangle, facing up the y axis
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="centre">Rectangle centre</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public void Rectangle( IPen pen, Point3 centre, float width, float height )
		{
			Rectangle( pen, centre.X, centre.Y, centre.Z, width, height );
		}

		/// <summary>
		/// Draws a rectangle, facing up the yaxis
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Rectangle centre X coordinate</param>
		/// <param name="y">Rectangle centre Y coordinate</param>
		/// <param name="z">Rectangle centre Z coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public abstract void Rectangle( IPen pen, float x, float y, float z, float width, float height );

		#endregion

		#region Filled Rectangles

		/// <summary>
		/// Draws a rectangle, facing up the y axis
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Rectangle centre</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public void Rectangle( IBrush brush, Point3 centre, float width, float height )
		{
			Rectangle( brush, centre.X, centre.Y, centre.Z, width, height );
		}

		/// <summary>
		/// Draws a rectangle, facing up the yaxis
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Rectangle centre X coordinate</param>
		/// <param name="y">Rectangle centre Y coordinate</param>
		/// <param name="z">Rectangle centre Z coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public abstract void Rectangle( IBrush brush, float x, float y, float z, float width, float height );

		#endregion

		#region Polygons

		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public abstract void Polygon( IPen pen, IEnumerable< Point3 > points );

		#endregion

		#region Filled Polygons

		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public abstract void Polygon( IBrush pen, IEnumerable< Point3 > points );

		#endregion


		#region Axis aligned boxes

		/// <summary>
		/// Draws an axis aligned box
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="min">Top left back position</param>
		/// <param name="max">Bottom right front position</param>
		public void AlignedBox( IPen pen, Point3 min, Point3 max )
		{
			AlignedBox( pen, min.X, min.Y, min.Z, max.X, max.Y, max.Z );
		}

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
		public abstract void AlignedBox( IPen pen, float minX, float minY, float minZ, float maxX, float maxY, float maxZ );

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
	}
}
