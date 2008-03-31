using Rb.Core.Maths;

namespace Rb.Rendering.Interfaces.Objects.Lights
{
	/// <summary>
	/// Spotlight interface
	/// </summary>
	public interface ISpotLight : IPointLight
	{
		/// <summary>
		/// Gets the direction of the light
		/// </summary>
		Vector3 Direction
		{
			get; set;
		}

		/// <summary>
		/// Sets the direction of the light by looking at a given point
		/// </summary>
		/// <remarks>
		/// Light position must be set before calling LookAt
		/// </remarks>
		Point3 LookAt
		{
			set;
		}

		/// <summary>
		/// Access to the arc of the light
		/// </summary>
		float ArcDegrees
		{
			get; set;
		}

	}
}
