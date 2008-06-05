using System.ComponentModel;
using Poc1.Particles.Interfaces;

namespace Poc1.Particles.Classes
{
	/// <summary>
	/// Implements the spawn rate as a function
	/// </summary>
	public class SpawnRateFunction : ISpawnRate
	{
		/// <summary>
		/// Table of samples used to define a function
		/// </summary>
		public class SampleTable
		{
			public SampleTable( )
			{
				m_Samples = new float[ 256 ];
			}

			public float this[ int index ]
			{
				get { return m_Samples[ index ]; }
				set { m_Samples[ index ] = value; }
			}

			public int Length
			{
				get { return m_Samples.Length; }
			}

			private readonly float[] m_Samples;
		}

		public SampleTable Samples
		{
			get { return m_Samples; }
		}

		[Description( "Time taken for the function to complete a single period" )]
		public float Time
		{
			get { return m_Time; }
			set { m_Time = value; }
		}

		#region ISpawnRate Members

		/// <summary>
		/// Returns the number of particles to spawn on this update step
		/// </summary>
		public int GetNumberOfParticlesToSpawn( )
		{
			return 0;
		}

		#endregion

		#region Private Members

		private float m_Time;
		private readonly SampleTable m_Samples = new SampleTable( );

		#endregion
	}
}
