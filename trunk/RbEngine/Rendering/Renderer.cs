using System;
using System.Collections;
using RbEngine.Maths;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Transform types
	/// </summary>
	public enum Transform
	{
		/// <summary>
		/// Transforms vertices from local space to view space
		/// </summary>
		kLocalToView,

		/// <summary>
		/// Transforms vertices from view space to screen space (projective transform)
		/// </summary>
		kViewToScreen,

		/// <summary>
		/// Total number of transforms
		/// </summary>
		kCount
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
				System.Diagnostics.Trace.Assert( ms_Singleton != null, "Renderer singleton was not yet initialised" );
				return ms_Singleton;
			}
		}

		#endregion

		#region	Standard operations

		/// <summary>
		/// Clears the viewport
		/// </summary>
		public abstract void	ClearColour( System.Drawing.Color colour );

		/// <summary>
		/// Clears the viewport using a vertical gradient fill
		/// </summary>
		public abstract void ClearVerticalGradient( System.Drawing.Color topColour, System.Drawing.Color bottomColour );

		/// <summary>
		/// Clears the viewport using a radial gradient fill (shit)
		/// </summary>
		public abstract void ClearRadialGradient( System.Drawing.Color centreColour, System.Drawing.Color outerColour );

		/// <summary>
		/// Clears the depth buffer
		/// </summary>
		public abstract void ClearDepth( float depth );

		/// <summary>
		/// Sets the specified colour as the current colour in the renderer (OpenGL specific)
		/// </summary>
		public abstract void ApplyColour( System.Drawing.Color colour );

		#endregion

		#region	Transform pipeline

		/// <summary>
		/// Gets the current matrix from the specified transform stack
		/// </summary>
		public abstract Maths.Matrix44 GetMatrix( Transform type );

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
		/// Applies the specified transform, multiplied by the current topmost transform, and adds it to the specified transform stack
		/// </summary>
		public abstract void	PushTransform( Transform type, Maths.Matrix44 matrix );

		/// <summary>
		/// Sets the current Transform.kLocalToView transform to a look-at matrix
		/// </summary>
		public abstract void SetLookAtTransform( Vector3 lookAt, Vector3 camPos, Vector3 camYAxis );

		/// <summary>
		/// Sets the current Transform.kViewToScreen matrix to a projection matrix with the specified attributes
		/// </summary>
		public abstract void SetPerspectiveProjectionTransform( float fov, float aspectRatio, float zNear, float zFar );

		/// <summary>
		/// Applies the specified transform, adds it to the specified transform stack
		/// </summary>
		public abstract void SetTransform( Transform type, Maths.Matrix44 matrix );

		/// <summary>
		/// Pops a matrix from the specified transform stack, applies the new topmost matrix
		/// </summary>
		public abstract void PopTransform( Transform type );

		#endregion

		#region	Picking

		/// <summary>
		/// Makes a 3d ray in world space from a screen space position
		/// </summary>
		public abstract Maths.Ray3 PickRay( int X, int Y );

		#endregion

		#region	Render states

		/// <summary>
		/// Pushes a new render state, and applies it
		/// </summary>
		public abstract void PushRenderState( RenderState state );

		/// <summary>
		/// Pushes the current state, or pushes a new render state, if the render state stack is empty
		/// </summary>
		public abstract void PushRenderState( );

		/// <summary>
		/// Pops the current render state, applies the previous one
		/// </summary>
		public abstract void PopRenderState( );

		#endregion

		#region	Private stuff

		private static Renderer	ms_Singleton;

		#endregion

	}
}
