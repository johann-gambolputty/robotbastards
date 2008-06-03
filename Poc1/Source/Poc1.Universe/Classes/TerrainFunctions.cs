
namespace Poc1.Universe.Classes
{
	/// <summary>
	/// Terrain function abstract base class
	/// </summary>
	public abstract class TerrainFunction
	{
		/// <summary>
		/// Gets/sets the parameters associated with this function
		/// </summary>
		public object Parameters
		{
			get { return m_Parameters; }
			set { m_Parameters = value; }
		}

		/// <summary>
		/// Gets the name of this terrain function
		/// </summary>
		public abstract string Name
		{
			get;
		}

		/// <summary>
		/// Creates a parameters object for this function
		/// </summary>
		public abstract object CreateParameters( );

		#region Private Members

		private object m_Parameters;
		
		#endregion
	}

	/// <summary>
	/// Simple fractal terrain function
	/// </summary>
	public class SimpleFractal : TerrainFunction
	{
		public class ParameterSet
		{
			public int Octaves
			{
				get { return m_Octaves; }
				set { m_Octaves = value; }
			}

			public float Frequency
			{
				get { return m_Frequency; }
				set { m_Frequency = value; }
			}

			public float Lacunarity
			{
				get { return m_Lacunarity; }
				set { m_Lacunarity = value; }
			}

			#region Private Members

			private int		m_Octaves		= 8;
			private float	m_Frequency		= 1.4f;
			private float	m_Lacunarity	= 0.8f;

			#endregion
		}

		/// <summary>
		/// Gets the name of this terrain function
		/// </summary>
		public override string Name
		{
			get { return "Simple Fractal"; }
		}

		/// <summary>
		/// Creates a parameters object for this function
		/// </summary>
		public override object CreateParameters( )
		{
			return new ParameterSet( );
		}
	}

	/// <summary>
	/// Ridged fractal terrain function
	/// </summary>
	public class RidgedFractal : TerrainFunction
	{
		#region ParameterSet Class

		public class ParameterSet
		{
			public int Octaves
			{
				get { return m_Octaves; }
				set { m_Octaves = value; }
			}

			public float Frequency
			{
				get { return m_Frequency; }
				set { m_Frequency = value; }
			}

			public float Lacunarity
			{
				get { return m_Lacunarity; }
				set { m_Lacunarity = value; }
			}

			#region Private Members

			private int		m_Octaves		= 8;
			private float	m_Frequency		= 1.4f;
			private float	m_Lacunarity	= 0.8f;

			#endregion	
		}

		#endregion

		/// <summary>
		/// Gets the name of this terrain function
		/// </summary>
		public override string Name
		{
			get { return "Ridged Multi-Fractal"; }
		}

		/// <summary>
		/// Creates a parameters object for this function
		/// </summary>
		public override object CreateParameters( )
		{
			return new ParameterSet( );
		}
	}
}
