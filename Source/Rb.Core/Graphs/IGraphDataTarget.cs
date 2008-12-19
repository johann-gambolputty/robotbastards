
namespace Rb.Core.Graphs
{
	/// <summary>
	/// Target site that can receive data from a data source
	/// </summary>
	public interface IGraphDataTarget
	{
		/// <summary>
		/// Gets the name of this target
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets/sets the value of this target
		/// </summary>
		object Value
		{
			set; get;
		}
	}
}
