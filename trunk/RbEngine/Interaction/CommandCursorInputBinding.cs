using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Input binding from a screen cursor (e.g. mouse pointer)
	/// </summary>
	public abstract class CommandCursorInputBinding : CommandInputBinding
	{
		/// <summary>
		/// Client-specific binding
		/// </summary>
		public new class ClientBinding : CommandInputBinding.ClientBinding
		{
			/// <summary>
			/// X position of the cursor
			/// </summary>
			public int		X
			{
				get
				{
					return m_X;
				}
			}

			/// <summary>
			/// Y position of the cursor
			/// </summary>
			public int		Y
			{
				get
				{
					return m_Y;
				}
			}

			/// <summary>
			/// X movement of the cursor since the last update
			/// </summary>
			public int		DeltaX
			{
				get
				{
					return m_X - m_LastX;
				}
			}

			/// <summary>
			/// Y movement of the cursor since the last update
			/// </summary>
			public int		DeltaY
			{
				get
				{
					return m_Y - m_LastY;
				}
			}

			/// <summary>
			/// Setup constructor
			/// </summary>
			/// <param name="client">Bound client</param>
			public			ClientBinding( Network.Client client ) :
				base( client )
			{
			}

			protected int	m_X;
			protected int	m_Y;
			protected int	m_LastX;
			protected int	m_LastY;
		}
	}
}
