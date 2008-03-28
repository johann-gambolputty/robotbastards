using System;
using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects.Lights;

namespace Rb.World.Services
{
	/// <summary>
	/// Manages lighting in a scene
	/// </summary>
	[Serializable]
	public class LightingService : ILightingService, ISceneObject
	{
		#region ILightingService Members

		/// <summary>
		/// Adds a light meter to the lighting manager
		/// </summary>
		/// <param name="lightMeter">Light meter to add</param>
		public void AddLightMeter( ILightMeter lightMeter )
		{
			m_Meters.Add( lightMeter );
		}

		/// <summary>
		/// Removes a light meter to the lighting manager
		/// </summary>
		/// <param name="lightMeter">Light meter to remove</param>
		public void RemoveLightMeter( ILightMeter lightMeter )
		{
			m_Meters.Remove( lightMeter );
		}

		/// <summary>
		/// Adds a light to the lighting manager
		/// </summary>
		/// <param name="light">Light to add</param>
		public void AddLight( ILight light )
		{
			m_Lights.Add( light );
		}

		/// <summary>
		/// Removes a light to the lighting manager
		/// </summary>
		/// <param name="light">Light to remove</param>
		public void RemoveLight( ILight light )
		{
			m_Lights.Remove( light );
		}

		/// <summary>
		/// Gets the array of lights in the scene
		/// </summary>
		public IList< ILight > Lights
		{
			get { return m_Lights; }
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the scene
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void AddedToScene( Scene scene )
		{
			scene.PreRender += OnPreRender;
		}

		/// <summary>
		/// Called when this object is removed from the scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public void RemovedFromScene( Scene scene )
		{
			scene.PreRender -= OnPreRender;
		}

		#endregion
		
		#region Private stuff

		private readonly List< ILightMeter > m_Meters = new List< ILightMeter >( );
		private readonly List< ILight > m_Lights = new List< ILight >( );

		/// <summary>
		/// Called before the scene gets rendered
		/// </summary>
		/// <param name="scene">Scene getting rendered</param>
		private void OnPreRender( Scene scene )
		{
			//	TODO: AP: BODGE - actually choose the damn lights per meter
			//	(this will break with more than 4 lights)
			ILight[] lights = m_Lights.ToArray( );
			foreach ( ILightMeter meter in m_Meters )
			{
				meter.SetLights( lights );
			}
		}

		#endregion

	}
}
