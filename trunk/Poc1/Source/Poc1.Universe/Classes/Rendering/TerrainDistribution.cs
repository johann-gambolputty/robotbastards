using System;
using Rb.Core.Maths;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Used to model density functions based on terrain elevation, latitude and slope
	/// </summary>
	/// <remarks>
	/// Used for defining terrain type and vegetation distributions in different biomes
	/// </remarks>
	public class TerrainDistribution
	{
		/// <summary>
		/// Creates a default terrain distribution
		/// </summary>
		public TerrainDistribution( )
		{
			m_Elevations = new float[] { 1 };
			m_Latitudes = new float[] { 1 };
			m_Slopes = new float[] { 1 };
		}

		/// <summary>
		/// Creates a terrain distribution
		/// </summary>
		public TerrainDistribution( Sample[] elevations, Sample[] latitudes, Sample[] slopes )
		{
			Setup( elevations, latitudes, slopes );
		}

		public struct Sample
		{
			public float Value;
			public float T;

			public Sample( float t, float value )
			{
				T = t;
				Value = value;
			}
		}

		/// <summary>
		/// Sets up this terrain distribution
		/// </summary>
		/// <param name="elevations">Elevation densities</param>
		/// <param name="latitudes">Latitude densities</param>
		/// <param name="slopes">Slope densities</param>
		public void Setup( Sample[] elevations, Sample[] latitudes, Sample[] slopes )
		{
			if ( elevations == null )
			{
				throw new ArgumentNullException( "elevations" );
			}
			if ( elevations.Length == 0 )
			{
				throw new ArgumentException( "Elevations array must contain 1 or more values" );
			}
			if ( latitudes == null )
			{
				throw new ArgumentNullException( "latitudes" );
			}
			if ( latitudes.Length == 0 )
			{
				throw new ArgumentException( "Latitudes array must contain 1 or more values" );
			}
			if ( slopes == null )
			{
				throw new ArgumentNullException( "slopes" );
			}
			if ( slopes.Length == 0 )
			{
				throw new ArgumentException( "Slopes array must contain 1 or more values" );
			}

			m_Elevations = CreateUniformSampleArray( elevations, 256 );
			m_Latitudes = CreateUniformSampleArray( latitudes, 256 );
			m_Slopes = CreateUniformSampleArray( slopes, 256 );
		}

		/// <summary>
		/// Samples the distribution function
		/// </summary>
		/// <param name="e">Normalised elevation</param>
		/// <param name="l">Normalised latitude</param>
		/// <param name="s">Normalised slope</param>
		/// <returns>Returns the value of the density function at (e,l,s)</returns>
		public float GetSample( float e, float l, float s )
		{
			float eD = GetValue( m_Elevations, e );
			float lD = GetValue( m_Latitudes, l );
			float sD = GetValue( m_Slopes, s );

			return eD * lD * sD;
		}

		#region Private Members

		private float[] m_Elevations;
		private float[] m_Latitudes;
		private float[] m_Slopes;

		private static float[] CreateUniformSampleArray( Sample[] samples, int count )
		{
			float[] results = new float[ count ];

			if ( samples.Length == 1 )
			{
				for( int i = 0; i < count; ++i )
				{
					results[ i ] = samples[ 0 ].Value;
				}
				return results;
			}

			float inc = 1.0f / ( count - 1 );
			int sampleIndex = 0;

			float t = 0.0f;
			float startT = samples[ 0 ].T;
			float endT = samples[ 1 ].T;
			float curSample = samples[ 0 ].Value;
			float nextSample = samples[ 1 ].Value;

			for ( int i = 0; i < count; ++i )
			{
				results[ i ] = t < startT ? curSample : Utils.Lerp( curSample, nextSample, ( t - startT ) / ( endT - startT ) );

				t += inc;
				if ( t >= endT )
				{
					++sampleIndex;
					bool lastSample = sampleIndex == samples.Length - 1;
					curSample = samples[ sampleIndex ].Value;
					nextSample = lastSample ? curSample : samples[ sampleIndex + 1 ].Value;
					startT = samples[ sampleIndex ].T;
					endT = lastSample ? 2.0f : samples[ sampleIndex + 1 ].T;
				}
			}
			return results;
		}

		/// <summary>
		/// Linearly interpolates between values from a value array
		/// </summary>
		private static float GetValue( float[] values, float v )
		{
			if ( ( values.Length == 1 ) || ( v <= 0 ) )
			{
				return values[ 0 ];
			}
			if ( v >= 1.0f )
			{
				return values[ values.Length - 1 ];
			}
			float step = 1.0f / ( values.Length - 1 );
			int i = ( int )( v / step );
			float f = ( v - ( step * i ) ) / step;
			return values[ i ] + ( values[ i + 1 ] - values[ i ] ) * f;
		}

		#endregion
	}
}