using System.Collections.Generic;
using Rb.Rendering;
using Rb.Rendering.Lights;
using Rb.World.Services;

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
		/// Scene shadow buffer
		/// </summary>
		public SceneShadowBufferTechnique( int resX, int resY ) :
			base( resX, resY )
		{
		}

		/// <summary>
		/// Applies this technique
		/// </summary>
        public override void Apply( IRenderContext context, IRenderable renderable )
		{
            //  TODO: AP: Shouldn't have to do this each frame
            ILightingService service = m_Scene.GetService< ILightingService >( );
            List< ILight > lights = new List< ILight >( );

            foreach ( ILight light in service.Lights )
            {
                //  TODO: AP: Proper volume/coverage determination
                if ( ( light.CastsShadows ) && ( light is ISpotLight ) )
                {
                    lights.Add( light );
                }
            }
            ShadowLights.Lights = lights.ToArray( );

            base.Apply( context, renderable );
		}

	    #region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
        public void AddedToScene( Scene scene )
        {
            m_Scene = scene;
        }
		
		/// <summary>
		/// Called when this object is removed from the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public void RemovedFromScene( Scene scene )
		{
		}

        #endregion

        #region Private stuff

	    private Scene m_Scene;

        #endregion
    }
}
