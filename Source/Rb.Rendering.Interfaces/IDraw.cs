using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Interfaces
{

	/// <summary>
	/// Simple 2d and 3d shape drawing interface
	/// </summary>
	public interface IDraw
	{
		#region Caching

		/// <summary>
		/// Starts caching draw calls. When <see cref="StopCache()"/> is called, a renderable
		/// object representing the cache will be created
		/// </summary>
		void StartCache( );

		/// <summary>
		/// Stops caching draw calls
		/// </summary>
		/// <returns>Returns a renderable object representing all cached draw calls</returns>
		IRenderable StopCache( );

		#endregion

		#region Pens

		/// <summary>
		/// Creates a new IPen, with a width of 1, drawing with the specified colour
		/// </summary>
		/// <param name="colour">Pen colour</param>
		/// <returns>New IPen</returns>
		Draw.IPen NewPen( Color colour );

		/// <summary>
		/// Creates a new IPen, with a specified width and colour
		/// </summary>
		/// <param name="colour">Pen colour</param>
		/// <param name="width">Pen width</param>
		/// <returns>New IPen</returns>
		Draw.IPen NewPen( Color colour, float width );

		#endregion

		#region Brushes

		/// <summary>
		/// Creates a new colour IBrush
		/// </summary>
		/// <param name="colour">Brush colour</param>
		/// <returns>New IBrush</returns>
		Draw.IBrush NewBrush( Color colour );

		/// <summary>
		/// Creates a new colour IBrush
		/// </summary>
		/// <param name="colour">Brush colour</param>
		/// <param name="outlineColour">Brush outline colour</param>
		/// <returns>New IBrush</returns>
		Draw.IBrush NewBrush( Color colour, Color outlineColour );

		#endregion

		#region Surfaces

		/// <summary>
		/// Creates a new surface
		/// </summary>
		Draw.ISurface NewSurface( Draw.IBrush faceBrush, Draw.IPen edgePen );

		/// <summary>
		/// Creates a new surface
		/// </summary>
		Draw.ISurface NewSurface( Color faceColour );
		
		/// <summary>
		/// Creates a new surface
		/// </summary>
		Draw.ISurface NewSurface( Color faceColour, Color edgeColour );

		#endregion

		#region 2D

		#region Lines

		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		void Line( Draw.IPen pen, Point start, Point end );

		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="startX">Line start X coordinate</param>
		/// <param name="startY">Line start Y coordinate</param>
		/// <param name="endX">Line end X coordinate</param>
		/// <param name="endY">Line end Y coordinate</param>
		void Line( Draw.IPen pen, float startX, float startY, float endX, float endY );

		/// <summary>
		/// Draws a series of lines connecting a list of points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		void Lines( Draw.IPen pen, IEnumerable< Point2 > points );

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="close">If true, then a line is drawn connecting the last point to the first</param>
		void Lines( Draw.IPen pen, IEnumerable<Point2> points, bool close );

		#endregion

		#region Circles

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		void Circle( Draw.IPen pen, Point centre, float radius );

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		void Circle( Draw.IPen pen, float x, float y, float radius, int samples );

		#endregion

		#region Filled circles

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		void Circle( Draw.IBrush brush, Point centre, float radius );

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="radius">Circle radius</param>
		void Circle( Draw.IBrush brush, float x, float y, float radius );

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		void Circle( Draw.IBrush brush, float x, float y, float radius, int samples );

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
		void Rectangle( Draw.IPen pen, float x, float y, float width, float height );

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
		void Rectangle( Draw.IBrush brush, float x, float y, float width, float height );

		#endregion

		#region Polygons
		
		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		void Polygon( Draw.IPen pen, IEnumerable< Point2 > points );
		
		#endregion

		#region Filled polygons

		/// <summary>
		/// Draws a filled polygon
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		void Polygon( Draw.IBrush brush, IEnumerable< Point2 > points );

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
		void Line( Draw.IPen pen, float x, float y, float z, float endX, float endY, float endZ );

		/// <summary>
		/// Draws a line between two points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		void Line( Draw.IPen pen, Point3 start, Point3 end );

		/// <summary>
		/// Draws a series of lines connecting a list of points
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		void Lines( Draw.IPen pen, IEnumerable< Point3 > points );

		/// <summary>
		/// Draws a series of lines connecting a list of points. Can join the last point to the first
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Points to connect</param>
		/// <param name="close">If true, then a line is drawn connecting the last point to the first</param>
		void Lines( Draw.IPen pen, IEnumerable< Point3 > points, bool close );

		#endregion

		#region Circles

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		void Circle( Draw.IPen pen, Point3 centre, float radius );

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		void Circle( Draw.IPen pen, float x, float y, float z, float radius );

		/// <summary>
		/// Draws a circle
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		void Circle( Draw.IPen pen, float x, float y, float z, float radius, int samples );

		#endregion

		#region Filled circles

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Circle centre</param>
		/// <param name="radius">Circle radius</param>
		void Circle( Draw.IBrush brush, Point3 centre, float radius );
		
		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		void Circle( Draw.IBrush brush, float x, float y, float z, float radius );

		/// <summary>
		/// Fills a circle
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Circle centre X coordinate</param>
		/// <param name="y">Circle centre Y coordinate</param>
		/// <param name="z">Circle centre Z coordinate</param>
		/// <param name="radius">Circle radius</param>
		/// <param name="samples">Number of subdivisions around the circumference</param>
		void Circle( Draw.IBrush brush, float x, float y, float z, float radius, int samples );

		#endregion

		#region Rectangles

		/// <summary>
		/// Draws a rectangle, facing up the y axis
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="centre">Rectangle centre</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		void Rectangle( Draw.IPen pen, Point3 centre, float width, float height );

		/// <summary>
		/// Draws a rectangle, facing up the yaxis
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="x">Rectangle centre X coordinate</param>
		/// <param name="y">Rectangle centre Y coordinate</param>
		/// <param name="z">Rectangle centre Z coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		void Rectangle( Draw.IPen pen, float x, float y, float z, float width, float height );

		#endregion

		#region Filled Rectangles

		/// <summary>
		/// Draws a rectangle, facing up the y axis
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Rectangle centre</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		void Rectangle( Draw.IBrush brush, Point3 centre, float width, float height );

		/// <summary>
		/// Draws a rectangle, facing up the yaxis
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="x">Rectangle centre X coordinate</param>
		/// <param name="y">Rectangle centre Y coordinate</param>
		/// <param name="z">Rectangle centre Z coordinate</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		void Rectangle( Draw.IBrush brush, float x, float y, float z, float width, float height );

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
		void Billboard( Draw.IPen pen, Point3 centre, float width, float height );

		#endregion

		#region Filled billboards

		//	TODO: AP: Add alternative methods

		/// <summary>
		/// Draws a rectangle, facing the viewer
		/// </summary>
		/// <param name="brush">Drawing properties</param>
		/// <param name="centre">Rectangle centre</param>
		/// <param name="width">Rectangle width</param>
		/// <param name="height">Rectangle height</param>
		void Billboard( Draw.IBrush brush, Point3 centre, float width, float height );


		#endregion

		#region Polygons

		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		void Polygon( Draw.IPen pen, IEnumerable< Point3 > points );

		#endregion

		#region Filled Polygons

		/// <summary>
		/// Draws a polygon
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="points">Polygon points</param>
		void Polygon( Draw.IBrush pen, IEnumerable< Point3 > points );

		#endregion

		#region Axis aligned boxes

		/// <summary>
		/// Draws an axis aligned box
		/// </summary>
		/// <param name="pen">Drawing properties</param>
		/// <param name="min">Top left back position</param>
		/// <param name="max">Bottom right front position</param>
		void AlignedBox( Draw.IPen pen, Point3 min, Point3 max );

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
		void AlignedBox( Draw.IPen pen, float minX, float minY, float minZ, float maxX, float maxY, float maxZ );

		#endregion

		#region Spheres

		/// <summary>
		/// Draws a sphere
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="centre">Centre of the sphere</param>
		/// <param name="radius">Sphere radius</param>
		void Sphere( Draw.ISurface surface, Point3 centre, float radius );

		/// <summary>
		/// An ST sphere
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="centre">Centre of the sphere</param>
		/// <param name="radius">Sphere radius</param>
		/// <param name="sSamples">Number of longitudinal samples</param>
		/// <param name="tSamples">Number of latitudinal samples</param>
		void Sphere( Draw.ISurface surface, Point3 centre, float radius, int sSamples, int tSamples );

		#endregion

		#region Cylinders

		/// <summary>
		/// Draws a cylinder. Cylinder is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Cylinder line start point</param>
		/// <param name="end">Cylinder line end point</param>
		/// <param name="radius">Cylinder radius</param>
		void Cylinder( Draw.ISurface surface, Point3 start, Point3 end, float radius );

		/// <summary>
		/// Draws a cylinder. Cylinder is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Cylinder line start point</param>
		/// <param name="end">Cylinder line end point</param>
		/// <param name="radius">Cylinder radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the cylinder</param>
		void Cylinder( Draw.ISurface surface, Point3 start, Point3 end, float radius, int sSamples );

		#endregion

		#region Capsules
		
		/// <summary>
		/// Draws a capsule. Capsule is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Capsule line start point</param>
		/// <param name="end">Capsule line end point</param>
		/// <param name="radius">Capsule radius</param>
		void Capsule( Draw.ISurface surface, Point3 start, Point3 end, float radius );

		/// <summary>
		/// Draws a capsule. Capsule is defined by a line and a radius value
		/// </summary>
		/// <param name="surface">Drawing parameters</param>
		/// <param name="start">Capsule line start point</param>
		/// <param name="end">Capsule line end point</param>
		/// <param name="radius">Capsule radius</param>
		/// <param name="sSamples">Number of samples around the circumference of the capsule</param>
		void Capsule( Draw.ISurface surface, Point3 start, Point3 end, float radius, int sSamples );

		#endregion

		#endregion

		#region Primitive lists

		/// <summary>
		/// Starts building a list of primitives
		/// </summary>
		/// <param name="primitive">Type of primitive</param>
		void BeginPrimitiveList( PrimitiveType primitive );

		/// <summary>
		/// Adds vertex data to the current primitive list. If semantic is <see cref="VertexFieldSemantic.Position"/>,
		/// then a new vertex is started.
		/// </summary>
		void AddVertexData( VertexFieldSemantic semantic, float x );

		/// <summary>
		/// Adds vertex data to the current primitive list. If semantic is <see cref="VertexFieldSemantic.Position"/>,
		/// then a new vertex is started.
		/// </summary>
		void AddVertexData( VertexFieldSemantic semantic, float x, float y );

		/// <summary>
		/// Adds vertex data to the current primitive list. If semantic is <see cref="VertexFieldSemantic.Position"/>,
		/// then a new vertex is started.
		/// </summary>
		void AddVertexData( VertexFieldSemantic semantic, Point2 pt );

		/// <summary>
		/// Adds vertex data to the current primitive list. If semantic is <see cref="VertexFieldSemantic.Position"/>,
		/// then a new vertex is started.
		/// </summary>
		void AddVertexData( VertexFieldSemantic semantic, float x, float y, float z );

		/// <summary>
		/// Adds vertex data to the current primitive list. If semantic is <see cref="VertexFieldSemantic.Position"/>,
		/// then a new vertex is started.
		/// </summary>
		void AddVertexData( VertexFieldSemantic semantic, Point3 pt );

		/// <summary>
		/// Ends the current primitive list, drawing it, or storing it in the current cache
		/// </summary>
		void EndPrimitiveList( );

		#endregion
	}
}
