using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Rb.Core.Maths;
using Rb.Core.Threading;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Rb.Rendering.Interfaces.Objects.Lights;

namespace Rb.Rendering
{

	/// <summary>
	/// Handy abstract base class for implementations of <see cref="IRenderer"/>
	/// </summary>
	public abstract class RendererBase : IRenderer
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="mainRenderingThread">Main rendering thread</param>
		/// <param name="mainRenderingThreadMarshaller">Marshaller for the main rendering thread</param>
		public RendererBase( Thread mainRenderingThread, DelegateMarshaller mainRenderingThreadMarshaller )
		{
			Arguments.CheckNotNull( mainRenderingThread, "mainRenderingThread" );
			Arguments.CheckNotNull( mainRenderingThreadMarshaller, "mainRenderingThreadMarshaller" );

			m_MainRenderingThread = mainRenderingThread;
			m_MainRenderingThreadMarshaller = mainRenderingThreadMarshaller;
		}


		#region Setup

		/// <summary>
		/// Dumps information about the renderer to the graphics log
		/// </summary>
		public virtual void DumpInfo( )
		{
		}

		/// <summary>
		/// Sets up the renderer
		/// </summary>
		public virtual void Setup( )
		{
			m_Textures = new ITexture[ MaxTextureUnits ];
			m_Lights = new ILight[ MaxActiveLights ];
		}

		/// <summary>
		/// Gets the main rendering thread
		/// </summary>
		public Thread MainRenderingThread
		{
			get { return m_MainRenderingThread; }
		}

		/// <summary>
		/// Gets a delegate marshalling context for the main rendering thread (i.e. main UI thread)
		/// </summary>
		public DelegateMarshaller MainRenderingThreadMarshaller
		{
			get { return m_MainRenderingThreadMarshaller; }
		}

		#endregion

		#region Frames


		/// <summary>
		/// Event, invoked by <see cref="Begin"/>
		/// </summary>
		public event ActionDelegates.Action FrameStart;

		/// <summary>
		/// Event, invoked by <see cref="End"/>
		/// </summary>
		public event ActionDelegates.Action FrameEnd;

		/// <summary>
		/// Sets up to render the next frame
		/// </summary>
		public virtual void Begin( )
		{
			if ( FrameStart != null )
			{
				FrameStart( );
			}
		}

		/// <summary>
		/// Cleans up after rendering the frame
		/// </summary>
		public virtual void End( )
		{
			if ( FrameEnd != null )
			{
				FrameEnd( );
			}
		}

		#endregion

		#region Cameras

		/// <summary>
		/// Pushes the specified camera, calls <see cref="ICamera.Begin"/>
		/// </summary>
		/// <param name="camera">Camera to push</param>
		public void PushCamera( ICamera camera )
		{
			Arguments.CheckNotNull( camera, "camera" );
			m_Cameras.Push( camera );
		}

		/// <summary>
		/// Gets the current camera. Returns null if no camera has been pushed
		/// </summary>
		public ICamera Camera
		{
			get { return m_Cameras.Count == 0 ? null : m_Cameras.Peek( ); }
		}

		/// <summary>
		/// Pops the camera stack, and calls Apply() on the new topmost camera
		/// </summary>
		public void PopCamera( )
		{
			if ( m_Cameras.Count == 0 )
			{
				throw new InvalidOperationException( "Camera stack was empty - cannot pop" );
			}
			m_Cameras.Pop( );
		}

		#endregion

		#region Clears

		/// <summary>
		/// Clears the colour buffer to a vertical gradient
		/// </summary>
		public abstract void ClearColourToVerticalGradient( Color topColour, Color bottomColour );

		/// <summary>
		/// Clears the viewport using a radial gradient fill
		/// </summary>
		public abstract void ClearColourToRadialGradient( Color centreColour, Color outerColour );

		/// <summary>
		/// Clears the colour buffer to a given value
		/// </summary>
		public abstract void ClearColour( Color colour );

		/// <summary>
		/// Clears the depth buffer to a given value
		/// </summary>
		public abstract void ClearDepth( float depth );

		/// <summary>
		/// Clears the stencil buffer to a given value
		/// </summary>
		public abstract void ClearStencil( int value );

		#endregion

		#region	Transform pipeline

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		public abstract Matrix44 GetTransform( TransformType type );

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		public abstract void GetTransform( TransformType type, Matrix44 matrix );

		/// <summary>
		/// Transforms a local point into screen space using the current transform pipeline
		/// </summary>
		/// <param name="pt">Local point</param>
		/// <returns>Screen space point</returns>
		public abstract Point3 Project( Point3 pt );

		/// <summary>
		/// Sets an identity matrix in the projection and model view transforms
		/// </summary>
		public abstract void Set2d( );

