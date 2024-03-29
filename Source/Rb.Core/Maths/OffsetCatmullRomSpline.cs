
namespace Rb.Core.Maths
{
	/// <summary>
	/// A catmull-rom spline with a fixed offset
	/// </summary>
	public class OffsetCatmullRomSpline : Curve
	{
		#region	Construction and setup

		/// <summary>
		/// Setup constructor. Sets the base spline and an offset from it
		/// </summary>
		/// <param name="baseSpline"> Base CM spline </param>
		/// <param name="offset"> Offset from base spline </param>
		public OffsetCatmullRomSpline( CatmullRomSpline baseSpline, float offset )
		{
			m_BaseSpline	= baseSpline;
			m_Offset		= offset;
			m_BaseSpline.OnChangedEvent += OnBaseSplineChanged;
		}

		#endregion

		#region	Point evaluation

		private static void MakeEvaluator( ref CatmullRomSpline.Evaluator eval, float t, ControlledSpline baseSpline, float offset )
		{
			eval.Points	= new Point3[ 4 ];
			eval.CpIndex = ( int )t;
			eval.LocalT	= t - eval.CpIndex;

			SplineControlPoints controlPoints = baseSpline.ControlPoints;
			if ( eval.CpIndex == 0 )
			{
				eval.Points[ 0 ] = controlPoints[ 0 ].Position + controlPoints[ 0 ].Frame.Binormal * offset;
			}
			else
			{
				eval.Points[ 0 ] = controlPoints[ eval.CpIndex - 1 ].Position + controlPoints[ eval.CpIndex - 1 ].Frame.Binormal * offset;
			}

			Vector3 point1Offset = controlPoints[ eval.CpIndex ].Frame.Binormal * offset;
			eval.Points[ 1 ] = controlPoints[ eval.CpIndex ].Position + point1Offset;

			Vector3 point2Offset;
			int lastCp = controlPoints.Count - 1;
			if ( eval.CpIndex >= lastCp )
			{
				point2Offset		= point1Offset;
				eval.Points[ 2 ]	= controlPoints[ lastCp ].Position + point2Offset;
			}
			else
			{
				point2Offset		= controlPoints[ eval.CpIndex + 1 ].Frame.Binormal * offset;
				eval.Points[ 2 ]	= controlPoints[ eval.CpIndex + 1 ].Position + point2Offset;
			}
			
			if ( ( eval.CpIndex + 1 ) >= lastCp )
			{
				eval.Points[ 3 ] = controlPoints[ lastCp ].Position + point2Offset;
			}
			else
			{
				eval.Points[ 3 ] = controlPoints[ eval.CpIndex + 2 ].Position + controlPoints[ eval.CpIndex + 2 ].Frame.Binormal * offset;
			}
		}

		/// <summary>
		/// Calculates the position on the spline at fraction t
		/// </summary>
		public override Point3 EvaluatePosition( float t )
		{
			CatmullRomSpline.Evaluator eval = new CatmullRomSpline.Evaluator( );
			MakeEvaluator( ref eval, t, m_BaseSpline, m_Offset );
			return eval.EvaluatePosition( );
		}

		/// <summary>
		/// Calculates the first derivative on the spline at fraction t
		/// </summary>
		public override Vector3 EvaluateFirstDerivative( float t )
		{
			CatmullRomSpline.Evaluator eval = new CatmullRomSpline.Evaluator( );
			MakeEvaluator( ref eval,  t, m_BaseSpline, m_Offset );
			return eval.EvaluateFirstDerivative( );
		}

		/// <summary>
		/// Calculates the second derivative on the spline at fraction t
		/// </summary>
		public override Vector3 EvaluateSecondDerivative( float t )
		{
			CatmullRomSpline.Evaluator eval = new CatmullRomSpline.Evaluator( );
			MakeEvaluator( ref eval, t, m_BaseSpline, m_Offset );
			return eval.EvaluateSecondDerivative( );
		}

		/// <summary>
		/// Calculates the tangent, bi-normal, normal, speed and curvature on the spline at fraction t
		/// </summary>
		public override CurveFrame EvaluateFrame( float t )
		{
			CatmullRomSpline.Evaluator eval = new CatmullRomSpline.Evaluator( );
			MakeEvaluator( ref eval, t, m_BaseSpline, m_Offset );
			return eval.EvaluateFrame( );
		}

		/// <summary>
		/// Evaluates the curvature on the spline at fraction t
		/// </summary>
		public override float EvaluateCurvature( float t )
		{
			CatmullRomSpline.Evaluator eval = new CatmullRomSpline.Evaluator( );
			MakeEvaluator( ref eval, t, m_BaseSpline, m_Offset );
			return eval.EvaluateCurvature( );
		}

		#endregion

		#region	Distance evaluation

		/// <summary>
		/// Finds the point on this spline that minimises a given distance function
		/// </summary>
		/// <param name="distance"> Distance function </param>
		/// <param name="iterations"> Number of iterations (accuracy). 5 is pretty good. </param>
		/// <returns> Returns the time on the spline of the closest point </returns>
		public override float FindClosestPoint( DistanceToPointDelegate distance, int iterations )
		{
			CatmullRomSpline.DistanceCalculator calculator = new CatmullRomSpline.DistanceCalculator( distance );

			float closestFraction = 0;
			int numControlPoints = m_BaseSpline.ControlPoints.Count;

			CatmullRomSpline.Evaluator eval = new CatmullRomSpline.Evaluator( );

			for ( int cpIndex = 0; cpIndex < numControlPoints; ++cpIndex )
			{
				MakeEvaluator( ref eval, cpIndex, m_BaseSpline, m_Offset );
				closestFraction = calculator.GetClosestPointInInterval( eval, 1.0f, iterations );
			}

			return closestFraction;
		}

		#endregion

		#region Private stuff

		private readonly CatmullRomSpline m_BaseSpline;
		private readonly float m_Offset;

		private void OnBaseSplineChanged( Curve baseSpline )
		{
			OnChanged( );
		}

		#endregion
	}
}
