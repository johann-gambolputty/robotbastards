using System;
using System.Collections.Generic;
using System.Threading;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Profiling section
	/// </summary>
	public class ProfileSection
	{
		#region Guard Class

		/// <summary>
		/// Profile section Begin/End guard class
		/// </summary>
		/// <remarks>
		/// Use like this:
		/// <code>
		/// using ( ProfileSection.Guard guard = new ProfileSection.Guard( MyProfileSections.Ai ) )
		/// {
		///		//	Do AI noodles
		/// }
		/// </code>
		/// </remarks>
		public class Guard : IDisposable
		{
			/// <summary>
			/// Creates the guard. Starts timing
			/// </summary>
			/// <param name="section">Section to start timing</param>
			public Guard( ProfileSection section )
			{
				m_Section = section;
				section.Begin( );
			}

			/// <summary>
			/// Stops timing the section
			/// </summary>
			public void Dispose( )
			{
				m_Section.End( );
			}

			#region Private Members

			private readonly ProfileSection m_Section;

			#endregion
		}

		#endregion

		#region Construction

		/// <summary>
		/// Profiling section default constructor. No name or parent section.
		/// </summary>
		public ProfileSection( ) :
			this( null, "" )
		{
		}

		/// <summary>
		/// Sets up this profiling section
		/// </summary>
		/// <param name="name">Profiling section name</param>
		public ProfileSection( string name ) :
			this( null, name )
		{
		}

		/// <summary>
		/// Sets up this profiling section
		/// </summary>
		/// <param name="parent">Profiling section parent</param>
		/// <param name="name">Profiling section name</param>
		public ProfileSection( ProfileSection parent, string name )
		{
			m_Name = name;
			Parent = parent;

			m_Samples = new Sample[ 512 ];
			m_Samples[ 0 ] = new Sample( );
			m_NumSamples = 1;
			m_TotalSampleCount = 1;
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Event, raised when Reset() is called
		/// </summary>
		public event Action<ProfileSection> OnReset;

		/// <summary>
		/// Gets the parent profiling section
		/// </summary>
		public ProfileSection Parent
		{
			get { return m_Parent; }
			set
			{
				if ( m_Parent != null )
				{
					m_Parent.m_SubSections.Remove( this );
				}
				m_Parent = value;
				if ( m_Parent != null )
				{
					m_Parent.m_SubSections.Add( this );
				}
			}
		}

		/// <summary>
		/// Gets/sets the name of this section
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		/// <summary>
		/// Gets the current sample
		/// </summary>
		public Sample CurrentSample
		{
			get { return m_Samples[ m_CurSample ]; }
		}

		/// <summary>
		/// Returns the total number of samples taken since the profiling started in this section
		/// </summary>
		public int TotalSampleCount
		{
			get { return m_TotalSampleCount; }
		}

		/// <summary>
		/// Gets the total number of samples available
		/// </summary>
		public int NumSamples
		{
			get { return m_NumSamples; }
		}

		/// <summary>
		/// Gets the average recorded time
		/// </summary>
		public long AverageTime
		{
			get
			{
				double avg = 0;
				foreach ( Sample sample in Samples )
				{
					avg += sample.TotalTime / ( double )NumSamples;
				}
				return ( long )avg;
			}
		}

		/// <summary>
		/// Gets the highest recorded time
		/// </summary>
		public long MaxTime
		{
			get
			{
				long max = 0;
				foreach ( Sample sample in Samples )
				{
					max = sample.TotalTime > max ? sample.TotalTime : max;
				}
				return max;
			}
		}

		/// <summary>
		/// Gets the list of sample, from the current backwards
		/// </summary>
		public IEnumerable<Sample> Samples
		{
			get
			{
				int sampleIndex = m_CurSample;
				for ( int count = 0; count < m_NumSamples; ++count )
				{
					yield return m_Samples[ sampleIndex ];
					if ( --sampleIndex < 0 )
					{
						sampleIndex = m_Samples.Length - 1;
					}
				}
			}
		}

		/// <summary>
		/// Gets the sub profile sections
		/// </summary>
		public IEnumerable<ProfileSection> SubSections
		{
			get { return m_SubSections; }
		}

		/// <summary>
		/// Creates a guard object for this profile
		/// </summary>
		public Guard CreateGuard( )
		{
			return new Guard( this );
		}

		/// <summary>
		/// Starts profiling this section (thread-safe)
		/// </summary>
		public void Begin( )
		{
			StartTime = TinyTime.CurrentTime;
		}

		/// <summary>
		/// Stops profiling this section (thread-safe)
		/// </summary>
		public void End( )
		{
			long end = TinyTime.CurrentTime;
			AddPeriod( end - StartTime );
		}
		
		/// <summary>
		/// Adds a time period spent in this section
		/// </summary>
		/// <param name="periodTicks">Time period, in tiny time ticks</param>
		public void AddPeriod( long periodTicks )
		{
			lock ( m_Lock )
			{
				CurrentSample.Add( periodTicks );
				if ( m_Parent != null )
				{
					m_Parent.AddPeriod( periodTicks );
				}
			}
		}

		/// <summary>
		/// Resets this section, and all child sections
		/// </summary>
		public void Reset( )
		{
			lock ( m_Lock )
			{
				if ( CurrentSample.TotalTime > m_MaxTime )
				{
					m_MaxTime = CurrentSample.TotalTime;
				}

				++m_TotalSampleCount;
				m_CurSample = ( m_CurSample + 1 ) % m_Samples.Length;
				if ( m_Samples[ m_CurSample ] == null )
				{
					m_Samples[ m_CurSample ] = new Sample( );
					++m_NumSamples;
				}
				else
				{
					m_Samples[ m_CurSample ].Set( 0, 0 );
				}

				foreach ( ProfileSection subSection in m_SubSections )
				{
					subSection.Reset( );
				}

				if ( OnReset != null )
				{
					OnReset( this );
				}
			}
		}

		#endregion

		#region Sample Public Class

		public class Sample
		{
			public void Set( long time, int count )
			{
				m_Time = time;
				m_Count = count;
			}

			public void Add( long time )
			{
				m_Time += time;
				++m_Count;
			}

			public long TotalTime
			{
				get { return m_Time; }
			}

			public int NumberOfEntries
			{
				get { return m_Count; }
			}

			#region Private Members

			private long m_Time;
			private int m_Count;

			#endregion
		}

		#endregion

		#region Private Members
		
		private static readonly object			m_Lock = new object( );
		private long							m_MaxTime;
		private readonly LocalDataStoreSlot		m_StartTime = Thread.AllocateDataSlot( );
		private string							m_Name;
		private readonly List< ProfileSection > m_SubSections = new List< ProfileSection >( );
		private readonly Sample[]				m_Samples;
		private int								m_CurSample;
		private int								m_NumSamples;
		private int								m_TotalSampleCount;
		private ProfileSection					m_Parent;

		private long StartTime
		{
			get { return ( long )Thread.GetData( m_StartTime ); }
			set { Thread.SetData( m_StartTime, value ); }
		}

		#endregion
	}
}