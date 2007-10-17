using System;
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
	public class Entity : Component, IMoveable, ITurnable, INamed, ISceneObject
	{
		#region IHasPosition Members

		/// <summary>
		/// Event, raised when Position is changed
		/// </summary>
		public event PositionChangedDelegate PositionChanged;

		/// <summary>
		/// Object's position
		/// </summary>
		public Point3 Position
		{
			get { return m_Travel.Current; }
			set
			{
				if ( PositionChanged == null )
				{
					m_Travel.Set( value );
				}
				else
				{
					Point3 oldPos = m_Travel.Current;
					m_Travel.Set( value );
					PositionChanged( this, oldPos, m_Travel.Start );
				}
			}
		}

		#endregion

		#region IMoveable members

		/// <summary>
		/// Object's position over time
		/// </summary>
		public Point3Interpolator Travel
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
			Travel.End += new Vector3( request.DeltaX, 0, request.DeltaZ );
		}

		/// <summary>
		/// Handles a turn request
		/// </summary>
		/// <param name="request">Turn request</param>
		[Dispatch]
		public void HandleMovementRequest( TurnRequest request )
		{
			Turn.End = Utils.Wrap( Turn.End + request.Rotation, 0, Constants.TwoPi );
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Updates the entity
		/// </summary>
		protected virtual void Update( Clock updateClock )
		{
			m_Travel.Step( updateClock.CurrentTickTime );
			m_Turn.Step( updateClock.CurrentTickTime );
		}

		#endregion
		
		#region IOriented Members

		/// <summary>
		/// Gets/sets the angle of this object
		/// </summary>
		public float Angle
		{
			get { return m_Turn.Current; }
			set { m_Turn.Set( value ); }
		}

		/// <summary>
		/// Forward vector
		/// </summary>
		public Vector3 Forward
		{
			get
			{
				return Vector3.ZAxis;
			}
		}

		/// <summary>
		/// Right vector
		/// </summary>
		public Vector3 Right
		{
			get
			{
				return Vector3.XAxis;
			}
		}

		/// <summary>
		/// Up vector
		/// </summary>
		public Vector3 Up
		{
			get
			{
				return Vector3.YAxis;
			}
		}

		#endregion

		#region ITurnable Members

		/// <summary>
		/// Gets the orientation interpolator
		/// </summary>
		public FloatInterpolator Turn
		{
			get { return m_Turn; }
		}

		#endregion

		#region Private members

		private readonly FloatInterpolator m_Turn = new FloatInterpolator( );
		private readonly Point3Interpolator m_Travel = new Point3Interpolator( );
		private string m_Name = "Bob";

		#endregion

	}
}
