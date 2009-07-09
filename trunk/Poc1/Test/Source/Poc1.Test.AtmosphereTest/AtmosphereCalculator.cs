using System;
using System.Drawing;
using Rb.Core.Maths;
using RbGraphics = Rb.Rendering.Graphics;

namespace Poc1.Test.AtmosphereTest
{
	/// <summary>
	/// Calculates atmospheric scattering integrals
	/// </summary>
	public unsafe class AtmosphereCalculator
	{
		public AtmosphereCalculator( Point3 cameraPos, AtmosphereCalculatorModel model )
		{
			m_CameraPos = cameraPos;
			m_Model = model;
			m_Inner = new Sphere3( Point3.Origin, model.PlanetRadius );
			m_Outer = new Sphere3( Point3.Origin, model.AtmosphereRadius );
			float heightRange = model.AtmosphereRadius - model.PlanetRadius;
			m_InvRH0 = -1.0f / ( heightRange * model.RayleighDensityScaleHeightFraction );
			m_InvMH0 = -1.0f / ( heightRange * model.MieDensityScaleHeightFraction );
		}

		private Point3 RenderPointToWorldPoint( Point3 pt )
		{
			Vector3 lVec = pt.ToVector3( );
			float lLength = lVec.Length;
			float lNorm = ( lLength - m_Model.PlanetRenderRadius ) / ( m_Model.AtmosphereRenderRadius - m_Model.PlanetRenderRadius );
			Vector3 norm = lVec.MakeNormal( );
			Vector3 tmp = norm * ( m_Model.PlanetRadius + lNorm * ( m_Model.AtmosphereRadius - m_Model.PlanetRadius ) );
			return tmp.ToPoint3( );
		}

		private static Point3 GetAtmosphereIntersection( Vector3 pt, Vector3 vec, float radius )
		{
			float a0 = pt.Dot( pt ) - ( radius * radius );
			float a1 = vec.Dot( pt );
			float d = ( a1 * a1 ) - a0;
			float root = ( float )Math.Sqrt( d );
			float t0 = -a1 - root;
			float t1 = -a1 + root;
			float closestT = ( t0 < t1 ) ? t0 : t1;

			return ( pt + vec * closestT ).ToPoint3( );
		}

		public Color CalculateColour( Point3 pt )
		{
			pt = RenderPointToWorldPoint( pt );
			Point3 startPt = RenderPointToWorldPoint( m_CameraPos );
			if ( !m_Outer.IsInside( startPt ) )
			{
				Line3Intersection intersection = Intersections3.GetRayIntersection( new Ray3( startPt, ( pt - startPt ).MakeNormal( ) ), m_Outer );
				if ( intersection == null )
				{
				    return Color.Black;
				}
			//	startPt = GetAtmosphereIntersection( pt.ToVector3( ), ( pt - startPt ).MakeNormal( ), m_Outer.Radius );
				startPt = intersection.IntersectionPosition;
			}

			Vector3 vecToPt = pt - startPt;
			Vector3 sampleStep = vecToPt / m_Model.Samples;

			Point3 samplePt = startPt;
			float mul = sampleStep.Length;
			if ( mul < 0.0001f )
			{
				return Color.Black;
			}

			float* rAccum = stackalloc float[ 3 ];
			float* mAccum = stackalloc float[ 3 ];
			float* rViewOutScatter = stackalloc float[ 3 ];
			float* mViewOutScatter = stackalloc float[ 3 ];
			float* rSunOutScatter = stackalloc float[ 3 ];
			float* mSunOutScatter = stackalloc float[ 3 ];
			samplePt += sampleStep;
			for ( int sample = 1; sample < m_Model.Samples; ++sample, samplePt += sampleStep )
			{
				Line3Intersection intToSun = Intersections3.GetRayIntersection( new Ray3( samplePt, m_Model.SunDirection ), m_Outer );
			//	Line3Intersection intToSun = GetRayIntersection( samplePt, m_Model.SunDirection );
				if ( intToSun == null )
				{
					continue;
				}

				float sampleHeight = Height( samplePt );
				float pRCoeff = Functions.Exp( sampleHeight * m_InvRH0 ) * mul;
				float pMCoeff = Functions.Exp( sampleHeight * m_InvMH0 ) * mul;

				//	Calculate (wavelength-dependent) out-scatter terms
				Vector3 viewStep = ( startPt - samplePt ) / m_Model.Samples;
				Vector3 sunStep = ( intToSun.IntersectionPosition - samplePt ) / m_Model.Samples;

				CalculateOutScatter( samplePt, sunStep, rSunOutScatter, mSunOutScatter );
				CalculateOutScatter( samplePt, viewStep, rViewOutScatter, mViewOutScatter );

				for ( int i = 0; i < 3; ++i )
				{
					float outScatterR = Functions.Exp( ( -rViewOutScatter[ i ] - rSunOutScatter[ i ] ) );
					float outScatterM = Functions.Exp( ( -mViewOutScatter[ i ] - mSunOutScatter[ i ] ) );
					mAccum[ i ] += pMCoeff * outScatterM;
					rAccum[ i ] += pRCoeff * outScatterR;
				}
			}

		//	float cosSunAngle = vecToPt.MakeNormal( ).Dot( m_Model.SunDirection );
		//	float rPhase = RayleighPhase( cosSunAngle );
		//	float mPhase = HeyneyGreensteinPhaseFunction( cosSunAngle, 0.99f ) * mieMul;
		//	float tR = rPhase * rAccum[ 0 ] * m_Model.RayleighCoefficients[ 0 ] + mPhase * mAccum[ 0 ] * m_Model.MieCoefficients[ 0 ];
		//	float tG = rPhase * rAccum[ 1 ] * m_Model.RayleighCoefficients[ 1 ] + mPhase * mAccum[ 1 ] * m_Model.MieCoefficients[ 1 ];
		//	float tB = rPhase * rAccum[ 2 ] * m_Model.RayleighCoefficients[ 2 ] + mPhase * mAccum[ 2 ] * m_Model.MieCoefficients[ 2 ];

			float tR = rAccum[ 0 ] * m_Model.RayleighCoefficients[ 0 ];
			float tG = rAccum[ 1 ] * m_Model.RayleighCoefficients[ 1 ];
			float tB = rAccum[ 2 ] * m_Model.RayleighCoefficients[ 2 ];
			float tA = ( mAccum[ 0 ]  * m_Model.MieCoefficients[ 0 ] + mAccum[ 1 ]  * m_Model.MieCoefficients[ 1 ] + mAccum[ 2 ] * m_Model.MieCoefficients[ 2 ] ) / 3;
			float n = f * 255.0f;
			int r = ( int )Utils.Clamp( tR * n, 0, 255 );
			int g = ( int )Utils.Clamp( tG * n, 0, 255 );
			int b = ( int )Utils.Clamp( tB * n, 0, 255 );
			int a = ( int )Utils.Clamp( tA * n, 0, 255 );
			return Color.FromArgb( a, r, g, b );
		}

