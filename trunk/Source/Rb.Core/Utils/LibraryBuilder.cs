using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Rb.Core.Components;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Supports serialization and creation of library objects
	/// </summary>
	public abstract class LibraryBuilder : ISerializationSurrogate
	{
		#region Setup

		/// <summary>
		/// Adds a library type
		/// </summary>
		public void Add( Type libraryType )
		{
			if ( libraryType.BaseType != null )
			{
				Add( libraryType.BaseType, libraryType );
			}
		}

		/// <summary>
		/// Associates a library type with a given base type
		/// </summary>
		public void Add( Type baseType, Type libraryType )
		{
			ComponentLog.Verbose( "LibraryBuilder: Adding library type \"{0}\"", libraryType );
			for ( ; baseType != typeof( object ); baseType = baseType.BaseType )
			{
				AddAssociation( baseType, libraryType );
			}

			AddInterfaces( libraryType );
		}

		/// <summary>
		/// Scans an assembly for library types (types that pass the <see cref="IsLibraryType"/> predicate)
		/// </summary>
		public void ScanAssembly( Assembly assembly )
		{
			foreach ( Type type in assembly.GetTypes( ) )
			{
				if ( IsLibraryType( type ) )
				{
					Add( type );
				}
			}
		}

		/// <summary>
		/// Sets auto assembly scanning
		/// </summary>
		/// <remarks>
		/// If auto-assembly scanning is true, then whenever an assembly loads, this object will scan it 
		/// for any types that pass the <see cref="IsLibraryType"/> predicate.
		/// </remarks>
		public bool AutoAssemblyScan
		{
			set
			{
				if ( value )
				{
					AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
				}
				else
				{
					AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoad;
				}
			}
		}

		#endregion

		#region Object creation

		/// <summary>
		/// Creates an instance of a library type from a given base type
		/// </summary>
		///	<exception cref="ArgumentException">Thrown if baseType is not associated with any library type</exception>
		public object Create( Type baseType )
		{
			return Create( Builder.Instance, baseType );
		}

		/// <summary>
		/// Creates an instance of a library type, with specified construction arguments, from a given base type
		/// </summary>
		///	<exception cref="ArgumentException">Thrown if baseType is not associated with any library type</exception>
		public object Create( Type baseType, params object[] constructorArgs )
		{
			return Create( Builder.Instance, baseType, constructorArgs );
		}

		/// <summary>
		/// Creates an instance of a library type from a given base type, using an IBuider
		/// </summary>
		///	<exception cref="ArgumentException">Thrown if baseType is not associated with any library type</exception>
		public object Create( IBuilder builder, Type baseType )
		{
			Type libraryType = GetLibraryType( baseType );
			return builder.CreateInstance( libraryType );
		}

		/// <summary>
		/// Creates an instance of a library type, using an IBuider, with specified construction arguments
		/// </summary>
		///	<exception cref="ArgumentException">Thrown if baseType is not associated with any library type</exception>
		public object Create( IBuilder builder, Type baseType, params object[] constructorArgs )
		{
			Type libraryType = GetLibraryType( baseType );
			return builder.CreateInstance( libraryType, constructorArgs );
		}

		/// <summary>
		/// Creates an instance of a library type from a given base type
		/// </summary>
		///	<exception cref="ArgumentException">Thrown if baseType is not associated with any library type</exception>
		public BaseType Create< BaseType >( )
		{
			return ( BaseType )Create( typeof( BaseType ) );
		}

		/// <summary>
		/// Creates an instance of a library type, with specified construction arguments, from a given base type
		/// </summary>
		///	<exception cref="ArgumentException">Thrown if baseType is not associated with any library type</exception>
		public BaseType Create< BaseType >( params object[] constructorArgs )
		{
			return ( BaseType )Create( typeof( BaseType ), constructorArgs );
		}

		/// <summary>
		/// Creates an instance of a library type from a given base type, using an IBuider
		/// </summary>
		///	<exception cref="ArgumentException">Thrown if baseType is not associated with any library type</exception>
		public BaseType Create< BaseType >( IBuilder builder )
		{
			return ( BaseType )Create( builder, typeof( BaseType ) );
		}

		/// <summary>
		/// Creates an instance of a library type, using an IBuider, with specified construction arguments
		/// </summary>
		///	<exception cref="ArgumentException">Thrown if baseType is not associated with any library type</exception>
		public BaseType Create< BaseType >( IBuilder builder, params object[] constructorArgs )
		{
			return ( BaseType )Create( builder, typeof( BaseType ), constructorArgs );
		}

		#endregion

		#region Protected members

		/// <summary>
		/// Returns true if a given type is a library type
		/// </summary>
		protected abstract bool IsLibraryType( Type type );

		#endregion

		#region Private members

		private readonly Dictionary< Type, Type > m_Associations = new Dictionary< Type, Type >( );

		/// <summary>
		/// Adds an association between a base type and a library type
		/// </summary>
		private void AddAssociation( Type baseType, Type libraryType )
		{
			if ( !m_Associations.ContainsKey( baseType ) )
			{
				m_Associations[ baseType ] = libraryType;
			}
			else
			{
				ComponentLog.Verbose( "LibraryBuilder: Invalidating base type \"{0}\" - already used by \"{1}\" ", baseType, m_Associations[ baseType ] );
				m_Associations[ baseType ] = typeof( object );
			}
		}

		/// <summary>
		/// Adds associations between a library type and its interfaces
		/// </summary>
		private void AddInterfaces( Type libraryType )
		{
			foreach ( Type interfaceType in libraryType.GetInterfaces( ) )
			{
				AddAssociation( interfaceType, libraryType );
			}
		}
		
		/// <summary>
		/// Gets a library type from a given base type
		/// </summary>
		private Type GetLibraryType( Type baseType )
		{
			Type libraryType;
			if ( ( !m_Associations.TryGetValue( baseType, out libraryType ) ) || ( libraryType == typeof( object ) ) )
			{
				throw new ArgumentException( string.Format( "No association exists between type \"{0}\" and a library type", baseType ) );
			}
			return libraryType;
		}
		
		/// <summary>
		/// Scans the assembly specified in the arguments
		/// </summary>
		private void OnAssemblyLoad( object sender, AssemblyLoadEventArgs args )
		{
			if ( !args.LoadedAssembly.ReflectionOnly )
			{
				ScanAssembly( args.LoadedAssembly );
			}
		}

		#endregion

		#region ISerializationSurrogate Members

		public void GetObjectData( object obj, SerializationInfo info, StreamingContext context )
		{
			throw new NotImplementedException( );
		}

		public object SetObjectData( object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector )
		{
			throw new NotImplementedException( );
		}

		#endregion
	}
}
