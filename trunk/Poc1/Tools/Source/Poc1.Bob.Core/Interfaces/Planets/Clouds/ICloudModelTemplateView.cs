
using System;
using Poc1.Core.Interfaces;
using Rb.Core.Maths;
namespace Poc1.Bob.Core.Interfaces.Planets.Clouds
{
	/// <summary>
	/// Cloud model template view
	/// </summary>
	public interface ICloudModelTemplateView
	{
		#region ProjectType Events

		/// <summary>
		/// Event invoked when the low coverage range is changed
		/// </summary>
		event Action<Range<float>> TemplateLowCoverageChanged;

		/// <summary>
		/// Event invoked when the high coverage range is changed
		/// </summary>
		event Action<Range<float>> TemplateHighCoverageChanged;

		/// <summary>
		/// Event invoked when the template cloud layer height range is changed
		/// </summary>
		event Action<Range<Units.Metres>> TemplateCloudLayerHeightChanged;

		#endregion

		#region Model events

		/// <summary>
		/// Event invoked when the low coverage value of the model is changed
		/// </summary>
		event Action<float> ModelLowCoverageChanged;

		/// <summary>
		/// Event invoked when the high coverage value of the model is changed
		/// </summary>
		event Action<float> ModelHighCoverageChanged;

		/// <summary>
		/// Event invoked when the high coverage value of the model is changed
		/// </summary>
		event Action<Units.Metres> ModelCloudLayerHeightChanged;

		#endregion

		/// <summary>
		/// Gets/sets the flag that enables model changes
		/// </summary>
		bool EnableModelChanges
		{
			set;
		}
	}
}
