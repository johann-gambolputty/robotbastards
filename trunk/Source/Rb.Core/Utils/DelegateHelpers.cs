using System;
using System.Reflection;

namespace Rb.Core.Utils
{
	/// <summary>
	/// Delegate helper methods
	/// </summary>
	public static class DelegateHelpers
	{
		/// <summary>
		/// Checks whether or not an array of argument types to be passed to a delegate have valid types
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if one of the arguments has an invalid type</exception>
		public static void ValidateDelegateArgumentTypes( Delegate del, Type[] argTypes )
		{
			if ( del == null )
			{
				throw new ArgumentNullException( "del" );
			}
			ParameterInfo[] parameters = del.Method.GetParameters( );
			int argTypesLength = argTypes == null ? 0 : argTypes.Length;
			if ( parameters.Length < argTypesLength )
			{
				throw new ArgumentException( string.Format( "Argument list passed to delegate {0} is too long. Was {1}, should be {2}.", del.Method.Name, argTypesLength, parameters.Length ) );
			}
			if ( parameters.Length > argTypesLength )
			{
				throw new ArgumentException( string.Format( "Not enough arguments passed to delegate {0}. Was {1}, should be {2}.", del.Method.Name, argTypesLength, parameters.Length ) );	
			}
			if ( argTypesLength == 0 )
			{
				return;
			}
			System.Diagnostics.Debug.Assert( argTypes != null );
			for ( int parameterIndex = 0; parameterIndex < parameters.Length; ++parameterIndex )
			{
				Type expectedType = parameters[ parameterIndex ].ParameterType;
				Type actualType = argTypes[ parameterIndex ];
				if ( !expectedType.IsAssignableFrom( actualType ) )
				{
					string msg = string.Format( "Invalid argument passed to delegate {0}. Expected argument {1} type to be assignable to parameter type {2}, was {3}", del.Method.Name, parameterIndex, expectedType, actualType );
					throw new ArgumentException( msg, "arg" + parameterIndex );
				}
			}
		}

		/// <summary>
		/// Checks whether or not an array of arguments to be passed to a delegate have valid types
		/// </summary>
		public static void ValidateDelegateArguments( Delegate del, object[] args )
		{
			Type[] argTypes = args == null ? null : Type.GetTypeArray( args );
			ValidateDelegateArgumentTypes( del, argTypes );
		}
	}
}
