using System;
using System.Collections;
using RbEngine.Maths;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Builds up to 4 depth textures for lights in a scene
	/// </summary>
	public class ShadowBufferTechnique : Components.Node, ITechnique
	{
		/// <summary>
		/// Maximum number of shadow casting lights
		/// </summary>
		public const int				MaxLights = 4;

		/// <summary>
		/// Gets the technique that the shadow buffer render technique uses to override rendered objects' techniques
		/// </summary>
		public static RenderTechnique	OverrideTechnique
		{
			get
			{
				return ms_OverrideTechnique;
			}
		}

		/// <summary>
		/// Temporary bodge to switch between depth texture and normal texture for shadow map source
		/// </summary>
		public static bool	DepthTextureMethod = true;

		/// <summary>
		/// Sets up the builder
		/// </summary>
		/// <exception cref="ApplicationException">Thrown if internal render target creation is not successful</exception>
		public ShadowBufferTechnique( )
		{
			//	Create a technique that is used to override standard scene rendering techniques (for shadow buffer generation)
			if ( ms_OverrideTechnique == null )
			{
				//	TODO: This should be an embedded resource, directly compiled from a hardcoded string, or something
				RenderEffect effect = ( RenderEffect )Resources.ResourceManager.Inst.Load( DepthTextureMethod ? "shadowMapDepthTexture.cgfx" : "shadowMapTexture.cgfx" );
				ms_OverrideTechnique = effect.FindTechnique( "DefaultTechnique" );
			}

			//	Create render targets for up to MaxLights lights
			try
			{
				for ( int lightIndex = 0; lightIndex < MaxLights; ++lightIndex )
				{
					RenderTarget target = RenderFactory.Inst.NewRenderTarget( );

					//	TODO: Remove hardcoded render target format
					target.Create( 512, 512, DepthTextureMethod ? TextureFormat.Undefined : TextureFormat.R8G8B8, 16, 0, DepthTextureMethod );

					m_RenderTargets[ lightIndex ] = target;
				}
			}
			catch ( Exception exception )
			{
				throw new ApplicationException( "Failed to create shadow buffer render technique (render target creation failed)", exception );
			}

			//	Create a shader parameter binding to the shadow matrix
			m_ShadowMatrixBinding	= ShaderParameterBindings.Inst.CreateBinding( "ShadowMatrix", ShaderParameterCustomBinding.ValueType.Matrix );
			m_ShadowNearZBinding	= ShaderParameterBindings.Inst.CreateBinding( "ShadowNearZ", ShaderParameterCustomBinding.ValueType.Float );
			m_ShadowFarZBinding		= ShaderParameterBindings.Inst.CreateBinding( "ShadowFarZ", ShaderParameterCustomBinding.ValueType.Float );
		}

		#region	IRender Members

		/// <summary>
		/// Makes the shadow buffers
		/// </summary>
		private int MakeBuffers( TechniqueRenderDelegate render, LightGroup lights )
		{
			Renderer.Inst.PushTransform( Transform.LocalToWorld );
			Renderer.Inst.PushTransform( Transform.WorldToView );
			Renderer.Inst.PushTransform( Transform.ViewToScreen );

			Renderer.Inst.SetTransform( Transform.LocalToWorld, Matrix44.Identity );

			//
			//	Ideal:
			//		- Run through currently active lights
			//		- Query the scene renderables for any intersecting the current light's cone
			//		- Render all objects in that list to current render target
			//		- as before
			//
			//	Issues:
			//		- Needs access to the light manager
			//		- Naive inside-cone query will return the entire environment. Query needs to be aware of environment subsections (e.g. BSP nodes)
			//

			int numLights = lights.NumLights > MaxLights ? MaxLights : lights.NumLights;

			for ( int lightIndex = 0; lightIndex < numLights; ++lightIndex )
			{
				RenderTarget	curTarget	= m_RenderTargets[ lightIndex ];
				SpotLight		curLight	= ( SpotLight )lights[ lightIndex ];	//	TODO: Assuming spotlight

				//	Set up the transform for the current light

				//	TODO: Need proper Y axis
				int		width		= curTarget.Width;
				int		height		= curTarget.Height;
				float	aspectRatio	= ( height == 0 ) ? 1.0f : ( ( float )width / ( float )height );

				Renderer.Inst.SetLookAtTransform( curLight.Position + curLight.Direction, curLight.Position, Vector3.YAxis );
				Renderer.Inst.SetPerspectiveProjectionTransform( curLight.ArcDegrees, aspectRatio, m_NearZ, m_FarZ );

				//	Set the current MVP matrix as the shadow transform. This is for after, when the scene is rendered properly
				Matrix44 shadowMat = Renderer.Inst.GetTransform( Transform.ViewToScreen ) * Renderer.Inst.GetTransform( Transform.WorldToView );
				m_ShadowMatrixBinding.Set( shadowMat );

				//	Set near and far Z plane bindings
				//	NOTE: This could be done once in setup - kept here for now so I can change them on the fly
				m_ShadowNearZBinding.Set( m_NearZ );
				m_ShadowFarZBinding.Set( m_FarZ );

				//	Set up the render target for the light
				curTarget.Begin( );
				Rendering.Renderer.Inst.ClearColour( System.Drawing.Color.Black );
				Rendering.Renderer.Inst.ClearDepth( 1.0f );

				RenderTechnique.Override = ms_OverrideTechnique;

				render( );

				RenderTechnique.Override = null;

				//	Stop using the current render target
				curTarget.End( );

				//	Save the depth buffer
				if ( ms_DumpLights )
				{
					if ( DepthTextureMethod )
					{
						curTarget.DepthTexture.Save( string.Format( "LightView{0}.png", lightIndex ) );
					}
					else
					{
						curTarget.Texture.Save( string.Format( "LightView{0}.png", lightIndex ) );
					}
					ms_DumpLights = false;
				}
			}

			Renderer.Inst.PopTransform( Transform.LocalToWorld );
			Renderer.Inst.PopTransform( Transform.WorldToView );
			Renderer.Inst.PopTransform( Transform.ViewToScreen );

			return numLights;
		}

		/// <summary>
		/// Applies this technique
		/// </summary>
		public void Apply( TechniqueRenderDelegate render )
		{
			//	Determine the active set of lights
			LightGroup lights = GetParentLightGroup( );

			int numBuffers = 0;
			if ( ( lights != null ) && ( lights.NumLights > 0 ) )
			{
				numBuffers = MakeBuffers( render, lights );
			}

			//	Apply all light textures
			for ( int bufferIndex = 0; bufferIndex < numBuffers; ++bufferIndex )
			{
				Renderer.Inst.BindTexture( bufferIndex, DepthTextureMethod ? m_RenderTargets[ bufferIndex ].DepthTexture : m_RenderTargets[ bufferIndex ].Texture );
			}

			//	Apply all child techniques
			foreach ( Object childObj in Children )
			{
				if ( childObj is ITechnique )
				{
					( ( ITechnique )childObj ).Apply( render );
				}
			}

			//	Stops applying all light textures
			for ( int bufferIndex = 0; bufferIndex < numBuffers; ++bufferIndex )
			{
				Renderer.Inst.UnbindTexture( bufferIndex );
			}
		}

		#endregion

		#region IChildObject Members

		/// <summary>
		/// Tries to extract a LightGroup from the parent object
		/// </summary>
		public override void AddedToParent( Object parentObject )
		{
			base.AddedToParent( parentObject );
			m_ParentLights	= null;
			m_ParentLights	= GetParentLightGroup( );
		}

		#endregion

		private LightGroup	GetParentLightGroup( )
		{
			//	Check if the parent's light group has already been cached
			if ( m_ParentLights != null )
			{
				return m_ParentLights;
			}

			//	No parent? No light group
			if ( ( Parent == null ) || ( !( Parent is Components.IParentObject ) ) )
			{
				return null;
			}

			//	Try to find the light group in the parent's child list
			foreach ( Object childObj in ( ( Components.IParentObject )Parent ).Children )
			{
				if ( childObj is LightGroup )
				{
					return ( LightGroup )childObj;
				}
			}

			return null;
		}

		private static RenderTechnique			ms_OverrideTechnique;
		private static bool						ms_DumpLights	= true;

		private LightGroup						m_ParentLights	= null;
		private RenderTarget[]					m_RenderTargets	= new RenderTarget[ MaxLights ];
		private float							m_NearZ			= 1.0f;
		private float							m_FarZ			= 500.0f;
		private ShaderParameterCustomBinding	m_ShadowMatrixBinding;
		private ShaderParameterCustomBinding	m_ShadowNearZBinding;
		private ShaderParameterCustomBinding	m_ShadowFarZBinding;
	}
}
