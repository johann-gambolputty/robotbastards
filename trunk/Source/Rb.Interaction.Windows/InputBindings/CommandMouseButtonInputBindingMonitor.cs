
using System.Windows.Forms;
using Rb.Core.Maths;
using Rb.Interaction.Classes;
using Rb.Interaction.Classes.InputBindings;
using Rb.Interaction.Interfaces;
using IntMouseButtons = Rb.Interaction.Classes.InputBindings.MouseButtons;
using MouseButtons=System.Windows.Forms.MouseButtons;

namespace Rb.Interaction.Windows.InputBindings
{
	public class CommandMouseButtonInputBindingMonitor : CommandBinaryInputBindingMonitor
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="control">Control to monitor for mouse button events</param>
		/// <param name="binding">Binding definition</param>
		/// <param name="user">Command user</param>
		public CommandMouseButtonInputBindingMonitor( Control control, CommandMouseButtonInputBinding binding, ICommandUser user ) :
			base( binding, user )
		{
			switch ( binding.Button )
			{
				case IntMouseButtons.Left	: m_Button = MouseButtons.Left;		break;
				case IntMouseButtons.Middle	: m_Button = MouseButtons.Middle;	break;
				case IntMouseButtons.Right	: m_Button = MouseButtons.Right;	break;
			}
			m_Control = control;
			MonitorState = binding.ButtonState;
		}

		/// <summary>
		/// Starts monitoring the associated control for mouse events
		/// </summary>
		public override void Start( )
		{
			m_Control.MouseDown += OnMouseDown;
			m_Control.MouseMove += OnMouseMove;
			m_Control.MouseUp += OnMouseUp;
		}


		/// <summary>
		/// Stops monitoring mouse events in the associated control
		/// </summary>
		public override void Stop( )
		{
			m_Control.MouseDown -= OnMouseDown;
			m_Control.MouseMove -= OnMouseMove;
			m_Control.MouseUp -= OnMouseUp;
		}

		/// <summary>
		/// Creates an input state from the state of this monitor
		/// </summary>
		public override ICommandInputState CreateInputState( ICommandInputStateFactory factory, object context )
		{
			return factory.NewPointInputState( context, m_LastPos.X, m_LastPos.Y, m_CurPos.X, m_CurPos.Y );
		}

		#region Private Members

		private readonly Control m_Control;
		private MouseButtons m_Button;
		private Point2 m_LastPos = new Point2( );
		private Point2 m_CurPos = new Point2( );

		private void OnMouseDown( object sender, MouseEventArgs args )
		{
			if ( args.Button == m_Button )
			{
				OnDown( );
			}
		}

		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			m_LastPos = m_CurPos;
			m_CurPos.X = args.X / ( float )m_Control.Width;
			m_CurPos.Y = args.Y / ( float )m_Control.Height;
		}

		private void OnMouseUp( object sender, MouseEventArgs args )
		{
			if ( args.Button == m_Button )
			{
				OnUp( );
			}
		} 
		#endregion
	}
}
