using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Rb.Core.Components
{
	/// <summary>
	/// Creates objects
	/// </summary>
	/// <remarks>
	/// When <see cref="CreateInstance"/> is called, it uses the <see cref="IBuilder"/> parameter to create
	/// an instance of the type specified in the template's constructor. The created object then has its
	/// properties set to values specified by <see cref="AddPropertyValue"/> calls.
	/// If the created object implements <see cref="IList"/> or <see cref="IParent"/>, and there are child
	/// instance builders (added using <see cref="AddChildInstanceBuilder"/>), the child instances are created
	/// and added using an appropriate method to the created object.
	/// </remarks>
	public class ObjectTemplate : IInstanceBuilder, INamed, IParent
	{
		/// <summary>
		/// Setup constructor. Creates object of given type with no constructor parameters
		/// </summary>
		/// <param name="objectType">Object type to create</param>
		/// <param name="name">Template name</param>
		public ObjectTemplate( Type objectType, string name )
		{
			m_Name = name;
			m_Type = objectType;
			m_CanAddChildren = CanAddChildrenToType( m_Type );
		}
		
		/// <summary>
		/// Setup constructor. Creates object of given type with no constructor parameters. Template is not named
		/// </summary>
		/// <param name="objectType">Object type to create</param>
		public ObjectTemplate( Type objectType )
		{
			m_Name = "unnamedTemplate";
			m_Type = objectType;
			m_CanAddChildren = CanAddChildrenToType( m_Type );
		}

		/// <summary>
		/// Setup constructor. Creates object of given type with specified constructor parameters
		/// </summary>
		/// <param name="objectType">Object type to create</param>
		/// <param name="constructorArgs">Arguments passed to the objectType constructor</param>
		public ObjectTemplate( Type objectType, object[] constructorArgs )
		{
			m_ConstructorArgs = constructorArgs;
			m_Type = objectType;
			m_CanAddChildren = CanAddChildrenToType( m_Type );
		}

		/// <summary>
		/// Adds a child instance builder
		/// </summary>
		/// <param name="builder">Child instance builder</param>
		/// <exception cref="InvalidOperationException">Thrown if the created object type can't handle child objects (e.g. not <see cref="IParent"/>)</exception>
		public void AddChildInstanceBuilder( IInstanceBuilder builder )
		{
			if ( !m_CanAddChildren )
			{
				throw new InvalidOperationException( string.Format( "Can't add child instance builders, because type {0} can't accept child objects", m_Type ) );
			}
			m_ChildBuilders.Add( builder );
		}

		/// <summary>
		/// Adds a property value to the builder. <see cref="CreateInstance"/> will set this property to the specified
		/// value after creating the object
		/// </summary>
		/// <param name="name">Property name</param>
		/// <param name="value">Property value</param>
		/// <exception cref="ArgumentException">Thown if name does not match a property in the type specified in the constructor</exception>
		public void AddPropertyValue( string name, object value )
		{
			PropertyInfo key = m_Type.GetProperty( name );
			if ( key == null )
			{
				throw new ArgumentException( string.Format("{0} is not a property in {1}", name, m_Type ) );
			}

			KeyValuePair< PropertyInfo, object > kvp = new KeyValuePair<PropertyInfo, object>( key, value );
			m_PropertyValues.Add( kvp );
		}

		#region Public properties

		/// <summary>
		/// Gets the name of this template
		/// </summary>
		public string Name
		{
			set { m_Name = value; }
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the type of object created
		/// </summary>
		public Type InstanceType
		{
			get { return m_Type; }
		}

		#endregion

		#region IInstanceBuilder Members

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		/// <param name="builder">Builder used to create objects</param>
		/// <returns>Returns the root object</returns>
		public object CreateInstance( IBuilder builder )
		{
			object result;
			if ( m_ConstructorArgs == null )
			{
				result = builder.CreateInstance( m_Type );
			}
			else
			{
				result = builder.CreateInstance( m_Type, m_ConstructorArgs );
			}

			foreach ( KeyValuePair< PropertyInfo, object > property in m_PropertyValues )
			{
				property.Key.SetValue( result, property.Value, null );
			}

			if ( m_ChildBuilders.Count == 0 )
			{
				return result;
			}

			if ( result is IParent )
			{
				IParent resultParent = ( IParent )result;
				foreach ( IInstanceBuilder childBuilder in m_ChildBuilders )
				{
					resultParent.AddChild( childBuilder.CreateInstance( builder ) );
				}
			}
			else if ( result is IList )
			{
				IList resultList = ( IList )result;
				foreach ( IInstanceBuilder childBuilder in m_ChildBuilders )
				{
					resultList.Add( childBuilder.CreateInstance( builder ) );
				}
			}

			return result;
		}

		#endregion

		#region Private members

		private string m_Name;
		private readonly Type m_Type;
		private readonly bool m_CanAddChildren;
		private readonly object[] m_ConstructorArgs;
		private readonly List< IInstanceBuilder > m_ChildBuilders = new List< IInstanceBuilder >( );
		private readonly List< KeyValuePair< PropertyInfo, object > > m_PropertyValues = new List< KeyValuePair< PropertyInfo, object > >( );

		private static bool CanAddChildrenToType( Type t )
		{
			return	( t.GetInterface( typeof( IParent ).Name ) != null ) ||
					( t.GetInterface( typeof( IList ).Name ) != null );
		}

		#endregion

		#region IParent Members

		public ICollection Children
		{
			get { return m_ChildBuilders; }
		}

		public void AddChild( object obj )
		{
			m_ChildBuilders.Add( ( IInstanceBuilder )obj );
		}

		public void RemoveChild( object obj )
		{
			m_ChildBuilders.Remove( ( IInstanceBuilder )obj );
		}

		public event OnChildAddedDelegate OnChildAdded;

		public event OnChildRemovedDelegate OnChildRemoved;

		#endregion
	}
}
