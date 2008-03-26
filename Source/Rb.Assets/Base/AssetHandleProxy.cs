using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Rb.Assets.Interfaces;

namespace Rb.Assets.Base
{
	/// <summary>
	/// Builds classes derived from <see cref="AssetHandle"/> that implement specified interfaces,
	/// deferring to the loaded assets to implement the interfaces.
	/// </summary>
	public class AssetHandleProxy
	{
		/// <summary>
		/// Builds an class derived from <see cref="AssetHandle"/>, that implements interface T. Returns an instance of this class
		/// </summary>
		/// <returns>Returns an asset handle that implements interface T</returns>
		/// <remarks>
		/// The derived class is built only once for a given interface type - the class definition is cached.
		/// </remarks>
		public static T Create< T >( )
			where T : class
		{
			return Create( typeof( T ) ) as T;
		}

		/// <summary>
		/// Builds an class derived from <see cref="AssetHandle"/>, that implements interface T. Returns an instance of this class
		/// </summary>
		/// <param name="source">Asset source, passed to handle constructor</param>
		/// <param name="trackChanges">Track changes flag, passed to handle constructor</param>
		/// <returns>Returns an asset handle that implements interface T</returns>
		/// <remarks>
		/// The derived class is built only once for a given interface type - the class definition is cached.
		/// </remarks>
		public static T Create< T >( ISource source, bool trackChanges )
			where T : class
		{
			return Create( typeof( T ), source, trackChanges ) as T;
		}

		/// <summary>
		/// Builds an class derived from <see cref="AssetHandle"/>, that implements the specified interface.
		/// Returns an instance of this class
		/// </summary>
		/// <param name="interfaceType">Interface type to implement</param>
		/// <returns>Returns an asset handle that implements the specified interface</returns>
		/// <remarks>
		/// The derived class is built only once for a given interface type - the class definition is cached.
		/// </remarks>
		public static AssetHandle Create( Type interfaceType )
		{
			return (AssetHandle)Activator.CreateInstance(GetProxyType(interfaceType));
		}

		/// <summary>
		/// Builds an class derived from <see cref="AssetHandle"/>, that implements the specified interface.
		/// Returns an instance of this class that points at a given asset source, and optionally tracks
		/// changes to that source
		/// </summary>
		/// <param name="interfaceType">Interface type to implement</param>
		/// <param name="source">Asset source, passed to handle constructor</param>
		/// <param name="trackChanges">Track changes flag, passed to handle constructor</param>
		/// <returns>Returns an asset handle that implements the specified interface</returns>
		/// <remarks>
		/// The derived class is built only once for a given interface type - the class definition is cached.
		/// </remarks>
		public static AssetHandle Create( Type interfaceType, ISource source, bool trackChanges )
		{
			return ( AssetHandle )Activator.CreateInstance( GetProxyType( interfaceType ), source, trackChanges );
		}

		#region Private members

		private static readonly Dictionary<Type, Type> ms_ProxyMap = new Dictionary<Type, Type>();
		private static readonly ModuleBuilder ms_ModuleBuilder;

		/// <summary>
		/// Gets a proxy type for a given interface type, creating it if necessary
		/// </summary>
		private static Type GetProxyType( Type interfaceType )
		{
			if ( !interfaceType.IsInterface )
			{
				throw new ArgumentException( string.Format( "Type \"{0}\" is not an interface - cannot create asset handle proxy", interfaceType ) );
			}

			//	Does a proxy type already exist for interfaceType?
			Type proxyType;
			if ( !ms_ProxyMap.TryGetValue( interfaceType, out proxyType ) )
			{
				//	No - create the proxy type
				proxyType = CreateProxyType( interfaceType );
				ms_ProxyMap[ interfaceType ] = proxyType;
			}

			return proxyType;
		}

		/// <summary>
		/// Creates an asset handle proxy type for a given interface
		/// </summary>
		private static Type CreateProxyType( Type interfaceType )
		{
			//	Create a new type, derived from AssetHandle, and implementing interfaceType
			string typeName = interfaceType.Name + "AssetProxy";
			TypeAttributes attributes = TypeAttributes.Public | TypeAttributes.Class;
			TypeBuilder builder = ms_ModuleBuilder.DefineType( typeName, attributes, typeof( AssetHandle ), new Type[] { interfaceType } );

			builder.AddInterfaceImplementation( interfaceType );
			
			//	Get the asset handle "Asset" property
			PropertyInfo assetProperty = typeof( AssetHandle ).GetProperty( "Asset" );
			
			ImplementInterface( builder, interfaceType, assetProperty );
			
			//	Add implementations for "inherited" interfaces
			foreach ( Type baseInterface in interfaceType.GetInterfaces( ) )
			{
				ImplementInterface( builder, baseInterface, assetProperty );
			}

			//	Create the built type
			return builder.CreateType( );
		}

