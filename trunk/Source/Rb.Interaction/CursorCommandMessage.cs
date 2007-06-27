using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Interaction
{
    /// <summary>
    /// Cursor pick intersection command message
    /// </summary>
    [Serializable]
    public class CursorCommandMessage : CommandMessage
    {
        public CursorCommandMessage( Command cmd, int lastX, int lastY, int x, int y ) :
            base( cmd )
        {
            m_LastX = lastX;
            m_LastY = lastY;
            m_X     = x;
            m_Y     = y;
        }

        public int LastX
        {
            get { return m_LastX; }
        }

        public int LastY
        {
            get { return m_LastY; }
        }

        public int X
        {
            get { return m_X; }
        }

        public int Y
        {
            get { return m_Y; }
        }

        private int m_LastX;
        private int m_LastY;
        private int m_X;
        private int m_Y;
    }
}
