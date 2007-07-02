using System;
using System.Collections.Generic;

namespace Rb.Muesli
{
    internal enum TypeId : byte
    {
        Null,
        Bool,
        Byte,
        SByte,
        Char,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Single,
        Double,
        Decimal,
        DateTime,
        String,
        Other
    }

    internal static class TypeIdUtils
    {
        public static int ReadId( IInput input )
        {
            int shift = 0;
            int result = 0;
            byte val;

            do
            {
                input.Read( out val );
                result |= ( val & ~0x80 ) << shift++;
            } while ( ( val & 0x80 ) != 0 );

            return result;
        }

        public static TypeId FromType( Type type )
        {
            //  TODO: AP: Make sure this uses the string hash code + jump table
            switch ( type.Name )
            {
                case "Boolean"  : return TypeId.Bool;
                case "Byte"     : return TypeId.Byte;
                case "SByte"    : return TypeId.SByte;
                case "Char"     : return TypeId.Char;
                case "Int16"    : return TypeId.Int16;
                case "UInt16"   : return TypeId.UInt16;
                case "Int32"    : return TypeId.Int32;
                case "UInt32"   : return TypeId.UInt32;
                case "Int64"    : return TypeId.Int64;
                case "UInt64"   : return TypeId.UInt64;
                case "Single"   : return TypeId.Single;
                case "Double"   : return TypeId.Double;
                case "Decimal"  : return TypeId.Decimal;
                case "DateTime" : return TypeId.DateTime;
            }

            return TypeId.Other;
        }
    }
}
