using System;
using RbEngine.Rendering;
using RbEngine.Maths;
using Tao.OpenGl;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Summary description for OpenGlRenderer.
	/// </summary>
	public class OpenGlRenderer : Renderer
	{
		public OpenGlRenderer( )
		{
			Gl.glHint( Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST );
			Gl.glDepthFunc( Gl.GL_LEQUAL );
			Gl.glShadeModel( Gl.GL_SMOOTH );
		}
		
		#region	Standard operations

		/// <summary>
		/// Checks for errors in the current state of the renderer, throwing an exception if there is one
		/// </summary>
		public void CheckErrors( )
		{
			int errorCode = Gl.glGetError( );
			if ( errorCode != Gl.GL_NO_ERROR )
			{
				throw new System.ApplicationException( String.Format( "GL error: {0}", Glu.gluErrorString( errorCode ) ) );
			}
		}

		/// <summary>
		/// Clears the viewport
		/// </summary>
		public override void	ClearColour( System.Drawing.Color colour )
		{
			Gl.glClearColor( ( float )colour.R / 255.0f, ( float )colour.G / 255.0f, ( float )colour.B / 255.0f, 1.0f );
			Gl.glClear( Gl.GL_COLOR_BUFFER_BIT );
		}

		/// <summary>
		/// Clears the viewport using a vertical gradient fill
		/// </summary>
		public override void ClearVerticalGradient( System.Drawing.Color topColour, System.Drawing.Color bottomColour )
		{
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			Gl.glDepthMask( false );

			Gl.glBegin( Gl.GL_QUADS );
			Draw2dQuad( -1, -1, 2, 2, topColour, topColour, bottomColour, bottomColour );
			Gl.glEnd( );

			Gl.glPopMatrix( );
			
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPopMatrix( );
		}

		/// <summary>
		/// Clears the viewport using a radial gradient fill (shit)
		/// </summary>
		public override void ClearRadialGradient( System.Drawing.Color centreColour, System.Drawing.Color outerColour )
		{
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			Gl.glDepthMask( false );

			Gl.glBegin( Gl.GL_QUADS );
			Draw2dQuad( -1, -1, 1, 1, outerColour, centreColour, outerColour, outerColour );
			Draw2dQuad(  0,  0, 1, 1, outerColour, outerColour, centreColour, outerColour );
			Draw2dQuad( -1,  0, 1, 1, outerColour, outerColour, outerColour, centreColour );
			Draw2dQuad(  0, -1, 1, 1, centreColour, outerColour, outerColour, outerColour );
			Gl.glEnd( );

			Gl.glPopMatrix( );

			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPopMatrix( );
		}

		/// <summary>
		/// Clears the depth buffer
		/// </summary>
		public override void ClearDepth( float depth )
		{
			Gl.glClearDepth( 1.0f );
			Gl.glClear( Gl.GL_DEPTH_BUFFER_BIT );
		}

		/// <summary>
		/// Sets the specified colour as the current colour in the renderer (OpenGL specific)
		/// </summary>
		public override void ApplyColour( System.Drawing.Color colour )
		{
			Gl.glColor3ub( colour.R, colour.G, colour.B );
		}

		/// <summary>
		/// Draws a 2d quad. Used by ClearVerticalGradient(), ClearRadialGradient()
		/// </summary>
		private void Draw2dQuad( float x, float y, float width, float height, System.Drawing.Color tlColour, System.Drawing.Color trColour, System.Drawing.Color blColour, System.Drawing.Color brColour )
		{
			float maxX = x + width;
			float maxY = y + height;

			ApplyColour( blColour );
			Gl.glVertex2f( x, y );

			ApplyColour( tlColour );
			Gl.glVertex2f( x, maxY );

			ApplyColour( trColour );
			Gl.glVertex2f( maxX, maxY );

			ApplyColour( brColour );
			Gl.glVertex2f( maxX, y );
		}

		#endregion

		#region	Transform pipeline

		/// <summary>
		/// Helper to set the current transform mode
		/// </summary>
		private void SetTransformMode( Transform type )
		{
			switch ( type )
			{
				case Transform.kLocalToView		:	Gl.glMatrixMode( Gl.GL_MODELVIEW );		break;
				case Transform.kViewToScreen	:	Gl.glMatrixMode( Gl.GL_PROJECTION );	break;
			}
		}

		/// <summary>
		/// Helper to convert a Matrix44 to a GL-friendly float array
		/// </summary>
		private float[] GetGlMatrix( Matrix44 matrix )
		{
			return matrix.Elements;
		}

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		public override Matrix44 GetMatrix( Transform type )
		{
			Matrix44 mat = new Matrix44( );
			switch ( type )
			{
				case Transform.kLocalToView		:	Gl.glGetFloatv( Gl.GL_MODELVIEW_MATRIX, mat.Elements );		break;
				case Transform.kViewToScreen	:	Gl.glGetFloatv( Gl.GL_PROJECTION_MATRIX, mat.Elements );	break;
			}

			return mat;
		}

		/// <summary>
		/// Sets an identity matrix in the projection and model view transforms
		/// </summary>
		public override void Set2d( )
		{
			Gl.glMatrixMode( Gl.GL_PROJECTION );

			int[] viewport = new int[ 4 ];
			Gl.glGetIntegerv( Gl.GL_VIEWPORT, viewport );
			Gl.glLoadIdentity( );
			Gl.glOrtho( viewport[ 0 ], viewport[ 0 ] + viewport[ 2 ], viewport[ 1 ] + viewport[ 3 ], viewport[ 1 ], -1, 1 );

			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glLoadIdentity( );
		}

		/// <summary>
		/// Pushes an identity matrix in the projection and model view transforms. The top left hand corner is (X,Y), the bottom right is (W,H) (where
		/// (W,H) are the viewport dimensions, and (X,Y) is the viewport minimum corner position)
		/// </summary>
		public override void Push2d( )
		{
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPushMatrix( );

			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPushMatrix( );

			Set2d( );
		}

		/// <summary>
		/// Pops the identity matrices pushed by Push2d( )
		/// </summary>
		public override void Pop2d( )
		{
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPopMatrix( );

			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPopMatrix( );
		}

		/// <summary>
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		public override void	PushTransform( Transform type, Matrix44 matrix )
		{
			SetTransformMode( type );
			Gl.glPushMatrix( );
			Gl.glMultMatrixf( GetGlMatrix( matrix ) );
		}

		/// <summary>
		/// Sets the current Transform.kLocalToView transform to a look-at matrix
		/// </summary>
		public override void SetLookAtTransform( Vector3 lookAt, Vector3 camPos, Vector3 camYAxis )
		{
			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glLoadIdentity( );
			Glu.gluLookAt( camPos.X, camPos.Y, camPos.Z, lookAt.X, lookAt.Y, lookAt.Z, camYAxis.X, camYAxis.Y, camYAxis.Z );
		}

		/// <summary>
		/// Sets the current Transform.kViewToScreen matrix to a projection matrix with the specified attributes
		/// </summary>
		public override void SetPerspectiveProjectionTransform( float fov, float aspectRatio, float zNear, float zFar )
		{
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glLoadIdentity( );
			Glu.gluPerspective( fov, aspectRatio, zNear, zFar );
		}

		/// <summary>
		/// Applies the specified transform, adds it to the specified transform stack
		/// </summary>
		public override void SetTransform( Transform type, Matrix44 matrix )
		{
			SetTransformMode( type );
			Gl.glLoadMatrixf( GetGlMatrix( matrix ) );
		}

		/// <summary>
		/// Pops a matrix from the specified transform stack, applies the new topmost matrix
		/// </summary>
		public override void PopTransform( Transform type )
		{
			SetTransformMode( type );
			Gl.glPopMatrix( );
		}

		#endregion

		#region	Picking

		/// <summary>
		/// Makes a 3d ray in world space from a screen space position
		/// </summary>
		public override Ray3 PickRay( int X, int Y )
		{
			double[] ModelMatrix		= new double[ 16 ];
			double[] ProjectionMatrix	= new double[ 16 ];
			int[] Viewport				= new int[ 4 ];

			double OutX;
			double OutY;
			double OutZ;

			Gl.glGetDoublev( Gl.GL_MODELVIEW_MATRIX, ModelMatrix );
			Gl.glGetDoublev( Gl.GL_PROJECTION_MATRIX, ProjectionMatrix );
			Gl.glGetIntegerv( Gl.GL_VIEWPORT, Viewport );

			//	Correct windows screen space into openGL screen space
			double InX = ( double )X;
			double InY = ( double )Viewport[ 3 ] - Y;
			double InZ	= 0;

			Glu.gluUnProject( InX, InY, InZ, ModelMatrix, ProjectionMatrix, Viewport, out OutX, out OutY, out OutZ );

			Ray3 Result = new Ray3( );
			Result.Origin.Set( ( float )OutX, ( float )OutY, ( float )OutZ );

			InZ	= 1;
			Glu.gluUnProject( InX, InY, InZ, ModelMatrix, ProjectionMatrix, Viewport, out OutX, out OutY, out OutZ );
			Result.Direction = new Vector3( ( float )OutX, ( float )OutY, ( float )OutZ );

			Result.Direction.Normalise( );

			return Result;
		}

		#endregion
	}
}
