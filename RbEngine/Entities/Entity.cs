using System;
using RbEngine.Maths;

namespace RbEngine.Entities
{
	/// <summary>
	/// Vector3Step is used to store vectors that update over time. 
	/// </summary>
	/// <remarks>
	/// Vector3Step is principally used for rendering, when the rendering rate is faster than the update rate
	/// (the renderer would use GetIntermediate() to get intermediate positions while waiting for the next update).
	/// </remarks>
	public class Vector3Step
	{
		/// <summary>
		/// Start of the step
		/// </summary>
		public Vector3	Start
		{
			get
			{
				return m_Start;
			}
		}

		/// <summary>
		/// End of the step
		/// </summary>
		public Vector3	End
		{
			get
			{
				return m_End;
			}
		}

		/// <summary>
		/// Sets the new end point, copying the old end point to the start point
		/// </summary>
		/// <param name="newEnd">The new end point</param>
		public void		Update( Vector3 newEnd )
		{
			m_Start = m_End;
			m_End = newEnd;
		}

		/// <summary>
		/// Gets an intermediate vector on the step
		/// </summary>
		/// <param name="t">Fraction between 0 and 1. 0 returns Start, 1 returns End</param>
		/// <returns>Returns the intermediate vector at time t</returns>
		public Vector3	GetIntermediate( float t )
		{
			//	Equivalent to ( Start + ( End - Start ) * t ). Looks a bit nastier, but is more effecient (only one
			//	vector is created, instead of 3)
			Vector3 result = new Vector3( m_End );
			result.IpSubtract( m_Start );
			result.IpMultiplyByValue( t );
			result.IpAdd( m_Start );
			return result;
		}
	}

	/// <summary>
	/// An entity is any object that can be controlled and rendered
	/// </summary>
	public class Entity : Components.Component, IEntity, Scene.ISceneEvents
	{

		#region IEntity Members

		/// <summary>
		/// The agent position
		/// </summary>
		public Vector3 Position
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

		#endregion

		#region	Private stuff

		private Vector3	m_LastPosition	= new Vector3( );
		private Vector3	m_LastFacing	= new Vector3( 0, 0, 1 );

		private Vector3	m_Position		= new Vector3( );
		private Vector3	m_Facing		= new Vector3( 0, 0, 1 );

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

		#region ISceneEvents Members

		//	TODO: Should the update clock be in the scene? Sounds like it should be in the engine, or something
		//	Anyway, the entity should subscribe to it, and update the render position

		public void AddedToScene(RbEngine.Scene.SceneDb db)
		{
		}

		public void RemovedFromScene(RbEngine.Scene.SceneDb db)
		{
		}

		#endregion
	}
}
