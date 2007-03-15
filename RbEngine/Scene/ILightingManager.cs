using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// Service for filling LightGroup objects
	/// </summary>
	public interface ILightingManager
	{
		/// <summary>
		/// Fills a light group with lights, based on their distances from the specified camera's frustum, intensities, and other properties
		/// </summary>
		void GetCameraLightGroup( Cameras.Camera3 camera, Rendering.LightGroup lights );

		/// <summary>
		/// Fills a light group with lights, based on their distances from a specified point, intensities, and other properties
		/// </summary>
		void GetObjectLightGroup( Maths.Point3 pos, Rendering.LightGroup lights );

		/// <summary>
		/// Gets the set of all stored lights
		/// </summary>
		ArrayList Lights
		{
			get;
		}
	}
}
