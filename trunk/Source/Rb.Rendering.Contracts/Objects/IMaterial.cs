using System.Drawing;

namespace Rb.Rendering.Contracts.Objects
{
	/// <summary>
	/// Defines material properties
	/// </summary>
	public interface IMaterial : IPass
	{
		/// <summary>
		/// The ambient response of the material
		/// </summary>
		Color Ambient
		{
			get; set;
		}

		/// <summary>
		/// The diffuse response of the material
		/// </summary>
		Color Diffuse
		{
			get;
			set;
		}

		/// <summary>
		/// The specular response of the material
		/// </summary>
		Color Specular
		{
			get; set;
		}
	}
}
