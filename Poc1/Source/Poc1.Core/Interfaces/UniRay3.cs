using Rb.Core.Maths;

namespace Poc1.Core.Interfaces
{
	/// <summary>
	/// Universe ray
	/// </summary>
	public class UniRay3
	{
		#region Construction
		
		/// <summary>
		/// Default constructor. Places the ray at the origin, pointing along the z axis
		/// </summary>
		public UniRay3( )
		{
			m_Origin = new UniPoint3( );
			m_Direction = Vector3.ZAxis;
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		public UniRay3( UniPoint3 origin, Vector3 dir )
		{
			m_Origin = origin;
			m_Direction = dir;
		}

		#endregion

		/// <summary>
		/// Gets/sets the origin of this ray
		/// </summary>
		public UniPoint3 Origin
		{
			get { return m_Origin; }
			set { m_Origin = value; }
		}

		/// <summary>
		/// Gets/sets the direction of this ray
		/// </summary>
		public Vector3 Direction
		{
			get { return m_Direction; }
			set { m_Direction = value; }
		}

		#region Private Members

		private UniPoint3 m_Origin;
		private Vector3 m_Direction;

		#endregion
	}
}
