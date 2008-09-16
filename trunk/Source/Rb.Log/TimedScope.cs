using System;
using System.Runtime.InteropServices;

namespace Rb.Log
{
	/// <summary>
	/// Timed scope writes timing information on construction and disposal
	/// </summary>
	/// <remarks>
	/// Use <see cref="StaticTag{T}.EnterTimedScope"/>
	/// Usage:
	/// <code>
	/// using ( AppLog.EnterTimedScope( "comments" ) ) // Writes "comments" with timestamp
	/// {
	/// } // Writes "comments" with timestamp
	/// </code>
	/// </remarks>
	public class TimedScope : IDisposable
	{
		/// <summary>
		/// Sets up the message written on scope entry and exit
		/// </summary>
		/// <param name="source">Source to write to</param>
		/// <param name="scopeName">Name of the scope (e.g. method name)</param>
		internal TimedScope( Source source, string scopeName )
		{
			m_Source = source;
			m_ScopeName = scopeName;

			QueryPerformanceCounter( out m_EntryTime );
			m_Source.Write( 1, "Entering \"{0}\"", m_ScopeName );
		}


		#region IDisposable Members

		/// <summary>
		/// Disposes of this object. Writes the exit string
		/// </summary>
		public void Dispose( )
		{
			long curTime;
			QueryPerformanceCounter( out curTime );
			double seconds = ( curTime - m_EntryTime ) * s_RcpTimerFrequency;
			m_Source.Write( 1, "Exiting \"{0}\" after {1:G6} seconds", m_ScopeName, seconds );
		}

		#endregion

		#region Private Members

		#region PInvoke

		[DllImport( "Kernel32.dll" )]
		private static extern bool QueryPerformanceCounter( out long lpPerformanceCount );

		[DllImport( "Kernel32.dll" )]
		private static extern bool QueryPerformanceFrequency( out long lpFrequency );

		#endregion

		private readonly Source m_Source;
		private readonly string m_ScopeName;
		private readonly long m_EntryTime;
		private readonly static double s_RcpTimerFrequency = GetRcpTimerFrequency( );

		/// <summary>
		/// Returns the reciprocal of the tick frequency (used for converting ticks into seconds)
		/// </summary>
		private static double GetRcpTimerFrequency( )
		{
			long freq;
			QueryPerformanceFrequency( out freq );
			return 1.0f / ( double )freq;
		}

		#endregion
	}
}
