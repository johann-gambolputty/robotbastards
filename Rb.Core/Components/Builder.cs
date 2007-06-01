using System;

namespace Rb.Core.Components
{
    /// <summary>
    /// Implements a standard version of the IBuilder interface
    /// </summary>
    public class Builder : IBuilder
    {
        #region IBuilder methods

        /// <summary>
        /// Creates an instance of the specified type
        /// </summary>
        /// <param name="type">Type to create</param>
        /// <returns>Type instance</returns>
        public object CreateInstance( Type type )
        {
            return System.Activator.CreateInstance( type );
        }

        /// <summary>
        /// Creates an instance of the specified type, with a given set of constructor arguments
        /// </summary>
        /// <param name="type">Instance type</param>
        /// <returns>New instance of type</returns>
        public object CreateInstance( Type type, object[] constructorArgs )
        {
            return System.Activator.CreateInstance( type, constructorArgs );
        }

        #endregion

        #region Instance creation generic helpers

        /// <summary>
        /// Creates an instance of ObjectType from the IBuilder singleton
        /// </summary>
        /// <typeparam name="ObjectType">Object type to create</typeparam>
        /// <returns>New instance of ObjectType</returns>
        public static ObjectType CreateInstance< ObjectType >( )
        {
            return ( ObjectType )Instance.CreateInstance( typeof( ObjectType ) );
        }
        
        /// <summary>
        /// Creates an instance of ObjectType from the IBuilder singleton
        /// </summary>
        /// <typeparam name="ObjectType">Object type to create</typeparam>
        /// <param name="args">ObjectType constructor parameters</param>
        /// <returns>New instance of ObjectType</returns>
        public static ObjectType CreateInstance< ObjectType >( params object[] args )
        {
            return ( ObjectType )Instance.CreateInstance( typeof( ObjectType ), args );
        }

        #endregion

        #region Builder singleton

        /// <summary>
        /// The current IBuilder singleton
        /// </summary>
        public IBuilder Instance
        {
            get { return ms_Instance; }
            set { ms_Instance = value; }
        }

        private IBuilder ms_Instance;

        #endregion
    }
}