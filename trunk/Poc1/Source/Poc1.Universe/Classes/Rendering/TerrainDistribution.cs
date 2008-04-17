
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
			Setup( new float[] { 1 }, new float[] { 1 }, new float[] { 1 } );
		}

		/// <summary>
		/// Sets up this terrain distribution
		/// </summary>
		/// <param name="elevations">Elevation densities</param>
		/// <param name="latitudes">Latitude densities</param>
		/// <param name="slopes">Slope densities</param>
		public void Setup( float[] elevations, float[] latitudes, float[] slopes )
		{
			m_Elevations = elevations;
			m_Latitudes = latitudes;
			m_Slopes = slopes;
		}

		/// <summary>
		/// Samples the distribution function
		/// </summary>
		/// <param name="e">Normalised elevation</param>
		/// <param name="l">Normalised latitude</param>
		/// <param name="s">Normalised slope</param>
		/// <returns>Returns the value of the density function at (e,l,s)</returns>
		public float Sample( float e, float l, float s )
		{
			float eD = GetValue( m_Elevations, e );
			float lD = GetValue( m_Latitudes, l );
			float sD = GetValue( m_Slopes, s );

			return eD * lD * sD;
		}

		/// <summary>
		/// Builds a set of weights from a series of terrain distribution functions. Sum of all weights is 1
		/// </summary>
		public static void GetWeights( TerrainDistribution[] distributions, float e, float l, float s, float[] weights )
		{
			float total = 0;
			for ( int index = 0; index < distributions.Length; ++index )
			{
				weights[ index ] = distributions[ index ].Sample( e, l, s );
				total += weights[ index ];
			}
			if ( total <= 0.00001f )
			{
				//	Just weight the first one - it's a bad situation, though
				weights[ index ] = 0;
				return;
			}
			for ( int index = 0; index < distributions.Length; ++index )
			{
				weights[ index ] /= total;
			}

		}

		#region Private Members

		private float[] m_Elevations;
		private float[] m_Latitudes;
		private float[] m_Slopes;

		/// <summary>
		/// Linearly interpolates between values from a value array
		/// </summary>
		private static void GetValue( float[] values, float v )
		{
			if ( ( values.Length == 1 ) || ( v <= 0 ) )
			{
				return values[ 0 ];
			}
			if ( v >= 1.0f )
			{
				return values[ values.Length - 1 ];
			}
			int i = ( int )v;
			float step = 1.0f / ( values.Length - 1 );
			float f = ( v - ( step * i ) ) / step;
			return values[ i ] + ( values[ i + 1 ] - values[ i - 1 ] ) * f;
		}

		#endregion
	}
}