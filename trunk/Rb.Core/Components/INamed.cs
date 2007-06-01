

namespace Rb.Core.Components
{

	/// <summary>
	/// Objects implementing this interface have human-readable, non-unique names
	/// </summary>
	public interface INamed
	{
		/// <summary>
		/// Access to the name of this object
		/// </summary>
		string Name { get; set; }
	};

};