		/// <summary>
		/// Pushes an identity matrix in the projection and model view transforms. The top left hand corner is (X,Y), the bottom right is (W,H) (where
		/// (W,H) are the viewport dimensions, and (X,Y) is the viewport minimum corner position)
		/// </summary>
		public abstract void Push2d( );

		/// <summary>
		/// Pops the identity matrices pushed by Push2d()
		/// </summary>
		public abstract void Pop2d( );

		/// <summary>
		/// Translates the current transform in the specified transform stack
		/// </summary>
		public abstract void Translate( TransformType type, float x, float y, float z );

		/// <summary>
		/// Rotates the current transform around a given axis
		/// </summary>
		public abstract void RotateAroundAxis( TransformType type, Vector3 axis, float angleInRadians );

		/// <summary>
		/// Scales the current transform in the specified transform stack
		/// </summary>
		public abstract void Scale( TransformType type, float scaleX, float scaleY, float scaleZ );

		/// <summary>
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		public abstract void PushTransform( TransformType type, InvariantMatrix44 matrix );

		/// <summary>
		/// Pushes a copy of the transform currently at the top of the specified transform stack
		/// </summary>
		public abstract void PushTransform( TransformType type );

		/// <summary>
		/// Sets the current Transform.kLocalToView transform to a look-at matrix
		/// </summary>
		public abstract void SetLookAtTransform( Point3 lookAt, Point3 camPos, Vector3 camYAxis );

		/// <summary>
		/// Sets the current Transform.kViewToScreen matrix to a projection matrix with the specified attributes
		/// </summary>
		public abstract void SetPerspectiveProjectionTransform( float fov, float aspectRatio, float zNear, float zFar );

		/// <summary>
		/// Applies the specified transform, adds it to the specified transform stack
		/// </summary>
		public abstract void SetTransform( TransformType type, InvariantMatrix44 matrix );

		/// <summary>
		/// Applies the specified transform, adds it to the specified transform stack
		/// </summary>
		public abstract void SetTransform( TransformType type, Point3 translation, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis );

		/// <summary>
		/// Pops a matrix from the specified transform stack, applies the new topmost matrix
		/// </summary>
		public abstract void PopTransform( TransformType type );

		/// <summary>
		/// Pushes a modifier that is applied to any matrix pushed onto the stack
		/// </summary>
		/// <remarks>
		/// Modifiers are only applied to the topmost matrix. For example, pushing a modifier matrix M,
		/// then pushing world matrices T0, T1 and T2, will result in T0.T1.T2.M being used for rendering.
		/// </remarks>
		public abstract void PushTransformPostModifier( TransformType type, InvariantMatrix44 matrix );

		/// <summary>
		/// Pops the current transform modifier
		/// </summary>
		public abstract void PopTransformPostModifier( TransformType type );

		#endregion

		#region	Unprojection

		/// <summary>
		/// Unprojects a point from screen space into world space
		/// </summary>
		public abstract Point3 Unproject( int x, int y, float depth );

		/// <summary>
		/// Makes a 3d ray in world space from a screen space position
		/// </summary>
		public abstract Ray3 PickRay( int x, int y );

		#endregion

		#region Viewport

		/// <summary>
		/// Sets the viewport (in pixels)
		/// </summary>
		public abstract void SetViewport( int x, int y, int width, int height );

		/// <summary>
		/// Gets the current viewport
		/// </summary>
		public abstract System.Drawing.Rectangle Viewport
		{
			get;
		}

		/// <summary>
		///	The viewport width
		/// </summary>
		public abstract int ViewportWidth
		{
			get;
		}

		/// <summary>
		/// The viewport height
		/// </summary>
		public abstract int ViewportHeight
		{
			get;
		}

		#endregion

		#region Render states

		/// <summary>
		/// Retrieves the topmost render state on the render state stack
		/// </summary>
		public IRenderState CurrentRenderState
		{
			get
			{
				return m_RenderStates[ m_RenderStates.Count - 1 ];
			}
		}

