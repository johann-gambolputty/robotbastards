using System;
using RbEngine.Maths;

namespace RbEngine.Rendering
{
	/// <summary>
	/// A spotlight
	/// </summary>
	public class SpotLight : PointLight
	{
		/// <summary>
		/// Gets the direction of the light
		/// </summary>
		public Vector3	Direction
		{
			get
			{
				return m_Direction;
			}
			set
			{
				m_Direction = value;
			}
		}

		/// <summary>
		/// Access to the arc of the light
		/// </summary>
		public float	ArcDegrees
		{
			get
			{
				return m_Arc;
			}
			set
			{
				m_Arc = value;
			}
		}

		/// <summary>
		/// Default initialisation (spotlight at the origin, looking along zaxis)
		/// </summary>
		public SpotLight( )
		{
		}
		
		/// <summary>
		/// Setup initialisation (spotlight at pos, looking along direction)
		/// </summary>
		public SpotLight( Point3 pos, Vector3 direction )
		{
			Position	= pos;
			Direction	= direction;
		}

		/// <summary>
		/// Setup initialisation (spotlight at pos, looking towards lookAt)
		/// </summary>
		public SpotLight( Point3 pos, Point3 lookAt )
		{
			Position	= pos;
			Direction	= ( lookAt - pos ).MakeNormal( );
		}

		private Vector3	m_Direction	= Vector3.ZAxis;
		private float	m_Arc		= 90;
	}
}
