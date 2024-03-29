using System;
using Rb.Core.Components;
using Rb.Core.Utils;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// Implementation of IBody interface
	/// </summary>
	[Serializable]
	public class Body : Component, IBody, ICloneable
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public Body( )
		{
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="src">Source body</param>
		public Body( Body src )
		{
			m_Health = src.m_Health;
			m_State = src.m_State;
		}

		#region IBody Members

		/// <summary>
		/// Gets/sets the walking speed
		/// </summary>
		public float WalkSpeed
		{
			get { return m_WalkSpeed; }
			set { m_WalkSpeed = value; }
		}

		/// <summary>
		/// Gets/sets the running speed
		/// </summary>
		public float RunSpeed
		{
			get { return m_RunSpeed; }
			set { m_RunSpeed = value; }
		}

		/// <summary>
		/// Gets/sets the turn speed, in degrees per second
		/// </summary>
		public float TurnSpeed
		{
			get { return m_TurnSpeed; }
			set { m_TurnSpeed = value; }
		}

		/// <summary>
		/// Current health
		/// </summary>
		public int Health
		{
			get { return m_Health; }
			set { m_Health = value; }
		}

		/// <summary>
		/// Gets the state of the body
		/// </summary>
		public BodyState State
		{
			get { return m_State; }
			set
			{
				BodyState oldState = m_State;
				m_State = value;
				if ( ( m_State != oldState ) && ( StateChanged != null ) )
				{
					StateChanged( this, oldState, m_State );
				}
			}
		}

		/// <summary>
		/// Event, called when this body's health changes
		/// </summary>
		public event BodyHealthChangedDelegate HealthChanged;

		/// <summary>
		/// Event, called when this body's state is changed
		/// </summary>
		public event BodyStateChangedDelegate StateChanged;

		/// <summary>
		/// Applies damage or healing to the body
		/// </summary>
		[Dispatch]
		public void ApplyHealthChange( HealthChange change )
		{
			m_Health += change.Amount;
			if ( HealthChanged != null )
			{
				HealthChanged( this, change );
			}
			if ( m_Health <= 0 )
			{
				State = BodyState.Dead;
			}
		}

		#endregion

		#region Private stuff

		private int			m_Health	= 100;
		private BodyState	m_State		= BodyState.Alive;
		private float		m_WalkSpeed	= 3.0f;
		private float		m_RunSpeed	= 5.0f;
		private float		m_TurnSpeed	= 90;

		#endregion

		#region ICloneable Members

		/// <summary>
		/// Creates a copy of this object
		/// </summary>
		/// <returns>Copy of this object</returns>
		public object Clone()
		{
			return new Body( this );
		}

		#endregion
	}
}
