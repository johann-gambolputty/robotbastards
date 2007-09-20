using System;
using Rb.Rendering;

namespace Rb.World
{
	/// <summary>
	/// Light meters are used to determine the lights will be used to illuminate a given object
	/// </summary>
	[Serializable]
	public class LightMeter : LightGroup, ILightMeter, ISceneObject
	{
		#region ISceneObject Members

		/// <summary>
		/// Sets the scene context
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void SetSceneContext( Scene scene )
		{
			ILightingManager lighting = scene.GetService< ILightingManager >( );
			if ( lighting == null )
			{
				throw new InvalidOperationException( "LightMeter requires that an ILightingManager service be present in the scene" );
			}

			lighting.AddLightMeter( this );
		}

		#endregion

		#region ILightMeter Members

		/// <summary>
		/// Sets the lights that this meter is affected by
		/// </summary>
		public void SetLights( Light[] lights )
		{
			Lights = lights;
		}

		#endregion
	}
}
