using System;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Rb.Rendering.Interfaces.Objects.Lights;

namespace Rb.Rendering.Interfaces
{
	/// <summary>
	/// Manages overall state of the rendering process
	/// </summary>
	public interface IRenderer : IDisposable
	{
		#region Setup

		/// <summary>
		/// Dumps information about the renderer to the graphics log
		/// </summary>
		void DumpInfo( );

		#endregion

		#region Frames

		/// <summary>
		/// Event, invoked by <see cref="Begin"/>
		/// </summary>
		event ActionDelegates.Action FrameStart;

		/// <summary>
		/// Event, invoked by <see cref="End"/>
		/// </summary>
		event ActionDelegates.Action FrameEnd;

		/// <summary>
		/// Sets up to render the next frame
		/// </summary>
		void Begin( );

		/// <summary>
		/// Cleans up after rendering the frame
		/// </summary>
		void End( );

		#endregion

		#region Clears

		/// <summary>
		/// Clears the colour buffer to a vertical gradient
		/// </summary>
		void ClearColourToVerticalGradient( Color topColour, Color bottomColour );

		/// <summary>
		/// Clears the viewport using a radial gradient fill
		/// </summary>
		void ClearColourToRadialGradient( Color centreColour, Color outerColour );

		/// <summary>
		/// Clears the colour buffer to a given value
		/// </summary>
		void ClearColour( Color colour );

		/// <summary>
		/// Clears the depth buffer to a given value
		/// </summary>
		void ClearDepth( float depth );

		/// <summary>
		/// Clears the stencil buffer to a given value
		/// </summary>
		void ClearStencil( int value );

		#endregion

		#region Cameras

		/// <summary>
		/// Pushes the specified camera
		/// </summary>
		/// <param name="camera">Camera to push</param>
		void PushCamera( ICamera camera );

		/// <summary>
		/// Gets the current camera. Returns null if no camera has been pushed
		/// </summary>
		ICamera Camera
		{
			get;
		}

		/// <summary>
		/// Pops the camera stack
		/// </summary>
		void PopCamera( );

		#endregion

		#region	Transform pipeline

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		Matrix44 GetTransform( TransformType type );

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		void GetTransform( TransformType type, Matrix44 matrix );

		/// <summary>
		/// Transforms a local point into screen space using the current transform pipeline
		/// </summary>
		/// <param name="pt">Local point</param>
		/// <returns>Screen space point</returns>
		Point3 Project( Point3 pt );

		/// <summary>
		/// Sets an identity matrix in the projection and model view transforms
		/// </summary>
		void Set2d( );

		/// <summary>
		/// Pushes an identity matrix in the projection and model view transforms. The top left hand corner is (X,Y), the bottom right is (W,H) (where
		/// (W,H) are the viewport dimensions, and (X,Y) is the viewport minimum corner position)
		/// </summary>
		void Push2d( );

		/// <summary>
		/// Pops the identity matrices pushed by Push2d()
		/// </summary>
		void Pop2d( );

		/// <summary>
		/// Rotates the current transform around a given axis
		/// </summary>
		void RotateAroundAxis( TransformType type, Vector3 axis, float angleInRadians );

		/// <summary>
		/// Translates the current transform in the specified transform stack
		/// </summary>
		void Translate( TransformType type, float x, float y, float z );

		/// <summary>
		/// Scales the current transform in the specified transform stack
		/// </summary>
		void Scale( TransformType type, float scaleX, float scaleY, float scaleZ );

		/// <summary>
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		void PushTransform( TransformType type, InvariantMatrix44 matrix );

		/// <summary>
		/// Pushes a copy of the transform currently at the top of the specified transform stack
		/// </summary>
		void PushTransform( TransformType type );

		/// <summary>
		/// Sets the current Transform.kLocalToView transform to a look-at matrix
		/// </summary>
		void SetLookAtTransform( Point3 lookAt, Point3 camPos, Vector3 camYAxis );

		/// <summary>
		/// Sets the current Transform.kViewToScreen matrix to a projection matrix with the specified attributes
		/// </summary>
		void SetPerspectiveProjectionTransform( float fov, float aspectRatio, float zNear, float zFar );

		/// <summary>
		/// Applies the specified transform, adds it to the specified transform stack
		/// </summary>
		void SetTransform( TransformType type, InvariantMatrix44 matrix );
		
		/// <summary>
		/// Applies the specified transform, adds it to the specified transform stack
		/// </summary>
		void SetTransform( TransformType type, Point3 translation, Vector3 xAxis, Vector3 yAxis, Vector3 zAxis );

