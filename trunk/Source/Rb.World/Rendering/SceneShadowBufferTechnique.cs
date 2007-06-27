using System;
using System.Collections.Generic;
using Rb.Core.Maths;
using Rb.Core.Resources;
using Rb.Rendering;

namespace Rb.World.Rendering
{
	/// <summary>
	/// Builds up to 4 depth textures for lights
	/// </summary>
	/// <remarks>
	/// This will do nothing, until a LightGroup is associated with the technique, using the ShadowLights property.
	/// Any child techniques this technique has will be applied after the shadow buffer build step.
	/// </remarks>
	public class SceneShadowBufferTechnique : ShadowBufferTechnique, ISceneObject
	{

		/// <summary>
		/// Applies this technique
		/// </summary>
        public override void Apply( IRenderContext context, IRenderable renderable )
		{
            //  TODO: AP: ShadowLights should be created on construction - no longer necessary to create on demand
            if ( ShadowLights == null )
            {
                ShadowLights = new LightGroup( );
            }

            //  TODO: AP: Shouldn't have to do this each frame
            ILightingManager manager = m_Scene.GetService< ILightingManager >( );
            List< Light > lights = new List< Light >( );

            foreach ( Light light in manager.Lights )
            {
                //  TODO: AP: Proper volume/coverage determination
                if ( ( light.ShadowCaster ) && ( light is SpotLight ) )
                {
                    lights.Add( light );
                }
            }
            ShadowLights.Lights = lights.ToArray( );

            base.Apply( context, renderable );
		}

	    #region ISceneObject Members

        /// <summary>
        /// Sets the scene context
        /// </summary>
        /// <param name="scene">Scene context</param>
        public void SetSceneContext( Scene scene )
        {
            m_Scene = scene;
        }

        #endregion


        #region Private stuff

	    private Scene m_Scene;

        #endregion
    }
}
