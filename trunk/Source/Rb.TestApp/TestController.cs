using Rb.Core.Components;
using Rb.Interaction;
using Rb.World.Entities;
using Rb.Core.Utils;
using Rb.Core.Maths;

namespace Rb.TestApp
{
	/// <summary>
	/// Test controller class
	/// </summary>
	public class TestController : Component
	{
		/// <summary>
		/// Called when the controller receives a command message
		/// </summary>
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
				case TestCommands.LookAt	:
					PickCommandMessage pickMsg = ( PickCommandMessage )msg;
					if ( pickMsg.Intersection != null )
					{
						SendLookAt( entity, pickMsg.Intersection.IntersectionPosition );
					}
					break;
			}
			return MessageRecipientResult.DeliverToNext;
		}

		/// <summary>
		/// Cheats and forces the entity to look at a given point
		/// </summary>
		private static void SendLookAt( Entity3d target, Point3 pos )
		{
			Vector3 ahead = ( pos - target.NextPosition ).MakeNormal( );
			Vector3 left = Vector3.Cross( target.Up, target.Ahead ).MakeNormal( );

			target.SetFrame( left, target.Up, ahead );
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
