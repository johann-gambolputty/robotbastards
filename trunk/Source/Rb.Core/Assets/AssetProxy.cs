using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Generates dynamic types that provide <see cref="AssetHandle"/> proxies for interfaces
	/// </summary>
	/// <remarks>
	/// An explicit example of an asset handle proxy is Rb.Rendering.RenderableAssetHandle. This implements
	/// the IRenderable interface, but defers all calls to another IRenderable object, that is loaded from an asset.
	/// This is particularly useful for serialization.
	/// </remarks>
	public class AssetProxy
	{

		/// <summary>
		/// Creates a proxy for the specified interface type
		/// </summary>
		/// <param name="interfaceType">Interface to create a proxy for</param>
		/// <param name="baseObject">Implementation object. Must implement the interface specified by interfaceType</param>
		/// <returns>Returns a new proxy object that implements the interface specified by interfaceType. All methods,
		/// properties and events are deferred to baseObject.</returns>
		public static object CreateProxy( Type interfaceType, object baseObject )
		{
			//	Does a proxy type already exist for interfaceType?
			Type proxyType;
			if ( !ms_ProxyTypes.TryGetValue( interfaceType, out proxyType ) )
			{
				//	No - create the proxy type
				proxyType = MakeProxy( interfaceType );
			}

			//	Create an instance of the new proxy type
			return Activator.CreateInstance( proxyType, baseObject );
		}

		#region Private stuff
		
		private static readonly Dictionary< Type, Type > ms_ProxyTypes = new Dictionary< Type, Type >( );
		private static readonly ModuleBuilder ms_ModuleBuilder;

		/// <summary>
		/// Makes a proxy type for the specified interface type
		/// </summary>
		private static Type MakeProxy( Type interfaceType )
		{
			//	Create a new type, derived from AssetHandle, and implementing interfaceType
			string typeName = interfaceType.Name + "AssetProxy";
			TypeAttributes attributes = TypeAttributes.Public | TypeAttributes.Class;
			TypeBuilder builder = ms_ModuleBuilder.DefineType( typeName, attributes, typeof( AssetHandle ), new Type[] { interfaceType } );

			builder.AddInterfaceImplementation( interfaceType );

			//	Add "m_Base" field that stores baseObject
			FieldInfo baseField = AddFields( builder, interfaceType );

			AddConstructor( builder, interfaceType, baseField );	//	Adds a constructor that stores baseObject in m_Base

			AddMethods( builder, interfaceType, baseField );		//	Adds method implementations for each method in interfaceType
			AddProperties( builder, interfaceType, baseField );		//	Adds property implementations for each property in interfaceType
			AddEvents( builder, interfaceType );					//	Adds event implementations for each event in interfaceType

			foreach ( Type baseInterface in interfaceType.GetInterfaces( ) )
			{
				AddMethods( builder, baseInterface, baseField );	//	Adds method implementations for each method in baseInterface
				AddProperties( builder, baseInterface, baseField );	//	Adds property implementations for each property in baseInterface
				AddEvents( builder, baseInterface );				//	Adds event implementations for each event in baseInterface
			}

			//	Create the type, and add it to the proxy type dictionary
			Type proxyType = builder.CreateType( );
			ms_ProxyTypes.Add( interfaceType, proxyType );

			return proxyType;
		}

		/// <summary>
		/// Adds events for each event in interfaceType
		/// </summary>
		private static void AddEvents( TypeBuilder builder, Type interfaceType )
		{
			foreach ( EventInfo interfaceEvent in interfaceType.GetEvents( ) )
			{
				builder.DefineEvent( interfaceEvent.Name, interfaceEvent.Attributes, interfaceEvent.EventHandlerType );

				//	NOTE: AP: I don't frikkin' know why this (incomplete) code isn't necessary - bafflingly, just defining
				//	the proxy's event magically defers to m_Base's implementation of the event!!! CRAZY!

				//string addMethodName = "eventAdd_" + interfaceEvent.Name;
				//string removeMethodName = "eventRemove_" + interfaceEvent.Name;
				//string raiseMethodName = "eventRaise_" + interfaceEvent.Name;

				//MethodAttributes attr = MethodAttributes.Private;
				//CallingConventions call = CallingConventions.HasThis;
				//Type returnType = typeof( void );
				//Type[] paramTypes = new Type[] { interfaceEvent.EventHandlerType };
				//Type[] raiseParamTypes = new Type[] {}; //GetParameterTypes( interfaceEvent.EventHandlerType );

				//MethodBuilder addMethod = builder.DefineMethod( addMethodName, attr, call, returnType, paramTypes );
				//MethodBuilder removeMethod = builder.DefineMethod( removeMethodName, attr, call, returnType, paramTypes  );
				//MethodBuilder raiseMethod = builder.DefineMethod( raiseMethodName, attr, call, returnType, raiseParamTypes );

				//ILGenerator addGen = addMethod.GetILGenerator( );
				//addGen.Emit( OpCodes.Ret );

				//ILGenerator removeGen = removeMethod.GetILGenerator( );
				//removeGen.Emit( OpCodes.Ret );

				//ILGenerator raiseGen = raiseMethod.GetILGenerator( );
				//raiseGen.Emit( OpCodes.Ret );

				//eventBuilder.SetAddOnMethod( addMethod );
				//eventBuilder.SetRemoveOnMethod( removeMethod );
				//eventBuilder.SetRaiseMethod( raiseMethod );
			}
		}

		/// <summary>
		/// Adds an "m_Base" field to the specified type builder
		/// </summary>
		private static FieldInfo AddFields( TypeBuilder builder, Type interfaceType )
		{
			return builder.DefineField( "m_Base", interfaceType, FieldAttributes.Private );
		}

		/// <summary>
		/// Adds properties for each property in interfaceType
		/// </summary>
		private static void AddProperties( TypeBuilder builder, Type interfaceType, FieldInfo baseField )
		{
			MethodAttributes propertyMethodAttrs = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

			foreach ( PropertyInfo interfaceProperty in interfaceType.GetProperties( ) )
			{
				Type[] paramTypes = GetParameterTypes( interfaceProperty.GetIndexParameters( ) );
				PropertyBuilder property = builder.DefineProperty( interfaceProperty.Name, PropertyAttributes.None, interfaceProperty.PropertyType, paramTypes );

				if ( interfaceProperty.CanRead )
				{
					property.SetGetMethod( MakeBaseCall( builder, baseField, interfaceProperty.GetGetMethod( ), propertyMethodAttrs, false ) );
				}

				if ( interfaceProperty.CanWrite )
				{
					property.SetSetMethod( MakeBaseCall( builder, baseField, interfaceProperty.GetSetMethod( ), propertyMethodAttrs, false ) );
				}
			}
		}
		
		/// <summary>
		/// Adds methods for each method in interfaceType.
		/// </summary>
		private static void AddMethods( TypeBuilder builder, Type interfaceType, FieldInfo baseField )
		{
			foreach ( MethodInfo interfaceMethod in interfaceType.GetMethods( ) )
			{
				ParameterInfo[] parameters = interfaceMethod.GetParameters( );
				Type[] paramTypes = new Type[ parameters.Length ];

				for ( int paramIndex = 0; paramIndex < parameters.Length; ++paramIndex )
				{
					paramTypes[ paramIndex ] = parameters[ paramIndex ].ParameterType;
				}


				MakeBaseCall( builder, baseField, interfaceMethod );
			}
		}

		/// <summary>
		/// Adds a constructor that stores the input argument in "m_Base"
		/// </summary>
		private static void AddConstructor( TypeBuilder builder, Type interfaceType, FieldInfo baseField )
		{
			ConstructorBuilder conBuilder = builder.DefineConstructor( MethodAttributes.Public, CallingConventions.HasThis, new Type[] { interfaceType } );
			ILGenerator gen = conBuilder.GetILGenerator( );

			gen.Emit( OpCodes.Ldarg_0 );
			gen.Emit( OpCodes.Ldarg_1 );
			gen.Emit( OpCodes.Stfld, baseField );
			gen.Emit( OpCodes.Ret );
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
		/// Creates a method that calls a specified interface method in the m_Base field
		/// </summary>
		private static void MakeBaseCall( TypeBuilder builder, FieldInfo baseField, MethodInfo interfaceMethod )
		{
			MakeBaseCall( builder, baseField, interfaceMethod, MethodAttributes.Public | MethodAttributes.Virtual, true );
		}

		/// <summary>
		/// Creates a method that calls a specified interface method in the m_Base field (used for property getter/setter methods)
		/// </summary>
		private static MethodBuilder MakeBaseCall( TypeBuilder builder, FieldInfo baseField, MethodInfo interfaceMethod, MethodAttributes attrs, bool defineOverride )
		{
			Type[] paramTypes = GetParameterTypes( interfaceMethod.GetParameters( ) );

			MethodBuilder method = builder.DefineMethod( interfaceMethod.Name, attrs, CallingConventions.HasThis, interfaceMethod.ReturnType, paramTypes );
			ILGenerator gen = method.GetILGenerator( );

			gen.Emit( OpCodes.Ldarg_0 );
			gen.Emit( OpCodes.Ldfld, baseField );

			for ( int paramCount = 0; paramCount < interfaceMethod.GetParameters( ).Length; ++paramCount )
			{
				gen.Emit( OpCodes.Ldarg, ( short )paramCount + 1 );
			}

			gen.Emit( OpCodes.Call, interfaceMethod );

			gen.Emit( OpCodes.Ret );

			if ( defineOverride )
			{
				builder.DefineMethodOverride( method, interfaceMethod );
			}

			return method;
		}
		
		/// <summary>
		/// Sets up the module builder that will be used to create dynamic proxy types
		/// </summary>
		static AssetProxy( )
		{
			AssemblyName asmName = new AssemblyName( "AssetProxies" );
			AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly( asmName, AssemblyBuilderAccess.Run );

			ms_ModuleBuilder = asmBuilder.DefineDynamicModule( "AssetProxiesModule" );

		}

		#endregion
	}
}
