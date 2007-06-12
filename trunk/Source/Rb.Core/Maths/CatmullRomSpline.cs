using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Summary description for CatmullRomSpline.
	/// </summary>
	public class CatmullRomSpline : ControlledSpline
	{
		#region	Evaluation helpers

		/// <summary>
		/// Used for evaluating properties on the spline at a given point
		/// </summary>
		public struct Evaluator
		{
			/// <summary>
			/// Control points
			/// </summary>
			public Point3[]		Points;

			/// <summary>
			/// Control point index
			/// </summary>
			public int			CpIndex;

			/// <summary>
			/// Local T value (time value between control points)
			/// </summary>
			public float		LocalT;

			/// <summary>
			/// Global T value (time value on the entire spline)
			/// </summary>
			public float		GlobalT;

			/// <summary>
			/// Evaluates the position on the spline
			/// </summary>
			/// <returns> Returns the position on the spline at the current T value </returns>
			public Point3		EvaluatePosition( )
			{
				Point3	position	= new Point3( );
				float 	t			= LocalT;
				float 	t2			= t * t;
				float 	t3			= t2 * t;

				for ( int i = 0; i < 3; ++i )
				{
					float term0 = Points[ 1 ][ i ] * 2.0f;
					float term1 = -Points[ 0 ][ i ] + Points[ 2 ][ i ];
					float term2 = Points[ 0 ][ i ] * 2.0f - Points[ 1 ][ i ] * 5.0f + Points[ 2 ][ i ] * 4.0f - Points[ 3 ][ i ];
					float term3 = -Points[ 0 ][ i] + Points[ 1 ][ i ] * 3.0f - Points[ 2 ][ i ] * 3.0f + Points[ 3 ][ i ];

					position[ i ] = 0.5f * ( term0 + ( term1 * t ) + ( term2 * t2 ) + ( term3 * t3 ) );
				}

				return position;
			}

			/// <summary>
			/// Evaluates the position on the spline with respect to a new value of T
			/// </summary>
			/// <param name="newGlobalT"> New T value </param>
			/// <returns> Returns the position on the spline at the new T value </returns>
			public Point3		EvaluatePositionWithClampedGlobalT( float newGlobalT )
			{
				GlobalT = newGlobalT;
				LocalT	= GlobalT - ( float )CpIndex; 
				return EvaluatePosition( );
			}

			/// <summary>
			/// Evaluates the first derivative on the spline
			/// </summary>
			/// <returns> Returns the tangent on the spline at the current T value </returns>
			public Vector3		EvaluateFirstDerivative( )
			{
				Vector3 velocity = new Vector3( );
				float t	= LocalT;
				float t2 = t * t;

				for ( int i = 0; i < 3; ++i )
				{
					float term0 = Points[ 1 ][ i ] * 2.0f;
					float term1 = -Points[ 0 ][ i ] + Points[ 2 ][ i ];
					float term2 = Points[ 0 ][ i ] * 2.0f - Points[ 1 ][ i ] * 5.0f + Points[ 2 ][ i ] * 4.0f - Points[ 3 ][ i ];
					float term3 = -Points[ 0 ][ i] + Points[ 1 ][ i ] * 3.0f - Points[ 2 ][ i ] * 3.0f + Points[ 3 ][ i ];

					velocity[ i ]		= 0.5f * ( term1 + ( 2.0f * term2 * t ) + ( 3.0f * term3 * t2 ) );
				}

				return velocity;
			}

			/// <summary>
			/// Evaluates the second derivative on the spline
			/// </summary>
			/// <returns> Returns the acceleration vector on the spline at the current T value </returns>
			public Vector3		EvaluateSecondDerivative( )
			{
				Vector3	acceleration	= new Vector3( );

				float t	= LocalT;

				for ( int i = 0; i < 3; ++i )
				{
					float term0 = Points[ 1 ][ i ] * 2.0f;
					float term1 = -Points[ 0 ][ i ] + Points[ 2 ][ i ];
					float term2 = Points[ 0 ][ i ] * 2.0f - Points[ 1 ][ i ] * 5.0f + Points[ 2 ][ i ] * 4.0f - Points[ 3 ][ i ];
					float term3 = -Points[ 0 ][ i] + Points[ 1 ][ i ] * 3.0f - Points[ 2 ][ i ] * 3.0f + Points[ 3 ][ i ];

					acceleration[ i ]	= 0.5f * ( ( 2.0f * term2 ) + ( 6 * term3 * t ) );
				}

				return acceleration;
			}

			/// <summary>
			/// Evaluates the full frame on the spline
			/// </summary>
			/// <returns> Returns the full Frenet frame on the spline at the current T value </returns>
			public SplineFrame	EvaluateFrame( )
			{
				Point3	position		= new Point3( );
				Vector3 velocity		= new Vector3( );
				Vector3	acceleration	= new Vector3( );

				float t	= LocalT;
				float t2 = t * t;
				float t3 = t2 * t;

				for ( int i = 0; i < 3; ++i )
				{
					float term0 = Points[ 1 ][ i ] * 2.0f;
					float term1 = -Points[ 0 ][ i ] + Points[ 2 ][ i ];
					float term2 = Points[ 0 ][ i ] * 2.0f - Points[ 1 ][ i ] * 5.0f + Points[ 2 ][ i ] * 4.0f - Points[ 3 ][ i ];
					float term3 = -Points[ 0 ][ i] + Points[ 1 ][ i ] * 3.0f - Points[ 2 ][ i ] * 3.0f + Points[ 3 ][ i ];

					position[ i ]		= 0.5f * ( term0 + ( term1 * t ) + ( term2 * t2 ) + ( term3 * t3 ) );
					velocity[ i ]		= 0.5f * ( term1 + ( 2.0f * term2 * t ) + ( 3.0f * term3 * t2 ) );
					acceleration[ i ]	= 0.5f * ( ( 2.0f * term2 ) + ( 6.0f * term3 * t ) );
				}

				return new SplineFrame( position, velocity, acceleration );
			}

			/// <summary>
			/// Evaluates the curvature on the spline
			/// </summary>
			/// <returns> Returns the curvature on the spline at the current T value </returns>
			public float		EvaluateCurvature( )
			{
				Vector3 velocity		= new Vector3( );
				Vector3	acceleration	= new Vector3( );

				float t	= LocalT;
				float t2 = t * t;

				for ( int i = 0; i < 3; ++i )
				{
					float term0 = Points[ 1 ][ i ] * 2.0f;
					float term1 = -Points[ 0 ][ i ] + Points[ 2 ][ i ];
					float term2 = Points[ 0 ][ i ] * 2.0f - Points[ 1 ][ i ] * 5.0f + Points[ 2 ][ i ] * 4.0f - Points[ 3 ][ i ];
					float term3 = -Points[ 0 ][ i] + Points[ 1 ][ i ] * 3.0f - Points[ 2 ][ i ] * 3.0f + Points[ 3 ][ i ];

					velocity[ i ]		= 0.5f * ( term1 + ( 2.0f * term2 * t ) + ( 3.0f * term3 * t2 ) );
					acceleration[ i ]	= 0.5f * ( ( 2.0f * term2 ) + ( 6.0f * term3 * t ) );
				}

				float speed = velocity.Length;

				return ( speed < 0.0001f ) ? 0 : ( Vector3.Cross( velocity, acceleration ).Length / ( speed * speed * speed ) );
			}
		}


		/// <summary>
		/// Makes an Evaluator object for a CatmullRomSpline at a given T value
		/// </summary>
		/// <param name="eval"> Evaluator to build </param>
		/// <param name="spline"> Spline to evaluate </param>
		/// <param name="t"> Global T value on spline to perform evaluations </param>
		private void MakeEvaluator( ref Evaluator eval, CatmullRomSpline spline, float t )
		{
			eval.Points		= new Point3[ 4 ];
			eval.CpIndex	= ( int )t;
			eval.GlobalT	= t;
			eval.LocalT		= t - ( float )eval.CpIndex;

			SplineControlPoints controlPoints = spline.ControlPoints;
			if ( eval.CpIndex == 0 )
			{
				eval.Points[ 0 ] = controlPoints[ 0 ].Position;
			}
			else
			{
				eval.Points[ 0 ] = controlPoints[ eval.CpIndex - 1 ].Position;
			}
			eval.Points[ 1 ] = controlPoints[ eval.CpIndex ].Position;

			int lastCp = controlPoints.Count - 1;
			if ( eval.CpIndex >= lastCp )
			{
				eval.Points[ 2 ] = controlPoints[ lastCp ].Position;
			}
			else
			{
				eval.Points[ 2 ] = controlPoints[ eval.CpIndex + 1 ].Position;
			}
			
			if ( ( eval.CpIndex + 1 ) >= lastCp )
			{
				eval.Points[ 3 ] = controlPoints[ lastCp ].Position;
			}
			else
			{
				eval.Points[ 3 ] = controlPoints[ eval.CpIndex + 2 ].Position;
			}
		}

		#endregion

		#region	Point evaluation

		/// <summary>
		/// Calculates the position on the spline at fraction t
		/// </summary>
		public override Point3		EvaluatePosition( float t )
		{
			Evaluator eval = new Evaluator( );
			MakeEvaluator( ref eval, this, t );

			return eval.EvaluatePosition( );
		}

		/// <summary>
		/// Calculates the first derivative on the spline at fraction t
		/// </summary>
		public override Vector3		EvaluateFirstDerivative( float t )
		{
			Evaluator eval = new Evaluator( );
			MakeEvaluator( ref eval, this, t );

			return eval.EvaluateFirstDerivative( );
		}

		/// <summary>
		/// Calculates the second derivative on the spline at fraction t
		/// </summary>
		public override Vector3		EvaluateSecondDerivative( float t )
		{
			Evaluator eval = new Evaluator( );
			MakeEvaluator( ref eval, this, t );

			return eval.EvaluateSecondDerivative( );
		}

		/// <summary>
		/// Calculates the tangent, binormal, normal, speed and curvature on the spline at fraction t
		/// </summary>
		public override SplineFrame	EvaluateFrame( float t )
		{
			Evaluator eval = new Evaluator( );
			MakeEvaluator( ref eval, this, t );

			return eval.EvaluateFrame( );
		}

		/// <summary>
		/// Evaluates the curvature on the spline at fraction t
		/// </summary>
		public override float		EvaluateCurvature( float t )
		{
			Evaluator eval = new Evaluator( );
			MakeEvaluator( ref eval, this, t );

			return eval.EvaluateCurvature( );
		}

		#endregion

		#region	Distance Helpers

		/// <summary>
		/// Minimises a distance function with respect to this spline
		/// </summary>
		/// <remarks>
		///	Let's use quadratic minimisation for now - maybe use hybrid newtonian/quadratic method later
		///	D(s) = (x(s) - x0)^2 + (y(s) - y0)^2 + (z(s) - z0)^2
		///	Want to find some s* that minimises D(s)
		///	Start with s* estimates s1, s2, s3
		///	</remarks>
		public struct DistanceCalculator
		{
			private float[]					m_S;
			private float[]					m_S2;
			private float[]					m_Distances;
			private float[]					m_PValues;
			private Point3[]				m_Points;
			private DistanceToPointDelegate	m_Distance;

			private float					m_ClosestDistance;
			private float					m_ClosestFraction;

			/// <summary>
			/// Sets up the distance calculator
			/// </summary>
			/// <param name="distance"> Distance function to minimise </param>
			public DistanceCalculator( DistanceToPointDelegate distance )
			{
				m_S					= new float[ 4 ];
				m_S2				= new float[ 4 ];
				m_Distances			= new float[ 4 ];
				m_Points			= new Point3[ 4 ];
				m_PValues			= new float[ 4 ];
				m_Distance			= distance;

				m_ClosestDistance	= float.MaxValue;
				m_ClosestFraction	= 0;
			}

			/// <summary>
			/// Minimise the stored distance function over a given interval
			/// </summary>
			/// <param name="eval"> Spline evaluator </param>
			/// <param name="interval"> Interval size </param>
			/// <param name="iterations"> Number of iterations </param>
			/// <returns> Point in interval that minimises stored distance function </returns>
			public float	GetClosestPointInInterval( Evaluator eval, float interval, int iterations )
			{
				m_S[ 0 ] = eval.GlobalT;
				m_S[ 1 ] = eval.GlobalT + ( interval / 2 );
				m_S[ 2 ] = eval.GlobalT + interval;

				float minT = m_S[ 0 ];
				float maxT = m_S[ 2 ];

				m_Points[ 0 ] 		= eval.EvaluatePositionWithClampedGlobalT( m_S[ 0 ] );
				m_Points[ 1 ] 		= eval.EvaluatePositionWithClampedGlobalT( m_S[ 1 ] );
				m_Points[ 2 ] 		= eval.EvaluatePositionWithClampedGlobalT( m_S[ 2 ] );

				m_Distances[ 0 ] 	= m_Distance( m_Points[ 0 ] );
				m_Distances[ 1 ] 	= m_Distance( m_Points[ 1 ] );
				m_Distances[ 2 ] 	= m_Distance( m_Points[ 2 ] );

				int BestIndex = 0;
				float ClosestDistance = float.MaxValue;

				for ( int k = 0; k < iterations; ++k )
				{
					m_S2[ 0 ] = m_S[ 0 ] * m_S[ 0 ];
					m_S2[ 1 ] = m_S[ 1 ] * m_S[ 1 ];
					m_S2[ 2 ] = m_S[ 2 ] * m_S[ 2 ];

					float PolyEstimateNum	=	( ( m_S2[ 1 ] - m_S2[ 2 ] ) * m_Distances[ 0 ] ) + 
												( ( m_S2[ 2 ] - m_S2[ 0 ] ) * m_Distances[ 1 ] ) + 
												( ( m_S2[ 0 ] - m_S2[ 1 ] ) * m_Distances[ 2 ] );
					float PolyEstimateDen	=	( (  m_S[ 1 ] -  m_S[ 2 ] ) * m_Distances[ 0 ] ) + 
												( (  m_S[ 2 ] -  m_S[ 0 ] ) * m_Distances[ 1 ] ) + 
												( (  m_S[ 0 ] -  m_S[ 1 ] ) * m_Distances[ 2 ] );

					if ( System.Math.Abs( PolyEstimateDen ) < 0.0001f )
					{
						break;
					}

					m_S[ 3 ]			= Maths.Utils.Clamp( 0.5f * ( PolyEstimateNum / PolyEstimateDen ), minT, maxT );
					m_Points[ 3 ]		= eval.EvaluatePositionWithClampedGlobalT( m_S[ 3 ] );
					m_Distances[ 3 ]	= m_Distance( m_Points[ 3 ] );

					m_PValues[ 0 ] 		= EvaluatePolynomial( m_S[ 0 ] );
					m_PValues[ 1 ] 		= EvaluatePolynomial( m_S[ 1 ] );
					m_PValues[ 2 ] 		= EvaluatePolynomial( m_S[ 2 ] );
					m_PValues[ 3 ] 		= EvaluatePolynomial( m_S[ 3 ] );

					//	Which estimate created the furthest distance?
					int		FurthestIndex		= 0;
					float	FurthestDistance	= -float.MaxValue;
					for ( int Index = 0; Index < 4; ++Index )
					{
						if ( m_PValues[ Index ] > FurthestDistance )
						{
							FurthestDistance	= m_PValues[ Index ];
							FurthestIndex		= Index;
						}
					}

					//	Remake arrays m_S and m_Distances using the 3 closest estimates
					int AddAt = 0;
					ClosestDistance = float.MaxValue;
					for ( int Index = 0; Index < 4; ++Index )
					{
						if ( Index != FurthestIndex )
						{
							m_S[ AddAt ]			= m_S[ Index ];
							m_Distances[ AddAt ]	= m_Distances[ Index ];
							m_Points[ AddAt ]		= m_Points[ Index ];
							m_PValues[ AddAt ]		= m_PValues[ Index ];

							if ( m_PValues[ AddAt ] < ClosestDistance ) 
							{
								BestIndex = AddAt;
							}

							++AddAt;
						}
					}
				}

				if ( ClosestDistance < m_ClosestDistance )
				{
					m_ClosestDistance	= ClosestDistance;
					m_ClosestFraction	= m_S[ BestIndex ];
				}

				return m_ClosestFraction;
			}

			private float EvaluatePolynomial( float Value )
			{
				float VS1 = Value - m_S[ 0 ];
				float VS2 = Value - m_S[ 1 ];
				float VS3 = Value - m_S[ 2 ];

				return	( VS2 * VS3 * m_Distances[ 0 ] ) / ( ( m_S[ 0 ] - m_S[ 1 ] ) * ( m_S[ 0 ] - m_S[ 2 ] ) ) +
						( VS1 * VS3 * m_Distances[ 1 ] ) / ( ( m_S[ 1 ] - m_S[ 0 ] ) * ( m_S[ 1 ] - m_S[ 2 ] ) ) +
						( VS1 * VS2 * m_Distances[ 2 ] ) / ( ( m_S[ 2 ] - m_S[ 0 ] ) * ( m_S[ 2 ] - m_S[ 1 ] ) );
			}


		}

		#endregion

		#region	Distance

		/// <summary>
		/// Finds the point on this spline that minimises a given distance function
		/// </summary>
		/// <param name="distance"> Distance function </param>
		/// <param name="iterations"> Number of iterations (accuracy). 5 is pretty good. </param>
		/// <returns> Returns the time on the spline of the closest point </returns>
		public override float FindClosestPoint( DistanceToPointDelegate distance, int iterations )
		{
			CatmullRomSpline.DistanceCalculator calculator = new DistanceCalculator( distance );

			float closestFraction = 0;
			int numControlPoints = ControlPoints.Count;

			Evaluator eval = new Evaluator( );

			for ( int cpIndex = 0; cpIndex < numControlPoints; ++cpIndex )
			{
				MakeEvaluator( ref eval, this, ( float )cpIndex );
				closestFraction = calculator.GetClosestPointInInterval( eval, 1.0f, iterations );
			}

			return closestFraction;			
		}

		#endregion
	}
}
