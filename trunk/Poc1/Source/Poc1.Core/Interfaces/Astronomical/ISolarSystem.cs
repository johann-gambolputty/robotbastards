
using Poc1.Core.Interfaces.Rendering;
using Rb.Core.Components.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Poc1.Core.Interfaces.Astronomical
{
	/// <summary>
	/// Solar system interface
	/// </summary>
	public interface ISolarSystem : IComposite<IUniObject>, IRenderable<IUniRenderContext>
	{
	}
}
