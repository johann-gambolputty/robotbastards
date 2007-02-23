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

	/*
	 * Binary bindings:
	 *	Command only needs check that the binding is active before sending the message
	 * 
	 * Analogue bindings:
	 *	Command requires specific anlogue interpretation.
	 *		example: mouse look in fps game; mouse input binding tracks movement deltas. Command turns them into S,T deltas
	 *		example: mouse track in 3rd person game; mouse input binding tracks screen position. Command shoots ray, intersects with world
	 */

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
		/// <param name="binding"></param>
		public override void								AddBinding( Interaction.CommandInputBinding binding )
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
			Maths.Ray3 pickRay = binding.Client.Camera.PickRay( screenX, screenY );

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
			//	TODO: HACK (should set entity better, instead of casting Parent. should send a MovementXzRequest, instead of directly modifying the position)
			Entity3 entity = ( Entity3 )Parent;
			switch ( ( ( Interaction.CommandMessage )msg ).Id )
			{
				case ( int )TestCommands.Forward	:	entity.Position += entity.Facing;	break;
				case ( int )TestCommands.Back		:	entity.Position -= entity.Facing;	break;
				case ( int )TestCommands.Left		:	entity.Position += entity.Left;		break;
				case ( int )TestCommands.Right		:	entity.Position += entity.Right;	break;

				case ( int )TestCommands.LookAt		:
				{
					m_LookAt = ( ( TestLookAtCommandMessage )msg ).LookAtPoint;
					break;
				}
			}

			entity.Facing	= ( m_LookAt - entity.Position ).MakeNormal( );
			entity.Left		= Maths.Vector3.Cross( entity.Up, entity.Facing ).MakeNormal( );

			msg.DeliverToNextRecipient( );
		}

		private Maths.Point3 m_LookAt;

		#region ISceneRenderable Members

		public void Render( float delta )
		{
			if ( m_LookAt != null )
			{
				Entity3 entity = ( Entity3 )Parent;
				Rendering.ShapeRenderer.Inst.RenderLine( entity.Position, entity.Position + entity.Facing * 3.0f );
				Rendering.ShapeRenderer.Inst.RenderLine( entity.Position, entity.Position + entity.Left * 3.0f );
				Rendering.ShapeRenderer.Inst.RenderLine( entity.Position, entity.Position + entity.Up * 3.0f );
				Rendering.ShapeRenderer.Inst.RenderSphere( m_LookAt, 1.0f );
			}
		}

		#endregion
	}
}
