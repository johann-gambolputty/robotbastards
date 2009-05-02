using Rb.Core.Maths;

namespace Poc1.Core.Interfaces.Astronomical.Planets.Renderers
{
	/// <summary>
	/// Inteface for reflective ocean renderers
	/// </summary>
	public interface IPlanetReflectiveOceanRenderer
	{
		/// <summary>
		/// Gets the tangent plane at the specified position
		/// </summary>
		Plane3 GetTangentPlaneUnderPoint( UniPoint3 pos, out Point3 pointOnPlane );

		/// <summary>
		/// Gets the tangent space matrix at the specified position
		/// </summary>
		Matrix44 GetTangentSpaceUnderPoint( UniPoint3 pos );
	}
}
