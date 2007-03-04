using System;
using RbEngine.Components;
using RbEngine.Rendering;
using RbEngine.Animation;
using RbEngine.Scene;
using RbEngine.Entities;


namespace RbOpenGlMd3Loader
{
	
	//	TODO: This has taken on some aspects of higher level animation control (movement states, etc.) - this should be handled
	//	by a different object

	/// <summary>
	/// An instance of an MD3-loaded model
	/// </summary>
	public class ModelInstance : IRender, IAnimationControl, ISceneObject, IChildObject
	{
		/// <summary>
		/// Sets the model that this object was instanced from
		/// </summary>
		public ModelInstance( Model source )
		{
			m_Source = source;

			//	Set up the animation layers
			m_Layers = new AnimationLayer[ ( int )ModelPart.NumParts ];
			for ( int layerIndex = 0; layerIndex < ( int )ModelPart.NumParts; ++layerIndex )
			{
				//	TODO: This assigns the entire animation set to each layer, which isn't correct (although it'll work OK)
				m_Layers[ layerIndex ] = new AnimationLayer( source.Animations, ( ModelPart )layerIndex );
			}
		}

		#region IRender Members

		/// <summary>
		/// Render all part meshes
		/// </summary>
		public void Render( )
		{
			m_Source.Render( m_Layers );
		}

		#endregion

		#region IAnimationControl Members

		/// <summary>
		/// Gets a named animation layer
		/// </summary>
		public IAnimationLayer GetLayer( string name )
		{
			return m_Layers[ ( int )( Enum.Parse( typeof( ModelPart ), name, true ) ) ];
		}

		#endregion

		#region	ISceneObject Members

		/// <summary>
		/// Invoked when this object is added to a scene by SceneDb.Remove()
		/// </summary>
		/// <param name="db">Database that this object was added to</param>
		public void	AddedToScene( SceneDb db )
		{
			db.GetNamedClock( "updateClock" ).Subscribe( new Clock.TickDelegate( Update ) );
		}

		/// <summary>
		/// Invoked when this object is removed from a scene by SceneDb.Remove()
		/// </summary>
		/// <param name="db">Database that this object was removed from</param>
		public void	RemovedFromScene( SceneDb db )
		{
		}

		#endregion

		#region	Private stuff

		private Model				m_Source;
		private AnimationLayer[]	m_Layers;

		#endregion

		/// <summary>
		/// Updates this model
		/// </summary>
		private void Update( Clock clock )
		{
			AnimationLayer lowerAnimLayer = m_Layers[ ( int )ModelPart.Lower ];
			if ( m_LastMovementState != m_MovementState )
			{
				AnimationType newLowerAnimType = AnimationType.LegsIdle;
				switch ( m_MovementState )
				{
					case MovementState.Walking	:	newLowerAnimType = AnimationType.LegsWalk;	break;
					case MovementState.Running	:	newLowerAnimType = AnimationType.LegsRun;	break;
					case MovementState.Standing	:	newLowerAnimType = AnimationType.LegsIdle;	break;
					case MovementState.Jumping	:	newLowerAnimType = AnimationType.LegsJump;	break;
				}

				lowerAnimLayer.PlayAnimation( m_Source.Animations.GetAnimation( newLowerAnimType ) );
			}
			else
			{
				if ( !lowerAnimLayer.Update( ) )
				{
					//	Reached the end of the animation - go back to idle
					m_MovementState = MovementState.Standing;
					lowerAnimLayer.PlayAnimation( m_Source.Animations.GetAnimation( AnimationType.LegsIdle ) );
				}
			}

			m_LastMovementState = m_MovementState;

			if ( m_MovementState != MovementState.Jumping )
			{
				m_MovementState	= MovementState.Standing;
			}
		}

		#region IChildObject Members

		/// <summary>
		/// Called when this object is added to its parent, or bound to a property in its parent
		/// </summary>
		public void AddedToParent( object parentObject )
		{
			if ( parentObject is IMessageHandler )
			{
				( ( IMessageHandler )parentObject ).AddRecipient( typeof( RbEngine.Entities.MovementRequest ), new MessageRecipientDelegate( HandleMovementRequest ), ( int )MessageRecipientOrder.Last );
			}
		}

		/// <summary>
		/// Handles a movement request message
		/// </summary>
		public void HandleMovementRequest( Message msg )
		{
			if ( msg is JumpRequest )
			{
				m_MovementState = MovementState.Jumping;
			}
			else
			{
				if ( m_MovementState != MovementState.Jumping )
				{
					if ( ( ( MovementRequest )msg ).Distance > 6.0f )
					{
						m_MovementState = MovementState.Running;
					}
					else
					{
						m_MovementState = MovementState.Walking;
					}
				}
			}
		}

		private enum MovementState
		{
			Standing,
			Walking,
			Running,
			Jumping
		}

		private MovementState	m_MovementState		= MovementState.Standing;
		private MovementState	m_LastMovementState	= MovementState.Standing;

		#endregion
	}
}
