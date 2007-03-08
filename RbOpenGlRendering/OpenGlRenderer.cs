using System;
using System.Collections;
using System.Windows.Forms;
using RbEngine;
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
		#region	Setup

		private Matrix44 GetMv( )
		{
			Matrix44 mat = new Matrix44( );
			Gl.glGetFloatv( Gl.GL_MODELVIEW_MATRIX, mat.Elements );
			return mat;
		}

		private void PrintMatrix( Matrix44 mat )
		{
			for ( int row = 0; row < 4; ++row )
			{
				for ( int col = 0; col < 4; ++col )
				{
					System.Diagnostics.Debug.Write( string.Format( "{0} ", mat[ col, row ].ToString( "G4" ) ) );
				}
				System.Diagnostics.Debug.WriteLine( "" );
			}
		}

		private void EnsureMvCorrectness( Matrix44 test )
		{
			Matrix44 mvMat = GetMv( );

			System.Diagnostics.Debug.WriteLine( "Matrix44:" );
			PrintMatrix( test );

			System.Diagnostics.Debug.WriteLine( "\nGL Matrix44" );
			PrintMatrix( mvMat );

			for ( int row = 0; row < 4; ++row )
			{
				for ( int col = 0; col < 4; ++col )
				{
					float diff = mvMat[ col, row ] - test[ col, row ];
					Output.DebugAssert( System.Math.Abs( diff ) < 0.1f, Output.RenderingError, "MV not correct" );
				}
			}
		}

		/// <summary>
		/// Sets up the renderer
		/// </summary>
		public OpenGlRenderer( )
		{
			Gl.glHint( Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST );

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

		private void TestMatrices( )
		{
			Gl.glMatrixMode( Gl.GL_MODELVIEW );
			Gl.glPushMatrix( );
			Gl.glLoadIdentity( );

			//	Test stuff...
			Matrix44 testMatrix = new Matrix44( );
			Gl.glTranslatef( 10, 0, 0 );		testMatrix.Translate( 10, 0, 0 );
			EnsureMvCorrectness( testMatrix );

			Gl.glTranslatef( 10, 0, 0 );		testMatrix.Translate( 10, 0, 0 );
			EnsureMvCorrectness( testMatrix );

			float[] testArray = new float[ 16 ]
			{
				0, 0, 1, 0,
			    0, 1,  0, 0,
			    -1, 0,  0, 0,
			    0, 0,  0, 1
			};

			Gl.glLoadMatrixf( testArray );
			Gl.glTranslatef( -10, 0, 0 );

		//	Gl.glLoadIdentity( ); Glu.gluLookAt( 10, 0, 0, 0, 0, 0, 0, 1, 0 );
			testMatrix.SetLookAt( new Point3( 10, 0, 0 ), Point3.Origin, Vector3.YAxis );
			EnsureMvCorrectness( testMatrix );

			

			Gl.glPopMatrix( );

		}

		/// <summary>
		/// Loads all supported OpenGL extensions
		/// </summary>
		public unsafe static void LoadExtensions( )
		{
			sbyte* mem = ( sbyte* )Gl.glGetString( Gl.GL_EXTENSIONS );
			string extensions = new string( mem );
			Output.WriteLine( Output.RenderingInfo, extensions.Replace( ' ', '\n' ) );

			GlExtensionLoader.LoadAllExtensions( );

		}

		#endregion

		#region	Forms

		/// <summary>
		/// Sets up a control to be rendered to by this renderer
		/// </summary>
		/// <param name="control"> Control to set up </param>
		public override ControlRenderContext	CreateControlContext( System.Windows.Forms.Control control )
		{
			return new OpenGlControlRenderContext( );
		}

		#endregion

		#region	Lighting

		#endregion

		#region	Standard operations

		/// <summary>
		/// Current control setup
		/// </summary>
		public override Control CurrentControl
		{
			set
			{
				base.CurrentControl = value;
				SetViewport( 0, 0, value.Width, value.Height );
			//	TestMatrices( );
			}
		}


		/// <summary>
		/// Checks for errors in the current state of the renderer, throwing an exception if there is one
		/// </summary>
		public static void CheckErrors( )
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
			Gl.glClearDepth( depth );
			Gl.glClear( Gl.GL_DEPTH_BUFFER_BIT );
		}

		/// <summary>
		/// Sets the specified colour as the current colour in the renderer (OpenGL specific)
		/// </summary>
		public void ApplyColour( System.Drawing.Color colour )
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
				case Transform.ViewToScreen :
				{
					Gl.glMatrixMode( Gl.GL_PROJECTION );
					Gl.glTranslatef( x, y, z );
					break;
				}
			}
		}

		/// <summary>
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		public override void	PushTransform( Transform type, Matrix44 matrix )
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

				case Transform.ViewToScreen :
				{
					Gl.glMatrixMode( Gl.GL_PROJECTION );
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

				case Transform.ViewToScreen :
				{
					Gl.glMatrixMode( Gl.GL_PROJECTION );
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

		//	Gl.glMatrixMode( Gl.GL_MODELVIEW );
		//	Gl.glPushMatrix( );
		//	Gl.glLoadIdentity( );
		//	Glu.gluLookAt( camPos.X, camPos.Y, camPos.Z, lookAt.X, lookAt.Y, lookAt.Z, camYAxis.X, camYAxis.Y, camYAxis.Z );
		//	EnsureMvCorrectness( CurrentWorldToView );
		//	Gl.glPopMatrix( );
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

				case Transform.ViewToScreen :
				{
					Gl.glMatrixMode( Gl.GL_PROJECTION );
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

				case Transform.ViewToScreen :
				{
					Gl.glMatrixMode( Gl.GL_PROJECTION );
					Gl.glPopMatrix( );
					break;
				}
			}
		}

		/// <summary>
		/// Sets the viewport (in pixels)
		/// </summary>
		public override void	SetViewport( int x, int y, int width, int height )
		{
			m_Width		= width;
			m_Height	= height;
			Gl.glViewport( x, y, width, height );
		}

		private int	m_Width;
		private int m_Height;

		/// <summary>
		///	The viewport width
		/// </summary>
		public override int				ViewportWidth
		{
			get
			{
				return m_Width;
			}
		}

		/// <summary>
		/// The viewport height
		/// </summary>
		public override int				ViewportHeight
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
		public override Point3	Unproject( int x, int y, float depth )
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
			double inX	= ( double )x;
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
			double inX = ( double )x;
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
		public override unsafe System.Drawing.Image	ColourBufferToImage( )
		{
			int width			= ViewportWidth;
			int height			= ViewportHeight;
			int bytesPerPixel	= 4;	//	TODO: Bodge
			byte[] bufferMem = new byte[ width * height * bytesPerPixel ];
			Gl.glReadPixels( 0, 0, width, height, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, bufferMem );

			System.Drawing.Bitmap bmp;
			fixed ( byte* bufferMemPtr = bufferMem )
			{
				bmp = new System.Drawing.Bitmap( width, height, width * bytesPerPixel, System.Drawing.Imaging.PixelFormat.Format32bppArgb, ( IntPtr )bufferMemPtr );
			}

			return bmp;
		}

		/// <summary>
		/// Creates an Image object from the depth buffer
		/// </summary>
		public override unsafe System.Drawing.Image	DepthBufferToImage( )
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

			System.Drawing.Bitmap bmp;
			fixed ( byte* bufferMemPtr = bufferMem )
			{
				bmp = new System.Drawing.Bitmap( width, height, width * bytesPerPixel, System.Drawing.Imaging.PixelFormat.Format24bppRgb, ( IntPtr )bufferMemPtr );
			}

			return bmp;
		}

		#endregion

		#region	Local to world transforms

		/// <summary>
		/// Gets the current local to world transform
		/// </summary>
		private Matrix44	CurrentLocalToWorld
		{
			get
			{
				return m_LocalToWorldStack[ m_TopOfLocalToWorldStack ];
			}
		}
		

		private Matrix44[]	m_LocalToWorldStack			= new Matrix44[ 8 ];
		private int			m_TopOfLocalToWorldStack	= 0;

		#endregion

		#region	World to view transforms

		/// <summary>
		/// Gets the current world to view transform
		/// </summary>
		private Matrix44	CurrentWorldToView
		{
			get
			{
				return m_WorldToViewStack[ m_TopOfWorldToViewStack ];
			}
		}

		private Matrix44[]	m_WorldToViewStack			= new Matrix44[ 4 ];
		private int			m_TopOfWorldToViewStack		= 0;

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
