using System;
using RbEngine.Maths;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Summary description for ShapeRenderer.
	/// </summary>
	public abstract class ShapeRenderer
	{
		/// <summary>
		///	Singleton access
		/// </summary>
		public static ShapeRenderer Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		#region 2d Rendering

		/// <summary>
		/// Renders a 2D rectangle
		/// </summary>
		/// <param name="x"> Rectangle top left corner x position </param>
		/// <param name="y"> Rectangle top left corner y position </param>
		/// <param name="width"> Width of the rectangle </param>
		/// <param name="height"> Height of the rectangle</param>
		/// <param name="colour"> Rectangle colour </param>
		public abstract void RenderRectangle( int x, int y, int width, int height, System.Drawing.Color colour );

		/// <summary>
		/// Renders a 2d line
		/// </summary>
		/// <param name="x"> Line start x position </param>
		/// <param name="y"> Line start y position </param>
		/// <param name="endX"> Line end x position </param>
		/// <param name="endY"> Line end y position </param>
		/// <param name="colour"> Line colour </param>
		public abstract void RenderLine( int x, int y, int endX, int endY, System.Drawing.Color colour );

		#endregion

		#region	3d Rendering

		/// <summary>
		/// Renders a 3d line 
		/// </summary>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		public abstract void	RenderLine( Point3 start, Point3 end );

		/// <summary>
		/// Renders a cylinder
		/// </summary>
		/// <param name="start"> Start position of the cylinder </param>
		/// <param name="end"> End position of the cylinder </param>
		/// <param name="radius"> Radius of the cylinder </param>
		public void	RenderCylinder( Point3 start, Point3 end, float radius )
		{
			RenderCylinder( start, end, radius, 16 );
		}

		/// <summary>
		/// Renders a cylinder
		/// </summary>
		/// <param name="start"> Start position of the cylinder </param>
		/// <param name="end"> End position of the cylinder </param>
		/// <param name="radius"> Radius of the cylinder </param>
		/// <param name="numCircumferenceSamples"> Number of subdivisions around the cylinder circumference</param>
		/// 
		public abstract void	RenderCylinder( Point3 start, Point3 end, float radius, int numCircumferenceSamples );

		/// <summary>
		/// Renders a sphere
		/// </summary>
		public void RenderSphere( Point3 pt, float radius )
		{
			RenderSphere( pt, radius, 10, 10 );
		}

		/// <summary>
		/// Renders a sphere, using a given sample rate at which to sample sphere longitude and latitude
		/// </summary>
		public abstract void RenderSphere( Point3 pt, float radius, int latitudeSamples, int longitudeSamples );

		#endregion


		/// <summary>
		/// Protected constructor. Sets the ShapeRenderer.Inst singleton
		/// </summary>
		protected ShapeRenderer( )
		{
			ms_Singleton = this;
		}

		#region	Private stuff

		private static ShapeRenderer	ms_Singleton;

		#endregion
	}
}
