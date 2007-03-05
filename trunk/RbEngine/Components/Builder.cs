using System;
using System.Reflection;
using System.Collections;

namespace RbEngine.Components
{
	/// <summary>
	/// An attribute that can be added to methods to tag them as builder methods
	/// </summary>
	public class BuilderMethodAttribute : Attribute
	{
	}

	/// <summary>
	/// The Builder singleton is responsible for building components and objects
	/// </summary>
	public class Builder
	{
		/// <summary>
		/// Gets the builder singleton
		/// </summary>
		public static Builder	Main
		{
			get
			{
				return ms_Singleton;
			}
		}

		/// <summary>
		/// Adds a factory object
		/// </summary>
		/// <remarks>
		/// All methods in factory that are tagged with the BuilderMethod attribute are added as builder methods
		/// </remarks>
		public void AddFactory( object factory )
		{
			foreach ( MethodInfo curMethod in factory.GetType( ).GetMethods( ) )
			{
				if ( curMethod.GetCustomAttributes( typeof( BuilderMethodAttribute ), true ).Length > 0 )
				{
					AddFactoryMethod( factory, curMethod );
				}
			}
		}

		/// <summary>
		/// Adds a factory method
		/// </summary>
		public void AddFactoryMethod( object factory, MethodInfo method )
		{
			Output.WriteLineCall( Output.ComponentInfo, "Registering factory method for type \"{0}\" using method \"{1}\"", method.ReturnType.Name, method.Name );
			m_MethodTable[ method.ReturnType ] = new FactoryMethod( factory, method );
		}

		/// <summary>
		/// Creates an object of the specified type. If there is no appropriate builder method for objectType, the System.Activator is used
		/// </summary>
		public object	Build( Type objectType )
		{
			FactoryMethod builder = ( FactoryMethod )m_MethodTable[ objectType ];
			if ( builder == null )
			{
				return System.Activator.CreateInstance( objectType );
			}
			return builder.Invoke( );
		}

		/// <summary>
		/// Stores factory and method information
		/// </summary>
		private class FactoryMethod
		{
			/// <summary>
			/// Stores a factory and a method from it
			/// </summary>
			public FactoryMethod( object factory, MethodInfo method )
			{
				m_Factory	= factory;
				m_Method	= method;
			}

			/// <summary>
			/// Invokes the stored method
			/// </summary>
			public object	Invoke( )
			{
				return m_Method.Invoke( m_Factory, null );
			}

			object		m_Factory;
			MethodInfo	m_Method;
		}

		private static Builder	ms_Singleton	= new Builder( );
		private Hashtable		m_MethodTable	= new Hashtable( );
	}
}
