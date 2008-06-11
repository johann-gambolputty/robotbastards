using System;
using System.ComponentModel;
using System.Drawing.Design;
using Rb.Core.Maths;
using Rb.Core.Utils;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Implements the spawn rate as a function
	/// </summary>
	public class SpawnRateFunction : SpawnRate
	{
		/// <summary>
		/// Gets/sets the function used to control the spawn rate
		/// </summary>
		[Editor( typeof( CustomUITypeEditor< IFunction1d > ), typeof( UITypeEditor ) )]
		[Description("Spawn rate function")]
		public IFunction1d Function
		{
			get { return m_Function; }
			set { m_Function = value; }
		}

		/// <summary>
		/// Gets/sets the time in seconds to complete a single period of the spawn rate function
		/// </summary>
		[Description( "Time taken for the function to complete a single period, in seconds" )]
		public float Time
		{
			get { return m_Time; }
			set
			{
				if ( value <= 0 )
				{
					throw new ArgumentException( "Time period must be > 0 seconds", "value" );
				}
				m_Time = value;
			}
		}


		#region ISpawnRate Members

		/// <summary>
		/// Returns the number of particles to spawn on this update step
		/// </summary>
		public override int GetNumberOfParticlesToSpawn( float updateTime )
		{
			if ( m_Function == null )
			{
				return MinimumRate;
			}
			m_CurrentTime += updateTime;
			while ( m_CurrentTime > m_Time )
			{
				m_CurrentTime -= m_Time;
			}
			float f = m_Function.GetValue( m_CurrentTime / m_Time );
			return ( int )( MinimumRate + ( f * ( MaximumRate - MinimumRate ) ) );
		}

		#endregion

		#region Private Members

		private float m_CurrentTime;
		private IFunction1d m_Function;
		private float m_Time = 1;

		#endregion
	}
}
