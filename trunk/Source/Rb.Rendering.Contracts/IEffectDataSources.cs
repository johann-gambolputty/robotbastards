using System;
using Rb.Rendering.Contracts.Objects;

namespace Rb.Rendering.Contracts
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
	/// the specified name get their values set to a 
	/// An IEffectDataSources object is available from <see cref="Graphics.EffectDataSources"/>.
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
		event Action<IEffectDataSource> DataSourceRemoved;

		/// <summary>
		/// Binds a data source to parameters in any effect with a given name. Invokes <see cref="DataSourceAdded"/>
		/// </summary>
		/// <param name="parameterName">Parameter name</param>
		/// <param name="source">Data source</param>
		void BindDataSourceToParameterName( string parameterName, IEffectDataSource source );

		/// <summary>
		/// Removes a data source. Invokes <see cref="DataSourceRemoved"/>
		/// </summary>
		/// <param name="source">Data source to remove</param>
		void UnbindDataSource( IEffectDataSource source );
	}
}
