using System;
using Rb.Core.Components;

namespace Rb.Core.Resources
{
	/// <summary>
	/// Extra parameters that can be passed to ResourceStreamLoader.Load() or ResourceDirectoryLoader.Load()
	/// </summary>
	public class LoadParameters : ISupportsDynamicProperties
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
			get { return m_Target; }
            set { m_Target = value; }
		}

		#region ISupportsDynamicProperties Members

		/// <summary>
		/// Gets loader dynamic properties
		/// </summary>
		public IDynamicProperties Properties
		{
			get { return m_Properties; }
		}

		#endregion


		#region Private stuff

		private object m_Target;
		private IDynamicProperties m_Properties = new DynamicProperties( );

		#endregion
	}
}
