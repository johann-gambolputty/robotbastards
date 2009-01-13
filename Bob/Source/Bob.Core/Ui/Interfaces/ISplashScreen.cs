using System.Drawing;
using Rb.Core.Threading;

namespace Bob.Core.Ui.Interface
{
	/// <summary>
	/// Splash screen control interface
	/// </summary>
	public interface ISplashScreen
	{
		/// <summary>
		/// Gets/sets the background bitmap of the splash screen
		/// </summary>
		Bitmap Background
		{
			get; set;
		}

		/// <summary>
		/// If there is work to be performed while the splash screen is shown, it can be
		/// queued up here
		/// </summary>
		IWorkItemQueue WorkItems
		{
			get;
		}

		/// <summary>
		/// Get/sets the amount that this screen is faded out by
		/// </summary>
		float Fade
		{
			get; set;
		}

		/// <summary>
		/// Shows the splash screen
		/// </summary>
		/// <remarks>
		/// Runs all the work items in the queue
		/// </remarks>
		void Show( );
	}
}
