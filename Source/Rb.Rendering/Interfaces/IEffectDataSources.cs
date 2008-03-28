using System;
using Rb.Core.Utils;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Interfaces
{
	/// <summary>
	/// Manages effect parameter data sources
	/// </summary>
	/// <remarks>
	/// Often, it's necessary to transfer runtime data that can't be captured in rendering state, into 
	/// effect programs (for example, shadow map shaders need to know the shadow casting lights).
	/// This is done by setting up a data source, then associating (binding) the source with effect
	/// parameters.
	/// Binding a data source to a parameter name means that for every effect, any parameters with
	/// the specified name get their values set to the value in the data source.
	/// Binding a data source to a parameter annotation means that for every effect, any parameters
	/// with the specified annotation get their values set to the value in the data source
	/// An IEffectDataSources singleton is available from <see cref="Graphics.EffectDataSources"/>.
	/// 
	/// Example:
	/// <code>
	/// public class Cheese
	/// {
	///		private readonly IEffectDataSource m_Source;
	/// 
	///		public Cheese( )
	///		{
	///			m_Source = Graphics.EffectDataSources.CreateDataSourceForNamedParameter( "cheese" );
	///		}
	/// 
	///		public void Update( )
	///		{
	///			m_Source.Value = "gouda";
	///		}
	/// }
	/// </code>
	/// 
	/// 
	/// </remarks>
	public interface IEffectDataSources
	{
		/// <summary>
		/// Event, invoked when a new data source is added
		/// </summary>
		event Action< IEffectDataSource > DataSourceAdded;

		/// <summary>
		/// Event, invoked when a data source is removed
		/// </summary>
		event Action< IEffectDataSource > DataSourceRemoved;

		/// <summary>
		/// Binds a parameter to any existing data source that can bind it (<see cref="IEffectDataSource.Bind"/>)/>
		/// </summary>
		void BindParameter( IEffectParameter parameter );

		/// <summary>
		/// Binds a data source to parameters in any effect with a given name. Invokes <see cref="DataSourceAdded"/>
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		/// <returns>Returns a new IEffectValueDataSource used to set and get the parameter's value</returns>
		IEffectValueDataSource<T>  CreateValueDataSourceForNamedParameter<T>( string parameterName );

		/// <summary>
		/// Binds a data source to parameters in any effect with a given annotation. Invokes <see cref="DataSourceAdded"/>
		/// </summary>
		/// <param name="annotationName">Annotation name</param>
		/// <returns>Returns a new IEffectValueDataSource used to set and get the parameter's value</returns>
		IEffectValueDataSource<T> CreateValueDataSourceForAnnotatedParameter<T>( string annotationName );

		/// <summary>
		/// Removes a data source. Invokes <see cref="DataSourceRemoved"/>
		/// </summary>
		/// <param name="source">Data source to remove</param>
		void UnbindDataSource( IEffectDataSource source );
	}
}
