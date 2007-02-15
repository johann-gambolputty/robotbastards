using System;
using System.Collections;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Summary description for ObjectRenderer.
	/// </summary>
	public class ObjectRenderer
	{
		/// <summary>
		///	Singleton access
		/// </summary>
		public static ObjectRenderer Get( )
		{
			return ms_Singleton;
		}

		/// <summary>
		/// Adds a new object to be rendered. Every frame, obj will be cast to IRender and get IRender.Render() called
		/// </summary>
		public void Add( Object obj )
		{
			m_ObjectRefs.Add( new WeakReference( obj ) );
			( ( Utilities.IDeathNotifier )obj ).OnDeathEvent += new Utilities.DeathNotifier.OnDeathDelegate( Remove );
		}

		/// <summary>
		/// Removes an object from the renderer
		/// </summary>
		/// <param name="obj"></param>
		public void Remove( Object obj )
		{
			for ( int refIndex = 0; refIndex < m_ObjectRefs.Count; ++refIndex )
			{
				if ( ( ( WeakReference )m_ObjectRefs[ refIndex ] ).Target == obj )
				{
					m_ObjectRefs.RemoveAt( refIndex );
				}
			}
		}

		/// <summary>
		/// Renders all objects in the list
		/// </summary>
		public void RenderAll( )
		{
			foreach ( WeakReference weakRef in m_ObjectRefs )
			{
				( ( IRender )weakRef.Target ).Render( );
			}
		}

		#region	Private stuff

		private static ObjectRenderer	ms_Singleton	= new ObjectRenderer( );
		private ArrayList				m_ObjectRefs	= new ArrayList( );

		#endregion
	}
}
