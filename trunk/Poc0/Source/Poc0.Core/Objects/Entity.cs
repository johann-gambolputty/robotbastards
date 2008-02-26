using System;
using Poc0.Core.Environment;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.World;
using Rb.World.Entities;
using Rb.World.Services;
using Component=Rb.Core.Components.Component;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Entity
	/// </summary>
	[Serializable]
	public class Entity : Component, IMoveable, INamed, ISceneObject
	{
		#region Construction

		/// <summary>
		/// Creates a standard entity object. Add it to the scene to get it to render and stuff
		/// </summary>
		public static Entity Create( string name, Point3 pos, float facing, ISource graphicsSource )
		{
			Entity entity = new Entity( );
			entity.Name = name;
			entity.Position = pos;
			entity.Angle = facing;
			entity.AddChild( new EntityGraphics( graphicsSource ) );
			entity.AddChild( new Body( ) );
			return entity;
		}

		#endregion

		#region IPlaceable Members

		/// <summary>
		/// Event, raised when Position is changed
		/// </summary>
		public event PositionChangedDelegate PositionChanged;

		/// <summary>
		/// Object's position
		/// </summary>
		public Point3 Position
		{
			get { return m_Travel.CurrentPosition; }
			set
			{
				if ( PositionChanged == null )
				{
					m_Travel.Set( value );
				}
				else
				{
					Point3 oldPos = m_Travel.CurrentPosition;
					m_Travel.Set( value );
					PositionChanged( this, oldPos, m_Travel.StartPosition );
				}
			}
		}

		/// <summary>
		/// Gets the 
		/// </summary>
		public Matrix44 Frame
		{
			get { return m_Frame; }
		}

		#endregion

		#region IMoveable members

		/// <summary>
		/// Object's position and orientation over time
		/// </summary>
		public Frame3Interpolator Travel
		{
			get { return m_Travel; }
		}

		#endregion

		#region INamed Members

		/// <summary>
		/// Name of the entity
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion
		
		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
        public virtual void AddedToScene( Scene scene )
        {
			m_Scene = scene;
			IUpdateService updates = scene.GetService< IUpdateService >( );
			if ( updates == null )
			{
				throw new InvalidOperationException( "Expected IUpdateService to be present in scene" );
			}

			//	Subscribe to the update clock
			updates[ "updateClock" ].Subscribe( Update );
        }

		/// <summary>
		/// Called when this object is removed from the specified scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public virtual void RemovedFromScene( Scene scene )
		{
			m_Scene = null;

			//	Unsubscribe from the update clock
			scene.GetService< IUpdateService >( )[ "updateClock" ].Unsubscribe( Update );
		}

        #endregion

		#region Command handlers

		/// <summary>
		/// Handles a movement request
		/// </summary>
		/// <param name="request">Movement request</param>
		[Dispatch]
		public void HandleMovementRequest( MovementXzRequest request )
		{
			Environment.Environment env = m_Scene.Objects.GetFirstOfType< Environment.Environment >( );
			IEnvironmentCollisions collisions = env.Collisions;

			Vector3 move = new Vector3( request.DeltaX, 0, request.DeltaZ );
			Collision col = collisions.CheckMovement( Travel.EndPosition, move );
			if ( col == null )
			{
				Travel.EndPosition += move;
			}
			else
			{
				//	Slide...
				Point3 stopPt = col.CollisionPoint + col.CollisionNormal * 0.01f;
				Point3 slidePt = stopPt;
				slidePt += col.CollisionNormal * 0.01f; // Push slightly away from the wall

				Vector3 perpCollisionNormal = new Vector3( col.CollisionNormal.Z, col.CollisionNormal.Y, -col.CollisionNormal.X );

				float dp = move.MakeNormal( ).Dot( perpCollisionNormal );
				float mod = ( dp < 0 ) ? -Functions.Pow( -dp, 0.2f ) : Functions.Pow( dp, 0.5f );
				slidePt += perpCollisionNormal * ( move.Length * ( 1 - col.T ) * mod );

				if ( !collisions.IsPointInObstacle( slidePt ) )
				{
					Travel.EndPosition = slidePt;
				}
				else
				{
					Travel.EndPosition = stopPt;
				}
			}
		}

		/// <summary>
		/// Handles a turn request
		/// </summary>
		/// <param name="request">Turn request</param>
		[Dispatch]
		public void HandleTurnRequest( TurnRequest request )
		{
			Travel.EndAngle = Utils.Wrap( Travel.EndAngle + request.Rotation, 0, Constants.TwoPi );
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Updates the entity
		/// </summary>
		protected virtual void Update( Clock updateClock )
		{
			m_Travel.Step( updateClock.CurrentTickTime );
		}

		#endregion
		
		#region IOriented Members

		/// <summary>
		/// Gets/sets the angle of this object
		/// </summary>
		public float Angle
		{
			get { return m_Travel.CurrentAngle; }
			set { m_Travel.Set( value ); }
		}

		#endregion

		#region Private members

		private Scene m_Scene;
		private readonly Matrix44 m_Frame = new Matrix44( );
		private readonly Frame3Interpolator m_Travel = new Frame3Interpolator( );
		private string m_Name = "Bob";

		#endregion

	}
}