		/// <summary>
		/// Pushes a new render state, and applies it
		/// </summary>
		public void PushRenderState( IRenderState state )
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
				m_RenderStates.Add( Graphics.Factory.CreateRenderState( ) );
			}
			else
			{
				m_RenderStates.Add( m_RenderStates[ m_RenderStates.Count - 1 ] );
			}
		}

		/// <summary>
		/// Pops the current render state, applies the previous one
		/// </summary>
		public void PopRenderState( )
		{
			int lastIndex = m_RenderStates.Count - 1;
			m_RenderStates[ lastIndex ].End( );
			m_RenderStates.RemoveAt( lastIndex );
			if ( lastIndex > 0 )
			{
				m_RenderStates[ lastIndex - 1 ].Begin( );
			}
		}

		#endregion

		#region Texture units

		/// <summary>
		/// Gets the maximum number of texture units supported by this renderer
		/// </summary>
		public virtual int MaxTextureUnits
		{
			get { return 8; }
		}
		
		/// <summary>
		/// Pushes the current set of textures onto the texture stack and unbinds them all
		/// </summary>
		public void PushTextures( )
		{
			m_TextureStack.Add( ( ITexture[] )m_Textures.Clone( ) );
			UnbindAllTextures( );
		}

		/// <summary>
		/// Pops the set of textures from the texture stack and rebinds them
		/// </summary>
		public void PopTextures( )
		{
			UnbindAllTextures( );
			ITexture[] oldTextures = m_TextureStack[ m_TextureStack.Count - 1 ];
			for ( int textureIndex = 0; textureIndex < oldTextures.Length; ++textureIndex )
			{
				if ( oldTextures[ textureIndex ] != null )
				{
					BindTexture( oldTextures[ textureIndex ] );
				}
			}
			m_TextureStack.RemoveAt( m_TextureStack.Count - 1 );
		}

		/// <summary>
		/// Binds a texture. Returns the unit that the texture occupies
		/// </summary>
		public virtual int BindTexture( ITexture texture )
		{
			if ( texture == null )
			{
				throw new ArgumentNullException( "texture" );
			}
			for ( int unit = 0; unit < m_Textures.Length; ++unit )
			{
				if ( m_Textures[ unit ] == null )
				{
					m_Textures[ unit ] = texture;
					texture.Bind( unit );
					return unit;
				}
			}
			throw new ApplicationException( "No free texture units" );
		}

		/// <summary>
		/// Unbinds a texture from the indexed texture stage. Returns the unit that the texture was bound to
		/// </summary>
		public virtual int UnbindTexture( ITexture texture )
		{
			for ( int unit = 0; unit < m_Textures.Length; ++unit )
			{
				if ( m_Textures[ unit ] == texture )
				{
					m_Textures[ unit ].Unbind( unit );
					m_Textures[ unit ] = null;
					return unit;
				}
			}
			throw new ApplicationException( "Could not find unit that texture was bound to" );
		}

		/// <summary>
		/// Unbinds all textures
		/// </summary>
		public virtual void UnbindAllTextures( )
		{
			for ( int unit = 0; unit < m_Textures.Length; ++unit )
			{
				if ( m_Textures[ unit ] != null )
				{
					m_Textures[ unit ].Unbind( unit );
					m_Textures[ unit ] = null;
				}
			}
		}

		/// <summary>
		/// Gets a 2D texture, indexed by stage
		/// </summary>
		public ITexture GetTexture( int index )
		{
			return m_Textures[ index ];
		}

		/// <summary>
		/// Gets the texture array
		/// </summary>
		public ITexture[] Textures
		{
			get { return m_Textures; }
		}

		#endregion

		#region	Lighting

		/// <summary>
		/// Clears the light array. This is done every frame
		/// </summary>
		public virtual void ClearLights( )
		{
			m_NumLights = 0;
		}

		/// <summary>
		/// Adds the specified light
		/// </summary>
		public virtual void AddLight( ILight light )
		{
			m_Lights[ m_NumLights++ ] = light;
		}

		/// <summary>
		/// Gets an indexed light
		/// </summary>
		public ILight GetLight( int index )
		{
			return m_Lights[ index ];
		}

		/// <summary>
		/// Returns the number of active lights that have been added since the last 
		/// </summary>
		public int NumActiveLights
		{
			get { return m_NumLights; }
		}

		/// <summary>
		/// Gets the maximum number of active lights
		/// </summary>
		public virtual int MaxActiveLights
		{
			get { return 4; }
		}

		#endregion

		#region	Frame dumps

		/// <summary>
		/// Creates an Image object from the colour buffer
		/// </summary>
		public abstract Image ColourBufferToImage( );

		/// <summary>
		/// Creates an Image object from the depth buffer
		/// </summary>
		public abstract Image DepthBufferToImage( );

		/// <summary>
		/// Saves the colour buffer to a file
		/// </summary>
		public void SaveColourBuffer( string path, System.Drawing.Imaging.ImageFormat format )
		{
			ColourBufferToImage( ).Save( path, format );
		}

		/// <summary>
		/// Saves the depth buffer to a file
		/// </summary>
		public void SaveDepthBuffer( string path, System.Drawing.Imaging.ImageFormat format )
		{
			DepthBufferToImage( ).Save( path, format );
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Clears up this renderer
		/// </summary>
		public virtual void Dispose( )
		{
		}

		#endregion

		#region	Private stuff

		private readonly Stack<ICamera>		m_Cameras = new Stack<ICamera>( );
		private readonly List<IRenderState>	m_RenderStates = new List<IRenderState>( );
		private ILight[]					m_Lights;
		private int							m_NumLights;
		private ITexture[]					m_Textures;
		private readonly List<ITexture[]>	m_TextureStack = new List<ITexture[]>( );
		private readonly Thread				m_MainRenderingThread;
		private readonly DelegateMarshaller	m_MainRenderingThreadMarshaller;

		#endregion

	}
}
