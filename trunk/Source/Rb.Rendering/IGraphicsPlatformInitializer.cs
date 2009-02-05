
namespace Rb.Rendering
{
	/// <summary>
	/// Interface for graphics platform initialization
	/// </summary>
	/// <remarks>
	/// When a graphics platform assembly is loaded
	/// </remarks>
	public interface IGraphicsPlatformInitializer
	{
		/// <summary>
		/// Runs platform-specific graphics initialization
		/// </summary>
		void Init( );
	}
}
