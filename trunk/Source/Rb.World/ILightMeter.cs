using Rb.Rendering;

namespace Rb.World
{
	/// <summary>
	/// Light meters are used to determine the lights will be used to illuminate a given object
	/// </summary>
	public interface ILightMeter
	{
		//	TODO: AP: Add IsAffectedByLight() call?

		/// <summary>
		/// Sets the lights that the meter is affected by
		/// </summary>
		void SetLights( Light[] lights );
	}
}
