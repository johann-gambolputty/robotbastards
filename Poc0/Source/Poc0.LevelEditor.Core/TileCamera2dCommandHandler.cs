
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Interaction;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// 2D tile camera commands
	/// </summary>
	public enum TileCamera2dCommands
	{
        [CommandDescription( "Pan", "Pans the camera" )]
		Pan,

		[CommandDescription( "Zoom", "Zooms the camera in and out")]
		Zoom
	}

	/// <summary>
	/// Camera for viewing tiles that are being rendered in 2D
	/// </summary>
	public class TileCamera2dCommandHandler : Component
	{
		/// <summary>
		/// Tile camera
		/// </summary>
		public TileCamera2d Camera
		{
			get { return m_Camera; }
			set { m_Camera = value; }
		}

		/// <summary>
		/// Movement speed of the camera
		/// </summary>
		public float MovementSpeed
		{
			get { return m_MovementSpeed;  }
			set { m_MovementSpeed = value; }
		}


		/// <summary>
		/// Called when the handler receives a command message
		/// </summary>
		[Dispatch]
		public MessageRecipientResult OnCommand( CommandMessage msg )
		{
			if ( m_Camera == null )
			{
				return MessageRecipientResult.DeliverToNext;
			}

			switch ( ( TileCamera2dCommands )msg.CommandId )
			{
				case TileCamera2dCommands.Pan :
					{
                        CursorCommandMessage cursorMsg = ( CursorCommandMessage )msg;
                        float deltaX = cursorMsg.X - cursorMsg.LastX;
                        float deltaY = cursorMsg.Y - cursorMsg.LastY;

						Camera.Origin += new Vector2( deltaX * MovementSpeed, deltaY * MovementSpeed );
                        break;
					}
				case TileCamera2dCommands.Zoom :
					{
						Camera.Scale = ( ( ScalarCommandMessage )msg ).Value;
						break;
					}
			}

			return MessageRecipientResult.DeliverToNext;
		}

		#region Private members

		private TileCamera2d m_Camera;
		private float m_MovementSpeed = 1.0f;

		#endregion
	}
}
