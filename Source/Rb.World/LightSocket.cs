using System;
using Rb.Core.Components;
using Rb.Rendering;

namespace Rb.World
{
	//	TODO: AP: Just used for adding a light to the lighting manager - it can't be used afterwards

	/// <summary>
	/// A very poorly named class that adds a <see cref="Rb.Rendering.Light"/> into the scene <see cref="LightingManager"/>
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
				ILightingManager manager = m_Scene.GetService< ILightingManager >( );
				if ( manager == null )
				{
					throw new InvalidOperationException( "LightSocket requires that an ILightingManager service be present in the scene" );
				}
				if ( m_Light != null )
				{
					manager.RemoveLight( m_Light );
				}
				m_Light = value;
				if ( m_Light != null )
				{
					manager.AddLight( m_Light );
				}
			}
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

		private Light m_Light;
		private Scene m_Scene;

		#endregion
	}
}
