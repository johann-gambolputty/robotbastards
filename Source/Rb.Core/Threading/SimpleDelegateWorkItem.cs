using System;
using Rb.Core.Utils;

namespace Rb.Core.Threading
{
	/// <summary>
	/// Work item with a single delegate used for performing work (no failure or completion support)
	/// </summary>
	public class SimpleDelegateWorkItem : AbstractWorkItem
	{
		/// <summary>
		/// Sets up a delegate to perform work, and parameters to pass
		/// </summary>
		public static SimpleDelegateWorkItem Create( string name, ActionDelegates.Action work )
		{
			return new SimpleDelegateWorkItem( name, work, null );
		}

		/// <summary>
		/// Sets up a delegate to perform work, and parameters to pass
		/// </summary>
		public static SimpleDelegateWorkItem Create<P0>( string name, ActionDelegates.Action<P0> work, P0 p0 )
		{
			return new SimpleDelegateWorkItem( name, work, new object[] { p0 } );
		}

		/// <summary>
		/// Sets up a delegate to perform work, and parameters to pass
		/// </summary>
		public static SimpleDelegateWorkItem Create<P0, P1>( string name, ActionDelegates.Action<P0, P1> work, P0 p0, P1 p1 )
		{
			return new SimpleDelegateWorkItem( name, work, new object[] { p0, p1 } );
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public SimpleDelegateWorkItem( string name, Delegate work, object[] workParams )
		{
			m_Name = name;
			m_Work = work;
			m_WorkParams = workParams;
		}

		#region IWorkItem Members

		/// <summary>
		/// Gets the name of this work item
		/// </summary>
		public override string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Does work
		/// </summary>
		public override void DoWork( IProgressMonitor progress )
		{
			progress.UpdateProgress( 0 );
			m_Work.DynamicInvoke( m_WorkParams );
			progress.UpdateProgress( 1 );
		}

		#endregion

		#region Private Members

		private readonly string m_Name;
		private readonly Delegate m_Work;
		private readonly object[] m_WorkParams;

		#endregion
	}
}
