using System;
using RbEngine.Maths;
using Tao.OpenGl;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Summary description for ShapeRenderer.
	/// </summary>
	public class ShapeRenderer
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

		public void RenderRectangle( System.Drawing.Color colour, int x, int y, int width, int height )
		{
			Gl.glColor3ub( colour.R, colour.G, colour.B );
			Gl.glBegin( Gl.GL_LINE_STRIP );

				Gl.glVertex2i( x, y );
				Gl.glVertex2i( x + width, y );
				Gl.glVertex2i( x + width, y + height );
				Gl.glVertex2i( x, y + height );
				Gl.glVertex2i( x, y );

			Gl.glEnd( );
		}

		public void RenderLine( Vector3 start, Vector3 end )
		{
		}

		public void RenderPoint( Vector3 pt )
		{
		}

		#endregion

		#region	3d Rendering

		public void	RenderCylinder( Vector3 start, Vector3 end, float radius )
		{
			RenderCylinder( start, end, radius, 16 );
		}

		public void	RenderCylinder( Vector3 start, Vector3 end, float radius, int numCircumferenceSamples )
		{
			float angleIncrement	= Constants.kTwoPi / ( float )numCircumferenceSamples;
			float angle				= Constants.kTwoPi - angleIncrement;
			float nextAngle			= 0.0f;

			//	Want to rotate vector (0,1,0) to |(end - start)|, push a matrix containing this rotation
			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPushMatrix( );

			Vector3 cylinderVec		= ( end - start );
			float	cylinderLength	= cylinderVec.Length;
			cylinderVec /= cylinderLength;

			Vector3	upVec			= Vector3.YAxis.Dot( cylinderVec ) < 0.99f ? Vector3.YAxis : Vector3.ZAxis;
			Vector3 rotateVec		= Vector3.Cross( upVec, cylinderVec );
			float	rotation		= ( float )System.Math.Acos( upVec.Dot( cylinderVec ) ) * Constants.kRadiansToDegrees;

			Gl.glTranslatef( start.X, start.Y, start.Z );
			Gl.glRotatef( rotation, rotateVec.X, rotateVec.Y, rotateVec.Z );

			Gl.glBegin( Gl.GL_QUADS );

			for ( int SampleCount = 0; SampleCount < numCircumferenceSamples; ++SampleCount )
			{
				float nx		= ( float )System.Math.Sin( angle );
				float nz		= ( float )System.Math.Cos( angle );
				float nextNX	= ( float )System.Math.Sin( nextAngle );
				float nextNZ	= ( float )System.Math.Cos( nextAngle );

				float x 		= nx * radius;
				float y 		= 0.0f;
				float z 		= nz * radius;

				float nextX		= nextNX * radius;
				float nextY		= cylinderLength;
				float nextZ		= nextNZ * radius;

				Gl.glNormal3f( nx, 0, nz );
				Gl.glVertex3f( x, y, z );

				Gl.glNormal3f( nx, 0, nz );
				Gl.glVertex3f( nextX, y, nextZ );

				Gl.glNormal3f( nx, 0, nz );
				Gl.glVertex3f( nextX, nextY, nextZ );

				Gl.glNormal3f( nx, 0, nz );
				Gl.glVertex3f( x, nextY, z );

				angle = nextAngle;
				nextAngle += angleIncrement;
			}

			Gl.glEnd( );

			Gl.glPopMatrix( );
		}

		/// <summary>
		/// Renders a sphere
		/// </summary>
		public void RenderSphere( Vector3 pt, float radius )
		{
			RenderSphere( pt, radius, 10, 10 );
		}

		/// <summary>
		/// Renders a sphere, using a given sample rate at which to sample sphere longitude and latitude
		/// </summary>
		public void RenderSphere( Vector3 pt, float radius, int latitudeSamples, int longitudeSamples )
		{
			//	Render the sphere as a series of strips
			float	latitudeAngleIncrement	= Maths.Constants.kPi / ( float )latitudeSamples;
			float	longitudeAngleIncrement	= Maths.Constants.kTwoPi / ( float )longitudeSamples;

			float t		= 0.0f;
			float nextT	= t + latitudeAngleIncrement;

			Gl.glBegin( Gl.GL_QUADS );

			for ( int latitudeSample = 0; latitudeSample < latitudeSamples; ++latitudeSample )
			{
				float s		= 0.0f;
				float nextS	= s + longitudeAngleIncrement;
				for ( int longitudeSample = 0; longitudeSample < longitudeSamples; ++longitudeSample )
				{
					//	TODO: This is wasteful, because 2 positions are shared from the previous samples
					//	Cache an array of points on the first entry in this function, and read from
					//	it instead of calculating points on the fly.
					RenderST( s, t, radius, pt );
					RenderST( s, nextT, radius, pt );
					RenderST( nextS, nextT, radius, pt );
					RenderST( nextS, t, radius, pt );

					s = nextS;
					nextS += longitudeAngleIncrement;
				}
				t = nextT;
				nextT += latitudeAngleIncrement;
			}

			Gl.glEnd( );
		}

		#endregion

		#region	Private stuff

		private static ShapeRenderer	ms_Singleton	= new ShapeRenderer( );

		/// <summary>
		/// Sphere rendering helper - renders a segment of the sphere
		/// </summary>
		private static void RenderST( float s, float t, float radius, Vector3 centre )
		{
			float cosS = ( float )System.Math.Cos( s );
			float sinS = ( float )System.Math.Sin( s );
			float sinT = ( float )System.Math.Sin( t );
			float cosT = ( float )System.Math.Cos( t );

			float x = cosS * sinT;
			float y = sinS * sinT;
			float z = cosT;

			Gl.glNormal3f( x, y, z );

			x *= radius;
			y *= radius;
			z *= radius;

			Gl.glVertex3f( x + centre.X, y + centre.Y, z + centre.Z );
		}

		#endregion
	}
}
