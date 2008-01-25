using System;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Weak reference delegate
	/// </summary>
	/// <remarks>
	/// Based on http://www.interact-sw.co.uk/iangblog/2004/06/06/weakeventhandler
	/// 
	/// Usage is
	/// <code>
	/// public class Test
	/// {
	///         public void DoStuff(object sender, EventArgs args) { }
	/// 
	///         public void TestWeakDelegate()
	///         {
	///         }
	/// new WeakDelegate{MyDelegateType}(MyDelegate)
	/// </code>
	/// 
	/// </remarks>
	/// <typeparam name="DelegateType">Delegate type</typeparam>
	public class WeakDelegate< DelegateType >
	{
		/// <summary>
		/// Builds the weak delegate
		/// </summary>
		/// <param name="original">Original delegate</param>
		public WeakDelegate( DelegateType original )
		{
			m_Original = original;
			bool isStatic = ( original as Delegate ).Target == null;

			if ( !isStatic )
			{
				//	Delegate is not static - create a weak reference to the original delegate
				m_Delegate = new WeakReference( original );
			}
		}

		/// <summary>
		/// Casts this delegate
		/// </summary>
		/// <param name="wd"></param>
		/// <returns></returns>
		public static implicit operator DelegateType( WeakDelegate< DelegateType > wd )
		{
			if ( wd.m_Delegate == null )
			{
				//	Delegate is static - return original
				return wd.m_Original;
			}

			object obj = Delegate.CreateDelegate( typeof( DelegateType ), wd, "Invoke" );
			return ( DelegateType )obj;
		}

		#region Private members

		private readonly DelegateType m_Original;
		private readonly WeakReference m_Delegate;

		/// <summary>
		/// Invokes the delegate
		/// </summary>
		/// <param name="args">Delegate arguments</param>
		protected internal void Invoke( params object[] args )
		{
			//	NOTE: AP: Not private, so I don't get a warning (resharper only, IIRC, but still...)
			if ( m_Delegate.Target != null )
			{
				( ( Delegate )m_Delegate.Target ).DynamicInvoke( args );
			}
		}

		#endregion
	}

	public static class TestWeakDelegate
	{
		public delegate void MyDelegate( );

		public static void Call( MyDelegate del )
		{
		    del();
		}

		public static void Test0( )
		{
			Call( new WeakDelegate<MyDelegate>( Test0 ) );
		}
	}
}
