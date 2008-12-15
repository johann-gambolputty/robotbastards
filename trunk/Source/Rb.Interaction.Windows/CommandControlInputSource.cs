using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;
using Rb.Interaction.Windows.InputBindings;

namespace Rb.Interaction.Windows
{

	/// <summary>
	/// Simple control input source. Updates the base CommandInputSource by hooking the application idle handler.
	/// </summary>
	public class CommandControlInputSource : CommandInputSource
	{
		/// <summary>
		/// Helper method that creates monitors for a set of command input bindings
		/// </summary>
		/// <param name="user">Originating user</param>
		/// <param name="control">Control to monitor</param>
		/// <param name="bindings">Binding definitions</param>
		/// <returns>Returns a new command input source that has started monitoring the specified control</returns>
		public static CommandControlInputSource StartMonitoring( ICommandUser user, Control control, IEnumerable<CommandInputBinding> bindings )
		{
			return StartMonitoring( user, control, null, bindings );
		}

		/// <summary>
		/// Helper method that creates monitors for a set of command input bindings
		/// </summary>
		/// <param name="user">Originating user</param>
		/// <param name="control">Control to monitor</param>
		/// <param name="context">User-specified context object cookie. Can be null</param>
		/// <param name="bindings">Binding definitions</param>
		/// <returns>Returns a new command input source that has started monitoring the specified control</returns>
		public static CommandControlInputSource StartMonitoring( ICommandUser user, Control control, object context, IEnumerable<CommandInputBinding> bindings )
		{
			CommandControlInputSource source = new CommandControlInputSource( control, context );
			source.AddBindings( user, bindings );
			source.Start( );
			return source;
		}

		/// <summary>
		/// Creates a control binding monitor factory for the base CommandInputSource implementation
		/// </summary>
		/// <param name="control">Control to bind to</param>
		/// <param name="context">User-specified context object cookie. Can be null</param>
		public CommandControlInputSource( Control control, object context ) :
			base( new CommandInputBindingMonitorFactory( control ), context )
		{
			//	Render loops in forms:
			//	https://blogs.msdn.com/tmiller/archive/2005/05/05/415008.aspx
			//	Alternative:
			//	http://blogs.msdn.com/rickhos/archive/2005/04/04/405327.aspx
		}

		/// <summary>
		/// Starts monitoring all input bindings. Begins update loop.
		/// </summary>
		public override void Start( )
		{
			base.Start( );
			InteractionUpdateTimer.Instance.Update += Update;
		}

		/// <summary>
		/// Stops monitoring all input bindings. Ends update loop.
		/// </summary>
		public override void Stop( )
		{
			InteractionUpdateTimer.Instance.Update -= Update;
		}

		/// <summary>
		/// Handles the application idle event. Updates all input binding monitors
		/// </summary>
		private void OnInteractionUpdate( object sender, EventArgs args )
		{
			while ( ApplicationIdleHandler.IsAppStillIdle )
			{
				Update( );
			}
		}
	}

}
