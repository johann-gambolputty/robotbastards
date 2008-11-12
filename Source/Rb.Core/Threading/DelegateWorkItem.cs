using System;
using Rb.Core.Utils;

namespace Rb.Core.Threading
{
	/// <summary>
	/// Implements <see cref="IWorkItem"/> with a pair of delegates
	/// </summary>
	public class DelegateWorkItem : IWorkItem
	{
		/// <summary>
		/// DelegateWorkItem builder
		/// </summary>
		public class Builder
		{
			#region DoWork setup

			/// <summary>
			/// Sets the worker delegate
			/// </summary>
			public void SetDoWork( ActionDelegates.Action<IProgressMonitor> work )
			{
				m_Work = work;
				m_WorkParams = new object[] { null };
			}

			/// <summary>
			/// Sets the worker delegate
			/// </summary>
			public void SetDoWork<P0>( ActionDelegates.Action<IProgressMonitor, P0> work, P0 p0 )
			{
				m_Work = work;
				m_WorkParams = new object[] { null, p0 };
			}

			/// <summary>
			/// Sets the worker delegate
			/// </summary>
			public void SetDoWork<P0, P1>( ActionDelegates.Action<IProgressMonitor, P0, P1> work, P0 p0, P1 p1 )
			{
				m_Work = work;
				m_WorkParams = new object[] { null, p0, p1 };
			}

			#endregion

			#region WorkFailed setup

			/// <summary>
			/// Sets the work failed delegate
			/// </summary>
			/// <param name="workFailed">Delegate invoked when the worker delegate throws an exception</param>
			public void SetWorkFailed( ActionDelegates.Action<Exception> workFailed )
			{
				m_WorkFailed = workFailed;
				m_WorkFailedParams = new object[] { null };
			}

			/// <summary>
			/// Sets the work failed delegate
			/// </summary>
			/// <param name="workFailed">Delegate invoked when the worker delegate throws an exception</param>
			/// <param name="p0">First parameter value after the exception, passed to workFailed</param>
			public void SetWorkFailed<P0>( ActionDelegates.Action<Exception, P0> workFailed, P0 p0 )
			{
				m_WorkFailed = workFailed;
				m_WorkFailedParams = new object[] { null, p0 };
			}

			/// <summary>
			/// Sets the work failed delegate with two extra parameters
			/// </summary>
			/// <param name="workFailed">Delegate invoked when the worker delegate throws an exception</param>
			/// <param name="p0">First parameter value after the exception, passed to workFailed</param>
			/// <param name="p1">Second parameter value after the exception, passed to workFailed</param>
			public void SetWorkFailed<P0, P1>( ActionDelegates.Action<Exception, P0, P1> workFailed, P0 p0, P1 p1 )
			{
				m_WorkFailed = workFailed;
				m_WorkFailedParams = new object[] { null, p0, p1 };
			}

			#endregion

			#region WorkComplete setup

			/// <summary>
			/// Sets the work complete delegate
			/// </summary>
			public void SetWorkComplete( ActionDelegates.Action workComplete )
			{
				m_WorkComplete = workComplete;
				m_WorkCompleteParams = new object[ 0 ];
			}

			/// <summary>
			/// Sets the delegate to call when work is complete
			/// </summary>
			public void SetWorkComplete<P0>( ActionDelegates.Action<P0> workComplete, P0 p0 )
			{
				m_WorkComplete = workComplete;
				m_WorkCompleteParams = new object[] { p0 };
			}

			/// <summary>
			/// Sets the sink
			/// </summary>
			public void SetWorkComplete<P0, P1>( ActionDelegates.Action<P0, P1> workComplete, P0 p0, P1 p1 )
			{
				m_WorkComplete = workComplete;
				m_WorkCompleteParams = new object[] { p0, p1 };
			}

			#endregion

			/// <summary>
			/// Builds a SourceSinkWorkItem from the current builder setup
			/// </summary>
			/// <param name="name">Name of the work item</param>
			/// <exception cref="InvalidOperationException">Thrown if source or sink are null, or sink is invalid</exception>
			public DelegateWorkItem Build( string name )
			{
				if ( m_Work == null )
				{
					throw new InvalidOperationException( "Work delegate cannot be null" );
				}
				return new DelegateWorkItem( name, m_Work, m_WorkParams, m_WorkComplete, m_WorkCompleteParams, m_WorkFailed, m_WorkFailedParams );
			}

