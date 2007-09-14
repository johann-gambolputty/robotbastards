using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Rb.Core.Assets
{
	public class AssetProxy
	{
		static AssetProxy( )
		{
			AssemblyName asmName = new AssemblyName( "AssetProxies" );
			AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly( asmName, AssemblyBuilderAccess.Run );

			ms_ModuleBuilder = asmBuilder.DefineDynamicModule( "AssetProxiesModule" );
			
		}

		public static object CreateProxy( Type interfaceType, object baseObject )
		{
			Type proxyType;
			if ( ms_ProxyTypes.TryGetValue( interfaceType, out proxyType ) )
			{
				return Activator.CreateInstance( proxyType, baseObject );
			}

			string typeName = interfaceType.Name + "AssetProxy";
			TypeAttributes attributes = TypeAttributes.Public | TypeAttributes.Class;
			TypeBuilder builder = ms_ModuleBuilder.DefineType( typeName, attributes, typeof( AssetHandle ), new Type[] { interfaceType } );

			builder.AddInterfaceImplementation( interfaceType );

			FieldInfo baseField = AddFields( builder, interfaceType );
			AddConstructor( builder, interfaceType, baseField );
			AddMethods( builder, interfaceType, baseField );
			AddProperties( builder, interfaceType, baseField );

			try
			{
				proxyType = builder.CreateType( );
			}
			catch ( Exception ex )
			{
				
			}
			ms_ProxyTypes.Add( interfaceType, proxyType );

			return Activator.CreateInstance( proxyType, baseObject );
		}

		private static FieldInfo AddFields( TypeBuilder builder, Type interfaceType )
		{
			return builder.DefineField( "m_Base", interfaceType, FieldAttributes.Private );
		}

		private static Type[] GetParameterTypes( ParameterInfo[] parameters )
		{
			Type[] paramTypes = new Type[ parameters.Length ];

			for ( int paramIndex = 0; paramIndex < parameters.Length; ++paramIndex )
			{
				paramTypes[ paramIndex ] = parameters[ paramIndex ].ParameterType;
			}
			return paramTypes;
		}

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

		private static MethodBuilder MakeBaseCall( TypeBuilder builder, FieldInfo baseField, MethodInfo interfaceMethod )
		{
			return MakeBaseCall( builder, baseField, interfaceMethod, MethodAttributes.Public | MethodAttributes.Virtual, true );
		}

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

		private static void AddConstructor( TypeBuilder builder, Type interfaceType, FieldInfo baseField )
		{
			ConstructorBuilder conBuilder = builder.DefineConstructor( MethodAttributes.Public, CallingConventions.HasThis, new Type[] { interfaceType } );
			ILGenerator gen = conBuilder.GetILGenerator( );

			gen.Emit( OpCodes.Ldarg_0 );
			gen.Emit( OpCodes.Ldarg_1 );
			gen.Emit( OpCodes.Stfld, baseField );
			gen.Emit( OpCodes.Ret );
		}

		private static readonly Dictionary< Type, Type > ms_ProxyTypes = new Dictionary< Type, Type >( );
		private static readonly ModuleBuilder ms_ModuleBuilder;
	}
}
