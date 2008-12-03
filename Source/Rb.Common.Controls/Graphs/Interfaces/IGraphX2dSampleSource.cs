
namespace Rb.Common.Controls.Graphs.Interfaces
{
	/// <summary>
	/// Sample-based source
	/// </summary>
	public interface IGraphX2dSampleSource : IGraphX2dSource
	{
		/// <summary>
		/// Gets/sets the X step per sample
		/// </summary>
		float XStepPerSample
		{
			get; set;
		}
	}
}
