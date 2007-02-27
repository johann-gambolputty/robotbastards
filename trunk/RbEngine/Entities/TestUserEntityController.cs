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

	public class TestLookAtCommandMessage : Interaction.CommandMessage
	{
		public TestLookAtCommandMessage( Interaction.Command cmd, Network.Client client, Maths.Point3 pt ) :
			base( cmd, client )
		{
			m_Point = pt;
		}

		public Maths.Point3		LookAtPoint
		{
			get
			{
				return m_Point;
			}
		}

		private Maths.Point3 m_Point;
	}

	public class TestLookAtCommand : Interaction.Command
	{

		public TestLookAtCommand( ) :
			base( "lookAt", "Looks at where the mouse is pointing", ( ushort )TestCommands.LookAt )
		{

		}

		/// <summary>
		/// Adds an input binding to this command
		/// </summary>
		/// <param name="binding">Binding to add (must be derived from <see cref="Interaction.CommandCursorInputBinding"/>)</param>
		public override void							AddBinding( Interaction.CommandInputBinding binding )
		{
			base.AddBinding( ( Interaction.CommandCursorInputBinding )binding );
		}

		/// <summary>
		/// Generates a TestLookAtCommandMessage from the active binding
		/// </summary>
		protected override Interaction.CommandMessage	GenerateMessageFromActiveBinding( Interaction.CommandInputBinding.ClientBinding binding )
		{
			int screenX = ( ( Interaction.CommandCursorInputBinding.ClientBinding )binding ).X;
			int screenY = ( ( Interaction.CommandCursorInputBinding.ClientBinding )binding ).Y;

			//	TODO: Bodge (should use active camera's projection setup, + scene query, etc.)
			Maths.Ray3 pickRay = ( ( Cameras.Camera3 )binding.Client.Camera ).PickRay( screenX, screenY );

			Maths.Ray3Intersection intersection = Scene.ClosestRay3IntersectionQuery.Get( pickRay, binding.Client.Scene.Objects );
			if ( intersection == null )
			{
				return null;
			}

			return new TestLookAtCommandMessage( this, binding.Client, intersection.IntersectionPosition );
		}
	}

	/// <summary>
	/// Summary description for TestUserEntityController.
	/// </summary>
	public class TestUserEntityController : Components.Component, Scene.ISceneRenderable
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
			float speed = 5.0f;

			//	TODO: HACK (should set entity better, instead of casting Parent. should send a MovementXzRequest, instead of directly modifying the position)
			Entity3 entity = ( Entity3 )Parent;
			switch ( ( ( Interaction.CommandMessage )msg ).Id )
			{
				case ( int )TestCommands.Forward	:
				{
					if ( m_LookAt.Current.DistanceTo( entity.Position.Current ) > speed )
					{
						entity.Position.Current += entity.Facing * speed;
					}
					break;
				}

				case ( int )TestCommands.Back		:	entity.Position.Current -= entity.Facing * speed;	break;
				case ( int )TestCommands.Left		:	entity.Position.Current += entity.Left * speed;		break;
				case ( int )TestCommands.Right		:	entity.Position.Current += entity.Right * speed;	break;

				case ( int )TestCommands.LookAt		:
				{
					//	TODO: This is a bit of a cheat, because I know that this controller only ever receives one look at message
					//	on every command update)
					m_LookAt.Step( TinyTime.CurrentTime );
					m_LookAt.Current = ( ( TestLookAtCommandMessage )msg ).LookAtPoint;

					break;
				}
			}

			entity.Facing	= ( m_LookAt.Current - entity.Position.Current ).MakeNormal( );
			entity.Left		= Maths.Vector3.Cross( entity.Up, entity.Facing ).MakeNormal( );

			msg.DeliverToNextRecipient( );
		}

		private Maths.Point3Interpolator m_LookAt = new Maths.Point3Interpolator( );

		#region ISceneRenderable Members

		public void Render( long renderTime )
		{
		//	( ( Cameras.ProjectionCamera )Rendering.Renderer.Inst.Camera ).PickRay( 0, 0 );

			Entity3			entity		= ( Entity3 )Parent;
			Maths.Point3	pos			= entity.Position.Get( renderTime );
			Maths.Point3	lookAtPos	= m_LookAt.Get( renderTime );

			Rendering.ShapeRenderer.Inst.DrawLine( pos, pos + entity.Facing * 3.0f );
			Rendering.ShapeRenderer.Inst.DrawLine( pos, pos + entity.Left * 3.0f );
			Rendering.ShapeRenderer.Inst.DrawLine( pos, pos + entity.Up * 3.0f );
			Rendering.ShapeRenderer.Inst.DrawSphere( lookAtPos, 1.0f );
		}

		#endregion
	}
}
