using System;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Utils;

namespace Rb.Core.Components
{
	/// <summary>
	/// Fluent component type builder
	/// </summary>
	public class ComponentTypeBuilder
	{
		/// <summary>
		/// Creates a new component type. Returns a helper object that can be used to add dependencies to the new component type
		/// </summary>
		public ComponentTypeDependsOn Add<T0>( )
		{
			ComponentTypeNode node;
			Type type = typeof( T0 );
			if ( !m_Nodes.TryGetValue( type, out node ) )
			{
				node = new ComponentTypeNode( type );
				m_Nodes.Add( type, node );
			}
			return new ComponentTypeDependsOnImpl( this, typeof( T0 ) );
		}

		/// <summary>
		/// Builds an array of component types added with by <see cref="Add{T}"/>
		/// </summary>
		public ComponentType[] Build( )
		{
			Dictionary<Type, ComponentType> componentTypes = new Dictionary<Type, ComponentType>( );
			foreach ( ComponentTypeNode node in m_Nodes.Values )
			{
				Build( componentTypes, node );
			}
			return new List<ComponentType>( componentTypes.Values ).ToArray( );
		}

		#region Private Members

		private readonly Dictionary<Type, ComponentTypeNode> m_Nodes = new Dictionary<Type, ComponentTypeNode>( );

		/// <summary>
		/// Builds a component type and all its dependencies
		/// </summary>
		private ComponentType Build( Dictionary<Type, ComponentType> componentTypes, ComponentTypeNode node )
		{
			if ( componentTypes.ContainsKey( node.Type ) )
			{
				return componentTypes[ node.Type ];
			}
			ComponentType componentType = new ComponentType( node.Type );
			componentTypes.Add( node.Type, componentType );

			foreach ( ComponentTypeNode dependency in node.Dependencies )
			{
				componentType.AddDependency( Build( componentTypes, dependency ) );
			}
			return componentType;
		}

		/// <summary>
		/// Gets a component type node for a type
		/// </summary>
		private ComponentTypeNode GetNode( Type type )
		{
			return m_Nodes[ type ];
		}

		/// <summary>
		/// Adds an array of dependencies to a component type node
		/// </summary>
		private ComponentTypeNode AddDependencies( Type dependentType, Type[] dependencyTypes )
		{
			ComponentTypeNode node = GetNode( dependentType );

			//	Create a map containin all the dependencies
			Dictionary<Type, ComponentTypeNode> dependencyMap = new Dictionary<Type, ComponentTypeNode>( );
			foreach ( Type dependencyType in dependencyTypes )
			{
				CreateDependencyMap( dependencyMap, GetNode( dependencyType ) );
			}

			//	Add each unique dependency in the dependency map to the node for dependentType
			foreach ( ComponentTypeNode dependencyNode in dependencyMap.Values )
			{
				if ( ( dependencyNode != null ) && !node.Dependencies.Contains( dependencyNode ) )
				{
					node.Dependencies.Add( dependencyNode );
				}
			}
			return node;
		}

		/// <summary>
		/// Creates a map of all dependencies
		/// </summary>
		private void CreateDependencyMap( Dictionary<Type, ComponentTypeNode> dependencyMap, ComponentTypeNode node )
		{
			if ( dependencyMap.ContainsKey( node.Type ) )
			{
				//	This dependency has already been analyzed
				return;
			}
			dependencyMap.Add( node.Type, node );
			foreach ( ComponentTypeNode dependencyNode in node.Dependencies )
			{
				RemoveRedundantDependencies( dependencyMap, dependencyNode );
			}
		}

		/// <summary>
		/// Removes dependencies from a dependency map
		/// </summary>
		private void RemoveRedundantDependencies( Dictionary<Type, ComponentTypeNode> dependencyMap, ComponentTypeNode node )
		{
			dependencyMap[ node.Type ] = null;
			foreach ( ComponentTypeNode dependency in node.Dependencies )
			{
				RemoveRedundantDependencies( dependencyMap, dependency );
			}
		}

		/// <summary>
		/// Makes sure that the dependency graph is acyclic
		/// </summary>
		private void TestDependenciesAreNotCyclic( ComponentTypeNode node )
		{
			Dictionary<Type, ComponentTypeNode> types = new Dictionary<Type, ComponentTypeNode>( );
			foreach ( ComponentTypeNode dependency in node.Dependencies )
			{
				TestDependenciesAreNotCyclic( node.Type, types, dependency );
			}
		}

		/// <summary>
		/// Makes sure that the dependency graph is acyclic
		/// </summary>
		private void TestDependenciesAreNotCyclic( Type originalType, Dictionary<Type, ComponentTypeNode> types, ComponentTypeNode node )
		{
			if ( node.Type == originalType )
			{
				//	Back to original type. Graph contains a cycle.
				throw new InvalidOperationException( string.Format( "Dependencies for type \"{0}\" form a cycle", originalType ) );
			}
			if ( types.ContainsKey( node.Type ) )
			{
				//	Already tested this subgraph
				return;
			}
			types.Add( node.Type, node );
			foreach ( ComponentTypeNode dependency in node.Dependencies )
			{
				TestDependenciesAreNotCyclic( originalType, types, dependency );
			}
		}

		#region ComponentTypeNode class

		private class ComponentTypeNode
		{
			public ComponentTypeNode( Type type )
			{
				m_Type = type;
			}

			public List<ComponentTypeNode> Dependencies
			{
				get { return m_Dependencies; }
			}

			public Type Type
			{
				get { return m_Type; }
			}

			#region Private Members

			private readonly Type m_Type;
			private readonly List<ComponentTypeNode> m_Dependencies = new List<ComponentTypeNode>( );

			#endregion
		}

		#endregion

		#region ComponentTypeDependsOnImpl

		private class ComponentTypeDependsOnImpl : ComponentTypeDependsOn
		{
			public ComponentTypeDependsOnImpl( ComponentTypeBuilder manager, Type type )
			{
				m_Builder = manager;
				m_Type = type;
			}

			public override ComponentTypeDependsOn DependsOn( Type[] types )
			{
				ComponentTypeNode node = m_Builder.AddDependencies( m_Type, types );

				try
				{
					m_Builder.TestDependenciesAreNotCyclic( node );
				}
				catch ( Exception ex )
				{
					string msg = string.Format( "Adding dependencies [{0}] to type \"{1}\" created a cycle in the dependency graph", StringUtils.StringifyEnumerable( types, ", ", delegate( Type type ) { return type.Name; } ), m_Type.Name );
					throw new InvalidOperationException( msg, ex );
				}

				return this;
			}

			private readonly Type m_Type;
			private readonly ComponentTypeBuilder m_Builder;
		}

		#endregion

		#endregion
	}

}
