using System;
using Rb.Core.Maths;

namespace Rb.Interaction
{
    /// <summary>
    /// Ray pick intersection command message
    /// </summary>
    [Serializable]
    public class PickCommandMessage : CommandMessage
    {
		/// <summary>
		/// Setup constructor
		/// </summary>
		public PickCommandMessage( Command cmd, Ray3Intersection intersection ) :
			base( cmd )
		{
			m_Intersection = intersection;
		}

		/// <summary>
		/// Gets the pick intersection
		/// </summary>
		public Ray3Intersection Intersection
		{
			get { return m_Intersection; }
		}

		private Ray3Intersection m_Intersection;
    }
}
