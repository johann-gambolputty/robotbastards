using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Loading parameters for component description files
	/// </summary>
	public class ComponentLoadParameters : Resources.LoadParameters
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ComponentLoadParameters( )
		{
		}

		/// <summary>
		/// Setup constructor. Sets a target object to load into
		/// </summary>
		/// <param name="target">Target object</param>
		public ComponentLoadParameters( Object target ) :
			base( target )
		{
		}

		/// <summary>
		/// Associated type factory. Can be null
		/// </summary>
		public ITypeFactory Factory
		{
			get
			{
				return m_Factory;
			}
			set
			{
				m_Factory = value;
			}
		}

		private ITypeFactory m_Factory;
	}
}
