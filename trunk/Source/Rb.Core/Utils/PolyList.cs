
using System.Collections.Generic;

namespace Rb.Core.Utils
{
    /// <summary>
    /// List implementation that allows overrides of IList methods
    /// </summary>
    /// <typeparam name="T">List item type</typeparam>
    public class PolyList< T > : IList< T >
    {
        /// <summary>
        /// Empty list
        /// </summary>
        public PolyList( )
        {
            m_List = new List< T >( );
        }

        /// <summary>
        /// List filled by enumerator
        /// </summary>
        /// <param name="items">Item enumerator</param>
        public PolyList( IEnumerable< T > items )
        {
            m_List = new List< T >( items );
        }

        /// <summary>
        /// List created with specified number of default (null) items
        /// </summary>
        /// <param name="capacity">Number of items</param>
        public PolyList( int capacity )
        {
            m_List = new List< T >( capacity );
        }

        //  TODO: AP: ...

        private List< T > m_List;
    }
}