			#region Private Members

			private Delegate m_Work;
			private object[] m_WorkParams;
			private Delegate m_WorkComplete;
			private object[] m_WorkCompleteParams;
			private Delegate m_WorkFailed;
			private object[] m_WorkFailedParams;

			#endregion
		}

		/// <summary>
		/// Work item default constructor
		/// </summary>
		/// <param name="name">Name of the work item</param>
		/// <param name="work">Delegate to invoke to perform work</param>
		/// <param name="workParams">Parameters to pass to the work delegate</param>
		/// <param name="workComplete">Delegate to invoke when work is completed (can be null)</param>
		/// <param name="workCompleteParams">Parameters to pass to the workComplete delegate</param>
		/// <param name="workFailed">Delegate to invoke when work fails (can be null)</param>
		/// <param name="workFailedParams">Parameters to pass to the workFailed delegate</param>
		/// <exception cref="ArgumentNullException">Thrown if work is null</exception>
		/// <exception cref="ArgumentException">Thrown if workParams or workCompleteParams are invalid for their respective delegates</exception>
		public DelegateWorkItem( string name, Delegate work, object[] workParams, Delegate workComplete, object[] workCompleteParams, Delegate workFailed, object[] workFailedParams )
		{
			if ( work == null )
			{
				throw new ArgumentNullException( "work" );
			}
			//List<Type> workArgTypes = new List<Type>( workParams == null ? new Type[ 0 ] : Type.GetTypeArray( workParams ) );
			//workArgTypes.Insert( 0, typeof( IProgressMonitor ) );
			//DelegateHelpers.ValidateDelegateArguments( work, workArgTypes.ToArray( ) );
			//if ( workComplete != null )
			//{
			//    DelegateHelpers.ValidateDelegateArguments( workComplete, workCompleteParams );
			//}
			//if ( workFailed != null )
			//{
			//    //	Add exception type to the list of work failed parameters before validating
			//    List<Type> failedArgTypes = new List<Type>( Type.GetTypeArray( workFailedParams ) );
			//    failedArgTypes.Insert( 0, typeof( Exception ) );
			//    DelegateHelpers.ValidateDelegateArgumentTypes( workFailed, failedArgTypes.ToArray( ) );	
			//}
			m_Name = name;
			m_Work = work;
			m_WorkParams = workParams;
			m_WorkComplete = workComplete;
			m_WorkCompleteParams = workCompleteParams;
			m_WorkFailed = workFailed;
			m_WorkFailedParams = workFailedParams;
		}

		/// <summary>
		/// Work item name
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Calls the work delegate passed to the constructor
		/// </summary>
		public void DoWork( IProgressMonitor monitor )
		{
			object[] workParams = new object[ m_WorkParams.Length ];
			Array.Copy( m_WorkParams, workParams, workParams.Length );
			workParams[ 0 ] = monitor;
			m_Work.DynamicInvoke( workParams );
		}

		/// <summary>
		/// Handles exceptions thrown by DoWork()
		/// </summary>
		/// <param name="progressMonitor">Progress monitor</param>
		/// <param name="ex">Exception details</param>
		public void WorkFailed( IProgressMonitor progressMonitor, Exception ex )
		{
			if ( m_WorkFailed != null )
			{
				object[] workFailedParams = new object[ m_WorkFailedParams.Length ];
				Array.Copy( m_WorkFailedParams, workFailedParams, workFailedParams.Length );
				workFailedParams[ 0 ] = ex;
				m_WorkFailed.DynamicInvoke( workFailedParams );
			}
			progressMonitor.WorkFailed( ex );
		}

		/// <summary>
		/// Calls the work complete delegate passed to the constructor
		/// </summary>
		public void WorkComplete( IProgressMonitor progressMonitor )
		{
			if ( m_WorkComplete != null )
			{
				m_WorkComplete.DynamicInvoke( m_WorkCompleteParams );
			}
			progressMonitor.WorkComplete( );
		}


		#region Private Members

		private readonly string m_Name;
		private readonly Delegate m_Work;
		private readonly object[] m_WorkParams;
		private readonly Delegate m_WorkComplete;
		private readonly object[] m_WorkCompleteParams;
		private readonly Delegate m_WorkFailed;
		private readonly object[] m_WorkFailedParams;

		#endregion

	}
}
