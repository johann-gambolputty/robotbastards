
namespace Rb.World.Entities
{
	/// <summary>
	/// Jump movement request
	/// </summary>
    public class JumpRequest : MovementRequest
	{
        /// <summary>
        /// Initialises the request
        /// </summary>
        /// <param name="movement">Underlying movement</param>
        public JumpRequest( MovementRequest movement )
        {
            m_Movement = movement;
        }

        /// <summary>
        /// Gets the movement implied by the jump
        /// </summary>
	    public MovementRequest Movement
	    {
            get { return m_Movement; }
	    }

        /// <summary>
        /// Gets the distance covered by the movement
        /// </summary>
        public override float Distance
        {
            get { return m_Movement.Distance; }
        }

	    private MovementRequest m_Movement;
	}
}