		/// <summary>
		/// Pops a matrix from the specified transform stack, applies the new topmost matrix
		/// </summary>
		void PopTransform( TransformType type );

		/// <summary>
		/// Pushes a modifier that is applied to any matrix pushed onto the stack
		/// </summary>
		/// <remarks>
		/// Modifiers are only applied to the topmost matrix. For example, pushing a modifier matrix M,
		/// then pushing world matrices T0, T1 and T2, will result in T0.T1.T2.M being used for rendering.
		/// </remarks>
		void PushTransformPostModifier( TransformType type, InvariantMatrix44 matrix );

		/// <summary>
		/// Pops the current transform modifier
		/// </summary>
		void PopTransformPostModifier( TransformType type );

		#endregion

		#region	Unprojection

		/// <summary>
		/// Unprojects a point from screen space into world space
		/// </summary>
		Point3 Unproject( int x, int y, float depth );

		/// <summary>
		/// Makes a 3d ray in world space from a screen space position
		/// </summary>
		Ray3 PickRay( int x, int y );

		#endregion

		#region Viewport

		/// <summary>
		/// Sets the viewport (in pixels)
		/// </summary>
		void SetViewport( int x, int y, int width, int height );

		/// <summary>
		/// Gets the current viewport
		/// </summary>
		System.Drawing.Rectangle Viewport
		{
			get;
		}

		/// <summary>
		///	The viewport width
		/// </summary>
		int ViewportWidth
		{
			get;
		}

		/// <summary>
		/// The viewport height
		/// </summary>
		int ViewportHeight
		{
			get;
		}

		#endregion

		#region Render state

		/// <summary>
		/// Retrieves the topmost render state on the render state stack
		/// </summary>
		IRenderState CurrentRenderState
		{
			get;
		}

		/// <summary>
		/// Pushes a render state onto the render state stack, and applies it
		/// </summary>
		void PushRenderState( IRenderState state );

		/// <summary>
		/// Pushes the current render state onto the render state stack again
		/// </summary>
		void PushRenderState( );

		/// <summary>
		/// Pops a render state from the render state stack, and applies the new topmost render state
		/// </summary>
		void PopRenderState( );

		#endregion

		#region Texture units

		/// <summary>
		/// Gets the maximum number of texture units supported by this renderer
		/// </summary>
		int MaxTextureUnits
		{
			get;
		}
		
        /// <summary>
        /// Pushes the current set of textures onto the texture stack and unbinds them all
        /// </summary>
        void PushTextures( );

        /// <summary>
        /// Pops the set of textures from the texture stack and rebinds them
        /// </summary>
        void PopTextures( );

		/// <summary>
		/// Binds a texture to the first available texture unit
		/// </summary>
		/// <param name="texture">Texture to bind</param>
		/// <returns>Returns the index of the texture unit that the texture was bound to</returns>
		int BindTexture( ITexture texture );

		/// <summary>
		/// Unbinds a texture from its texture unit
		/// </summary>
		/// <param name="texture">Texture to unbind</param>
		/// <returns>Returns the index of the texture unit that the texture was unbound from</returns>
		int UnbindTexture( ITexture texture );

		/// <summary>
		/// Unbinds all textures from all texture units
		/// </summary>
		void UnbindAllTextures( );

		/// <summary>
		/// Gets a texture from a given texture unit
		/// </summary>
		/// <param name="unit">Unit index</param>
		/// <returns>Returns the texture current bound to the specified unit</returns>
		ITexture GetTexture( int unit );

		#endregion

		#region Lights
		
		/// <summary>
		/// Clears the light array. This is done every frame
		/// </summary>
		void ClearLights( );

		/// <summary>
		/// Adds the specified light
		/// </summary>
		void AddLight( ILight light );

		/// <summary>
		/// Gets an indexed light
		/// </summary>
		ILight GetLight( int index );

		/// <summary>
		/// Returns the number of active lights that have been added since the last 
		/// </summary>
		int NumActiveLights
		{
			get;
		}

		/// <summary>
		/// Returns the maximum number of lights supported by this renderer
		/// </summary>
		int MaxActiveLights
		{
			get;
		}

		#endregion
		
		#region	Frame dumps

		/// <summary>
		/// Creates an Image object from the colour buffer
		/// </summary>
		Image ColourBufferToImage( );

		/// <summary>
		/// Creates an Image object from the depth buffer
		/// </summary>
		Image DepthBufferToImage( );

		#endregion
	}
}
