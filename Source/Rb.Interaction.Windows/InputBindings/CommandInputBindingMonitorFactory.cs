using System;
using System.Windows.Forms;
using Rb.Core.Utils;
using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Windows.InputBindings
{

	/// <summary>
	/// Creates monitors for input bindings, that use a control to provide input events
	/// </summary>
	public class CommandInputBindingMonitorFactory : ICommandInputBindingMonitorFactory
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="control">Control to bind to</param>
		/// <exception cref="ArgumentNullException">Thrown if control is null</exception>
		public CommandInputBindingMonitorFactory( Control control )
		{
			Arguments.CheckNotNull( control, "control" );
			m_Control = control;
		}

		#region ICommandInputBindingMonitorFactory Members

		/// <summary>
		/// Creates a monitor for a key binding
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if binding or user are null</exception>
		public ICommandInputBindingMonitor CreateBindingMonitor( CommandKeyInputBinding binding, ICommandUser user )
		{
			Arguments.CheckNotNull( binding, "binding" );
			Arguments.CheckNotNull( user, "user" );

			return new CommandKeyInputBindingMonitor( m_Control, binding, user );
		}

		/// <summary>
		/// Creates a monitor for a mouse button binding
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if binding or user are null</exception>
		public ICommandInputBindingMonitor CreateBindingMonitor( CommandMouseButtonInputBinding binding, ICommandUser user )
		{
			Arguments.CheckNotNull( binding, "binding" );
			Arguments.CheckNotNull( user, "user" );

			return new CommandMouseButtonInputBindingMonitor( m_Control, binding, user );
		}

		/// <summary>
		/// Creates a monitor for a mouse wheel binding
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if binding or user are null</exception>
		public ICommandInputBindingMonitor CreateBindingMonitor( CommandMouseWheelInputBinding binding, ICommandUser user )
		{
			Arguments.CheckNotNull( binding, "binding" );
			Arguments.CheckNotNull( user, "user" );

			return new CommandMouseWheelInputBindingMonitor( m_Control, binding, user );
		}

		/// <summary>
		/// Creates a monitor for an unknown binding type
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if binding or user are null</exception>
		/// <exception cref="NotSupportedException">Always thrown</exception>
		public virtual ICommandInputBindingMonitor CreateBindingMonitor( CommandInputBinding binding, ICommandUser user )
		{
			Arguments.CheckNotNull( binding, "binding" );
			Arguments.CheckNotNull( user, "user" );
			throw new NotSupportedException( "Unsupported binding type " + binding.GetType( ) );
		}

		#endregion

		#region Private Members

		private readonly Control m_Control;

		#endregion
	}


}
