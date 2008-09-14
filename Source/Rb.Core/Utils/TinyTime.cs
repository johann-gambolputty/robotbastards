using System.Runtime.InteropServices;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Provides fine grain timing, and a shared start time for Scene.Clock objects
	/// </summary>
	public class TinyTime
	{
		/// <summary>
		/// The time that the tiny timer started ticking
		/// </summary>
		public static long StartTime
		{
			get { return s_StartTime; }
		}

		/// <summary>
		/// Returns the current time, in ticks
		/// </summary>
		public static long CurrentTime
		{
			get
			{
				long time;
				QueryPerformanceCounter( out time );
				return time;
			}
		}

		/// <summary>
		/// Converts the specified time period, measured in ticks, into seconds
		/// </summary>
		/// <returns>Returns the time period in seconds</returns>
		public static double ToSeconds( long period )
		{
			return ToSeconds( 0, period );
		}

		/// <summary>
		/// Gets the time period from when the tiny clock started ticking, to the current time, in seconds
		/// </summary>
		/// <returns>Returns the time period in seconds</returns>
		public static double ToSeconds( )
		{
			return ToSeconds( StartTime, CurrentTime );
		}

		/// <summary>
		/// Gets the time period from when the tiny clock started ticking, to the current time, in milliseconds
		/// </summary>
		/// <returns>Returns the time period in seconds</returns>
		public static double ToMilliSeconds( )
		{
			return ToMilliSeconds( StartTime, CurrentTime );
		}

		/// <summary>
		/// Converts a tiny time period into seconds
		/// </summary>
		/// <param name="startTime">Start time</param>
		/// <param name="curTime">Current time</param>
		/// <returns>Returns the time period in seconds</returns>
		public static double ToSeconds( long startTime, long curTime )
		{
			return ( curTime - startTime ) * s_RcpFrequency;
		}

		/// <summary>
		/// Converts a tiny time period into milliseconds
		/// </summary>
		/// <param name="startTime">Start time</param>
		/// <param name="curTime">Current time</param>
		/// <returns>Returns the time period in milliseconds</returns>
		public static double ToMilliSeconds( long startTime, long curTime )
		{
			return ( curTime - startTime ) * s_RcpMsFrequency;
		}

		/// <summary>
		/// Returns the reciprocal of the tick frequency (used for converting ticks into seconds)
		/// </summary>
		private static double GetRcpFrequency( )
		{
			long freq;
			QueryPerformanceFrequency( out freq );
			return 1.0f / ( double )freq;
		}


		/// <summary>
		/// The start time of the tiny clock
		/// </summary>
		private static readonly long s_StartTime	= CurrentTime;

		/// <summary>
		/// Reciprocal frequency (converts to seconds)
		/// </summary>
		private static readonly double s_RcpFrequency = GetRcpFrequency();

		/// <summary>
		/// Reciprocal frequency (converts to milliseconds)
		/// </summary>
		private static readonly double s_RcpMsFrequency = GetRcpFrequency() * 1000.0f;

		[ DllImport("Kernel32.dll") ]
		private static extern bool QueryPerformanceCounter( out long lpPerformanceCount );

		[ DllImport("Kernel32.dll") ]
		private static extern bool QueryPerformanceFrequency( out long lpFrequency );

	}
}
