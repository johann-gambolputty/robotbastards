using System;
using System.Collections;

namespace RbEngine.Scene
{
	/// <summary>
	/// For scene objects that update on a regular basis, e.g. for physics, rendering, AI, etc.
	/// </summary>
	public class Clock
	{
		/*
		/// <summary>
		/// Sets up the clock
		/// </summary>
		/// <param name="tickTime">Time between clock updates</param>
		public Clock( float tickTime )
		{
			TickTime = tickTime;
		}

		/// <summary>
		/// Pauses or unpauses the clock
		/// </summary>
		public bool		Pause
		{
			get
			{
			}
			set
			{
			}
		}

		/// <summary>
		/// Accessor for the time between ticks
		/// </summary>
		public float	TickTime
		{
			get
			{
			}
			set
			{
			}
		}

		/// <summary>
		/// Delegate for the Tick event
		/// </summary>
		public delegate void			TickDelegate( Clock clock );

		/// <summary>
		/// Adds a subscriber
		/// </summary>
		/// <param name="tick"> Subscriber tick delegate </param>
		public virtual void				Subscribe( TickDelegate tick )
		{
			m_Tick += tick;
		}

		/// <summary>
		/// Removes a subscriber
		/// </summary>
		/// <param name="tick"> Subscriber tick delegate </param>
		public virtual void				Unsubscribe( TickDelegate tick )
		{
			m_Tick -= tick;
		}

		/// <summary>
		/// Calls the Tick() event
		/// </summary>
		public void						Update( )
		{
			if ( m_Tick != null )
			{
				m_Tick( );
			}
		}

		/// <summary>
		/// Event, invoked Update() is called
		/// </summary>
		private event TickDelegate		m_Tick;

		*/
	}
}
