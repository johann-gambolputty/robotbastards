using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Rb.Core.Utils
{
    /// <summary>
    /// Marks a method as a dispatch method
    /// </summary>
    public class DispatchAttribute : Attribute
    {
    }

    //  TODO: AP: Add DispatchMap class parameterised by return type

    /// <summary>
    /// Dispatch map
    /// </summary>
    public class DispatchMap
    {
        /// <summary>
        /// Gets a DispatchMap for type t
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>Returns a DispatchMap for type t</returns>
        public static DispatchMap Get( Type t )
        {
            DispatchMap map = null;
            if ( ms_Maps.ContainsKey( t ) )
            {
				map = ms_Maps[ t ];
			}
			else
			{
                map = new DispatchMap( t );
                ms_Maps[ t ] = map;
            }
            return map;
        }

        /// <summary>
        /// Thread-safe access to the DispatchMap for type t
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>Returns a DispatchMap for type t</returns>
        public static DispatchMap SafeGet( Type t )
        {
            DispatchMap map = null;
            lock ( ms_Maps )
            {
                map = Get( t );
            }
            return map;
        }

        /// <summary>
        /// Dispatch delegate type
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <param name="arg">Argument object</param>
        /// <returns>Returns the result of the dispatch handler method</returns>
        public delegate object DispatchDelegate( object obj, object arg );

        /// <summary>
        /// Calls the appropriate dispatch method, given the two arguments
        /// </summary>
        public DispatchDelegate Dispatch;

        #region Private stuff

		/// <summary>
		/// Compares two methods that take 1 parameter.
		/// </summary>
        private class MethodComparer : IComparer< MethodInfo >
        {
            public int Compare( MethodInfo lhs, MethodInfo rhs )
            {
                Type lhsType = lhs.GetParameters( )[ 0 ].ParameterType;
				Type rhsType = rhs.GetParameters( )[ 0 ].ParameterType;

                if ( lhsType.IsSubclassOf( rhsType ) )
                {
                    return -1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Builds a dispatch map for type t
        /// </summary>
        /// <param name="t">Type</param>
        private DispatchMap( Type t )
        {
            ComponentLog.Info( "Building dispatch map for type \"{0}\"", t );

			DynamicMethod dispatchMethod = new DynamicMethod( "Dispatch", typeof( object ), new Type[ ] { typeof( object ), typeof( object ) }, t, true );
			ILGenerator generator = dispatchMethod.GetILGenerator( );

			Build( t, generator );

			generator.Emit( OpCodes.Ldnull );
			generator.Emit( OpCodes.Ret );

			Dispatch = ( DispatchDelegate )dispatchMethod.CreateDelegate( typeof( DispatchDelegate ) );
        }

		private void Build( Type t, ILGenerator generator )
		{
			//  Get the list of dispatch methods in the specified type
			MethodInfo[ ] methods = GetDispatchMethods( t.GetMethods( ) ).ToArray( );
			if ( methods.Length > 0 )
			{
				Type returnType = methods[ 0 ].ReturnType;

				// Sort the array, so the method with the most derived parameter type comes first
				Array.Sort( methods, new MethodComparer( ) );

				//  Build the dispatcher
				foreach ( MethodInfo method in methods )
				{
					AddHandlerMethod( method, generator, returnType );
				}
			}

			Type subclass = t.BaseType;
			if ( ( subclass != null ) && ( subclass != typeof( object ) ) )
			{
				Build( subclass, generator );
			}
		}

        /// <summary>
        /// Validates a dispatch method
        /// </summary>
        /// <param name="returnType">Expected return type</param>
        /// <param name="method">Method to evaluate</param>
        private static void ValidateDispatchMethod( Type returnType, MethodInfo method )
        {
            ParameterInfo[] parameters = method.GetParameters( );
            if ( parameters.Length != 1 )
            {
				throw new ApplicationException( string.Format( "Dispatch method \"{0}\" can have only 1 parameter", method ) );
            }
            Type paramType = parameters[ 0 ].ParameterType;
            if ( method.ReturnType != returnType )
            {
				throw new ApplicationException( string.Format( "Dispatch method \"{0}\" did not have the consistent return type (\"{1}\")", method, returnType ) );
            }
        }

        /// <summary>
        /// Builds a list of the dispatch methods in a set of methods
        /// </summary>
        /// <param name="allMethods">Method set</param>
        /// <returns>Subset of methods that have the DispatchAttribute attribute</returns>
        private static List< MethodInfo > GetDispatchMethods( MethodInfo[] allMethods )
        {
            List< MethodInfo > dispatchMethods = new List< MethodInfo >( );

            Type returnType = null;

            foreach ( MethodInfo method in allMethods )
            {
                object[] dispatchAttributes = method.GetCustomAttributes( typeof( DispatchAttribute ), false );
                if ( dispatchAttributes.Length == 1 )
                {
                    if ( returnType == null )
                    {
                        returnType = method.ReturnType;
                    }

                    ValidateDispatchMethod( returnType, method );
                    dispatchMethods.Add( method );
                }
            }

            return dispatchMethods;
        }

        /// <summary>
        /// Adds a handler method.
        /// </summary>
        /// <param name="method">Handler method</param>
        /// <param name="generator">IL generator for main handler method</param>
        /// <param name="returnType">The return type of the previous handler method (null if this is the first)</param>
        private static void AddHandlerMethod( MethodInfo method, ILGenerator generator, Type returnType )
        {
			Type paramType = method.GetParameters( )[ 0 ].ParameterType;
            ComponentLog.Verbose( "Building calling code for dispatch method \"{0}\". Dispatch type is \"{1}\"", method, paramType );

            generator.Emit( OpCodes.Ldarg_1 );                  //  Load argument 1 (the argument) onto the stack
            generator.Emit( OpCodes.Isinst, paramType );        //  Test if the argument is the right type (null or object pushed)

            Label isNullBranch = generator.DefineLabel( );
			generator.Emit( OpCodes.Brfalse, isNullBranch );    //  If the argument cast returned null, then jump to the null branch

            if ( method.IsStatic )
            {
                generator.Emit( OpCodes.Ldarg_1 );              //  Load argument 1
                generator.Emit( OpCodes.Call, method );         //  Call handler
            }
            else
            {
                generator.Emit( OpCodes.Ldarg_0 );              //  Load argument 0 (this)
                generator.Emit( OpCodes.Ldarg_1 );              //  Load argument 1

                if ( method.IsVirtual || method.IsAbstract )
                {
                    generator.Emit( OpCodes.Callvirt, method ); //  Call handler (virtual)
                }
                else
                {
                    generator.Emit( OpCodes.Call, method );     //  Call handler (non-virtual)
                }
            }

            //  Got to return something...
            if ( returnType == typeof( void ) )
            {
                generator.Emit( OpCodes.Ldnull );               //  Load null onto the stack
            }
			else if ( returnType.IsValueType )
			{
				generator.Emit( OpCodes.Box, returnType );		//	Box value types
			}

            generator.Emit( OpCodes.Ret );                      //  Return instruction
            generator.MarkLabel( isNullBranch );                //  Mark the null branch label
        }

        private static readonly Dictionary< Type, DispatchMap > ms_Maps = new Dictionary< Type, DispatchMap >( );

        #endregion

    }

    /// <summary>
    /// Caches a DispatchMap for a given type T
    /// </summary>
    /// <typeparam name="T">The DispatchMap for this type is built and stored in Instance</typeparam>
    public class DispatchMapT< T >
    {
        /// <summary>
        /// Gets the DispatchMap for type T
        /// </summary>
        public static DispatchMap Instance
        {
            get { return ms_Instance; }
        }

        private static DispatchMap ms_Instance = DispatchMap.SafeGet( typeof( T ) );
    }

}
