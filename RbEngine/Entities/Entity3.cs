using System;
using System.Collections;
using RbEngine.Maths;
using RbEngine.Rendering;

namespace RbEngine.Entities
{
	/*
	 * Degrees of freedom:
	 * An entity can be characterised by any number of degrees of freedom. This characterisation is separate from their frame (transformation)
	 * e.g. might have a train constrained to a spline = 1 degree of freedom, despite having a varying 3d position and orientation. Any movement
	 * requests and orientation requests should be specified in terms of degrees of freedom - or some mapping (to avoid gimbal lock in orientation
	 * requests).
	 * I guess the only thing is to have separate interfaces and classes for different DOF setups:
	 * MovementXRequest (1dof)
	 * MovementXyRequest (2dof - xy planar movement (e.g. sprites))
	 * MovementXzRequest (2dof - xz planar movement)
	 * MovementXyzRequest (3dof - full 3d movement)
	 * OrientationTurnRequest (turn around the entity axis)
	 * OrientationXyzwRequest (3dof - specified as quaternion)
	 *
	 * Maybe the entity implementation should provide it's own Commit(), invoked the movement/orientation request, that interprets the movement.
	 * It would be nice to have a generic enough representation such that a MovementXzRequest or MovementXyzRequest can be added to a request chain,
	 * and work for the same entity type.
	 * Setup should probably specify: Chain, Handlers, Commiter
	 *
	*/

	//	TODO: Entities should not be renderable - they should have child nodes that handle all the rendering shit

	/// <summary>
	/// An entity is any object that can be controlled and rendered. Entity3 represents an entity in 3 dimensional space
	/// </summary>
	public class Entity3 : Entity, Scene.ISceneRenderable, Scene.ISceneObject
	{
		#region Entity frame

		/// <summary>
		/// The agent position
		/// </summary>
		public Point3Interpolator Position
		{
			get
			{
				return m_Position;
			}
			set
			{
				m_Position = value;
			}
		}

		public Point3	NextPosition
		{
			get
			{
				return m_Position.Next;
			}
			set
			{
				m_Position.Next = value;
			}
		}

		/// <summary>
		/// The agent facing vector
		/// </summary>
		public Vector3 Facing
		{
			set
			{
				m_ZAxis = value;
			}
			get
			{
				return m_ZAxis;
			}
		}

		/// <summary>
		/// The agent left-hand vector
		/// </summary>
		public Vector3 Left
		{
			set
			{
				m_XAxis = value;
			}
			get
			{
				return m_XAxis;
			}
		}

		/// <summary>
		/// The agent right-hand vector
		/// </summary>
		public Vector3 Right
		{
			set
			{
				m_XAxis = value * -1.0f;
			}
			get
			{
				return m_XAxis * -1.0f;
			}
		}

		/// <summary>
		/// The agent up vector
		/// </summary>
		public Vector3 Up
		{
			set
			{
				m_YAxis = value;
			}
			get
			{
				return m_YAxis;
			}
		}

		/// <summary>
		/// The agent down vector
		/// </summary>
		public Vector3 Down
		{
			set
			{
				m_YAxis = value * -1.0f;
			}
			get
			{
				return m_YAxis * -1.0f;
			}
		}

		#endregion

		#region	Private stuff

		private ApplianceList				m_PreRenders	= new ApplianceList( );
		private Maths.Point3Interpolator	m_Position		= new Maths.Point3Interpolator( );
		private Vector3						m_XAxis			= Vector3.XAxis;
		private Vector3						m_YAxis			= Vector3.YAxis;
		private Vector3						m_ZAxis			= Vector3.ZAxis;
		private IRender						m_Graphics;

		#endregion

		#region	Graphics

		/// <summary>
		/// Entity graphics
		/// </summary>
		public IRender	Graphics
		{
			get
			{
				return m_Graphics;
			}
			set
			{
				m_Graphics = value;
			}
		}

		#endregion

		#region ISceneRenderable Members

