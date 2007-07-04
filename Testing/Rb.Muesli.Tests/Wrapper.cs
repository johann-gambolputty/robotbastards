using System;

namespace Rb.Muesli.Tests
{
        [Serializable]
        class Wrapper : IEquatable< Wrapper >
        {
            public Primitives m_Object0 = new Primitives( );
            public DateTime m_Now = DateTime.Now;
            public Guid m_Guid = Guid.NewGuid( );

            #region IEquatable<WrapperTest> Members

            public bool Equals( Wrapper other )
            {
                return ( m_Now == other.m_Now ) && ( m_Object0.Equals( other.m_Object0 ) );
            }

            #endregion
        }

}
