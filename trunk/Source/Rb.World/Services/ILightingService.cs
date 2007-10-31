using System.Collections.Generic;
using Rb.Rendering.Lights;

namespace Rb.World.Services
{
	/// <summary>
	/// Service for filling LightGroup objects
	/// </summary>
	public interface ILightingService
	{
		#region Light meters

		/// <summary>
		/// Adds a light meter to the lighting manager
		/// </summary>
		/// <param name="lightMeter">Light meter to add</param>
		void AddLightMeter( ILightMeter lightMeter );

		/// <summary>
		/// Removes a light meter to the lighting manager
		/// </summary>
		/// <param name="lightMeter">Light meter to remove</param>
		void RemoveLightMeter( ILightMeter lightMeter );

		#endregion

		#region Lights

		/// <summary>
		/// Adds a light to the lighting manager
		/// </summary>
		/// <param name="light">Light to add</param>
		void AddLight( ILight light );

		/// <summary>
		/// Removes a light to the lighting manager
		/// </summary>
		/// <param name="light">Light to remove</param>
		void RemoveLight( ILight light );

		/// <summary>
		/// Gets the set of all stored lights
		/// </summary>
		IList< ILight > Lights
		{
			get;
		}

		#endregion

	}
}
