using System;

namespace Rb.Core.Components
{
    /// <summary>
    /// Object builder interface
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Creates an instance of the specified type
        /// </summary>
        /// <param name="type">Instance type</param>
        /// <returns>New instance of type</returns>
        /// <remarks>
        /// <seealso cref="Builder.CreateInstance(Type, object[])"/>
        /// </remarks>
        object CreateInstance( Type type );

        /// <summary>
        /// Creates an instance of the specified type, with a given set of constructor arguments
        /// </summary>
        /// <param name="type">Instance type</param>
        /// <param name="constructorArgs">Argument passed to object constructor</param>
        /// <returns>New instance of type</returns>
        /// <remarks>
        /// <seealso cref="Builder.CreateInstance(Type)"/>
        /// </remarks>
        object CreateInstance( Type type, object[] constructorArgs );
    }
}