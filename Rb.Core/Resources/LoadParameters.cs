using System;

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
		/// Gets the target object
		/// </summary>
		public Object	Target
		{
			get
			{
				return m_Target;
			}
		}

		private Object	m_Target;
	}
}
