using System;

namespace RbEngine.Components
{

	/// <summary>
	/// Component factory interface
	/// </summary>
	public interface IComponentFactory
	{
		object Create( Type type );
	}

	/// <summary>
	/// The default component factory
	/// </summary>
	public class DefaultComponentFactory : IComponentFactory
	{
		/// <summary>
		/// Creates an object of the specified type
		/// </summary>
		public object	Create( Type type )
		{
			return System.Activator.CreateInstance( type );
		}

		/// <summary>
		/// The default component factory
		/// </summary>
		public static DefaultComponentFactory	Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		private static DefaultComponentFactory ms_Singleton = new DefaultComponentFactory( );
	}

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
			m_DefaultFactory = DefaultComponentFactory.Inst;
		}

		/// <summary>
		/// Setup constructor. Sets a target object to load into
		/// </summary>
		/// <param name="target">Target object</param>
		public ComponentLoadParameters( Object target ) :
			base( target )
		{
			m_DefaultFactory = DefaultComponentFactory.Inst;
		}

		/// <summary>
		/// Sets the default factory, that is used to create objects
		/// </summary>
		public ComponentLoadParameters SetDefaultFactory( IComponentFactory factory )
		{
			m_DefaultFactory = factory;
			return this;
		}

		/// <summary>
		/// Gets the default factory
		/// </summary>
		public IComponentFactory	DefaultFactory
		{
			get
			{
				return m_DefaultFactory;
			}
		}

		private IComponentFactory	m_DefaultFactory;
	}
}
