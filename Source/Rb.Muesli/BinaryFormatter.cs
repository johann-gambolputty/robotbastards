
using System.IO;
using System.Runtime.Serialization;

namespace Rb.Muesli
{
    public class BinaryFormatter : Formatter
    {
		public BinaryFormatter( )
		{
		}

		public BinaryFormatter( ISurrogateSelector selector ) :
			base( selector )
		{	
		}

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
