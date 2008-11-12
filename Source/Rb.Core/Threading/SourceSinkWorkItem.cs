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
	/// Errors are optionally handled by an error sink.
	/// </remarks>
	public class SourceSinkWorkItem : IWorkItem
	{
		#region Builder class

		/// <summary>
		/// SourceSinkWorkItem builder
		/// </summary>
		/// <typeparam name="SwapType">Type of value passed from source to sink</typeparam>
		public class Builder<SwapType>
		{
			#region Source setup

			/// <summary>
			/// Sets the source
			/// </summary>
			public void SetSource( FunctionDelegates.Function<SwapType, IProgressMonitor> source )
			{
				m_Source = source;
				m_SourceParams = new object[] { null };
			}

			/// <summary>
			/// Sets the source
			/// </summary>
			public void SetSource<P0>( FunctionDelegates.Function<SwapType, IProgressMonitor, P0> source, P0 p0 )
			{
				m_Source = source;
				m_SourceParams = new object[] { null, p0 };
			}

			/// <summary>
			/// Sets the source
			/// </summary>
			public void SetSource<P0, P1>( FunctionDelegates.Function<SwapType, IProgressMonitor, P0, P1> source, P0 p0, P1 p1 )
			{
				m_Source = source;
				m_SourceParams = new object[] { null, p0, p1 };
			}

			#endregion

			#region Error sink setup

			/// <summary>
			/// Sets the error sink
			/// </summary>
			/// <param name="errorSink">Error sink action</param>
			public void SetErrorSink( ActionDelegates.Action<Exception> errorSink )
			{
				m_ErrorSink = errorSink;
				m_ErrorSinkParams = new object[] { null };
			}

			/// <summary>
			/// Sets the error sink
			/// </summary>
			/// <param name="errorSink">Error sink action</param>
			/// <param name="p0">Parameter to pass to the error sink action</param>
			public void SetErrorSink<P0>( ActionDelegates.Action<Exception, P0> errorSink, P0 p0 )
			{
				m_ErrorSink = errorSink;
				m_ErrorSinkParams = new object[] { null, p0 };
			}

			#endregion

			#region Sink setup

			/// <summary>
			/// Sets the sink 
			/// </summary>
			public void SetSink( ActionDelegates.Action<SwapType> action )
			{
				m_Sink = action;
				m_SinkParams = new object[] { null };
			}

			/// <summary>
			/// Sets the sink
			/// </summary>
			public void SetSink<P0>( ActionDelegates.Action<SwapType, P0> action, P0 p0 )
			{
				m_Sink = action;
				m_SinkParams = new object[] { null, p0 };
			}

			#endregion

			/// <summary>
			/// Builds a SourceSinkWorkItem from the current builder setup
			/// </summary>
			/// <param name="name">Name of the built work item</param>
			/// <exception cref="InvalidOperationException">Thrown if source or sink are null, or sink is invalid</exception>
			public SourceSinkWorkItem Build( string name )
			{
				if ( m_Source == null )
				{
					throw new MissingFieldException( "Source cannot be null" );
				}
				if ( m_Sink == null )
				{
					throw new MissingFieldException( "Sink cannot be null" );
				}
				return new SourceSinkWorkItem( name, m_Source, m_SourceParams, m_Sink, m_SinkParams, m_ErrorSink, m_ErrorSinkParams );
			}

			#region Private Members

			private Delegate m_Source;
			private object[] m_SourceParams;
			private Delegate m_Sink;
			private object[] m_SinkParams;
			private Delegate m_ErrorSink;
			private object[] m_ErrorSinkParams = new object[] { null };

			#endregion
		}

		#endregion

		#region IWorkItem Members

		/// <summary>
		/// Work item name
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Calls the source method
		/// </summary>
		public void DoWork( IProgressMonitor monitor )
		{
			m_SourceParams[ 0 ] = monitor;
			m_SinkParams[ 0 ] = m_Source.DynamicInvoke( m_SourceParams );
		}

		/// <summary>
		/// Handles exceptions thrown by the worker method
		/// </summary>
		/// <param name="monitor">Progress monitor</param>
		/// <param name="ex">Exception thrown by the worker method</param>
		public void WorkFailed( IProgressMonitor monitor, Exception ex )
		{
			if ( m_ErrorSink != null )
			{
				m_ErrorSinkParams[ 0 ] = ex;
				m_ErrorSink.DynamicInvoke( m_ErrorSinkParams );
			}
			monitor.WorkFailed( ex );
		}

		/// <summary>
		/// Calls the sink method
		/// </summary>
		public void WorkComplete( IProgressMonitor monitor )
		{
			m_Sink.DynamicInvoke( m_SinkParams );
			monitor.WorkComplete( );
		}

		#endregion

		#region Private Members

		private readonly Delegate m_Source;
		private readonly object[] m_SourceParams;
		private readonly Delegate m_Sink;
		private readonly object[] m_SinkParams;
		private readonly Delegate m_ErrorSink;
		private readonly object[] m_ErrorSinkParams;
		private readonly string m_Name;

		/// <summary>
		/// Sets up this work item
		/// </summary>
		private SourceSinkWorkItem( string name, Delegate source, object[] sourceParams, Delegate sink, object[] sinkParams, Delegate errorSink, object[] errorSinkParams )
		{
			m_Name = name;
			m_Source = source;
			m_SourceParams = sourceParams;
			m_Sink = sink;
			m_SinkParams = sinkParams;
			m_ErrorSink = errorSink;
			m_ErrorSinkParams = errorSinkParams;
		}
		
		#endregion

	}
}
