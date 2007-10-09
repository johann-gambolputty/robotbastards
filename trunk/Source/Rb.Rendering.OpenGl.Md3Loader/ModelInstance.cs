using System;
using Rb.Core.Components;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Animation;
using Rb.World;
using Rb.World.Entities;
using Rb.World.Services;


namespace Rb.Rendering.OpenGl.Md3Loader
{
	
	//	TODO: This has taken on some aspects of higher level animation control (movement states, etc.) - this should be handled
	//	by a different object

	/// <summary>
	/// An instance of an MD3-loaded model
	/// </summary>
	public class ModelInstance : IRenderable, IAnimationControl, ISceneObject
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

		#region IRenderable Members

		/// <summary>
		/// Render all part meshes
		/// </summary>
		public void Render( IRenderContext context )
		{
			m_Source.Render( context, m_Layers );
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
		/// Called when this object is added to a scene
		/// </summary>
        /// <param name="scene">Scene that this object was added to</param>
		public void	AddedToScene( Scene scene )
		{
            scene.GetService< IUpdateService >( )[ "animationClock" ].Subscribe( Update );
		}
		
		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
        /// <param name="scene">Scene that this object was added to</param>
		public void	RemovedFromScene( Scene scene )
		{
            scene.GetService< IUpdateService >( )[ "animationClock" ].Unsubscribe( Update );
		}

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
		/// Handles a movement request message
		/// </summary>
		[Dispatch]
		public MessageRecipientResult HandleMovementRequest( MovementRequest msg )
		{
			if ( msg is JumpRequest )
			{
				m_MovementState = MovementState.Jumping;
			}
			else
			{
				if ( m_MovementState != MovementState.Jumping )
				{
					if ( msg.Distance > 6.0f )
					{
						m_MovementState = MovementState.Running;
					}
					else
					{
						m_MovementState = MovementState.Walking;
					}
				}
			}

			return MessageRecipientResult.DeliverToNext;
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
		private object			m_MessageSource;

		#endregion

		#region Message source

		public object MessageSource
		{
			get { return m_MessageSource; }
			set
			{
				if ( m_MessageSource != null )
				{
					( ( IMessageHub )m_MessageSource ).RemoveRecipient( typeof( MovementRequest ), this );
				}
				m_MessageSource = value;
				if ( value != null )
				{
					MessageHub.AddDispatchRecipient( ( IMessageHub )value, typeof( MovementRequest ), this, MessageRecipientOrder.Last );
				}
			}
		}

		#endregion

		#region	Private stuff

		private readonly Model m_Source;
		private readonly AnimationLayer[] m_Layers;

		#endregion

	}
}
