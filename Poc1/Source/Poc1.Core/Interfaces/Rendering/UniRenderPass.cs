
namespace Poc1.Core.Interfaces.Rendering
{
	/// <summary>
	/// Rendering passes supported by <see cref="IUniRenderContext"/>
	/// </summary>
	public enum UniRenderPass
	{
		FarObjects,
		//	TODO: Maybe add FarAtmospheres/CloseAtmospheres/CloseOccludingObjects
		CloseReflectedObjects,
		CloseShadowCastingObjects,
		CloseObjects
	}
}
