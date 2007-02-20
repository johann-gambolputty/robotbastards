using System;
using RbEngine.Maths;

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
	 * MovementXyzRequest (3dof - full movement)
	 * OrientationTurnRequest (turn around the entity axis)
	 * OrientationXyzwRequest (3dof - specified as quaternion)
	 *
	 * Maybe the entity implementation should provide it's own Commit(), invoked the movement/orientation request, that interprets the movement.
	 * It would be nice to have a generic enough representation such that a MovementXzRequest or MovementXyzRequest can be added to a request chain,
	 * and work for the same entity type.
	 * Setup should probably specify: Chain, Handlers, Commiter
	 *
	*/

	/// <summary>
	/// An entity is any object that can be controlled and rendered. Entity3 represents an entity in 3 dimensional space
	/// </summary>
	public class Entity3 : Components.Component
	{

		#region Entity frame

		/// <summary>
		/// The agent position
		/// </summary>
		public Point3 Position
		{
			get
			{
				return m_Position.Current;
			}
			set
			{
				m_Position.Current = value;
			}
		}

		/// <summary>
		/// The agent facing vector
		/// </summary>
		public Vector3 Facing
		{
			get
			{
				return m_Facing;
			}
			set
			{
				m_Facing = value;
			}
		}

		/// <summary>
		/// The agent left-hand vector
		/// </summary>
		public Vector3 Left
		{
			get
			{
				return m_Facing;
			}
		}

		/// <summary>
		/// The agent right-hand vector
		/// </summary>
		public Vector3 Right
		{
			get
			{
				return m_Facing;
			}
			set
			{
				m_Facing = value;
			}
		}

		/// <summary>
		/// The agent up vector
		/// </summary>
		public Vector3 Up
		{
			get
			{
				return m_Facing;
			}
			set
			{
				m_Facing = value;
			}
		}

		/// <summary>
		/// The agent down vector
		/// </summary>
		public Vector3 Down
		{
			get
			{
				return m_Facing;
			}
			set
			{
				m_Facing = value;
			}
		}

		#endregion

		#region	Private stuff

		private Maths.Point3Interpolator	m_Position	= new Maths.Point3Interpolator( );
		private Vector3						m_Facing	= Vector3.ZAxis;

		#endregion

		public void		Update( )
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
		}

	}
}
