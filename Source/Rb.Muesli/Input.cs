using System;

namespace Rb.Muesli
{
    /// <summary>
    /// IInput helpers
    /// </summary>
    public static class Input
    {
        public static Boolean ReadBoolean( IInput input )
        {
            Boolean value;
            input.Read( out value );
            return value;
        }

        public static Byte ReadByte(IInput input)
        {
            Byte value;
            input.Read(out value);
            return value;
        }

        public static SByte ReadSByte(IInput input)
        {
            SByte value;
            input.Read(out value);
            return value;
        }

        public static Char ReadChar(IInput input)
        {
            Char value;
            input.Read(out value);
            return value;
        }

        public static Int16 ReadInt16(IInput input)
        {
            Int16 value;
            input.Read(out value);
            return value;
        }

        public static UInt16 ReadUInt16(IInput input)
        {
            UInt16 value;
            input.Read(out value);
            return value;
        }

        public static Int32 ReadInt32(IInput input)
        {
            Int32 value;
            input.Read(out value);
            return value;
        }

        public static UInt32 ReadUInt32(IInput input)
        {
            UInt32 value;
            input.Read(out value);
            return value;
        }

        public static Int64 ReadInt64(IInput input)
        {
            Int64 value;
            input.Read(out value);
            return value;
        }

        public static UInt64 ReadUInt64(IInput input)
        {
            UInt64 value;
            input.Read(out value);
            return value;
        }

        public static Single ReadSingle(IInput input)
        {
            Single value;
            input.Read(out value);
            return value;
        }

        public static Double ReadDouble(IInput input)
        {
            Double value;
            input.Read(out value);
            return value;
        }

        public static Decimal ReadDecimal(IInput input)
        {
            Decimal value;
            input.Read(out value);
            return value;
        }

        public static DateTime ReadDateTime(IInput input)
        {
            DateTime value;
            input.Read( out value );
            return value;
        }
        
        public static T[] ReadArray< T >(IInput input)
        {
            T[] value;
            input.Read( out value );
            return value;
        }

        public static Guid ReadGuid(IInput input)
        {
            Guid value;
            input.Read(out value);
            return value;
        }

        public static String ReadString(IInput input)
        {
            String value;
            input.Read(out value);
            return value;
        }
    }
}
