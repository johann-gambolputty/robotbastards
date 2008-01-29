using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering.Contracts.Objects;

namespace Rb.Rendering.Contracts
{
	/// <summary>
	/// Manages overall state of the rendering process
	/// </summary>
	public interface IRenderer
	{
		#region	Transform pipeline

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		Matrix44 GetTransform( Transform type );

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
		/// Translates the current transform in the specified transform stack
		/// </summary>
		void Translate( Transform type, float x, float y, float z );

		/// <summary>
		/// Scales the current transform in the specified transform stack
		/// </summary>
		void Scale( Transform type, float scaleX, float scaleY, float scaleZ );

		/// <summary>
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		void PushTransform( Transform type, Matrix44 matrix );

		/// <summary>
		/// Pushes a copy of the transform currently at the top of the specified transform stack
		/// </summary>
		void PushTransform( Transform type );

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
		void SetTransform( Transform type, Matrix44 matrix );

		/// <summary>
		/// Pops a matrix from the specified transform stack, applies the new topmost matrix
		/// </summary>
		void PopTransform( Transform type );

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
		/// Pops a render state from the render state stack, and applies the new topmost render state
		/// </summary>
		void PopRenderState( );

		#endregion

		#region Texture units
		
        /// <summary>
        /// Pushes the current set of textures onto the texture stack and unbinds them all
        /// </summary>
        void PushTextures( );

        /// <summary>
        /// Pops the set of textures from the texture stack and rebinds them
        /// </summary>
        void PopTextures( );

		/// <summary>
		/// Binds a texture to a given texture unit
		/// </summary>
		/// <param name="unit">Unit to bind to</param>
		/// <param name="texture">Texture to bind</param>
		void BindTexture( int unit, ITexture2d texture );
		
		/// <summary>
		/// Unbinds a texture from its texture unit
		/// </summary>
		/// <param name="texture">Texture to unbind</param>
		void UnbindTexture( ITexture2d texture );

		/// <summary>
		/// Unbinds all textures from all texture units
		/// </summary>
		void UnbindAllTextures( );

		/// <summary>
		/// Gets a texture from a given texture unit
		/// </summary>
		/// <param name="unit">Unit index</param>
		/// <returns>Returns the texture current bound to the specified unit</returns>
		ITexture2d GetTexture( int unit );

		#endregion

		#region Lights

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
