using System;
using Rb.Rendering.Base.Lights;
using Rb.Rendering.Interfaces.Objects.Lights;
using Rb.World.Services;

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
		/// Called when this object is added to the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public virtual void AddedToScene( Scene scene )
		{
			ILightingService lighting = scene.GetService< ILightingService >( );
			if ( lighting == null )
			{
				throw new InvalidOperationException( "LightMeter requires that an ILightingService be present in the scene" );
			}

			lighting.AddLightMeter( this );
		}
		
		/// <summary>
		/// Called when this object is removed from the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public virtual void RemovedFromScene( Scene scene )
		{
			scene.GetService< ILightingService >( ).RemoveLightMeter( this );
		}

		#endregion

		#region ILightMeter Members

		/// <summary>
		/// Sets the lights that this meter is affected by
		/// </summary>
		public void SetLights( ILight[] lights )
		{
			Lights = lights;
		}

		#endregion
	}
}
