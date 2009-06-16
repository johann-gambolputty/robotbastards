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
		public AtmosphereCalculator( Point3 cameraPos, Vector3 cameraDir, AtmosphereCalculatorModel model )
		{
			m_CameraPos = cameraPos;
			m_CameraDir = cameraDir;
			m_Model = model;
			m_Inner = new Sphere3( Point3.Origin, model.PlanetRadius );
			m_Outer = new Sphere3( Point3.Origin, model.AtmosphereRadius );
			float heightRange = model.AtmosphereRadius - model.AtmosphereRadius;
			m_InvRH0 = -1.0f / ( heightRange * model.RayleighDensityScaleHeightFraction );
			m_InvMH0 = -1.0f / ( heightRange * model.MieDensityScaleHeightFraction );
		}

		public Color CalculateColour( Point3 pt )
		{
			Line3Intersection intersection = Intersections3.GetRayIntersection( new Ray3( m_CameraPos, ( pt - m_CameraPos ).MakeNormal( ) ), m_Outer );
			if ( intersection == null )
			{
				return Color.Black;
			}

			Point3 startPt = intersection.IntersectionPosition;
			Vector3 vecToPt = pt - startPt;
			Vector3 sampleStep = vecToPt / m_Model.Samples;

			Point3 samplePt = m_CameraPos;
			float mul = vecToPt.Length;

			float[] rAccum = stackalloc float[ 3 ];
			float[] mAccum = stackalloc float[ 3 ];
			float[] rViewOutScatter = stackalloc float[ 3 ];
			float[] mViewOutScatter = stackalloc float[ 3 ];
			float[] rSunOutScatter = stackalloc float[ 3 ];
			float[] mSunOutScatter = stackalloc float[ 3 ];
			for ( int sample = 0; sample < m_Model.Samples; ++sample, samplePt += sampleStep )
			{
				intersection = Intersections3.GetRayIntersection( new Ray3( samplePt, m_Model.SunDirection ), m_Outer );
				if ( intersection == null )
				{
					continue;
				}

				float sampleHeight = samplePt.DistanceTo( Point3.Origin ) - m_Inner.Radius;
				sampleHeight = Utils.Max( sampleHeight, 0 );
				float pRCoeff = Functions.Exp( sampleHeight * m_InvRH0 ) * mul;
				float pMCoeff = Functions.Exp( sampleHeight * m_InvMH0 ) * mul;

				//	Calculate (wavelength-dependent) out-scatter terms
				Vector3 viewStep = ( m_CameraPos - samplePt ) / ( m_Model.Samples - 1 );
				Vector3 sunStep = ( intersection.IntersectionPosition - samplePt ) / ( m_Model.Samples - 1 );

				CalculateOutScatter( samplePt, sunStep, rSunOutScatter, mSunOutScatter );
				CalculateOutScatter( samplePt, viewStep, rViewOutScatter, mViewOutScatter );

				for ( int i = 0; i < 3; ++i )
				{
					float outScatterR = Functions.Exp( ( -rSunOutScatter[ i ] - rViewOutScatter[ i ] ) );
					float outScatterM = Functions.Exp( ( -mSunOutScatter[ i ] - mViewOutScatter[ i ] ) );
					mAccum[ i ] += pMCoeff * outScatterM;
					rAccum[ i ] += pRCoeff * outScatterR;
				}
			}

			int r = rAccum[ 0 ] + mAccum[ 0 ];
			int g = 
			return Color.Black;
		}

		private void CalculateOutScatter( Point3 startPt, Vector3 step, float[] rOutScatter, float[] mOutScatter )
		{
			float mul = step.Length;
			Point3 samplePt = startPt;
			float rAccum = 0;
			float mAccum = 0;
			for ( int sample = 0; sample < m_Model.Samples; ++sample )
			{
				float samplePtHeight = samplePt.DistanceTo( Point3.Origin ) - m_Inner.Radius;
				samplePtHeight = Utils.Max( samplePtHeight, 0 );
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
		private readonly Vector3 m_CameraDir;
		private readonly AtmosphereCalculatorModel m_Model;

	}
}
