using System;
using System.Collections.Generic;
using Rb.Rendering;

namespace Rb.World
{
	/// <summary>
	/// Manages lighting in a scene
	/// </summary>
	public class LightingManager : ILightingManager, ISceneObject
	{
		#region ILightingManager Members

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
		public void AddLight( Light light )
		{
			m_Lights.Add( light );
		}

		/// <summary>
		/// Removes a light to the lighting manager
		/// </summary>
		/// <param name="light">Light to remove</param>
		public void RemoveLight( Light light )
		{
			m_Lights.Remove( light );
		}

		/// <summary>
		/// Gets the array of lights in the scene
		/// </summary>
		public IList< Light > Lights
		{
			get { return m_Lights; }
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Sets the scene context
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void SetSceneContext( Scene scene )
		{
			scene.PreRender += new Scene.RenderEventDelegate( OnPreRender );
		}

		#endregion
		
		#region Private stuff

		private List< ILightMeter > m_Meters	= new List< ILightMeter >( );
		private List< Light >		m_Lights	= new List< Light >( );

		/// <summary>
		/// Called before the scene gets rendered
		/// </summary>
		/// <param name="scene">Scene getting rendered</param>
		private void OnPreRender( Scene scene )
		{
			//	TODO: AP: BODGE - actually choose the damn lights per meter
			//	(this will break with more than 4 lights)
			Light[] lights = m_Lights.ToArray( );
			foreach ( ILightMeter meter in m_Meters )
			{
				meter.SetLights( lights );
			}
		}

		#endregion

	}
}
