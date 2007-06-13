
using Rb.Core.Maths;

namespace Rb.World.Entities
{
    /// <summary>
    /// Entity occupying 3d space
    /// </summary>
    public class Entity3d : Entity
    {
        #region Position

        public Point3Interpolator InterpolatedPosition
        {
            get { return m_Position;  }
            set { m_Position = value; }
        }

        public Point3 NextPosition
        {
            get { return m_Position.Next; }
            set { m_Position.Next = value; }
        }

        #endregion

        private Point3Interpolator m_Position = new Point3Interpolator( );
    }
}
