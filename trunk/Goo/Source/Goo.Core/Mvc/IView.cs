using Rb.Core.Utils;

namespace Goo.Core.Mvc
{
	/// <summary>
	/// Basic view interface
	/// </summary>
	public interface IView
	{
		/// <summary>
		/// Frame closing event, raised by the OnFrameClosing()
		/// </summary>
		event ActionDelegates.Action<IView> FrameClosing;

		/// <summary>
		/// Called by a view's frame when it is closing. Raises the event <see cref="FrameClosing"/>
		/// </summary>
		void OnFrameClosing( );
	}
}
