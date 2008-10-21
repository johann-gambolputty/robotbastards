using System;
using System.Collections.Generic;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Handy abstract base class implementation of <see cref="IEffectDataSources"/>
	/// </summary>
	public abstract class EffectDataSourcesBase : IEffectDataSources
	{
		#region IEffectDataSources Members

		/// <summary>
		/// Event, invoked when a new data source is added
		/// </summary>
		public event Action<IEffectDataSource> DataSourceAdded;

		/// <summary>
		/// Event, invoked when a data source is removed
		/// </summary>
		public event Action<IEffectDataSource> DataSourceRemoved;

		/// <summary>
		/// Binds a parameter to any existing data source that can bind it (<see cref="IEffectDataSource.Bind"/>)/>
		/// </summary>
		public void BindParameter( IEffectParameter parameter )
		{
			if ( parameter == null )
			{
				throw new ArgumentNullException( "parameter" );
			}

			IEffectDataSource source;
			if ( m_ParameterNameDataSources.TryGetValue( parameter.Name, out source ) )
			{
				source.Bind( parameter );
			}
			else if ( m_SemanticNameDataSources.TryGetValue( parameter.Semantic, out source ) )
			{
				source.Bind( parameter );
			}
		}

		/// <summary>
		/// Binds a data source to parameters in any effect with a given name. Invokes <see cref="DataSourceAdded"/>
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		/// <returns>Returns a new IEffectDataSource used to set and get the parameter's value</returns>
		public IEffectValueDataSource<T> CreateValueDataSourceForNamedParameter<T>( string parameterName )
		{
			IEffectDataSource source;
			if ( !m_ParameterNameDataSources.TryGetValue( parameterName, out source ) )
			{
				source = CreateValueDataSource<T>( );
				m_ParameterNameDataSources.Add( parameterName, source );
			}

			IEffectValueDataSource<T> valueSource = source as IEffectValueDataSource<T>;
			if ( valueSource == null )
			{
				throw new ArgumentException( string.Format( "Value data source for parameter \"{0}\" already exists, but it is of type \"{1}\", not \"{2}\"", parameterName, source.GetType( ), typeof( IEffectValueDataSource<T> ) ) );
			}

			return valueSource;
		}

		/// <summary>
		/// Binds a data source to parameters in any effect with a given annotation. Invokes <see cref="DataSourceAdded"/>
		/// </summary>
		/// <param name="semantic">Semantic name</param>
		/// <returns>Returns a new IEffectDataSource used to set and get the parameter's value</returns>
		public IEffectValueDataSource<T> CreateValueDataSourceForSemantic<T>( string semantic )
		{
			IEffectDataSource source;
			if ( !m_SemanticNameDataSources.TryGetValue( semantic, out source ) )
			{
				source = CreateValueDataSource<T>( );
				m_SemanticNameDataSources.Add( semantic, source );
			}
			IEffectValueDataSource<T> valueSource = source as IEffectValueDataSource<T>;
			if ( valueSource == null )
			{
				throw new ArgumentException( string.Format( "Value data source for semantic \"{0}\" already exists, but it is of type \"{1}\", not \"{2}\"", semantic, source.GetType( ), typeof( IEffectValueDataSource<T> ) ) );
			}

			return valueSource;
		}

		/// <summary>
		/// Removes a data source. Invokes <see cref="DataSourceRemoved"/>
		/// </summary>
		/// <param name="source">Data source to remove</param>
		public void UnbindDataSource( IEffectDataSource source )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Gets the dictionary that maps parameter names to data sources
		/// </summary>
		protected IDictionary<string, IEffectDataSource> ParameterNameDataSources
		{
			get { return m_ParameterNameDataSources; }
		}

		/// <summary>
		/// Gets the dictionary that maps annotation names to data sources
		/// </summary>
		protected IDictionary<string, IEffectDataSource> SemanticNameDataSources
		{
			get { return m_SemanticNameDataSources; }
		}

		/// <summary>
		/// Calls the DataSourceAdded event
		/// </summary>
		protected void OnDataSourceAdded( IEffectDataSource dataSource )
		{
			if ( DataSourceAdded != null )
			{
				DataSourceAdded( dataSource );
			}
		}

		/// <summary>
		/// Calls the DataSourceRemoved event
		/// </summary>
		protected void OnDataSourceRemoved( IEffectDataSource dataSource )
		{
			if ( DataSourceRemoved != null )
			{
				DataSourceRemoved( dataSource );
			}
		}

		/// <summary>
		/// Creates a new IEffectValueDataSource
		/// </summary>
		protected abstract IEffectValueDataSource<T> CreateValueDataSource<T>( );

		#endregion

		#region Private Members

		private readonly Dictionary<string, IEffectDataSource> m_ParameterNameDataSources = new Dictionary<string, IEffectDataSource>( );
		private readonly Dictionary<string, IEffectDataSource> m_SemanticNameDataSources = new Dictionary<string, IEffectDataSource>( );

		#endregion
	}
}