		/// <summary>
		/// Gets the pre-render list for this entity
		/// </summary>
		public ApplianceList	PreRenderList
		{
			get
			{
				return m_PreRenders;
			}
		}

		/// <summary>
		/// Renders this entity
		/// </summary>
		public void Render( long renderTime )
		{
			m_PreRenders.Apply( );

			//	Get the interpolated position of the entity
			float t = ( float )( renderTime - m_Position.LastStepTime ) / ( float )m_Position.LastStepInterval;
			Point3 curPos = m_Position.Get( t );

			//	TODO: Get the interpolated rotation of the entity

			//	Push the entity transform
			Maths.Matrix44 mat = new Maths.Matrix44( curPos, Left, Up, Facing );

			Renderer.Inst.PushTransform( Transform.LocalToView, mat );

			//	TODO: Render associated IRender object
			if ( m_Graphics != null )
			{
				m_Graphics.Render( );
			}

			//	Pop the entity transform
			Renderer.Inst.PopTransform( Transform.LocalToView );
		}

		#endregion

		#region	Entity message handling

		/// <summary>
		/// Handles a movement request message
		/// </summary>
		/// <param name="movement">Movement request message</param>
		public override void				HandleMovement( MovementRequest movement )
		{
			MovementXzRequest moveXz = movement as MovementXzRequest;
			if ( moveXz != null )
			{
				Vector3 moveVec = new Vector3( moveXz.MoveX, 0, moveXz.MoveZ );
				if ( moveXz.Local )
				{
					//	Local movement relative to current frame
					NextPosition += moveVec * Facing;
					NextPosition += moveVec * Left;
				}
				else
				{
					NextPosition += moveVec;
				}
			}
			base.HandleMovement( movement );
		}


		#endregion

		public void		Update( Scene.Clock updateClock )
		{
			//	Entity frame update:
			//		AI controller requests new position
			//		Animation may override
			//		Physics may override
			//		Final position is committed into entity
			//
			//	Design decision:
			//		AI, animation and physics will probably all work at different time steps, so there should be some sort of pending
			//		frame queue that successive services get the chance to modify, before the frame is finally committed to the entity.
			//
			//	Example 0:
			//		Player controller registers "Forward" action - puts in frame change request f0 into the animation controller frame change queue
			//		Animation controller is in the middle of a "Jump" action, so overrules the request. Instead, adds its own frame change request to the physics controller
			//		Physics controller registers collision with an object, so overrules the animation controller's request, adding its own frame change request
			//		Final commiting controller commits the movement change to the agent
			//
			//	Notes on example 0:
			//		Order is important here, which suggests an explicit chain of handlers, instead of a message passing affair
			//		Timing is also important. If a handler on the chain was running slower than a handler further up, then it might
			//		have to handle multiple requests. There should be some sort of conflict resolution in these cases
			//
			//	ActionRequestChain (IActionRequestHandler)
			//		AiController - AnimationController - PhysicsController
			//
			//	interface IActionRequestHandler
			//	{
			//		public ActionRequest Pending { get; set; };
			//	};
			//

			m_Position.Step( updateClock.CurrentTickTime );
		}

		#region ISceneObject Members

		/// <summary>
		/// Called when this entity is added to a scene
		/// </summary>
		/// <param name="db">Scene database</param>
		public void AddedToScene( RbEngine.Scene.SceneDb db )
		{
			//	Add the entity to the render manager
			db.Rendering.AddObject( this );

			//	Subscribe to the update clock
			Scene.Clock updateClock = db.GetNamedClock( "updateClock" );
            updateClock.Subscribe( new RbEngine.Scene.Clock.TickDelegate( Update ) );
		}

		/// <summary>
		/// Called when this entity is removed from a scene
		/// </summary>
		/// <param name="db">Scene database</param>
		public void RemovedFromScene( RbEngine.Scene.SceneDb db )
		{
			//	Remove the entity from the render manager
			db.Rendering.RemoveObject( this );
		}

		#endregion
	}
}
