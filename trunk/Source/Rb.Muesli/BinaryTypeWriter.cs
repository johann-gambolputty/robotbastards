using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    /*
     * Design:
     *  - ISerializable objects should be able to write using standard techniques
     *      - Reading and writing a specific type to the SerializationContext should be in order
     *      - Writing a specific value type should not write the type ID to the stream
     *      - Writing a generic object should write a type ID to the stream
     *  - [Serializable] objects should work as normal
     */
    public class BinaryTypeWriter : ITypeWriter
    {
        public BinaryTypeWriter( )
        {
            
        }

        #region ITypeWriter Members

        public void WriteHeader( System.IO.Stream stream )
        {
            //  TODO: AP: Write type GUIDs ordered by type IDs
        }

        public void Write( IOutput output, object obj )
        {
            if ( obj == null )
            {
                output.WriteNull( );
                return;
            }

            //  TODO: AP: Check if obj exists. If it does, write a hardcoded type (Existing), followed by object ID

            CustomWriter writer = GetCustomWriter( obj.GetType( ) );
            output.WriteTypeId( writer.m_TypeId );
            writer.m_Writer( output, obj );
        }

        #endregion

        private struct CustomWriter
        {
            public CustomWriterDelegate m_Writer;
            public int m_TypeId;

            public CustomWriter( int typeId, CustomWriterDelegate writer )
            {
                m_Writer = writer;
                m_TypeId = typeId;
            }
        }



        private CustomWriter GetCustomWriter(Type type)
        {
            CustomWriter writer;
            if ( !m_TypeIds.TryGetValue( type, out writer ) )
            {
                writer = new CustomWriter( m_TypeIds.Count, CustomTypeWriterCache.Instance.GetWriter( type ) );
                m_TypeIds[ type ] = new CustomWriter( id, writer );
            }
            return writer;
        }

        private Dictionary< Type, CustomWriter > m_TypeIds = new Dictionary< Type, CustomWriter >( );
    }
}
