using System;
using System.Collections.Generic;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Extra parameters that can be passed to ResourceStreamLoader.Load() or ResourceDirectoryLoader.Load()
	/// </summary>
	public class LoadParameters
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public LoadParameters( )
		{
		}

		/// <summary>
		/// Sets an object that is loaded into
		/// </summary>
		/// <param name="target">Target object</param>
		public LoadParameters( Object target )
		{
			m_Target = target;
		}

        /// <summary>
        /// Dynamic property indexer
        /// </summary>
        /// <param name="name">Property name</param>
        /// <returns>Property value</returns>
	    public object this[ string name ]
	    {
            get
            {
                return m_Properties.ContainsKey( name ) ? m_Properties[ name ] : null;
            }
            set
            {
                m_Properties[ name ] = value;
            }
	    }

		/// <summary>
		/// Gets the target object
		/// </summary>
		public Object	Target
		{
			get
			{
				return m_Target;
			}
		}

		private Object	                        m_Target;
	    private Dictionary< string, object >    m_Properties = new Dictionary< string, object >( );
	}
}
