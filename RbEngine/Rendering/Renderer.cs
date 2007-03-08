using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using RbEngine.Maths;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Transform types
	/// </summary>
	public enum Transform
	{
		/// <summary>
		/// Transforms vertices from local space to world space
		/// </summary>
		LocalToWorld,

		/// <summary>
		/// Transforms vertices from world space to view space
		/// </summary>
		WorldToView,

		//	TODO: REMOVEME (want separation between local to world and world to view transforms - it's easier to do lighting in world space :)
	//	LocalToView,

		/// <summary>
		/// Transforms vertices from view space to screen space (projective transform)
		/// </summary>
		ViewToScreen,

		/// <summary>
		/// Total number of transforms
		/// </summary>
		Count
	}

	/// <summary>
	/// Summary description for Renderer.
	/// </summary>
	public abstract class Renderer
	{

		#region	Construction and singleton access

		/// <summary>
		/// Protected constructor - use Get() accessor
		/// </summary>
		protected Renderer( )
		{
			System.Diagnostics.Trace.Assert( ms_Singleton == null, "Only one Renderer object is allowed" );
			ms_Singleton = this;

			//	Add a default renderstate
			PushRenderState( RenderFactory.Inst.NewRenderState( ) );
		}

		/// <summary>
		/// Gets the renderer
		/// </summary>
		public static Renderer Inst
		{
			get
			{
			//	System.Diagnostics.Trace.Assert( ms_Singleton != null, "Renderer singleton was not yet initialised" );
				return ms_Singleton;
			}
		}

		/// <summary>
		/// true if the renderer singleton, accessible via Inst, exists
		/// </summary>
		public static bool	Exists
		{
			get
			{
				return ms_Singleton != null;
			}
		}

		#endregion

		#region	Forms

		/// <summary>
		/// Returns a rendering context for a control
		/// </summary>
		/// <param name="control"> Control to set up </param>
		public abstract ControlRenderContext	CreateControlContext( System.Windows.Forms.Control control );

		#endregion

		#region	Lighting

		/// <summary>
		/// Clears the light array. This is done every frame
		/// </summary>
		public void			ClearLights( )
		{
			m_NumLights = 0;
		}

		/// <summary>
		/// Adds the specified light
		/// </summary>
		public void			AddLight( Light light )
		{
			m_Lights[ m_NumLights++ ] = light;
		}

		/// <summary>
		/// Gets an indexed light
		/// </summary>
		public Light		GetLight( int index )
		{
			return m_Lights[ index ];
		}

		/// <summary>
		/// Returns the number of active lights that have been added since the last 
		/// </summary>
		public int			NumActiveLights
		{
			get
			{
				return m_NumLights;
			}
		}

		/// <summary>
		/// The maximum number of lights supported
		/// </summary>
		public const int	MaxActiveLights = 8;


		#endregion

		#region	Standard operations

		/// <summary>
		/// The control currently being rendered to
		/// </summary>
		/// <seealso cref="ControlRenderContext.BeginPaint()"/>
		public virtual Control	CurrentControl
		{
			set
			{
				m_Control = value;
			}
			get
			{
				return m_Control;
			}
		}

		/// <summary>
		/// Should be called at the beginning of each frame
		/// </summary>
		public virtual void		Begin( )
		{
			ClearLights( );
		}

		/// <summary>
		/// Should be called at the end of each frame
		/// </summary>
		public virtual void		End( )
		{
		}

		/// <summary>
		/// Clears the viewport
		/// </summary>
		public abstract void	ClearColour( System.Drawing.Color colour );

		/// <summary>
		/// Clears the viewport using a vertical gradient fill
		/// </summary>
		public abstract void	ClearVerticalGradient( System.Drawing.Color topColour, System.Drawing.Color bottomColour );

		/// <summary>
		/// Clears the viewport using a radial gradient fill (shit)
		/// </summary>
		public abstract void	ClearRadialGradient( System.Drawing.Color centreColour, System.Drawing.Color outerColour );

		/// <summary>
		/// Clears the depth buffer
		/// </summary>
		public abstract void	ClearDepth( float depth );

		#endregion

		#region	Transform pipeline

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		public abstract Matrix44		GetTransform( Transform type );


		/// <summary>
		/// The current camera
		/// </summary>
		/// <remarks>
		/// This sets up data that is only required for following reasons:
		///		- PickRay() is being called
		///		- A ShaderParameter is bound to ShaderParameterBinding.EyePosition, ShaderParameterBinding.EyeXAxis, ShaderParameterBinding.EyeYAxis or ShaderParameterBinding.EyeZAxis
		/// </remarks>
		public Cameras.CameraBase		Camera
		{
			set
			{
				m_Camera = value;
			}
			get
			{
				return m_Camera;
			}
		}

		/// <summary>
		/// Sets an identity matrix in the projection and model view transforms
		/// </summary>
		public abstract void			Set2d( );

		/// <summary>
		/// Pushes an identity matrix in the projection and model view transforms. The top left hand corner is (X,Y), the bottom right is (W,H) (where
		/// (W,H) are the viewport dimensions, and (X,Y) is the viewport minimum corner position)
		/// </summary>
		public abstract void			Push2d( );

		/// <summary>
		/// Pops the identity matrices pushed by Push2d( )
		/// </summary>
		public abstract void			Pop2d( );

		/// <summary>
		/// Translates the current transform in the specified transform stack
		/// </summary>
		public abstract void			Translate( Transform type, float x, float y, float z );

		/// <summary>
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		public abstract void			PushTransform( Transform type, Matrix44 matrix );

		/// <summary>
		/// Pushes a copy of the transform currently at the top of the specified transform stack
		/// </summary>
		public abstract void			PushTransform( Transform type );

		/// <summary>
		/// Sets the current Transform.kLocalToView transform to a look-at matrix
		/// </summary>
		public abstract void			SetLookAtTransform( Point3 lookAt, Point3 camPos, Vector3 camYAxis );

		/// <summary>
		/// Sets the current Transform.kViewToScreen matrix to a projection matrix with the specified attributes
		/// </summary>
		public abstract void			SetPerspectiveProjectionTransform( float fov, float aspectRatio, float zNear, float zFar );

		/// <summary>
		/// Applies the specified transform, adds it to the specified transform stack
		/// </summary>
		public abstract void			SetTransform( Transform type, Matrix44 matrix );

		/// <summary>
		/// Pops a matrix from the specified transform stack, applies the new topmost matrix
		/// </summary>
		public abstract void			PopTransform( Transform type );

		/// <summary>
		/// Sets the viewport (in pixels)
		/// </summary>
		public abstract void			SetViewport( int x, int y, int width, int height );

		/// <summary>
		///	The viewport width
		/// </summary>
		public abstract int				ViewportWidth
		{
			get;
		}

		/// <summary>
		/// The viewport height
		/// </summary>
		public abstract int				ViewportHeight
		{
			get;
		}

		#endregion

		#region	Unprojection

		/// <summary>
		/// Unprojects a point from screen space into world space
		/// </summary>
		public abstract Maths.Point3	Unproject( int x, int y, float depth );

		/// <summary>
		/// Makes a 3d ray in world space from a screen space position
		/// </summary>
		public abstract Maths.Ray3		PickRay( int x, int y );

		#endregion

		#region	Render states

		/// <summary>
		/// Pushes a new render state, and applies it
		/// </summary>
		public void PushRenderState( RenderState state )
		{
			m_RenderStates.Add( state );
			state.Begin( );
		}

		/// <summary>
		/// Pushes the current state, or pushes a new render state, if the render state stack is empty
		/// </summary>
		public void PushRenderState( )
		{
			if ( m_RenderStates.Count == 0 )
			{
				m_RenderStates.Add( RenderFactory.Inst.NewRenderState( ) );
			}
			else
			{
				m_RenderStates.Add( ( RenderState )m_RenderStates[ m_RenderStates.Count - 1 ] );
			}
		}

		/// <summary>
		/// Pops the current render state, applies the previous one
		/// </summary>
		public void PopRenderState( )
		{
			int lastIndex = m_RenderStates.Count - 1;
			( ( RenderState )m_RenderStates[ lastIndex ] ).End( );
			m_RenderStates.RemoveAt( lastIndex );
		}

		#endregion

		#region	Frame dumps

		/// <summary>
		/// Creates an Image object from the colour buffer
		/// </summary>
		public abstract Image	ColourBufferToImage( );

		/// <summary>
		/// Creates an Image object from the depth buffer
		/// </summary>
		public abstract Image	DepthBufferToImage( );

		/// <summary>
		/// Saves the colour buffer to a file
		/// </summary>
		public void				SaveColourBuffer( string path, System.Drawing.Imaging.ImageFormat format )
		{
			ColourBufferToImage( ).Save( path, format );
		}
		
		/// <summary>
		/// Saves the depth buffer to a file
		/// </summary>
		public void				SaveDepthBuffer( string path, System.Drawing.Imaging.ImageFormat format )
		{
			DepthBufferToImage( ).Save( path, format );
		}

		#endregion

		#region	Private stuff

		private static Renderer		ms_Singleton;
		private ArrayList			m_RenderStates	= new ArrayList( );
		private Control				m_Control;
		private Cameras.CameraBase	m_Camera;
		private Light[]				m_Lights = new Light[ MaxActiveLights ];
		private int					m_NumLights;

		#endregion

	}
}