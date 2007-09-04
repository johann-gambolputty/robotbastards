namespace Rb.Core.Components
{
	/// <summary>
	/// Dynamic property interface
	/// </summary>
	public interface IDynamicProperty
	{
		/// <summary>
		/// Gets the name of this property
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Accessor of the value of this property
		/// </summary>
		object Value
		{
			get; set;
		}

	}
}
