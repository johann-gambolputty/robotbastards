using System;
using RbEngine.Components;
using RbEngine.Rendering;

namespace RbEngine.Entities
{
	/// <summary>
	/// Test command set
	/// </summary>
	public enum TestCommands
	{
		[ Interaction.CommandEnumDescription( "Moves forwards" ) ]
		Forward,

		[ Interaction.CommandEnumDescription( "Moves backwards" ) ]
		Back,

		[ Interaction.CommandEnumDescription( "Moves left" ) ]
		Left,

		[ Interaction.CommandEnumDescription( "Moves right" ) ]
		Right,

		[ Interaction.CommandEnumDescription( "Fires the current weapon" ) ]
		Shoot,

		[ Interaction.CommandEnumDescription( "Jumps" ) ]
		Jump,

		[ Interaction.CommandEnumDescription( "Looks at a point" ) ]
		LookAt
	}

	/// <summary>
	/// Summary description for TestUserEntityController.
	/// </summary>
	public class TestUserEntityController : Components.Component, Scene.ISceneRenderable, Scene.ISceneObject
	{
		/// <summary>
		/// Command list for this controller
		/// </summary>
		public static Interaction.CommandList Commands = Interaction.CommandListManager.CreateFromEnum( typeof( TestCommands ) );

		/// <summary>
		/// Gets an Interaction.Command object from a TestCommandId identifier
		/// </summary>
		public static Interaction.Command GetCommand( TestCommands id )
		{
			return Commands.GetCommandById( ( int )id );
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public TestUserEntityController( )
		{
			GetCommand( TestCommands.Forward	).Active	+= new Interaction.CommandEventDelegate( OnForward );
			GetCommand( TestCommands.Back		).Active	+= new Interaction.CommandEventDelegate( OnBack );
			GetCommand( TestCommands.Left		).Active	+= new Interaction.CommandEventDelegate( OnLeft );
			GetCommand( TestCommands.Right		).Active	+= new Interaction.CommandEventDelegate( OnRight );
			GetCommand( TestCommands.Shoot		).Activated += new Interaction.CommandEventDelegate( OnShoot );
			GetCommand( TestCommands.Jump		).Activated += new Interaction.CommandEventDelegate( OnJump );
			GetCommand( TestCommands.LookAt		).Active	+= new Interaction.CommandEventDelegate( OnLookAt );
		}

		#region	Command event handling

		private float	Speed
		{
			get
			{
				return 15.0f;
			}
		}

		private void OnForward( Interaction.CommandInputBinding binding )
		{
			Entity3	entity				= ParentEntity;
			float	distanceToLookAt	= m_LookAt.Next.DistanceTo( entity.Position.Next );
			float	curSpeed			= Speed;
			if ( distanceToLookAt > curSpeed )
			{
				curSpeed = ( distanceToLookAt > 80.0f ) ? 8.0f : curSpeed;
				SendMovement( entity.Facing * curSpeed );
			}
		}

		private void OnBack( Interaction.CommandInputBinding binding )
		{
			SendMovement( ParentEntity.Facing * -Speed );
		}

		private void OnLeft( Interaction.CommandInputBinding binding )
		{
			SendMovement( ParentEntity.Left * Speed );
		}

		private void OnRight( Interaction.CommandInputBinding binding )
		{
			SendMovement( ParentEntity.Right * Speed );
		}

		private void OnShoot( Interaction.CommandInputBinding binding )
		{
		}

		private void OnJump( Interaction.CommandInputBinding binding )
		{
			ParentEntity.HandleMessage( new JumpRequest( 0, 0, false ) );
		}

		private void OnLookAt( Interaction.CommandInputBinding binding )
		{
			//	TODO: This is a bit of a cheat, because I know that this controller only ever receives one look at message
			//	on every command update)
			m_LookAt.Step( TinyTime.CurrentTime );

			Scene.SceneDb db = binding.View.Scene;
			if ( db == null )
			{
				return;
			}
			Scene.IRayCaster rayCaster = ( Scene.IRayCaster )db.GetSystem( typeof( Scene.IRayCaster ) );
			if ( rayCaster == null )
			{
				return;
			}

			Interaction.CommandCursorInputBinding cursorBinding = ( Interaction.CommandCursorInputBinding )binding;

			Maths.Ray3				pickRay				= ( ( Cameras.Camera3 )cursorBinding.View.Camera ).PickRay( cursorBinding.X, cursorBinding.Y );
			Maths.Ray3Intersection	pickIntersection	= rayCaster.GetFirstIntersection( pickRay, null );

			if ( pickIntersection != null )
			{
				m_LookAt.Next = pickIntersection.IntersectionPosition;
			}

			Entity3 entity	= ParentEntity;
			entity.Facing	= ( m_LookAt.Next - entity.Position.Next ).MakeNormal( );
			entity.Left		= Maths.Vector3.Cross( entity.Up, entity.Facing ).MakeNormal( );
		}

		/// <summary>
		/// Sends a movement message to the parent entity
		/// </summary>
		private void SendMovement( Maths.Vector3 vec )
		{
			Entity3 entity = ParentEntity;

			//	Turn movement into units per second (irrespective of clock update rate)
			vec *= ( float )TinyTime.ToSeconds( entity.Position.LastStepInterval );

			ParentEntity.HandleMessage( new MovementXzRequest( vec.X, vec.Z, false ) );

			entity.Facing	= ( m_LookAt.Next - entity.Position.Next ).MakeNormal( );
			entity.Left		= Maths.Vector3.Cross( entity.Up, entity.Facing ).MakeNormal( );
		}

		#endregion

		/// <summary>
		/// Gets the entity that owns this controller
		/// </summary>
		private Entity3				ParentEntity
		{
			get
			{
				return ( Entity3 )Parent;
			}
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
