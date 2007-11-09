using System.Collections.Generic;

namespace Rb.Muesli
{
    public class ObjectTable
    {
        public void Add( object obj )
        {
            if ( !m_Map.ContainsKey( obj ) )
            {
                int index = m_Map.Count;
                m_Map[ obj ] = index;
            }
        }

        public int Find( object key )
        {
            int result;
            return m_Map.TryGetValue( key, out result ) ? result : -1;
        }

        public int Count
        {
            get { return m_Map.Count; }
        }

        private readonly Dictionary< object, int > m_Map = new Dictionary< object, int >( );
    }
}
