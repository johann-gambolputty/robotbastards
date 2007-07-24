using System;
using Rb.Core.Components;
using Rb.Interaction;
using Rb.Rendering;
using Rb.World;
using Rb.World.Entities;
using Rb.Core.Utils;
using Rb.Core.Maths;

namespace Rb.TestApp
{
	/// <summary>
	/// Handles commands from the <see cref="TestCommands"/> enumeration
	/// </summary>
	public class TestCommandHandler : Component, IRenderable, ISceneObject
	{
        /// <summary>
        /// Entity speed
        /// </summary>
	    public float Speed
	    {
            get { return m_Speed; }
            set { m_Speed = value; }
	    }

		/// <summary>
		/// Called when the controller receives a command message
		/// </summary>
		[Dispatch]
		public MessageRecipientResult OnCommand( CommandMessage msg )
		{
            float speed = m_Speed;
			float rotateSpeed = 0.1f;

			//	TODO: AP: Query entity frame instead?
			Entity3d entity = ( Entity3d )Parent;
			switch ( ( TestCommands )msg.CommandId )
			{
				case TestCommands.Forward		: SendMovement( entity, entity.Ahead * speed ); break;
				case TestCommands.Back			: SendMovement( entity, entity.Back * speed ); break;
				case TestCommands.Left			: SendMovement( entity, entity.Left * speed ); break;
				case TestCommands.Right			: SendMovement( entity, entity.Right * speed ); break;
				case TestCommands.RotateRight	: SendRotation( entity, rotateSpeed ); break;
				case TestCommands.RotateLeft	: SendRotation( entity, -rotateSpeed ); break;
				case TestCommands.Jump			: entity.HandleMessage( new JumpRequest( null ) ); break;
				case TestCommands.LookAt		:
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
		/// Called when this object is added to a parent
		/// </summary>
		/// <param name="parent">Parent object</param>
		public override void AddedToParent( object parent )
		{
			base.AddedToParent( parent );
			MessageHub.AddDispatchRecipient( ( IMessageHub )parent, typeof( CommandMessage ), this, MessageRecipientOrder.Last );
		}

		#region ISceneObject Members

		/// <summary>
		/// Attaches this object to the scene render list
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void SetSceneContext( Scene scene )
		{
			scene.Renderables.Add( this );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this object
		/// </summary>
		public void Render( IRenderContext context )
		{
			if ( m_RenderState == null )
			{
				m_RenderState = RenderFactory.Instance.NewRenderState( );
				m_RenderState
					.DisableLighting( )
					.SetColour( System.Drawing.Color.White )
				;
			}
			m_RenderState.Begin( );
			ShapeRenderer.Instance.DrawSphere( m_LookAt, 0.5f );
			m_RenderState.End( );
		}

		#endregion

		#region Private stuff

		private Point3		m_LookAt = Point3.Origin;
		private RenderState	m_RenderState;
        private float       m_Speed = 15.0f;

		/// <summary>
		/// Cheats and forces the entity to look at a given point
		/// </summary>
		private void SendLookAt( Entity3d target, Point3 pos )
		{
			m_LookAt = pos;
			/*

			Vector3 ahead = ( pos - target.NextPosition );
			ahead.Y = 0;

			float aheadLength = ahead.Length;
			if ( aheadLength < 0.001f )
			{
				return;
			}
			ahead /= aheadLength;

			Vector3 left = Vector3.Cross( target.Up, ahead ).MakeNormal( );

			target.SetFrame( left, target.Up, ahead );
			*/
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

		private static void SendRotation( Entity3d target, float delta )
		{
			float s = ( float )Math.Atan2( target.Ahead.Z, target.Ahead.X );
			s = Utils.Wrap( s + delta, 0, ( float )( 2 * Math.PI ) );

			target.HandleMessage( new RotateXzRequest( s ) );
		}

		#endregion
	}
}
