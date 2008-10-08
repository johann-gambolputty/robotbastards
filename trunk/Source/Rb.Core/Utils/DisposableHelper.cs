using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Contains vital helper methods to cope with the complicated IDisposable interface.
	/// </summary>
	public static class DisposableHelper
	{
		/// <summary>
		/// If obj implements IDisposable, this method casts it, then calls <see cref="IDisposable.Dispose()"/>
		/// </summary>
		/// <param name="obj">Object to dispose</param>
		public static void Dispose( object obj )
		{
			IDisposable disposable = obj as IDisposable;
			if ( disposable != null )
			{
				disposable.Dispose( );
			}
		}

		/// <summary>
		/// If obj implements IDisposable, this method casts it, then calls <see cref="IDisposable.Dispose()"/>
		/// </summary>
		/// <param name="obj">Object to dispose</param>
		/// <param name="suppressFinalizer">If true, the finalizer for obj is not called when it is garbage collected</param>
		public static void Dispose( object obj, bool suppressFinalizer )
		{
			IDisposable disposable = obj as IDisposable;
			if ( disposable != null )
			{
				disposable.Dispose( );
				if ( suppressFinalizer )
				{
					GC.SuppressFinalize( obj );
				}
			}
		}
	}
}
