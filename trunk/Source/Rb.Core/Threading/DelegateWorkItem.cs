using System;
using System.Reflection;
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
			public void SetDoWork( ActionDelegates.Action work )
			{
				m_Work = work;
				m_WorkParams = new object[ 0 ];
			}

			/// <summary>
			/// Sets the worker delegate
			/// </summary>
			public void SetDoWork<P0>( ActionDelegates.Action<P0> work, P0 p0 )
			{
				m_Work = work;
				m_WorkParams = new object[] { p0 };
			}

			/// <summary>
			/// Sets the worker delegate
			/// </summary>
			public void SetDoWork<P0, P1>( ActionDelegates.Action<P0, P1> work, P0 p0, P1 p1 )
			{
				m_Work = work;
				m_WorkParams = new object[] { p0, p1 };
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
			/// <exception cref="InvalidOperationException">Thrown if source or sink are null, or sink is invalid</exception>
			public DelegateWorkItem Build( )
			{
				if ( m_Work == null )
				{
					throw new InvalidOperationException( "Work delegate cannot be null" );
				}
				return new DelegateWorkItem( m_Work, m_WorkParams, m_WorkComplete, m_WorkCompleteParams );
			}

			#region Private Members

			private Delegate m_Work;
			private object[] m_WorkParams;
			private Delegate m_WorkComplete;
			private object[] m_WorkCompleteParams;

			#endregion
		}

		/// <summary>
		/// Work item default constructor
		/// </summary>
		/// <param name="work">Delegate to invoke to perform work</param>
		/// <param name="workParams">Parameters to pass to the work delegate</param>
		/// <param name="workComplete">Delegate to invoke when work is completed (can be null)</param>
		/// <param name="workCompleteParams">Parameters to pass to the workComplete delegate</param>
		/// <exception cref="ArgumentNullException">Thrown if work is null</exception>
		/// <exception cref="ArgumentException">Thrown if workParams or workCompleteParams are invalid for their respective delegates</exception>
		public DelegateWorkItem( Delegate work, object[] workParams, Delegate workComplete, object[] workCompleteParams )
		{
			if ( work == null )
			{
				throw new ArgumentNullException( "work" );
			}
			string checkResult = CheckMethodHasCorrectArguments( work.Method, workParams );
			if ( !string.IsNullOrEmpty( checkResult ) )
			{
				throw new ArgumentException( checkResult, "work" );
			}
			if ( workComplete != null )
			{
				checkResult = CheckMethodHasCorrectArguments( workComplete.Method, workCompleteParams );
				if ( !string.IsNullOrEmpty( checkResult ) )
				{
					throw new ArgumentException( checkResult, "workComplete" );
				}
			}
			m_Work = work;
			m_WorkParams = workParams;
			m_WorkComplete = workComplete;
			m_WorkCompleteParams = workCompleteParams;
		}

		/// <summary>
		/// Calls the work delegate passed to the constructor
		/// </summary>
		public void DoWork( )
		{
			m_Work.DynamicInvoke( m_WorkParams );
		}

		/// <summary>
		/// Calls the work complete delegate passed to the constructor
		/// </summary>
		public void WorkComplete( )
		{
			if ( m_WorkComplete != null )
			{
				m_WorkComplete.DynamicInvoke( m_WorkCompleteParams );
			}
		}


		/// <summary>
		/// Ensures that the method is called with the correct parameters
		/// </summary>
		public static string CheckMethodHasCorrectArguments( MethodInfo method, object[] args )
		{
			ParameterInfo[] parameters = method.GetParameters( );
			if ( args == null )
			{
				if ( parameters.Length != 0 )
				{
					return string.Format( "Error setting up work delegate {0} with 0 arguments - requires {1}", method.Name, parameters.Length );
				}
				return string.Empty;
			}
			if ( parameters.Length != args.Length )
			{
				return string.Format( "Error setting up work delegate {0} with {1} arguments - requires {2}", method.Name, args.Length, parameters.Length );
			}
			for ( int argIndex = 0; argIndex < args.Length; ++argIndex )
			{
				Type parameterType = parameters[ argIndex ].ParameterType;
				if ( ( args[ argIndex ] != null ) && ( parameterType.IsAssignableFrom( args[ argIndex ].GetType( ) ) ) )
				{
					return string.Format( "Error setting up work delegate {0}. Argument {1} type invalid: {2} can't be assigned to parameter type {3}", method.Name, args.Length, parameters.Length, parameterType );
				}
			}
			return string.Empty;
		}


		#region Private Members

		private readonly Delegate m_Work;
		private readonly object[] m_WorkParams;
		private readonly Delegate m_WorkComplete;
		private readonly object[] m_WorkCompleteParams;

		#endregion

	}
}
