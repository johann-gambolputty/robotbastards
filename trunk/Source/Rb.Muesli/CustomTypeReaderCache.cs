using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Muesli
{
    public delegate object CustomReaderDelegate( IInput input );

    public class CustomTypeReaderCache
    {
        #region Custom reader methods for pre-defined value types

        private static object ReadBool(IInput input) { bool val; input.Read( out val ); return val; }
        private static object ReadByte( IInput input ) { byte val; input.Read( out val ); return val; }
        private static object ReadSByte( IInput input ) { sbyte val; input.Read( out val ); return val; }
        private static object ReadInt16( IInput input ) { short val; input.Read( out val ); return val; }
        private static object ReadUInt16( IInput input ) { ushort val; input.Read( out val ); return val;  }
        private static object ReadInt32( IInput input ) { int val; input.Read( out val ); return val; }
        private static object ReadUInt32( IInput input ) { uint val; input.Read( out val ); return val; }
        private static object ReadInt64( IInput input ) { long val; input.Read( out val ); return val; }
        private static object ReadUInt64( IInput input ) { ulong val; input.Read( out val ); return val;  }
        private static object ReadSingle( IInput input ) { float val; input.Read( out val ); return val;  }
        private static object ReadDouble( IInput input ) { double val; input.Read( out val ); return val; }
        private static object ReadDecimal( IInput input ) { decimal val; input.Read( out val ); return val; }
        private static object ReadString( IInput input ) { string val; input.Read( out val ); return val; }

        #endregion
    }
    }
}
