using System;
using System.Collections;

namespace Rb.Core.Utils
{
	/// <summary>
	/// For objects that update on a regular basis, e.g. for physics, rendering, AI, etc.
	/// </summary>
	public class Clock
	{
		/// <summary>
		/// Sets up the clock
		/// </summary>
		/// <param name="tickTime">Time in milliseconds between clock updates</param>
		public Clock( string name, int tickTime )
		{
			m_Name				= name;
			m_Timer				= new System.Windows.Forms.Timer( );
			m_Timer.Interval	= tickTime;
			m_Timer.Tick		+= new EventHandler( Update );
			m_Timer.Enabled		= true;
		}

		/// <summary>
		/// Returns the time since the clock started, in TinyTime ticks
		/// </summary>
		public long	CurrentTickTime
		{
			get { return m_Time; }
		}

		/// <summary>
		/// Pauses or unpauses the clock
		/// </summary>
		public bool		Pause
		{
			get { return m_Timer.Enabled; }
			set { m_Timer.Enabled = !value; }
		}

		/// <summary>
		/// Accessor for the time between ticks (in milliseconds)
		/// </summary>
		public int		TickTime
		{
			get { return ( int )m_Interval; }
			set { m_Timer.Interval = value; }
		}

		/// <summary>
		/// The clock name
		/// </summary>
		public string	Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Delegate for the Tick event
		/// </summary>
		public delegate void TickDelegate( Clock clock );

		/// <summary>
		/// Adds a subscriber
		/// </summary>
		/// <param name="tick"> Subscriber tick delegate </param>
		public virtual void Subscribe( TickDelegate tick )
		{
			m_Tick += tick;
		}

		/// <summary>
		/// Removes a subscriber
		/// </summary>
		/// <param name="tick"> Subscriber tick delegate </param>
		public virtual void Unsubscribe( TickDelegate tick )
		{
			m_Tick -= tick;
		}

		/// <summary>
		/// Calls the Tick() event
		/// </summary>
		private void Update( object sender, EventArgs args )
		{
			long curTime = TinyTime.CurrentTime;
			m_Interval = ( int )( curTime - m_Time );
			m_Time = curTime;
			if ( m_Tick != null )
			{
				m_Tick( this );
			}
        }

        #region Private stuff

        /// <summary>
		/// Event, invoked Update() is called
		/// </summary>
		private event TickDelegate m_Tick;

		/// <summary>
		/// Clock timer (must be forms timer, to keep it in the main thread)
		/// </summary>
		private System.Windows.Forms.Timer m_Timer;

		/// <summary>
		/// Timer name
		/// </summary>
		private string m_Name;

		/// <summary>
		/// Time of the last tick
		/// </summary>
		private long m_Time;

		private int m_Interval;

        #endregion

	}
}
