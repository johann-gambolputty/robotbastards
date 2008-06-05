using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Utilities for find types and resources in all assemblies in the app domain
	/// </summary>
	public class AppDomainUtils
	{
		public static Type FindTypeImplementingInterface( IEnumerable<Type> types, Type interfaceType )
		{
			foreach ( Type type in types )
			{
				if ( type.GetInterface( interfaceType.Name ) != null )
				{
					return type;
				}
			}
			return null;
		}

		public static Type[] FindTypesImplementingInterface( IEnumerable< Type > types, Type interfaceType )
		{
			List<Type> results = new List<Type>( );
			foreach ( Type type in types )
			{
				if ( type.GetInterface( interfaceType.Name ) != null )
				{
					results.Add( type );
				}
			}
			return results.ToArray( );
		}

		
		public static Type[] FindTypesImplementingInterface( Type interfaceType )
		{
			List<Type> results = new List<Type>( );
			foreach ( Assembly curAssembly in AppDomain.CurrentDomain.GetAssemblies( ) )
			{
				foreach ( Type type in curAssembly.GetTypes( ) )
				{
					if ( type.GetInterface( interfaceType.Name ) != null )
					{
						results.Add( type );
					}
				}
			}
			return results.ToArray( );
		}

		/// <summary>
		/// Looks for a named type in all the currently loaded assemblies
		/// </summary>
		/// <param name="name"> Type name </param>
		/// <returns> Returns the named type, or null if it could not be found </returns>
		public static Type FindType( string name )
		{
			foreach ( Assembly curAssembly in AppDomain.CurrentDomain.GetAssemblies( ) )
			{
				Type type = curAssembly.GetType( name );
				if ( type != null )
				{
					return type;
				}
			}
			return null;
		}

		/// <summary>
		/// Looks for a manifest resource in all the currently loaded assemblies
		/// </summary>
		/// <param name="name"> Resource name </param>
		/// <returns> Returns the named resource stream, or null if it could not be found </returns>
		public static System.IO.Stream FindManifestResource( string name )
		{
			foreach ( Assembly curAssembly in AppDomain.CurrentDomain.GetAssemblies( ) )
			{
				System.IO.Stream str = curAssembly.GetManifestResourceStream( name );
				if ( str != null )
				{
					return str;
				}
			}
			return null;
		}

		/// <summary>
		/// Searches through the loaded assemblies for one that supports the named type, then creates an instance of it
		/// </summary>
		/// <param name="name"> Type name </param>
		/// <returns> Returns a new instance of the named type, or null if the type could not be found </returns>
		public static Object CreateInstance( string name )
		{
			Type namedType = FindType( name );
			if ( namedType == null )
			{
				return null;
			}
			return Activator.CreateInstance( namedType );
		}
	}
}
