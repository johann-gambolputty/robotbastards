using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rb.Core.Utils;

namespace Rb.Core.Components
{
	/// <summary>
	/// Stores the type of a component, and any component types it is dependent on
	/// </summary>
	/// <remarks>
	/// Although this class can be used as-is, there's a handy generic version, ComponentType{T}, without
	/// a setup constructor.
	/// </remarks>
	public class ComponentType
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="type">Underlying component type</param>
		public ComponentType( Type type )
		{
			Arguments.CheckNotNull( type, "type" );
			m_Type = type;
		}

		/// <summary>
		/// Gets the type of component created by this object
		/// </summary>
		public Type Type
		{
			get { return m_Type; }
		}

		/// <summary>
		/// Creates an instance of this type, and any dependencies
		/// </summary>
		public virtual void Create( IComposite composite )
		{
			Arguments.CheckNotNull( composite, "composite" );

			//	Remove existing model component in category
			//IModelTemplate existingTemplate = component.GetModelTemplate( m_Category.BaseType );
			//if ( existingTemplate != null )
			//{
			//    composite.Remove( existingTemplate );
			//}
			if ( CompositeUtils.GetComponent( composite, m_Type ) != null )
			{
				//	This type of component already exists in the composite
				return;
			}
			composite.Add( Activator.CreateInstance( m_Type ) );
			CreateDependencies( composite );
		}

		/// <summary>
		/// Adds a dependency to this
		/// </summary>
		/// <param name="componentType">Type dependency</param>
		/// <remarks>
		/// When an instance of this type is created, any dependencies must be created also
		/// </remarks>
		public void AddDependency( ComponentType componentType )
		{
			Arguments.CheckNotNull( componentType, "componentType" );
			if ( !m_Dependencies.Contains( componentType ) )
			{
				m_Dependencies.Add( componentType );
			}
			if ( !componentType.m_Dependents.Contains( this ) )
			{
				componentType.m_Dependents.Add( this );
			}
		}

		/// <summary>
		/// Gets the dependencies of this type
		/// </summary>
		public ReadOnlyCollection<ComponentType> Dependencies
		{
			get { return m_Dependencies.AsReadOnly( ); }
		}

		/// <summary>
		/// Gets the dependents of this type
		/// </summary>
		public ReadOnlyCollection<ComponentType> Dependents
		{
			get { return m_Dependents.AsReadOnly( ); }
		}

		/// <summary>
		/// Stringifies this object
		/// </summary>
		public override string ToString( )
		{
			return m_Type.Name;
		}

		#region Private Members

		private readonly Type m_Type;
		private readonly List<ComponentType> m_Dependencies = new List<ComponentType>( );
		private readonly List<ComponentType> m_Dependents = new List<ComponentType>( );

		/// <summary>
		/// Creates all dependencies of this type in a composite
		/// </summary>
		private void CreateDependencies( IComposite composite )
		{
			foreach ( ComponentType dependency in Dependencies )
			{
				dependency.Create( composite );
			}
		}

		#endregion
	}
}
