using System.Windows.Forms;

namespace Rb.Interaction.Windows
{
	enum MouseButtonState
	{
		Down,
		Held,
		Up,
		DoubleClick,
	}

	/// <summary>
	/// Mouse button input
	/// </summary>
	class MouseButtonInput : CursorInput
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="context">Input context</param>
		/// <param name="button">Mouse button to check for</param>
		/// <param name="state">Mouse state to look for</param>
		/// <param name="key">Key to check for</param>
		public MouseButtonInput( InputContext context, MouseButtons button, MouseButtonState state, Keys key ) :
			base( context )
		{
			m_Button = button;
			m_State = state;

			Control control = ( Control )context.Control;

			if ( m_State == MouseButtonState.DoubleClick )
			{
				control.MouseDoubleClick += OnMouseDoubleClick;
			}
			else
			{
				control.MouseDown += OnMouseDown;
				control.MouseUp += OnMouseUp;
			}

			if ( key != Keys.None )
			{
				m_KeyInput = new KeyInput( context, key, KeyState.Held );
			}

			control.MouseMove += new MouseEventHandler( OnMouseMove );
		}

		/// <summary>
		/// Returns true if not detecting held buttons (otherwise the input will remain active after a double click)
		/// </summary>
		public override bool DeactivateOnUpdate
		{
			get
			{
				return	( m_State == MouseButtonState.DoubleClick ) ||
						( m_State == MouseButtonState.Down ) ||
						( m_State == MouseButtonState.Up );
			}
		} 

		private bool IsKeyActive
		{
			get
			{
				return ( m_KeyInput == null ) || ( m_KeyInput.IsActive );
			}
		}

		/// <summary>
		/// Triggers the command, if the appropriate mouse button is pressed
		/// </summary>
		private void OnMouseMove( object sender, MouseEventArgs args )
		{
			m_LastX = m_X;
			m_LastY = m_Y;

			m_X = args.X;
			m_Y = args.Y;
		}

		private void OnMouseDoubleClick( object sender, MouseEventArgs args )
		{
			IsActive = IsKeyActive;
		}

		private void OnMouseDown( object sender, MouseEventArgs args )
		{
			if ( m_State == MouseButtonState.Down || m_State == MouseButtonState.Held )
			{
				IsActive = ( args.Button == m_Button ) && ( IsKeyActive );
			}
		}

		private void OnMouseUp( object sender, MouseEventArgs args )
		{
			if ( m_State == MouseButtonState.Up )
			{
				IsActive = ( args.Button == m_Button ) && ( IsKeyActive );
			}
			else
			{
				if ( args.Button == m_Button )
				{
					IsActive = false;
				}
			}
		}

		private readonly MouseButtons m_Button;
		private readonly MouseButtonState m_State;
		private readonly KeyInput m_KeyInput;
	}
}
