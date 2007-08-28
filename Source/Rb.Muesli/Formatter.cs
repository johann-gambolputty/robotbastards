using System.Runtime.Serialization;
using System.IO;

namespace Rb.Muesli
{
    public abstract class Formatter : IFormatter
    {
		public Formatter( )
		{
		}

		public Formatter( ISurrogateSelector selector )
		{
			m_Selector = selector;
		}

        #region Protected stuff

        protected abstract IInput   CreateInput( Stream stream );
        protected abstract IOutput  CreateOutput( Stream stream );

    	protected ISurrogateSelector Selector
    	{
    		get { return m_Selector; }
    	}

        #endregion

		#region Private stuff

    	private readonly ISurrogateSelector m_Selector;

		#endregion

		#region IFormatter Members

		public SerializationBinder Binder
        {
            get
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }

        public StreamingContext Context
        {
            get
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }

        public object Deserialize( Stream serializationStream )
        {
            IInput input = new BinaryInput( serializationStream );
                
            object result;
            input.Read( out result );

			input.Finish( );

            return result;
        }

        public void Serialize( Stream serializationStream, object graph )
        {
            IOutput output = new BinaryOutput( serializationStream );
            output.Write(graph);
            output.Finish( );
        }

        public ISurrogateSelector SurrogateSelector
        {
            get
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new System.Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}
