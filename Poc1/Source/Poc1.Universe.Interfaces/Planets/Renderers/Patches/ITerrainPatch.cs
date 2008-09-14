
using Rb.Core.Maths;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Universe.Interfaces.Planets.Renderers.Patches
{
	/// <summary>
	/// Terrain patch interface
	/// </summary>
	public unsafe interface ITerrainPatch : IRenderable
	{
		/// <summary>
		/// Gets/sets the maximum error between the patch geometry and the underlying terrain
		/// </summary>
		float PatchError
		{
			get; set;
		}

		/// <summary>
		/// Gets/sets the UV resolution of the patch
		/// </summary>
		float UvResolution
		{
			get; set;
		}

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

		/// <summary>
		/// Called when the terrain patch has finished building
		/// </summary>
		void OnBuildComplete( byte* vertexData, float increaseDetailDistance, int[] baseIndices );
	}
}
