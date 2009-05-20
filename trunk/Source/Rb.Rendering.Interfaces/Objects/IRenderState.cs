using System.Drawing;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// A render state modifies the state of the renderer when applied
	/// </summary>
	/// <remarks>
	/// Rendering states are managed by pushing or popping them from the renderer
	/// (<see cref="IRenderer.PushRenderState(IRenderState)"/>, <see cref="IRenderer.PopRenderState"/>)
	/// </remarks>
	public interface IRenderState : IPass
	{
		/// <summary>
		/// Creates a clone of this render state
		/// </summary>
		IRenderState Clone( );

		#region Lighting

		/// <summary>
		/// Enables/disables lighting
		/// </summary>
		bool Lighting
		{
			get; set;
		}

		#endregion

		#region Depth

		/// <summary>
		/// Gets/sets the depth bias
		/// </summary>
		float DepthBias
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the depth offset
		/// </summary>
		float DepthOffset
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the depth test pass function
		/// </summary>
		PassDepthTest PassDepthTest
		{
			get; set;
		}

		/// <summary>
		/// Enables/disables depth-testing
		/// </summary>
		bool DepthTest
		{
			get; set;
		}

		/// <summary>
		/// Enables/disables depth writes
		/// </summary>
		bool DepthWrite
		{
			get; set;
		}

		#endregion

		#region Faces

		/// <summary>
		/// Enables/disables backface culling
		/// </summary>
		bool CullBackFaces
		{
			get; set;
		}

		/// <summary>
		/// Enables/disables front face culling
		/// </summary>
		bool CullFrontFaces
		{
			get; set;
		}

		/// <summary>
		/// Enables/disables culling both front and back faces
		/// </summary>
		bool CullFaces
		{
			get; set;
		}

		/// <summary>
		/// Sets/gets the polygon winding
		/// </summary>
		Winding FaceWinding
		{
			get; set;
		}

		/// <summary>
		/// Sets the rendering mode for front and back facing polygons
		/// </summary>
		PolygonRenderMode FaceRenderMode
		{
			set;
		}

		/// <summary>
		/// Gets/sets the rendering mode for front facing polygons
		/// </summary>
		PolygonRenderMode FrontFaceRenderMode
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the rendering mode for back facing polygons
		/// </summary>
		PolygonRenderMode BackFaceRenderMode
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the rendering colour
		/// </summary>
		Color Colour
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the shade mode
		/// </summary>
		PolygonShadeMode ShadeMode
		{
			get; set;
		}

		#endregion

		#region Texturing

		/// <summary>
		/// Enables/disables 2d texturing
		/// </summary>
		bool Enable2dTextures
		{
			get; set;
		}

		/// <summary>
		/// Enables or disables a given 2d texture unit
		/// </summary>
		void Enable2dTextureUnit( int unit, bool enabled );

		/// <summary>
		/// Returns true if a given texture unit is enabled
		/// </summary>
		bool Is2dTextureUnitEnabled( int unit );

		#endregion

		#region Blending

		/// <summary>
		/// Enables/disables blending
		/// </summary>
		bool Blend
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the source blending factor
		/// </summary>
		BlendFactor SourceBlend
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the destrination blending factor
		/// </summary>
		BlendFactor DestinationBlend
		{
			get; set;
		}

		#endregion

		#region Primitive properties

		/// <summary>
		/// Gets/sets the point size
		/// </summary>
		float PointSize
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the line width
		/// </summary>
		float LineWidth
		{
			get; set;
		}

		#endregion
	}
}
