using Rb.Core.Components;
using Rb.Interaction;
using Rb.World.Entities;
using Rb.Core.Utils;
using Rb.Core.Maths;

namespace Rb.TestApp
{
	public class TestController : Component
	{
		[Dispatch]
		public MessageRecipientResult OnCommand( CommandMessage msg )
		{
			float speed = 15.0f;

			//	TODO: AP: Query entity frame instead?
			Entity3d entity = ( Entity3d )Parent;
			switch ( ( TestCommands )msg.CommandId )
			{
				case TestCommands.Forward	: SendMovement( entity, entity.Ahead * speed ); break;
				case TestCommands.Back		: SendMovement( entity, entity.Back * speed ); break;
				case TestCommands.Left		: SendMovement( entity, entity.Left * speed ); break;
				case TestCommands.Right		: SendMovement( entity, entity.Right * speed ); break;
				case TestCommands.Jump		: entity.HandleMessage( new JumpRequest( null ) ); break;
			}
			return MessageRecipientResult.DeliverToNext;
		}

		/// <summary>
		/// Sends a movement message to the specified target
		/// </summary>
		private static void SendMovement( Entity3d target, Vector3 move )
		{
			//	Turn movement into units per second (irrespective of clock update rate)
			move *= ( float )TinyTime.ToSeconds( target.CurrentPosition.LastStepInterval );
			target.HandleMessage( new MovementXzRequest( move.X, move.Z ) );
		}

		/// <summary>
		/// Called when this object is added to a parent
		/// </summary>
		/// <param name="parent">Parent object</param>
		public override void AddedToParent( object parent )
		{
			base.AddedToParent( parent );
			MessageHub.AddDispatchRecipient( ( IMessageHub )parent, typeof( CommandMessage ), this, 0 );
		}

	}
}
