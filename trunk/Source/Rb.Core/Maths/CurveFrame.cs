using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Orthonormal coordinate frame, created by evaluating a curve at a given point
	/// </summary>
	public class CurveFrame : Matrix44
	{
		/// <summary>
		/// The length of the first derivative (tangent) vector, prior to normalisation
		/// </summary>
		/// <remarks>
		/// Not really part of the the frame, but is calculated during frame creation, so might as well cache it here.
		/// </remarks>
		public float Speed
		{
			get { return m_Speed; }
		}

		/// <summary>
		/// Synonym for frame translation
		/// </summary>
		public Point3 Position
		{
			get { return Translation; }
			set { Translation = value; }
		}

		/// <summary>
		/// Synonym for frame X axis
		/// </summary>
		/// <remarks>
		/// Be careful when setting this to ensure that Tangent, Normal, and Binormal are orthonormal
		/// </remarks>
		public Vector3 Tangent
		{
			get { return XAxis; }
			set { XAxis = value; }
		}

		/// <summary>
		/// Synonym for frame Y axis
		/// </summary>
		/// <remarks>
		/// Be careful when setting this to ensure that Tangent, Normal, and Binormal are orthonormal
		/// </remarks>
		public Vector3 Normal
		{
			get { return YAxis; }
			set { YAxis = value; }
		}

		/// <summary>
		/// Synonym for frame Z axis
		/// </summary>
		/// <remarks>
		/// Be careful when setting this to ensure that Tangent, Normal, and Binormal are orthonormal
		/// </remarks>
		public Vector3 Binormal
		{
			get { return ZAxis; }
			set { ZAxis = value; }
		}

		#region	Construction

		/// <summary>
		/// Sets up standard X/Y/Z axis at the origin
		/// </summary>
		public CurveFrame( )
		{
			m_Speed	= 0;
		}

		/// <summary>
		/// Sets up the frame
		/// </summary>
		/// <param name="position"> Frame origin </param>
		/// <param name="velocity"> Velocity. Becomes frame tangent (normalised by constructor) </param>
		/// <param name="acceleration"> Acceleration. Used to calculate frame normal and binormal </param>
		public CurveFrame( Point3 position, Vector3 velocity, Vector3 acceleration )
		{
			float	sqrSpeed	= velocity.SqrLength;
			float	dotVA		= velocity.Dot( acceleration );

			Translation	= position;
			XAxis		= velocity.MakeNormal( );
			YAxis		= ( acceleration * sqrSpeed ) - ( velocity * dotVA );
			ZAxis		= Vector3.Cross( XAxis, YAxis );
			m_Speed		= Functions.Sqrt( sqrSpeed );

		//	NOTE: It's quite easy to calculate curvature here (because we've got first and second derivatives, and the square of speed, ready at hand).
		//	I've removed the calculation because it does involve a length and a cross-product, which is a bit much if the caller isn't interested in the
		//	curvature value (as is likely). Call Curve.EvaluateCurvature() instead.
		}

		/// <summary>
		/// Sets up the frame
		/// </summary>
		/// <param name="position"> Frame origin </param>
		/// <param name="tangent"> Frame tangent </param>
		/// <param name="binormal"> Frame binormal </param>
		/// <param name="normal"> Frame normal </param>
		/// <param name="speed"> Frame speed (velocity length) </param>
		public CurveFrame( Point3 position, Vector3 tangent, Vector3 binormal, Vector3 normal, float speed ) :
			base( position, tangent, binormal, normal )
		{
			m_Speed	= speed;
		}

		#endregion

		private readonly float m_Speed;

	}
}
