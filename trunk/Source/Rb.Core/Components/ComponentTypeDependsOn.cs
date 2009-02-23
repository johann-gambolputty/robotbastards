using System;

namespace Rb.Core.Components
{
	/// <summary>
	/// Part of the fluent interface for component type dependency specification
	/// </summary>
	public abstract class ComponentTypeDependsOn
	{
		/// <summary>
		/// Sets up a dependency with a single type
		/// </summary>
		public ComponentTypeDependsOn DependsOn<T0>( )
		{
			return DependsOn( new Type[] { typeof( T0 ) } );
		}

		/// <summary>
		/// Sets up a dependency with 2 types
		/// </summary>
		public ComponentTypeDependsOn DependsOn<T0, T1>( )
		{
			return DependsOn( new Type[] { typeof( T0 ), typeof( T1 ) } );
		}

		/// <summary>
		/// Sets up a dependency with 3 types
		/// </summary>
		public ComponentTypeDependsOn DependsOn<T0, T1, T2>( )
		{
			return DependsOn( new Type[] { typeof( T0 ), typeof( T1 ), typeof( T2 ) } );
		}

		/// <summary>
		/// Sets up a dependency with any number of types
		/// </summary>
		public abstract ComponentTypeDependsOn DependsOn( Type[] types );
	}

}