		/// <summary>
		/// Implements an interface in the specified type builder
		/// </summary>
		private static void ImplementInterface( TypeBuilder builder, Type interfaceType, PropertyInfo assetProperty )
		{
			//	NOTE: Don't need to handle events, because when AddMethods() is called, it automatically overrides
			//	the add_ and remove_ methods for all events the interface anyway
			AddMethods( builder, interfaceType, assetProperty );
			AddProperties( builder, interfaceType, assetProperty );
		}
		
		/// <summary>
		/// Adds properties for each property in interfaceType
		/// </summary>
		private static void AddProperties( TypeBuilder builder, Type interfaceType, PropertyInfo assetProperty )
		{
			MethodAttributes propertyMethodAttrs = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

			foreach ( PropertyInfo interfaceProperty in interfaceType.GetProperties( ) )
			{
				Type[] paramTypes = GetParameterTypes( interfaceProperty.GetIndexParameters( ) );
				PropertyBuilder property = builder.DefineProperty( interfaceProperty.Name, PropertyAttributes.None, interfaceProperty.PropertyType, paramTypes );

				if ( interfaceProperty.CanRead )
				{
					property.SetGetMethod( MakeBaseCall( builder, assetProperty, interfaceProperty.GetGetMethod( ), propertyMethodAttrs, false ) );
				}

				if ( interfaceProperty.CanWrite )
				{
					property.SetSetMethod( MakeBaseCall( builder, assetProperty, interfaceProperty.GetSetMethod( ), propertyMethodAttrs, false ) );
				}
			}
		}
		/// <summary>
		/// Adds methods for each method in interfaceType.
		/// </summary>
		private static void AddMethods( TypeBuilder builder, Type interfaceType, PropertyInfo assetProperty )
		{
			foreach ( MethodInfo interfaceMethod in interfaceType.GetMethods( ) )
			{
				ParameterInfo[] parameters = interfaceMethod.GetParameters( );
				Type[] paramTypes = new Type[ parameters.Length ];

				for ( int paramIndex = 0; paramIndex < parameters.Length; ++paramIndex )
				{
					paramTypes[ paramIndex ] = parameters[ paramIndex ].ParameterType;
				}


				MakeBaseCall( builder, assetProperty, interfaceMethod );
			}
		}
		
		/// <summary>
		/// Creates a method that calls a specified interface method in the m_Base field
		/// </summary>
		private static void MakeBaseCall( TypeBuilder builder, PropertyInfo assetProperty, MethodInfo interfaceMethod )
		{
			MakeBaseCall( builder, assetProperty, interfaceMethod, MethodAttributes.Public | MethodAttributes.Virtual, true );
		}

		/// <summary>
		/// Creates a method that calls a specified interface method in the m_Base field (used for property getter/setter methods)
		/// </summary>
		private static MethodBuilder MakeBaseCall( TypeBuilder builder, PropertyInfo assetProperty, MethodInfo interfaceMethod, MethodAttributes attrs, bool defineOverride )
		{
			Type[] paramTypes = GetParameterTypes( interfaceMethod.GetParameters( ) );

			MethodBuilder method = builder.DefineMethod( interfaceMethod.Name, attrs, CallingConventions.HasThis, interfaceMethod.ReturnType, paramTypes );
			ILGenerator gen = method.GetILGenerator( );

			//	Load the asset
			gen.Emit( OpCodes.Ldarg_0 );
			gen.Emit( OpCodes.Call, assetProperty.GetGetMethod( ) );

			//	Load the method arguments
			for ( int paramCount = 0; paramCount < interfaceMethod.GetParameters( ).Length; ++paramCount )
			{
				gen.Emit( OpCodes.Ldarg, ( short )paramCount + 1 );
			}

			//	Call the interface method
			gen.Emit( OpCodes.Call, interfaceMethod );

			//	Return with whatever results
			gen.Emit( OpCodes.Ret );

			//	Define the new method as an override of the interface method, if requested
			if ( defineOverride )
			{
				builder.DefineMethodOverride( method, interfaceMethod );
			}

			return method;
		}
		
		
		/// <summary>
		/// Gets the types of the specified array of parameters
		/// </summary>
		private static Type[] GetParameterTypes( ParameterInfo[] parameters )
		{
			Type[] paramTypes = new Type[ parameters.Length ];

			for ( int paramIndex = 0; paramIndex < parameters.Length; ++paramIndex )
			{
				paramTypes[ paramIndex ] = parameters[ paramIndex ].ParameterType;
			}
			return paramTypes;
		}

		/// <summary>
		/// Sets up the module builder that will be used to create dynamic proxy types
		/// </summary>
		static AssetHandleProxy( )
		{
			AssemblyName asmName = new AssemblyName( "AssetProxies" );
			AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly( asmName, AssemblyBuilderAccess.Run );

			ms_ModuleBuilder = asmBuilder.DefineDynamicModule( "AssetProxiesModule" );

		}

		#endregion
	}
}
