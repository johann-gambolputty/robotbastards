using System;
using System.Drawing;
using System.Drawing.Imaging;
using Rb.Core.Maths;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Renderer implementation using OpenGL
	/// </summary>
	public class OpenGlRenderer : Renderer
	{
		#region	Setup

		/// <summary>
		/// Sets up the renderer
		/// </summary>
		public OpenGlRenderer( )
		{
			int stackIndex = 0;
			for ( ; stackIndex < m_LocalToWorldStack.Length; ++stackIndex )
			{
				m_LocalToWorldStack[ stackIndex ] = new Matrix44( );
			}
			for ( stackIndex = 0; stackIndex < m_WorldToViewStack.Length; ++stackIndex )
			{
				m_WorldToViewStack[ stackIndex ] = new Matrix44( );
			}
		}

        /// <summary>
		/// Loads all supported OpenGL extensions
		/// </summary>
		public static void LoadExtensions( )
        {	
			//	Show the extensions
            string extensions = Gl.glGetString( Gl.GL_EXTENSIONS );
            GraphicsLog.Info( extensions.Replace( ' ', '\n' ) );

			//	Write some important caps to the info
			int[] result = new int[ 1 ];
			Gl.glGetIntegerv( Gl.GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS, result );
			GraphicsLog.Info( "Max texture units: " + result );

			//	Hinty-hinty
            Gl.glHint( Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST );

			//	

			//	Add a default renderstate
			Graphics.Renderer.PushRenderState( Graphics.Factory.NewRenderState( ) );
		}

		#endregion

		#region	Lighting

		/// <summary>
		/// Clears the light array. This is done every frame
		/// </summary>
		public override void ClearLights( )
		{
			//	TODO: AP: Lights should work for non-effect rendered object
			for ( int lightIndex = 0; lightIndex < NumActiveLights; ++lightIndex )
			{
				Gl.glDisable( Gl.GL_LIGHT0 + lightIndex );
			}
			base.ClearLights( );
		}

		/// <summary>
		/// Adds the specified light
		/// </summary>
		public override void AddLight( Light light )
		{
			//	TODO: AP: Lights should work for non-effect rendered objects. Need to set up lighting properties
			int lightId = Gl.GL_LIGHT0 + NumActiveLights;
			Gl.glEnable( lightId );
			base.AddLight( light );
		}

		#endregion

		#region	Standard operations

		/// <summary>
		/// Checks for errors in the current state of the renderer, throwing an exception if there is one
		/// </summary>
		public static void CheckErrors( )
		{
			int errorCode = Gl.glGetError( );
			if ( errorCode != Gl.GL_NO_ERROR )
			{
				throw new ApplicationException( string.Format( "GL error: {0}", Glu.gluErrorString( errorCode ) ) );
			}
		}

		/// <summary>
		/// Clears the viewport
		/// </summary>
		public override void ClearColour( Color colour )
		{
			Gl.glClearColor( colour.R / 255.0f, colour.G / 255.0f, colour.B / 255.0f, 1.0f );
			Gl.glClear( Gl.GL_COLOR_BUFFER_BIT );
		}

		/// <summary>
		/// Clears the viewport using a vertical gradient fill
		/// </summary>
		public override void ClearVerticalGradient( Color topColour, Color bottomColour )
		{
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			Gl.glDepthMask( 0 );

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
		public override void ClearRadialGradient( Color centreColour, Color outerColour )
		{
			Gl.glMatrixMode( Gl.GL_PROJECTION );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			Gl.glDepthMask( 0 );

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
			Gl.glDepthMask( 1 );
			Gl.glClearDepth( depth );
			Gl.glClear( Gl.GL_DEPTH_BUFFER_BIT );
		}

		/// <summary>
		/// Sets the specified colour as the current colour in the renderer (OpenGL specific)
		/// </summary>
		public void ApplyColour( Color colour )
		{
			Gl.glColor3ub( colour.R, colour.G, colour.B );
		}

		/// <summary>
		/// Draws a 2d quad. Used by ClearVerticalGradient(), ClearRadialGradient()
		/// </summary>
		private void Draw2dQuad( float x, float y, float width, float height, Color tlColour, Color trColour, Color blColour, Color brColour )
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
		/// Helper to convert a Matrix44 to a GL-friendly float array
		/// </summary>
		private static float[] GetGlMatrix( Matrix44 matrix )
		{
			return matrix.Elements;
		}

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		public override Matrix44 GetTransform( Transform type )
		{
			Matrix44 mat = new Matrix44( );
			switch ( type )
			{
				case Transform.LocalToWorld	:
				{
					mat.Copy( CurrentLocalToWorld );
					break;
				}
				case Transform.WorldToView	:
				{
					mat.Copy( CurrentWorldToView );
					break;
				}
				case Transform.ViewToScreen	:
				{
					Gl.glGetFloatv( Gl.GL_PROJECTION_MATRIX, mat.Elements );
					break;
				}
			}

			return mat;
		}

		/// <summary>
		/// Transforms a local point into screen space
		/// </summary>
		/// <param name="pt">Local point</param>
		/// <returns>Screen space point</returns>
		public override Point3 Project( Point3 pt )
		{
			double[]	modelMatrix			= new double[ 16 ];
			double[]	projectionMatrix	= new double[ 16 ];
			int[]		viewport			= new int[ 4 ];

			double winX, winY, winZ;

			Gl.glGetDoublev( Gl.GL_MODELVIEW_MATRIX, modelMatrix );
			Gl.glGetDoublev( Gl.GL_PROJECTION_MATRIX, projectionMatrix );
			Gl.glGetIntegerv( Gl.GL_VIEWPORT, viewport );

			Glu.gluProject( pt.X, pt.Y, pt.Z, modelMatrix, projectionMatrix, viewport, out winX, out winY, out winZ );

			winY = viewport[ 3 ] - winY;

			return new Point3( ( float )winX, ( float )winY, ( float )winZ );
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
		/// Sets up an OpenGL matrix mode that has a direct correspondence with a Transform value
		/// </summary>
		/// <param name="type"></param>
		private static void SetSupportedMatrixMode( Transform type )
		{
			switch ( type )
			{
				case Transform.ViewToScreen :
				{
					Gl.glMatrixMode( Gl.GL_PROJECTION );
					break;
				}
				case Transform.Texture0		:
				{
					Gl.glMatrixMode( Gl.GL_TEXTURE );
					break;
				}
				//	TODO: HMMmmmm I'm not really sure how to handle texture transforms, because that implies setting the active texture stage to 1-7, 
				//	with no nice way of resetting it after... I suppose RB could use the transform state model that OpenGL uses, because that would
				//	map easily to DirectX (but the reverse isn't easy)
				default : throw new ApplicationException( string.Format( "\"{0}\" is not supported in Translate()", type ) );
			}
		}
		
		/// <summary>
		/// Scales the current transform in the specified transform stack
		/// </summary>
		public override void Scale( Transform type, float scaleX, float scaleY, float scaleZ )
		{
			switch ( type )
			{
				case Transform.LocalToWorld :
				{
					CurrentLocalToWorld.Scale( scaleX, scaleY, scaleZ );
					UpdateModelView( );
					break;
				}
				case Transform.WorldToView :
				{
					CurrentWorldToView.Scale( scaleX, scaleY, scaleZ );
					UpdateModelView( );
					break;
				}
				default :
				{
					SetSupportedMatrixMode( type );
					Gl.glTranslatef( scaleX, scaleY, scaleZ );
					break;
				}
			}
		}

		/// <summary>
		/// Translates the current transform in the specified transform stack
		/// </summary>
		public override void	Translate( Transform type, float x, float y, float z )
		{
			switch ( type )
			{
				case Transform.LocalToWorld :
				{
					CurrentLocalToWorld.Translate( x, y, z );
					UpdateModelView( );
					break;
				}
				case Transform.WorldToView :
				{
					CurrentWorldToView.Translate( x, y, z );
					UpdateModelView( );
					break;
				}
				default :
				{
					SetSupportedMatrixMode( type );
					Gl.glTranslatef( x, y, z );
					break;
				}
			}
		}

		/// <summary>
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		public override void PushTransform( Transform type, Matrix44 matrix )
		{
			switch ( type )
			{
				case Transform.LocalToWorld :
				{
					Matrix44 lastLocalToWorld = CurrentLocalToWorld;
					++m_TopOfLocalToWorldStack;
					CurrentLocalToWorld.StoreMultiply( lastLocalToWorld, matrix );
					UpdateModelView( );
					break;
				}

				case Transform.WorldToView :
				{
					Matrix44 lastWorldToView = CurrentWorldToView;
					++m_TopOfWorldToViewStack;
					CurrentWorldToView.StoreMultiply( lastWorldToView, matrix );
					UpdateModelView( );
					break;
				}

				default :
				{
					SetSupportedMatrixMode( type );
					Gl.glPushMatrix( );
					Gl.glMultMatrixf( GetGlMatrix( matrix ) );
					break;
				}
			}
		}

		/// <summary>
		/// Pushes a copy of the transform currently at the top of the specified transform stack
		/// </summary>
		public override void	PushTransform( Transform type )
		{
			switch ( type )
			{
				case Transform.LocalToWorld :
				{
					Matrix44 lastLocalToWorld = CurrentLocalToWorld;
					++m_TopOfLocalToWorldStack;
					CurrentLocalToWorld.Copy( lastLocalToWorld );
					UpdateModelView( );
					break;
				}

				case Transform.WorldToView :
				{
					Matrix44 lastWorldToView = CurrentWorldToView;
					++m_TopOfWorldToViewStack;
					CurrentWorldToView.Copy( lastWorldToView );
					UpdateModelView( );
					break;
				}

				default :
				{
					SetSupportedMatrixMode( type );
					Gl.glPushMatrix( );
					break;
				}
			}
		}

		/// <summary>
		/// Sets the current Transform.kLocalToView transform to a look-at matrix
		/// </summary>
		public override void SetLookAtTransform( Point3 lookAt, Point3 camPos, Vector3 camYAxis )
		{
			CurrentWorldToView.SetLookAt( camPos, lookAt, camYAxis );
			UpdateModelView( );
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
			switch ( type )
			{
				case Transform.LocalToWorld :
				{
					CurrentLocalToWorld.Copy( matrix );
					UpdateModelView( );
					break;
				}

				case Transform.WorldToView :
				{
					CurrentWorldToView.Copy( matrix );
					UpdateModelView( );
					break;
				}

				default :
				{
					SetSupportedMatrixMode( type );
					Gl.glLoadMatrixf( GetGlMatrix( matrix ) );
					break;
				}
			}
		}

		/// <summary>
		/// Pops a matrix from the specified transform stack, applies the new topmost matrix
		/// </summary>
		public override void PopTransform( Transform type )
		{
			switch ( type )
			{
				case Transform.LocalToWorld :
				{
					--m_TopOfLocalToWorldStack;
					UpdateModelView( );
					break;
				}

				case Transform.WorldToView :
				{
					--m_TopOfWorldToViewStack;
					UpdateModelView( );
					break;
				}

				default :
				{
					SetSupportedMatrixMode( type );
					Gl.glPopMatrix( );
					break;
				}
			}
		}

		/// <summary>
		/// Sets the viewport (in pixels)
		/// </summary>
		public override void SetViewport( int x, int y, int width, int height )
		{
			m_X			= x;
			m_Y			= y;
			m_Width		= width;
			m_Height	= height;
			Gl.glViewport( x, y, width, height );
		}

		private int m_X;
		private int m_Y;
		private int	m_Width;
		private int m_Height;

		/// <summary>
		/// Gets the current viewport
		/// </summary>
		public override System.Drawing.Rectangle Viewport
		{
			get { return new System.Drawing.Rectangle( m_X, m_Y, m_Width, m_Height );}
		}

		/// <summary>
		///	The viewport width
		/// </summary>
		public override int ViewportWidth
		{
			get
			{
				return m_Width;
			}
		}

		/// <summary>
		/// The viewport height
		/// </summary>
		public override int ViewportHeight
		{
			get
			{
				return m_Height;
			}
		}

		#endregion

		#region	Unprojection
		
		/// <summary>
		/// Unprojects a point from screen space into world space
		/// </summary>
		public override Point3 Unproject( int x, int y, float depth )
		{
			double[]	modelMatrix			= new double[ 16 ];
			double[]	projectionMatrix	= new double[ 16 ];
			int[]		viewport			= new int[ 4 ];

			double outX;
			double outY;
			double outZ;

			Gl.glGetDoublev( Gl.GL_MODELVIEW_MATRIX, modelMatrix );
			Gl.glGetDoublev( Gl.GL_PROJECTION_MATRIX, projectionMatrix );
			Gl.glGetIntegerv( Gl.GL_VIEWPORT, viewport );

			//	Correct windows screen space into openGL screen space
			double inX	= x;
			double inY	= ( double )viewport[ 3 ] - y;
			double inZ	= depth;

			Glu.gluUnProject( inX, inY, inZ, modelMatrix, projectionMatrix, viewport, out outX, out outY, out outZ );

			return new Point3( ( float )outX, ( float )outY, ( float )outZ );
		}


		/// <summary>
		/// Makes a 3d ray in world space from a screen space position
		/// </summary>
		public override Ray3 PickRay( int x, int y )
		{
			double[]	modelMatrix			= new double[ 16 ];
			double[]	projectionMatrix	= new double[ 16 ];
			int[]		viewport			= new int[ 4 ];

			double outX;
			double outY;
			double outZ;

			Gl.glGetDoublev( Gl.GL_MODELVIEW_MATRIX, modelMatrix );
			Gl.glGetDoublev( Gl.GL_PROJECTION_MATRIX, projectionMatrix );
			Gl.glGetIntegerv( Gl.GL_VIEWPORT, viewport );

			//	Correct windows screen space into openGL screen space
			double inX = x;
			double inY = ( double )viewport[ 3 ] - y;

			//	TODO:This isn't right - the pick ray origin should be the camera origin
			Glu.gluUnProject( inX, inY, 0, modelMatrix, projectionMatrix, viewport, out outX, out outY, out outZ );

			Ray3 result = new Ray3( );
			result.Origin.Set( ( float )outX, ( float )outY, ( float )outZ );

			Glu.gluUnProject( inX, inY, 1, modelMatrix, projectionMatrix, viewport, out outX, out outY, out outZ );
			result.Direction = new Vector3( ( float )outX, ( float )outY, ( float )outZ );

			result.Direction.Normalise( );

			return result;
		}

		#endregion

		#region	Frame dumps

		/// <summary>
		/// Creates an Image object from the colour buffer
		/// </summary>
		public override unsafe Image ColourBufferToImage( )
		{
			int width			= ViewportWidth;
			int height			= ViewportHeight;
			int bytesPerPixel	= 4;	//	TODO: Bodge
			byte[] bufferMem = new byte[ width * height * bytesPerPixel ];
			Gl.glReadPixels( 0, 0, width, height, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, bufferMem );

			Bitmap bmp;
			fixed ( byte* bufferMemPtr = bufferMem )
			{
				bmp = new Bitmap( width, height, width * bytesPerPixel, PixelFormat.Format32bppArgb, ( IntPtr )bufferMemPtr );
			}

			return bmp;
		}

		/// <summary>
		/// Creates an Image object from the depth buffer
		/// </summary>
		public override unsafe Image DepthBufferToImage( )
		{
			int width			= ViewportWidth;
			int height			= ViewportHeight;
			int bytesPerPixel	= 3;

			float[] depthMem = new float[ width * height ];

			Gl.glDepthRange( 0, 1 );
			Gl.glPixelTransferf( Gl.GL_DEPTH_SCALE, 1 );
			Gl.glPixelTransferf( Gl.GL_DEPTH_BIAS, 0 );
			Gl.glReadPixels( 0, 0, width, height, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, depthMem );

			CheckErrors( );

			byte[] bufferMem = new byte[ width * height * bytesPerPixel ];
			int pixIndex = 0;
			for ( int depthIndex = 0; depthIndex < depthMem.Length; ++depthIndex )
			{
				bufferMem[ pixIndex++ ] = ( byte )( depthMem[ depthIndex ] * 255.0f );
				bufferMem[ pixIndex++ ] = ( byte )( depthMem[ depthIndex ] * 255.0f );
				bufferMem[ pixIndex++ ] = ( byte )( depthMem[ depthIndex ] * 255.0f );
			}

			Bitmap bmp;
			fixed ( byte* bufferMemPtr = bufferMem )
			{
				bmp = new Bitmap( width, height, width * bytesPerPixel, PixelFormat.Format24bppRgb, ( IntPtr )bufferMemPtr );
			}

			return bmp;
		}

		#endregion

		#region	Local to world transforms

		/// <summary>
		/// Gets the current local to world transform
		/// </summary>
		private Matrix44 CurrentLocalToWorld
		{
			get
			{
				return m_LocalToWorldStack[ m_TopOfLocalToWorldStack ];
			}
		}
		

		private readonly Matrix44[] m_LocalToWorldStack = new Matrix44[ 8 ];
		private int m_TopOfLocalToWorldStack	= 0;

		#endregion

		#region	World to view transforms

		/// <summary>
		/// Gets the current world to view transform
		/// </summary>
		private Matrix44 CurrentWorldToView
		{
			get
			{
				return m_WorldToViewStack[ m_TopOfWorldToViewStack ];
			}
		}

		private readonly Matrix44[] m_WorldToViewStack = new Matrix44[ 4 ];
		private int m_TopOfWorldToViewStack = 0;

		#endregion

		#region	ModelView transform

		/// <summary>
		/// Updates the local to view matrix
		/// </summary>
		private void UpdateModelView( )
		{
			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glLoadMatrixf( GetGlMatrix( CurrentWorldToView ) );
			Gl.glMultMatrixf( GetGlMatrix( CurrentLocalToWorld ) );
		}

		#endregion
	}

}
