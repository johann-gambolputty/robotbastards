using Rb.Core.Maths;

namespace Rb.Rendering
{
	/// <summary>
	/// Summary description for ShapeRenderer.
	/// </summary>
	public abstract class ShapeRenderer
	{

		#region 2d Rendering

		/// <summary>
		/// Renders a 2D rectangle
		/// </summary>
		/// <param name="x"> Rectangle top left corner x position </param>
		/// <param name="y"> Rectangle top left corner y position </param>
		/// <param name="width"> Width of the rectangle </param>
		/// <param name="height"> Height of the rectangle</param>
		/// <param name="colour"> Rectangle colour </param>
		public abstract void	DrawRectangle( int x, int y, int width, int height, System.Drawing.Color colour );

		/// <summary>
		/// Renders a 2d line
		/// </summary>
		/// <param name="x"> Line start x position </param>
		/// <param name="y"> Line start y position </param>
		/// <param name="endX"> Line end x position </param>
		/// <param name="endY"> Line end y position </param>
		/// <param name="colour"> Line colour </param>
		public abstract void DrawLine( int x, int y, int endX, int endY, System.Drawing.Color colour );

		/// <summary>
		/// Draws an image
		/// </summary>
		/// <param name="x">Screen x position of the image</param>
		/// <param name="y">Screen y position of the image</param>
		/// <param name="width">Screen width of the image</param>
		/// <param name="height">Screen height of the image</param>
		/// <param name="texture">Sprite texture</param>
		public abstract void DrawImage( int x, int y, int width, int height, ITexture2d texture );

		#endregion

		#region	3d Rendering

		/// <summary>
		/// Renders a 3d line 
		/// </summary>
		/// <param name="start">Line start</param>
		/// <param name="end">Line end</param>
		public abstract void	DrawLine( Point3 start, Point3 end );

		/// <summary>
		/// Renders a cylinder
		/// </summary>
		/// <param name="start"> Start position of the cylinder </param>
		/// <param name="end"> End position of the cylinder </param>
		/// <param name="radius"> Radius of the cylinder </param>
		public void				DrawCylinder( Point3 start, Point3 end, float radius )
		{
			DrawCylinder( start, end, radius, 16 );
		}

		/// <summary>
		/// Renders a cylinder
		/// </summary>
		/// <param name="start"> Start position of the cylinder </param>
		/// <param name="end"> End position of the cylinder </param>
		/// <param name="radius"> Radius of the cylinder </param>
		/// <param name="numCircumferenceSamples"> Number of subdivisions around the cylinder circumference</param>
		/// 
		public abstract void	DrawCylinder( Point3 start, Point3 end, float radius, int numCircumferenceSamples );

		/// <summary>
		/// Renders a sphere
		/// </summary>
		public void				DrawSphere( Point3 pt, float radius )
		{
			DrawSphere( pt, radius, 10, 10 );
		}

		/// <summary>
		/// Renders a sphere, using a given sample rate at which to sample sphere longitude and latitude
		/// </summary>
		public abstract void	DrawSphere( Point3 pt, float radius, int latitudeSamples, int longitudeSamples );

		#endregion

		/// <summary>
		/// Draws a string, using this font, at position (x,y)
		/// </summary>
		/// <param name="x">X screen coordinate</param>
		/// <param name="y">Y screen coordinate</param>
		/// <param name="s">String to display</param>
		public void DrawText( int x, int y, string s )
		{
			//	HACK: dirty dirty hack
			if ( m_Font == null )
			{
				m_Font = Graphics.Factory.NewFont( );
				m_Font.Setup( new System.Drawing.Font( "arial", 12 ) );
			}
			m_Font.DrawText( x, y, System.Drawing.Color.White, s );
		}

		#region	Private stuff

		private RenderFont m_Font;

		#endregion
	}
}
