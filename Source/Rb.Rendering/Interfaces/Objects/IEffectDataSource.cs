
using Rb.Rendering.Interfaces;

namespace Rb.Rendering.Interfaces.Objects
{
	/// <summary>
	/// A data source for effect parameters
	/// </summary>
	public interface IEffectDataSource
	{
		/// <summary>
		/// Tries to bind this data source to a given parameter. Returns true if this data source was bound succesfully
		/// </summary>
		void Bind( IEffectParameter parameter );

		/// <summary>
		/// Sets the value of a parameter to the value of this data source
		/// </summary>
		void Apply( IEffectParameter parameter );
	}

	/// <summary>
	/// Value data asource
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
	/// <remarks>
	/// IEffectValueDataSource objects are created using the <see cref="IEffectDataSources.CreateValueDataSourceForNamedParameter{T}"/>
	/// and <see cref="IEffectDataSources.CreateValueDataSourceForAnnotatedParameter{T}"/> methods.
	/// </remarks>
	public interface IEffectValueDataSource< T > : IEffectDataSource
	{
		T Value
		{
			get; set;
		}
	}
}
