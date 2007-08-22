
namespace Rb.Core.Maths
{
	/// <summary>
	/// Abstract base class for curves (Maybe inherit from "Path" or similar?)
	/// </summary>
	public abstract class Curve
	{
		#region	Curve alteration

		/// <summary>
		/// OnChangedEvent delegate type
		/// </summary>
		public delegate void ChangedDelegate( Curve curve );

		/// <summary>
		/// Change event, invoked by OnChanged() (called whenever the curve data is altered)
		/// </summary>
		public event ChangedDelegate OnChangedEvent;

		/// <summary>
		/// Call to invoke the OnChangedEvent
		/// </summary>
		public virtual void OnChanged( )
		{
			//	Recalculate the length of the curve
			m_Length = GetSegmentLength( StartT, EndT );
			OnChangedEvent( this );
		}

		#endregion

		#region	Point evaluation

		/// <summary>
		/// Calculates the position on the curve at fraction t
		/// </summary>
		public abstract Point3 EvaluatePosition( float t );

		/// <summary>
		/// Calculates the first derivative on the curve at fraction t
		/// </summary>
		public abstract Vector3 EvaluateFirstDerivative( float t );

		/// <summary>
		/// Calculates the second derivative on the curve at fraction t
		/// </summary>
		public abstract Vector3 EvaluateSecondDerivative( float t );

		/// <summary>
		/// Calculates the tangent, binormal, normal, speed and curvature on the curve at fraction t
		/// </summary>
		public abstract CurveFrame EvaluateFrame( float t );

		/// <summary>
		/// Evaluates the curvature on the curve at fraction t
		/// </summary>
		public abstract float EvaluateCurvature( float t );

		#endregion

		#region	Point evaluation - extrapolated methods

		/// <summary>
		///	Evaluates the normalised tangent vector at fraction t on the curve
		/// </summary>
		public Vector3 EvaluateTangent( float t )
		{
			Vector3 vec = EvaluateFirstDerivative( t );
			vec.Normalise( );
			return vec;
		}

		/// <summary>
		/// Evaluates the speed (length of the first derivative vector) at fraction t on the curve
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public float EvaluateSpeed( float t )
		{
			return EvaluateFirstDerivative( t ).Length;
		}

		#endregion

		#region	Segment evaluation (integrals)

		/// <summary>
		/// Determines the integral of a property of the curve over a given interval [startT..endT]
		/// </summary>
		public float GetSegmentIntegral( float startT, float endT, Integral.FunctionDelegate functionToIntegrate )
		{
			if ( ( startT + 1.0f ) <= endT )
			{
				return GaussianQuadrature.Integrate( startT, endT, functionToIntegrate );
			}

			int		startIndex	= ( ( int )startT ) + 1;
			int		endIndex	= ( int )endT;
			float	intStartT	= startIndex;

			float result	= GaussianQuadrature.Integrate( startT, intStartT, functionToIntegrate );

			for ( ; startIndex < endIndex; ++startIndex )
			{
				result += GaussianQuadrature.Integrate( intStartT, intStartT + 1.0f, functionToIntegrate );
				++intStartT;
			}

			result += GaussianQuadrature.Integrate( endIndex, endT, functionToIntegrate );

			return result;
		}

		/// <summary>
		/// Evaluates the arc length of a segment on the curve
		/// </summary>
		public float GetSegmentLength( float startT, float endT )
		{
			return GetSegmentIntegral( startT, endT, EvaluateSpeed );
		}

		/// <summary>
		/// Evaluates the curvature of a segment on the curve
		/// </summary>
		public float GetSegmentCurvature( float startT, float endT )
		{
			return GetSegmentIntegral( startT, endT, EvaluateCurvature );
		}

		#endregion

		#region	Distance

		/// <summary>
		/// Delegate that returns the distance to a point
		/// </summary>
		public delegate float DistanceToPointDelegate( Point3 pt );

		/// <summary>
		/// Returns the point on the curve that minimises the specified distance function
		/// </summary>
		/// <remarks>
		/// It's faster to pass in squared distance function to minimise (e.g. Line3.GetSqrDistanceToPoint).
		/// </remarks>
		public abstract float FindClosestPoint( DistanceToPointDelegate distance, int iterations );

		/// <summary>
		/// Helper - returns the closest point on this curve to a point
		/// </summary>
		public float FindClosestPoint( Point3 pt, int iterations )
		{
			return FindClosestPoint( new DistanceToPointDelegate( pt.SqrDistanceTo ), iterations );
		}

		/// <summary>
		/// Helper - returns the closest point on this curve to a line
		/// </summary>
		public float FindClosestPoint( Line3 line, int iterations )
		{
			return FindClosestPoint( new DistanceToPointDelegate( line.GetSqrDistanceToPoint ), iterations );
		}

		#endregion

		#region	Properties

		/// <summary>
		/// Returns the start of the curve
		/// </summary>
		private float StartT
		{
			get { return m_StartT; }
		}

		/// <summary>
		/// Returns the end of the curve
		/// </summary>
		public float EndT
		{
			get { return m_EndT; }
		}

		/// <summary>
		/// Returns the (approximate) length of the curve
		/// </summary>
		public float Length
		{
			get { return m_Length; }
		}

		#endregion

		#region	Fields

		private float	m_StartT	= 0.0f;
		private float	m_EndT		= 0.0f;
		private float	m_Length	= 0.0f;

		/// <summary>
		/// Sets the range over which this curve is defined 
		/// </summary>
		/// <param name="min"> Minimum time </param>
		/// <param name="max"> Maximum time </param>
		protected void SetRange( float min, float max )
		{
			m_StartT	= min;
			m_EndT		= max;
		}

		#endregion
	}
}
