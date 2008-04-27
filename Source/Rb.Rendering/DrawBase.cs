
using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// A handy abstract base class that does most of the legwork implementing <see cref="IDraw"/>
	/// </summary>
	public abstract class DrawBase : Draw, IDraw
	{
		#region IDraw Members

		#region Caching

		/// <summary>
		/// Starts caching draw calls. When <see cref="StopCache()"/> is called, a renderable
		/// object representing the cache will be created
		/// </summary>
		public abstract void StartCache( );

		/// <summary>
		/// Stops caching draw calls
		/// </summary>
		/// <returns>Returns a renderable object representing all cached draw calls</returns>
		public abstract IRenderable StopCache( );

		#endregion

		#region Pens

		/// <summary>
		/// Creates a new IPen, with a width of 1, drawing with the specified colour
		/// </summary>
		/// <param name="colour">Pen colour</param>
		/// <returns>New IPen</returns>
		public IPen NewPen( Color colour )
		{
			return NewPen( colour, 1 );
		}

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

		#region Surfaces

		/// <summary>
		/// Creates a new surface
		/// </summary>
		public abstract ISurface NewSurface( IBrush brush, IPen pen );

		/// <summary>
		/// Creates a new surface
		/// </summary>
		public ISurface NewSurface( Color faceColour )
		{
			return NewSurface( NewBrush( faceColour ), null );
		}

		/// <summary>
		/// Creates a new surface
		/// </summary>
		public ISurface NewSurface( Color faceColour, Color edgeColour )
		{
			return NewSurface( NewBrush( faceColour ), NewPen( edgeColour ) );
		}

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
		public abstract void Line( IPen pen, float startX, float startY, float endX, float endY );

		/// <summary>
		/// Draws a series of lines connecting a list of points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		public void Lines( IPen pen, IEnumerable<Point2> points )
		{
			Lines( pen, points, false );
		}

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="close">If true, then a line is drawn connecting the last point to the first</param>
		public abstract void Lines( IPen pen, IEnumerable<Point2> points, bool close );

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
			Circle( pen, centre.X, centre.Y, radius, DefaultCircleSamples );
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

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		public void Circle( IBrush brush, Point centre, float radius )
		{
			Circle( brush, centre.X, centre.Y, radius );
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
			Circle( brush, x, y, radius, DefaultCircleSamples );
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
		public abstract void Polygon( IPen pen, IEnumerable<Point2> points );

		/// <summary>
		/// Draws a filled polygon
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public abstract void Polygon( IBrush brush, IEnumerable<Point2> points );

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
		public void Lines( IPen pen, IEnumerable<Point3> points )
		{
			Lines( pen, points, false );
		}

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="close">If true, then a line is drawn connecting the last point to the first</param>
		public abstract void Lines( IPen pen, IEnumerable<Point3> points, bool close );

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
			Circle( pen, centre.X, centre.Y, centre.Z, radius );
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
			Circle( pen, x, y, z, radius, DefaultCircleSamples );
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

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		public void Circle( IBrush brush, Point3 centre, float radius )
		{
			Circle( brush, centre.X, centre.Y, centre.Z, radius );
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
			Circle( brush, x, y, z, radius, DefaultCircleSamples );
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
		/// Fills a rectangle, facing up the yaxis
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Rectangle centre X coordinate</param>
		/// <param name="y">Rectangle centre Y coordinate</param>
		/// <param name="z">Rectangle centre Z coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public abstract void Rectangle( IBrush brush, float x, float y, float z, float width, float height );

		#endregion

		#region Billboards

		//	TODO: AP: Add alternative methods

		/// <summary>
		/// Draws a rectangle, facing the viewer
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="centre">Rectangle centre</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public abstract void Billboard( IPen pen, Point3 centre, float width, float height );

		#endregion

		#region Filled billboards

		//	TODO: AP: Add alternative methods

		/// <summary>
		/// Draws a rectangle, facing up the viewer
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Rectangle centre</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		public abstract void Billboard( IBrush brush, Point3 centre, float width, float height );


		#endregion

		#region Polygons

		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public abstract void Polygon( IPen pen, IEnumerable<Point3> points );

		/// <summary>
		/// Fills a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		public abstract void Polygon( IBrush pen, IEnumerable<Point3> points );

		#endregion

		#region Axis-aligned boxes

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
		/// <param name="surface">Drawing parameters</param>
		/// <param name="centre">Centre of the sphere</param>
		/// <param name="radius">Sphere radius</param>
		public void Sphere( ISurface surface, Point3 centre, float radius )
		{
			Sphere( surface, centre, radius, DefaultSphereSSamples, DefaultSphereTSamples );
		}

		/// <summary>
		/// An ST sphere
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="centre">Centre of the sphere</param>
		/// <param name="radius">Sphere radius</param>
		/// <param name="sSamples">Number of longitudinal samples</param>
		/// <param name="tSamples">Number of latitudinal samples</param>
		public abstract void Sphere( ISurface surface, Point3 centre, float radius, int sSamples, int tSamples );

		#endregion

		#region Cylinders

		/// <summary>
		/// Draws a cylinder. Cylinder is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Cylinder line start point</param>
		/// <param name="end">Cylinder line end point</param>
		/// <param name="radius">Cylinder radius</param>
		public void Cylinder( ISurface surface, Point3 start, Point3 end, float radius )
		{
			Cylinder( surface, start, end, radius, DefaultCylinderSamples );
		}

		/// <summary>
		/// Draws a cylinder. Cylinder is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Cylinder line start point</param>
		/// <param name="end">Cylinder line end point</param>
		/// <param name="radius">Cylinder radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the cylinder</param>
		public abstract void Cylinder( ISurface surface, Point3 start, Point3 end, float radius, int sSamples );

		#endregion

		#region Capsules

		/// <summary>
		/// Draws a capsule. Capsule is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Capsule line start point</param>
		/// <param name="end">Capsule line end point</param>
		/// <param name="radius">Capsule radius</param>
		public void Capsule( ISurface surface, Point3 start, Point3 end, float radius )
		{
			Capsule( surface, start, end, radius, DefaultCapsuleSamples );
		}

		/// <summary>
		/// Draws a capsule. Capsule is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Capsule line start point</param>
		/// <param name="end">Capsule line end point</param>
		/// <param name="radius">Capsule radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the capsule</param>
		public abstract void Capsule( ISurface surface, Point3 start, Point3 end, float radius, int sSamples );

		#endregion

		#endregion

		#endregion

		#region Default parameterizations

		private const int DefaultCircleSamples = 10;
		private const int DefaultSphereSSamples = 10;
		private const int DefaultSphereTSamples = 10;
		private const int DefaultCylinderSamples = 10;
		private const int DefaultCapsuleSamples = 10;

		#endregion
	}
}
