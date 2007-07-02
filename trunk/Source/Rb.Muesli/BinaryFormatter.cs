
using System.IO;

namespace Rb.Muesli
{
    public class BinaryFormatter : Formatter
    {
        protected override IInput CreateInput( Stream stream )
        {
            return new BinaryInput( stream );
        }

        protected override IOutput CreateOutput( Stream stream )
        {
            return new BinaryOutput( stream );
        }
    }
}
