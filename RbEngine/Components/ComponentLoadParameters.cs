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
			m_DefaultFactory = Builder.Inst;
		}

		/// <summary>
		/// Setup constructor. Sets a target object to load into
		/// </summary>
		/// <param name="target">Target object</param>
		public ComponentLoadParameters( Object target ) :
			base( target )
		{
			m_DefaultFactory = Builder.Inst;
		}

		/// <summary>
		/// Sets the default factory, that is used to create objects
		/// </summary>
		public ComponentLoadParameters SetDefaultFactory( IBuilder factory )
		{
			m_DefaultFactory = factory;
			return this;
		}

		/// <summary>
		/// Gets the default factory
		/// </summary>
		public IBuilder	DefaultFactory
		{
			get
			{
				return m_DefaultFactory;
			}
		}

		private IBuilder	m_DefaultFactory;
	}
}
