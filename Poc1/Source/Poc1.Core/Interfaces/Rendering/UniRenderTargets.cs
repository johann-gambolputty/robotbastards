
namespace Poc1.Core.Interfaces.Rendering
{
	/// <summary>
	/// Enumerates the available render targets supported by <see cref="IUniRenderContext"/>
	/// </summary>
	/// <remarks>
	/// Don't explicitly set enum values - other classes require that the enum values form
	/// a zero-based index.
	/// </remarks>
	public enum UniRenderTargets
	{
		/// <summary>
		/// Rendering of planar ocean reflections
		/// </summary>
		OceanReflections
	}
}
