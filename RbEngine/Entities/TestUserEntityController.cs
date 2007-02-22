using System;
using RbEngine.Components;

namespace RbEngine.Entities
{
	public enum TestCommands
	{
		Forward,
		Back,
		Left,
		Right,
		Shoot,
		LookAt
	}

	/// <summary>
	/// Summary description for TestUserEntityController.
	/// </summary>
	public class TestUserEntityController : Components.Component
	{

		/// <summary>
		/// Constructor
		/// </summary>
		public TestUserEntityController( )
		{
			//	TODO: HACK REMOVEME. Requires reference resolution in RbXmlLoader, so controller can be defined as:
			//	<object type="RbEngine.Entities.TestUserEntityController">
			//		<reference name="$server" property="CommandMessageSource"/>
			//	</object>
			CommandMessageSource = RbEngine.Network.ServerManager.Inst.FindServer( "server0" );
		}

		/// <summary>
		/// Sets the source of command messages that this controller responds to
		/// </summary>
		public Components.IMessageHandler	CommandMessageSource
		{
			set
			{
				value.AddRecipient( typeof( Interaction.CommandMessage ), new MessageRecipientDelegate( HandleCommandMessage ), ( int )MessageRecipientOrder.Last );
			}
		}

		/// <summary>
		/// Handles command messages
		/// </summary>
		/// <param name="msg">Command message</param>
		private void				HandleCommandMessage( Message msg )
		{
			//	TODO: HACK (should set entity better, instead of casting Parent. should send a MovementXzRequest, instead of directly modifying the position)
			Entity3 entity = ( Entity3 )Parent;
			switch ( ( ( Interaction.CommandMessage )msg ).Id )
			{
				case ( int )TestCommands.Forward	:	entity.Position += entity.Facing;	break;
				case ( int )TestCommands.Back		:	entity.Position -= entity.Facing;	break;
				case ( int )TestCommands.Left		:	entity.Position += entity.Left;		break;
				case ( int )TestCommands.Right		:	entity.Position += entity.Right;	break;
			}

			msg.DeliverToNextRecipient( );
		}
	}
}
