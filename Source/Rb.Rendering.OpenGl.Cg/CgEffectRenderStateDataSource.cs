using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.OpenGl.Cg
{
	public class CgEffectRenderStateDataSource : IEffectDataSource
	{
		public enum RenderStateSource
		{
			/// <summary>
			/// Parameter bound to the current model matrix
			/// </summary>
			ModelMatrix,

			/// <summary>
			/// Parameter bound to the current model matrix
			/// </summary>
			InverseTransposeModelMatrix,

			/// <summary>
			/// Parameter bound to the current view matrix
			/// </summary>
			ViewMatrix,

			/// <summary>
			/// Parameter bound to the current projection matrix
			/// </summary>
			ProjectionMatrix,

			/// <summary>
			/// Parameter bound to current texture matrix
			/// </summary>
			TextureMatrix,

			/// <summary>
			/// Parameter bound to current model view matrix
			/// </summary>
			ModelViewMatrix,

			/// <summary>
			/// Parameter bound to the inverse of the current model view matrix
			/// </summary>
			InverseModelViewMatrix,

			/// <summary>
			/// Parameter bound to the inverse transpose of the current model view matrix
			/// </summary>
			InverseTransposeModelViewMatrix,

			/// <summary>
			/// Parameter bound to current model view projection matrix
			/// </summary>
			ModelViewProjectionMatrix,

			/// <summary>
			/// Parameter bound to current eye position (world space)
			/// </summary>
			EyePosition,

			/// <summary>
			/// Parameter bound to current eye x axis (world space)
			/// </summary>
			EyeXAxis,

			/// <summary>
			/// Parameter bound to current eye y axis(world space)
			/// </summary>
			EyeYAxis,

			/// <summary>
			/// Parameter bound to current eye z axis (world space)
			/// </summary>
			EyeZAxis,

			//	TODO: This is very specific - requires a certain data structure to be present in the shader... 
			/// <summary>
			/// Data about the active point lights in the renderer
			/// </summary>
			/// <remarks>
			/// Binds to a struct like this:
			/// <code>
			/// struct PointLights
			/// {
			///    int m_NumLights;
			///    float4 m_Positions[ 4 ];
			/// }
			/// </code>
			/// </remarks>
			PointLights,

			//	TODO: This is very specific - requires a certain data structure to be present in the shader... 
			/// <summary>
			/// Data about the active spotlights in the renderer
			/// </summary>
			/// <remarks>
			/// Binds to a struct like this:
			/// <code>
			/// struct SpotLights
			/// {
			///    int m_NumLights;
			///    float4 m_Positions[ 4 ];
			///    float4 m_Directions[ 4 ];
			///    float  m_ArcRadians[ 4 ];
			/// }
			/// </code>
			/// </remarks>
			SpotLights,

			/// <summary>
			/// Binds the parameter to the first texture in the renderer. Parameter must be a texture sampler
			/// </summary>
			Texture0,

			/// <summary>
			/// Binds the parameter to the second texture in the renderer. Parameter must be a texture sampler
			/// </summary>
			Texture1,

			/// <summary>
			/// Binds the parameter to the third texture in the renderer. Parameter must be a texture sampler
			/// </summary>
			Texture2,

			/// <summary>
			/// Binds the parameter to the fourth texture in the renderer. Parameter must be a texture sampler
			/// </summary>
			Texture3,

			/// <summary>
			/// Binds the parameter to the fifth texture in the renderer. Parameter must be a texture sampler
			/// </summary>
			Texture4,

			/// <summary>
			/// Binds the parameter to the sixth texture in the renderer. Parameter must be a texture sampler
			/// </summary>
			Texture5,

			/// <summary>
			/// Binds the parameter to the seventh texture in the renderer. Parameter must be a texture sampler
			/// </summary>
			Texture6,

			/// <summary>
			/// Binds the parameter to the eighth texture in the renderer. Parameter must be a texture sampler
			/// </summary>
			Texture7,
		}

		public CgEffectRenderStateDataSource( RenderStateSource source )
		{
			m_Source = source;
		}

		private readonly RenderStateSource m_Source;

		#region IEffectDataSource Members

		public void Bind( IEffectParameter parameter )
		{
		}

		public void Apply( IEffectParameter parameter )
		{
		}

		#endregion
	}
}
