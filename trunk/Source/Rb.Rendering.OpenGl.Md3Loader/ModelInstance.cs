using System;
using Rb.Core.Components;
using Rb.Core.Maths;
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
	/// <remarks>
	/// How the MD3 model works:
	/// 
	/// <see cref="Model"/> stores a set of mesh objects, one for each <see cref="ModelPart"/> value.
	/// Each part mesh contains a list of transforms for child objects, called tags, for each animation frame.
	/// A part mesh can have a single nested mesh that is connected by a tag - lower body connects to upper
	/// body, upper body connects to head. There's also a tag for a weapon attachment point in the upper body.
	/// 
	/// When a model instance is rendered, it renders its source model, passing in a set of animation layers
	/// to the render method. The model render method renders the root part mesh (the lower body) with an
	/// identity transform. When a mesh is rendered, it renders the associated vertex buffers, then pushes
	/// a transform for the nested mesh, based on transform in the tag that corresponds to the associated
	/// animation layer's current frame
	/// 
	/// </remarks>
	public class ModelInstance : IRenderable, IAnimationControl, ISceneObject, IChild, IReferencePoints
	{
		/// <summary>
		/// Sets the model that this object was instanced from
		/// </summary>
		public ModelInstance( Model source )
		{
			m_Source = source;

			//	Set up the animation layers
			if ( source.Animations == null )
			{
				return;
			}

			m_Layers = new AnimationLayer[ ( int )ModelPart.NumParts ];
			for ( int layerIndex = 0; layerIndex < ( int )ModelPart.NumParts; ++layerIndex )
			{
				m_ReferencePoints[ layerIndex ] = new ReferencePoint( ( ModelPart )layerIndex );

				ModelMesh partMesh = source.GetPartMesh( ( ModelPart )layerIndex );

				if ( partMesh == null )
				{
					continue;
				}
				if ( source.Animations != null )
				{
					//	TODO: This assigns the entire animation set to each layer, which isn't correct (although it'll work OK)
					m_Layers[ layerIndex ] = new AnimationLayer( source.Animations, ( ModelPart )layerIndex );
				}
			}
		}

		public class ReferencePoint : IReferencePoint
		{
			public ReferencePoint( ModelPart modelPart )
			{
				m_Part = modelPart;
				m_Name = modelPart.ToString( );
			}

			public ModelPart Part
			{
				get { return m_Part; }
			}

			public void Render( IRenderContext context )
			{
				Graphics.Renderer.GetTransform( Rb.Rendering.Transform.LocalToWorld, m_Transform );
				if ( OnRender != null )
				{
					OnRender( context );
				}
			}

			private readonly ModelPart m_Part;
			private readonly string m_Name;
			private readonly Matrix44 m_Transform = new Matrix44( );

			#region IReferencePoint Members

			public event RenderDelegate OnRender;

			public string Name
			{
				get { return m_Name; }
			}

			public Matrix44 Transform
			{
				get { return m_Transform; }
			}

			#endregion
		}

		#region IRenderable Members

		/// <summary>
		/// Render all part meshes
		/// </summary>
		public void Render( IRenderContext context )
		{
			m_Source.Render( context, m_Layers, m_ReferencePoints );
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

		#region IChild Members

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

		/// <summary>
		/// The source of comman messages used by the animation control
		/// </summary>
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

		#region IChild Members

		/// <summary>
		/// Called when this object has been added as a child
		/// </summary>
		public void AddedToParent( object parent )
		{
			//	Use the parent as a message source, if one hasn't been set already
			if ( MessageSource == null && parent is IMessageHub )
			{
				MessageSource = parent;
			}
		}


		/// <summary>
		/// Called when this object is removed from a parent object
		/// </summary>
		/// <param name="parent">Parent object</param>
		public void RemovedFromParent( object parent )
		{
			if ( MessageSource == parent )
			{
				MessageSource = null;
			}
		}

		#endregion

		#region IReferencePoints Members

		/// <summary>
		/// Gets a named reference point
		/// </summary>
		/// <param name="name">The reference point name to look up</param>
		/// <returns>Returns the named reference point</returns>
		/// <exception cref="System.ArgumentException">Thrown if the named reference point does not exist</exception>
		public IReferencePoint this[ string name ]
		{
			get
			{
				foreach ( ReferencePoint refPoint in m_ReferencePoints )
				{
					if ( string.Compare( refPoint.Name, name, StringComparison.InvariantCultureIgnoreCase ) == 0 )
					{
						return refPoint;
					}
				}
				throw new ArgumentException( string.Format( "Reference point \"{0}\" not found", name ), "name" );
			}
		}

		#endregion

		#region	Private stuff

		private readonly Model m_Source;
		private readonly AnimationLayer[] m_Layers;
		private readonly ReferencePoint[] m_ReferencePoints = new ReferencePoint[ ( int )ModelPart.NumParts ];


		#endregion
	}
}
