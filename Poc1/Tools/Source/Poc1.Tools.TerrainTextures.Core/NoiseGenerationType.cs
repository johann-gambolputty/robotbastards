namespace Poc1.Tools.TerrainTextures.Core
{
	/// <summary>
	/// Ways by which noise can be generated
	/// </summary>
	public enum NoiseGenerationType
	{
		/// <summary>
		/// Noise values are stored as grayscale
		/// </summary>
		Grayscale,

		/// <summary>
		/// Noise from different offsets is stored in each colour channel
		/// </summary>
		MultiChannel,

		/// <summary>
		/// Noise values are stored as grayscale, with a toroidal tiling (tiles in 2d and 3d)
		/// </summary>
		TilingGrayscale,

		/// <summary>
		/// Noise from different offsets is stored in each colour channel, with a toroidal tiling (tiles in 2d and 3d)
		/// </summary>
		TilingMultiChannel,

		/// <summary>
		/// Small volume noise that can be scaled up to cover large volumes
		/// </summary>
		/// <remarks>
		/// Noise generation stores tiling, complex noise values. Output format must be RGBA.
		/// Channel assignment:
		///		- Red: Complex noise real component
		///		- Green: Complex noise imaginary component
		///		- Blue: Sine of red component
		///		- Alpha: Cosine of green component
		/// </remarks>
		BigNoise
	}
}
