using Rb.Core.Maths;

namespace Poc1.Universe.Interfaces.Rendering
{
	public interface ITerrainPatch
	{
		/// <summary>
		/// Gets the local origin of the patch
		/// </summary>
		Point3 LocalOrigin
		{
			get;
		}

		/// <summary>
		/// Gets the (unnormalized) u axis for this patch. Also == to the LocalUStep * patch resolution
		/// </summary>
		Vector3 LocalUAxis
		{
			get;
		}

		/// <summary>
		/// Gets the (unnormalized) v axis for this patch. Also == to the LocalVStep * patch resolution
		/// </summary>
		Vector3 LocalVAxis
		{
			get;
		}

		/// <summary>
		/// Gets the U step vector for this patch
		/// </summary>
		Vector3 LocalUStep
		{
			get;
		}

		/// <summary>
		/// Gets the V step vector for this patch
		/// </summary>
		Vector3 LocalVStep
		{
			get;
		}

		/// <summary>
		/// Sets planet-space parameters
		/// </summary>
		void SetPlanetParameters( Point3 centre, float radius );

	}
}