		private readonly static float mieMul = 1;
		private readonly static float f = 1;

		private static float RayleighPhase( float cosAngle )
		{
			return 0.75f * ( 1 + cosAngle * cosAngle );
		}

		private static float HeyneyGreensteinPhaseFunction( float cosSunAngle, float g )
		{
		    float g2 = g * g;
		    return ( 1.0f - g2 ) / ( float )Math.Pow( ( 1.0f + g2 ) - ( 2.0f * g * cosSunAngle ), 1.5f );
		}

		private float Height( Point3 pt )
		{
			return Utils.Clamp( pt.DistanceTo( Point3.Origin ) - m_Inner.Radius, 0, m_Outer.Radius - m_Inner.Radius );
		}

		private void CalculateOutScatter( Point3 startPt, Vector3 step, float* rOutScatter, float* mOutScatter )
		{
			float mul = step.Length;
			Point3 samplePt = startPt;
			float rAccum = 0;
			float mAccum = 0;
			for ( int sample = 0; sample < m_Model.Samples; ++sample )
			{
				float samplePtHeight = Height( samplePt );
				float samplePtRCoeff = Functions.Exp( samplePtHeight * m_InvRH0 );
				float samplePtMCoeff = Functions.Exp( samplePtHeight * m_InvMH0 );

				rAccum += samplePtRCoeff * mul;
				mAccum += samplePtMCoeff * mul;

				samplePt += step;
			}
			for ( int i = 0; i < 3; ++i )
			{
				rOutScatter[ i ] = m_Model.RayleighCoefficients[ i ] * rAccum;
				mOutScatter[ i ] = m_Model.MieCoefficients[ i ] * mAccum;
			}
		}

		private readonly float m_InvRH0;
		private readonly float m_InvMH0;
		private readonly Sphere3 m_Inner;
		private readonly Sphere3 m_Outer;
		private readonly Point3 m_CameraPos;
		private readonly AtmosphereCalculatorModel m_Model;

		private Line3Intersection GetRayIntersection( Point3 startPt, Vector3 vec )
		{
			Line3Intersection groundIntersection = Intersections3.GetRayIntersection( new Ray3( startPt, vec ), m_Inner );
			Line3Intersection skyIntersection = Intersections3.GetRayIntersection( new Ray3( startPt, vec ), m_Outer );
			if ( groundIntersection == null )
			{
				return skyIntersection;
			}
			if ( skyIntersection == null )
			{
				return groundIntersection;
			}
			return groundIntersection.Distance < skyIntersection.Distance ? groundIntersection : skyIntersection;
		}


	}
}
