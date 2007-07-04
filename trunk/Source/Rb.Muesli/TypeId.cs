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
        String,

        DateTime,   //< NOTE: AP: Annoyingly, DateTime doesn't have the requisite deserialization constructor, so it must be handled explicitly

        Array,
        Existing,

        Other
    }
}
