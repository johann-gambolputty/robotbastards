using Rb.Core.Maths;

namespace Rb.Rendering.Lights
{
	/// <summary>
	/// Point light interface
	/// </summary>
	public interface IPointLight : ILight
	{
		/// <summary>
		/// Light position
		/// </summary>
		Point3 Position
		{
			get; set;
		}

		/// <summary>
		/// Light inner radius
		/// </summary>
		float InnerRadius
		{
			get; set;
		}

		/// <summary>
		/// Light outer radius
		/// </summary>
		float OuterRadius
		{
			get; set;
		}

		/// <summary>
		/// Light attenuation
		/// </summary>
		float Attenuation
		{
			get; set;
		}
	}
}
