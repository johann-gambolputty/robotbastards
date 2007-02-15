using System;

namespace RbEngine.Maths
{
	/// <summary>
	/// Orthonormal coordinate frame, created by evaluating a spline at a given point
	/// </summary>
	public class SplineFrame
	{
		/// <summary>
		/// Position of the frame on the spline
		/// </summary>
		/// <remarks>
		/// Position isn't really part of the frame, but it falls out of the frame calculations nicely (faster than calculating it from scratch), so it's cached here
		/// </remarks>
		public Vector3	Position
		{
			get
			{
				return m_Position;
			}
			set
			{
				m_Position = value;
			}
		}

		/// <summary>
		///	The tangent vector (normalised). Usually interpreted as the Z-axis
		/// </summary>
		public Vector3	Tangent
		{
			get
			{
				return m_Tangent;
			}
		}

		/// <summary>
		/// The binormal vector (normalised). Usually interpreted as the X-axis
		/// </summary>
		public Vector3 Binormal
		{
			get
			{
				return m_Binormal;
			}
		}

		/// <summary>
		/// The normal vector (normalised). Usually interpreted as the Y-axis
		/// </summary>
		public Vector3 Normal
		{
			get
			{
				return m_Normal;
			}
		}
		
		/// <summary>
		/// The length of the first derivative (tangent) vector, prior to normalisation
		/// </summary>
		/// <remarks>
		/// Like Position, not really part of the the frame, but is calculated during frame creation, so might as well cache it here.
		/// </remarks>
		public float Speed
		{
			get
			{
				return m_Speed;
			}
		}


		#region	Construction

		/// <summary>
		/// Sets up standard X/Y/Z axis at the origin
		/// </summary>
		public SplineFrame( )
		{
			m_Position	= new Vector3( );
			m_Tangent	= Vector3.XAxis;
			m_Normal	= Vector3.YAxis;
			m_Binormal	= Vector3.ZAxis;
			m_Speed		= 0;
		}

		/// <summary>
		/// Sets up the frame
		/// </summary>
		/// <param name="position"> Frame origin </param>
		/// <param name="velocity"> Velocity. Becomes frame tangent (normalised by constructor) </param>
		/// <param name="acceleration"> Acceleration. Used to calculate frame normal and binormal </param>
		public SplineFrame( Vector3 position, Vector3 velocity, Vector3 acceleration )
		{
			float	sqrSpeed	= velocity.SqrLength;
			float	dotVA		= velocity.Dot( acceleration );

			m_Position	= position;
			m_Tangent	= velocity.MakeNormal( );
			m_Normal	= ( acceleration * sqrSpeed ) - ( velocity * dotVA );
			m_Binormal	= Vector3.Cross( m_Tangent, m_Normal );
			m_Speed		= ( float )System.Math.Sqrt( sqrSpeed );

		//	NOTE: It's quite easy to calculate curvature here (because we've got first and second derivatives, and the square of speed, ready at hand).
		//	I've removed the calculation because it does involve a length and a cross-product, which is a bit much if the caller isn't interested in the
		//	curvature value (as is likely). Call Spline.EvaluateCurvature() instead.
		}

		/// <summary>
		/// Sets up the frame
		/// </summary>
		/// <param name="position"> Frame origin </param>
		/// <param name="tangent"> Frame tangent </param>
		/// <param name="binormal"> Frame binormal </param>
		/// <param name="normal"> Frame normal </param>
		/// <param name="speed"> Frame speed (velocity length) </param>
		public SplineFrame( Vector3 position, Vector3 tangent, Vector3 binormal, Vector3 normal, float speed )
		{
			m_Position	= position;
			m_Tangent	= tangent;
			m_Binormal	= binormal;
			m_Normal	= normal;
			m_Speed		= speed;
		}

		#endregion


		#region	Private stuff

		private Vector3	m_Position;
		private Vector3	m_Tangent;
		private Vector3 m_Binormal;
		private Vector3 m_Normal;
		private float	m_Speed;

		#endregion

	}
}
