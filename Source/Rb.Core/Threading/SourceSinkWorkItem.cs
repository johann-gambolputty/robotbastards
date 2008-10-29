using System;
using Rb.Core.Utils;

namespace Rb.Core.Threading
{
	/// <summary>
	/// A source-sink work item.
	/// </summary>
	/// <remarks>
	/// Defined as a pair of delegates. The first (the "source") does work asynchronously, the second (the "sink")
	/// processes the results of the source.
	/// </remarks>
	public class SourceSinkWorkItem : IWorkItem
	{
		/// <summary>
		/// SourceSinkWorkItem builder
		/// </summary>
		public class Builder
		{
			#region Source setup

			/// <summary>
			/// Sets the source
			/// </summary>
			public void SetSource<R>( FunctionDelegates.Function<R> source )
			{
				m_Source = source;
			}

			/// <summary>
			/// Sets the source
			/// </summary>
			public void SetSource<R, P0>( FunctionDelegates.Function<R, P0> source, P0 p0 )
			{
				m_Source = source;
				m_SourceParams = new object[] { p0 };
			}

			/// <summary>
			/// Sets the source
			/// </summary>
			public void SetSource<R, P0, P1>( FunctionDelegates.Function<R, P0, P1> source, P0 p0, P1 p1 )
			{
				m_Source = source;
				m_SourceParams = new object[] { p0, p1 };
			}

			#endregion

			#region Sink setup

			/// <summary>
			/// Sets the sink
			/// </summary>
			public void SetSink<P0>( ActionDelegates.Action<P0> action )
			{
				m_Sink = action;
				m_SinkParams = new object[ 1 ];
			}

			/// <summary>
			/// Sets the sink
			/// </summary>
			public void SetSink<P0, P1>( ActionDelegates.Action<P0, P1> action, P0 p0 )
			{
				m_Sink = action;
				m_SinkParams = new object[ 2 ] { p0, null };
			}

			#endregion

			/// <summary>
			/// Builds a SourceSinkWorkItem from the current builder setup
			/// </summary>
			/// <exception cref="InvalidOperationException">Thrown if source or sink are null, or sink is invalid</exception>
			public SourceSinkWorkItem Build( )
			{
				if ( m_Source == null )
				{
					throw new InvalidOperationException( "Source cannot be null" );
				}
				if ( m_Sink == null )
				{
					throw new InvalidOperationException( "Sink cannot be null" );
				}
				return new SourceSinkWorkItem( m_Source, m_SourceParams, m_Sink, m_SinkParams );
			}

			#region Private Members

			private Delegate m_Source;
			private object[] m_SourceParams;
			private Delegate m_Sink;
			private object[] m_SinkParams;

			#endregion
		}


		#region IWorkItem Members

		/// <summary>
		/// Calls the source method
		/// </summary>
		public void DoWork( )
		{
			object sourceResult = m_Source.DynamicInvoke( m_SourceParams );
			m_SinkParams[ m_SinkParams.Length - 1 ] = sourceResult;
		}

		/// <summary>
		/// Calls the sink method
		/// </summary>
		public void WorkComplete( )
		{
			m_Sink.DynamicInvoke( m_SinkParams );
		}

		#endregion
		#region Private Members

		private Delegate m_Source;
		private object[] m_SourceParams;
		private Delegate m_Sink;
		private object[] m_SinkParams;

		/// <summary>
		/// Sets up this work item
		/// </summary>
		private SourceSinkWorkItem( Delegate source, object[] sourceParams, Delegate sink, object[] sinkParams )
		{
			m_Source = source;
			m_SourceParams = sourceParams;
			m_Sink = sink;
			m_SinkParams = sinkParams;
		}
		
		#endregion

	}
}
