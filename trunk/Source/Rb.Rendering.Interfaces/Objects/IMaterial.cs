using System.Drawing;
using Rb.Rendering.Interfaces;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// Defines surface material properties
	/// </summary>
	/// <remarks>
	/// Create IMaterial objects using <see cref="IGraphicsFactory.CreateMaterial"/>
	/// </remarks>
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
