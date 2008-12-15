using System.Windows.Forms;
using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Windows.InputBindings
{
	/// <summary>
	/// Monitors mouse wheel events in a control
	/// </summary>
	public class CommandMouseWheelInputBindingMonitor : CommandInputBindingMonitor
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="control">Control to monitor for mouse wheel events</param>
		/// <param name="binding">Input binding</param>
		/// <param name="user">Command user</param>
		public CommandMouseWheelInputBindingMonitor( Control control, CommandMouseWheelInputBinding binding, ICommandUser user ) :
			base( binding, user )
		{
			m_Control = control;
		}

		/// <summary>
		/// Starts listening for mouse wheel events
		/// </summary>
		public override void Start( )
		{
			m_Control.MouseWheel += OnMouseWheel;
		}

		/// <summary>
		/// Updates this monitor
		/// </summary>
		/// <returns>Returns true if the binding is active</returns>
		public override bool Update( )
		{
			bool active = m_Active;
			m_Active = false;
			return active;
		}

		/// <summary>
		/// Returns true if the binding is active
		/// </summary>
		public override bool IsActive
		{
			get { return m_Active; }
		}

		/// <summary>
		/// Stops listening for mouse wheel events
		/// </summary>
		public override void Stop( )
		{
			m_Control.MouseWheel -= OnMouseWheel;
		}

		/// <summary>
		/// Creates an input state from the state of this monitor
		/// </summary>
		public override ICommandInputState CreateInputState( ICommandInputStateFactory factory, object context )
		{
			return factory.NewScalarInputState( context, 0, m_Value );
		}

		#region Private Members

		private readonly Control m_Control;
		private float m_Value;
		private bool m_Active;

		/// <summary>
		/// Handles mouse wheel events raised from the associated control
		/// </summary>
		private void OnMouseWheel( object sender, MouseEventArgs args )
		{
			m_Active = true;
			m_Value = args.Delta > 0 ? 1 : -1;
		}

		#endregion
	}
}
