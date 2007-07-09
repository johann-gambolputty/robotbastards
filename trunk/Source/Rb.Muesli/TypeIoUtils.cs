using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Rb.Muesli
{
	/// <summary>
	/// Utilities for <see cref="CustomTypeReaderCache"/> and <see cref="CustomTypeWriterCache"/>
	/// </summary>
	internal class TypeIoUtils
	{
		/// <summary>
		/// Generates opcodes to call a method (that takes a StreamingContext parameter and has no return value) that has a given attribute type
		/// </summary>
		/// <remarks>
		/// Used to generate method calls for OnSerializingAttribute, OnSerializedAttribute, etc.
		/// </remarks>
		public static void CallSerializationEventMethod( ILGenerator generator, OpCode pushObjectOpCode, OpCode pushStreamingContextOpCode, Type objType, Type attributeType )
		{
			foreach ( MethodInfo method in objType.GetMethods( ) ) 
			{
				if ( method.GetCustomAttributes( attributeType, false ).Length > 0 )
				{
					generator.Emit( pushObjectOpCode );
					generator.Emit( pushStreamingContextOpCode );
					generator.Emit( OpCodes.Call, method );
					return;
				}
			}
		}
	}
}