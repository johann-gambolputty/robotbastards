using System.Collections.Generic;
using Rb.Common.Controls.Graphs.Classes.Renderers;
using Rb.Common.Controls.Graphs.Interfaces;

namespace Rb.Common.Controls.Graphs.Classes.Sources
{
	/// <summary>
	/// Dynamic data source
	/// </summary>
	public class GraphX2dSourceSamples : GraphX2dSourceAbstract, IGraphX2dSampleSource
	{
		/// <summary>
		/// Gets/sets the maximum number of samples that will be stored in this source. -1 to set no limits
		/// </summary>
		public int MaximumNumberOfSamples
		{
			get { return m_MaximumNumberOfSamples; }
			set { m_MaximumNumberOfSamples = value; }
		}

		/// <summary>
		/// Adds a value to the source
		/// </summary>
		/// <param name="value">Value to add</param>
		public void AddValue( float value )
		{
			if ( ( m_MaximumNumberOfSamples == -1 ) || ( m_Samples.Count < m_MaximumNumberOfSamples ) )
			{
				m_Samples.Add( value );
			}
			else
			{
				m_Samples[ m_InsertAt ] = value;
				m_InsertAt = ( m_InsertAt + 1 ) % m_Samples.Count;
			}
			++m_TotalSamples;
			MaximumX = m_TotalSamples * m_XStepPerSample;
			OnGraphChanged( true );
		}

		/// <summary>
		/// Creates an instance of the default renderer type for this source
		/// </summary>
		public override IGraph2dRenderer CreateRenderer( )
		{
			return new GraphX2dSamplesBarRenderer( );
		} 

		/// <summary>
		/// Evaluates the source at position x
		/// </summary>
		public override float Evaluate( float x )
		{
			if ( m_Samples.Count == 0 )
			{
				return 0;
			}
			if ( ( m_MaximumNumberOfSamples == -1 ) || ( m_Samples.Count < m_MaximumNumberOfSamples ) )
			{
				return EvaluateDirectMapping( x );
			}
			return EvaluateConstrainedMapping( x );
		}

		#region IGraphX2dSampleSource Members

		/// <summary>
		/// X increment per sample in the source
		/// </summary>
		public float XStepPerSample
		{
			get { return m_XStepPerSample; }
			set { m_XStepPerSample = value; }
		}

		#endregion

		#region Private Members

		private int			m_InsertAt = 0;
		private int			m_TotalSamples = 0;
		private int			m_MaximumNumberOfSamples = -1;
		private float		m_XStepPerSample = 0.1f;
		private List<float>	m_Samples = new List<float>( );

		/// <summary>
		/// Evaluates a direct mapping from x to the sample list
		/// </summary>
		private float EvaluateDirectMapping( float x )
		{
			int index = ( int )( x / m_XStepPerSample );
			index = index < 0 ? 0 : ( index >= m_Samples.Count ? m_Samples.Count - 1 : index );
			return m_Samples[ index ];
		}
		
		/// <summary>
		/// Evaluates a mapping from x to the constrained sample list
		/// </summary>
		private float EvaluateConstrainedMapping( float x )
		{
			float firstX = ( m_TotalSamples - m_Samples.Count ) * m_XStepPerSample;
			float lastX = m_TotalSamples * m_XStepPerSample;
			x = x < firstX ? firstX : ( x > lastX ? lastX : x );

			int firstIndex = m_InsertAt;
			int index = ( firstIndex + ( int )( ( x - firstX ) / m_XStepPerSample ) ) % m_Samples.Count;
			return m_Samples[ index ];
		}

		#endregion
	}
}
