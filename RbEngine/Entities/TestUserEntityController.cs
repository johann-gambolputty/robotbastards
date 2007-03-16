using System;
using RbEngine.Components;
using RbEngine.Rendering;

namespace RbEngine.Entities
{
	/// <summary>
	/// Test command set
	/// </summary>
	public enum TestCommandId
	{
		Forward,
		Back,
		Left,
		Right,
		Shoot,
		Jump,
		LookAt
	}

	/// <summary>
	/// Summary description for TestUserEntityController.
	/// </summary>
	public class TestUserEntityController : Components.Component, Scene.ISceneRenderable, Scene.ISceneObject
	{

		/// <summary>
		/// Constructor
		/// </summary>
		public TestUserEntityController( )
		{
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
			float speed = 5.0f;

			//	TODO: HACK (should set entity better, instead of casting Parent. should send a MovementXzRequest, instead of directly modifying the position)
			Entity3 entity = ( Entity3 )Parent;

			Maths.Vector3 movement = new Maths.Vector3( );

			switch ( ( TestCommands )( ( Interaction.CommandMessage )msg ).Id )
			{
				case TestCommands.Forward	:
				{
					float distanceToLookAt = m_LookAt.Next.DistanceTo( entity.Position.Next );
					if ( distanceToLookAt > speed )
					{
						speed = ( distanceToLookAt > 80.0f ) ? 8.0f : speed;
						movement = entity.Facing * speed;
					}
					break;
				}

				case TestCommands.Back		:	movement = entity.Facing * -speed;	break;
				case TestCommands.Left		:	movement = entity.Left * speed;		break;
				case TestCommands.Right		:	movement = entity.Right * speed;	break;

				case TestCommands.LookAt	:
				{
					//	TODO: This is a bit of a cheat, because I know that this controller only ever receives one look at message
					//	on every command update)
					m_LookAt.Step( TinyTime.CurrentTime );
					m_LookAt.Next = ( ( TestLookAtCommandMessage )msg ).LookAtPoint;

					break;
				}

				case TestCommands.Jump		:
				{
				//	Maths.Vector3 jumpVector = entity.Facing * 10.0f;
					Maths.Vector3 jumpVector = Maths.Vector3.Origin;
					entity.HandleMessage( new JumpRequest( jumpVector.X, jumpVector.Z, false ) );
					break;
				}
			}

			if ( movement.SqrLength > 0 )
			{
				entity.HandleMessage( new MovementXzRequest( movement.X, movement.Z, false ) );
			}

			entity.Facing	= ( m_LookAt.Next - entity.Position.Next ).MakeNormal( );
			entity.Left		= Maths.Vector3.Cross( entity.Up, entity.Facing ).MakeNormal( );

			msg.DeliverToNextRecipient( );
		}

		#region ISceneRenderable Members

		/// <summary>
		/// Gets the list of objects to apply before rendering
		/// </summary>
		public ApplianceList	PreRenderList
		{
			get
			{
				return m_PreRenderAppliances;
			}
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		public void Render( long renderTime )
		{
			Entity3			entity		= ( Entity3 )Parent;
			Maths.Point3	pos			= entity.Position.Get( renderTime );
			Maths.Point3	lookAtPos	= m_LookAt.Get( renderTime );

		//	Rendering.ShapeRenderer.Inst.DrawLine( pos, pos + entity.Facing * 3.0f );
		//	Rendering.ShapeRenderer.Inst.DrawLine( pos, pos + entity.Left * 3.0f );
		//	Rendering.ShapeRenderer.Inst.DrawLine( pos, pos + entity.Up * 3.0f );
			Rendering.ShapeRenderer.Inst.DrawSphere( lookAtPos, 1.0f );
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		public void AddedToScene( Scene.SceneDb db )
		{
			CommandMessageSource = db.Server;
			
			//	Add this object to the render manager
			db.Rendering.AddObject( this );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		public void RemovedFromScene( Scene.SceneDb db )
		{
			//	Remove this object from the render manager
			db.Rendering.RemoveObject( this );
		}

		#endregion

		#region	Private stuff

		private Maths.Point3Interpolator	m_LookAt				= new Maths.Point3Interpolator( );
		private ApplianceList				m_PreRenderAppliances	= new ApplianceList( );

		#endregion
	}
}
