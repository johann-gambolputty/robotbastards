using System;
using Rb.Core.Components;
using Rb.Rendering;
using Rb.World.Services;

namespace Rb.World
{
	//	TODO: AP: Just used for adding a light to the lighting manager - it can't be used afterwards

	/// <summary>
	/// A very poorly named class that adds a <see cref="Rb.Rendering.Light"/> into the scene <see cref="ILightingService"/>
	/// </summary>
	public class LightSocket : Component, ISceneObject
	{
		/// <summary>
		/// The attached light
		/// </summary>
		public Light Light
		{
			get
			{
				return m_Light;
			}
			set
			{
				if ( m_Scene == null )
				{
					//	Socket hasn't been added to a scene yet - just store the light object. When
					//	the socket is added to the scene, it'll add the light to the lighting service
					m_Light = value;
					return;
				}

				//	Socket is in scene. Remove the current light from the lighting service, add the new one
				ILightingService service = m_Scene.GetService< ILightingService >( );
				if ( service == null )
				{
					throw new InvalidOperationException( "LightSocket requires that an ILightingService service be present in the scene" );
				}
				if ( m_Light != null )
				{
					service.RemoveLight( m_Light );
				}
				m_Light = value;
				if ( m_Light != null )
				{
					service.AddLight( m_Light );
				}
			}
		}

		#region ISceneObject Members

		/// <summary>
		/// Sets the scene context
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void AddedToScene( Scene scene )
		{
			m_Scene = scene;
			if ( m_Light != null )
			{
				scene.GetService< ILightingService >( ).AddLight( m_Light );
			}
		}

		/// <summary>
		/// Called when this object is removed from the scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public void RemovedFromScene( Scene scene )
		{
			if ( m_Light != null )
			{
				scene.GetService< ILightingService >( ).RemoveLight( m_Light );
			}
			m_Scene = null;
		}

		#endregion

		#region Private stuff

		private Light m_Light;
		private Scene m_Scene;

		#endregion
	}
}
