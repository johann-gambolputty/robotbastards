using System.Drawing;

namespace Rb.Rendering.Interfaces
{
	/// <summary>
	/// Interface for cube mapped textures
	/// </summary>
	public interface ICubeMapTexture : ITexture
	{
		/// <summary>
		/// Builds this cube map from 6 bitmaps
		/// </summary>
		/// <param name="posX">Positive X axis bitmap</param>
		/// <param name="negX">Negative X axis bitmap</param>
		/// <param name="posY">Positive Y axis bitmap</param>
		/// <param name="negY">Negative Y axis bitmap</param>
		/// <param name="posZ">Positive Z axis bitmap</param>
		/// <param name="negZ">Negative Z axis bitmap</param>
		void Build( Bitmap posX, Bitmap negX, Bitmap posY, Bitmap negY, Bitmap posZ, Bitmap negZ );

		/// <summary>
		/// Builds this cube map from 6 textures
		/// </summary>
		/// <param name="posX">Positive X axis texture</param>
		/// <param name="negX">Negative X axis texture</param>
		/// <param name="posY">Positive Y axis texture</param>
		/// <param name="negY">Negative Y axis texture</param>
		/// <param name="posZ">Positive Z axis texture</param>
		/// <param name="negZ">Negative Z axis texture</param>
		void Build( ITexture2d posX, ITexture2d negX, ITexture2d posY, ITexture2d negY, ITexture2d posZ, ITexture2d negZ );

		/// <summary>
		/// Renders this cubemap texture to a series of bitmaps
		/// </summary>
		Bitmap[] ToBitmaps( );
	}
}
