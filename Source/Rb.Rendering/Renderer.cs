using System;
using System.Collections.Generic;
using System.Drawing;
using Rb.Core.Maths;

namespace Rb.Rendering
{

	/// <summary>
	/// Abstract base class for rendering
	/// </summary>
	public abstract class Renderer
	{
        #region Setup

        //  TODO: Add first setup all to push first render state

        #endregion

        #region	Rendering context

        //	TODO: I really don't like this stuff being here - it would be far better to have a RenderContext object that is passed around in ISceneRenderable.Render(),
		//	but that breaks with the design of the rest of the renderer (an opengl-style state machine with all state encapsulated by the Renderer)

		/// <summary>
		/// The current camera
		/// </summary>
		/// <remarks>
		/// This sets up data that is only required for following reasons:
		///		- PickRay() is being called
		///		- A ShaderParameter is bound to ShaderParameterBinding.EyePosition, ShaderParameterBinding.EyeXAxis, ShaderParameterBinding.EyeYAxis or ShaderParameterBinding.EyeZAxis
		/// </remarks>
		public Cameras.CameraBase Camera
		{
			set { m_Camera = value; }
			get { return m_Camera; }
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
		public virtual void AddLight( Light light )
		{
			m_Lights[ m_NumLights++ ] = light;
		}

		/// <summary>
		/// Gets an indexed light
		/// </summary>
		public Light GetLight( int index )
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
		/// The maximum number of lights supported
		/// </summary>
		public const int MaxActiveLights = 8;


		#endregion

		#region	Standard operations

		/// <summary>
		/// Should be called at the beginning of each frame
		/// </summary>
		public virtual void Begin( )
		{
			ClearLights( );
		}

		/// <summary>
		/// Should be called at the end of each frame
		/// </summary>
		public virtual void End( )
		{
		}

		/// <summary>
		/// Clears the viewport
		/// </summary>
		public abstract void ClearColour( Color colour );

		/// <summary>
		/// Clears the viewport using a vertical gradient fill
		/// </summary>
		public abstract void ClearVerticalGradient( Color topColour, Color bottomColour );

		/// <summary>
		/// Clears the viewport using a radial gradient fill (shit)
		/// </summary>
		public abstract void ClearRadialGradient( Color centreColour, Color outerColour );

		/// <summary>
		/// Clears the depth buffer
		/// </summary>
		public abstract void ClearDepth( float depth );

		#endregion

		#region	Transform pipeline

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		public abstract Matrix44 GetTransform( Transform type );

		/// <summary>
		/// Gets an indexed texture transform
		/// </summary>
		public static Transform TextureTransform( int textureUnit )
		{
			Transform result = ( Transform )( ( int )Transform.Texture0 + textureUnit );
			return result;
		}


		/// <summary>
		/// Transforms a local point into screen space
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
		/// Pops the identity matrices pushed by Push2d( )
		/// </summary>
		public abstract void Pop2d( );

		/// <summary>
		/// Translates the current transform in the specified transform stack
		/// </summary>
		public abstract void Translate( Transform type, float x, float y, float z );
		
		/// <summary>
		/// Scales the current transform in the specified transform stack
		/// </summary>
		public abstract void Scale( Transform type, float scaleX, float scaleY, float scaleZ );

		/// <summary>
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		public abstract void PushTransform( Transform type, Matrix44 matrix );

		/// <summary>
		/// Pushes a copy of the transform currently at the top of the specified transform stack
		/// </summary>
		public abstract void PushTransform( Transform type );

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
		public abstract void SetTransform( Transform type, Matrix44 matrix );

		/// <summary>
		/// Pops a matrix from the specified transform stack, applies the new topmost matrix
		/// </summary>
		public abstract void PopTransform( Transform type );

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

		#region	Textures

		/// <summary>
		/// The maximum number of texture stages
		/// </summary>
		public const int MaxTextures = 8;

        /// <summary>
        /// Pushes the current set of textures onto the texture stack and unbinds them all
        /// </summary>
        public void PushTextures( )
        {
            m_TextureStack.Add( ( Texture2d[] )m_Textures.Clone( ) );
            UnbindAllTextures( );
        }

        /// <summary>
        /// Pops the set of textures from the texture stack and rebinds them
        /// </summary>
        public void PopTextures( )
        {
            UnbindAllTextures( );
            Texture2d[] oldTextures = m_TextureStack[ m_TextureStack.Count - 1 ];
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
		public virtual int BindTexture( Texture2d texture )
		{
            for ( int unit = 0; unit < m_Textures.Length; ++unit )
            {
                if ( m_Textures[ unit ] == null )
                {
                    m_Textures[ unit ] = texture;
                    return unit;
                }
            }
            throw new ApplicationException( "No free texture units" );
		}

		/// <summary>
		/// Unbinds a texture from the indexed texture stage. Returns the unit that the texture was bound to
		/// </summary>
		public virtual int UnbindTexture( Texture2d texture )
		{
            for ( int unit = 0; unit < m_Textures.Length; ++unit )
            {
                if ( m_Textures[ unit ] == texture )
                {
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
                m_Textures[ unit ] = null;
            }
        }

		/// <summary>
		/// Gets a 2D texture, indexed by stage
		/// </summary>
		public Texture2d GetTexture( int index )
		{
			return m_Textures[ index ];
		}

        /// <summary>
        /// Gets the texture array
        /// </summary>
	    public Texture2d[] Textures
	    {
	        get { return m_Textures; }
	    }

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
				m_RenderStates.Add( Graphics.Factory.NewRenderState( ) );
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

		#region Protected stuff

		#endregion

		#region	Private stuff

		private readonly List< RenderState >	m_RenderStates	= new List< RenderState >( );
		private Cameras.CameraBase				m_Camera;
		private readonly Light[]				m_Lights = new Light[ MaxActiveLights ];
		private int								m_NumLights;
		private readonly Texture2d[]			m_Textures = new Texture2d[ MaxTextures ];
		private readonly List<Texture2d[]>		m_TextureStack = new List<Texture2d[]>( );

		#endregion

	}
}
