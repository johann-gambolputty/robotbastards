using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Planets.Clouds;
using Poc1.Universe.Interfaces;
using Rb.Core.Maths;

namespace Poc1.Bob.Controls.Planet.Clouds
{
	public partial class CloudModelTemplateControl : UserControl, ICloudModelTemplateView
	{
		public CloudModelTemplateControl( )
		{
			InitializeComponent( );
		}

		#region ICloudModelTemplateView Members

		#region Template Events

		/// <summary>
		/// Event invoked when the low coverage range is changed
		/// </summary>
		public event Action<Range<float>> TemplateLowCoverageChanged;

		/// <summary>
		/// Event invoked when the high coverage range is changed
		/// </summary>
		public event Action<Range<float>> TemplateHighCoverageChanged;

		/// <summary>
		/// Event invoked when the template cloud layer height range is changed
		/// </summary>
		public event Action<Range<Units.Metres>> TemplateCloudLayerHeightChanged;

		#endregion

		#region Model events

		/// <summary>
		/// Event invoked when the low coverage value of the model is changed
		/// </summary>
		public event Action<float> ModelLowCoverageChanged;

		/// <summary>
		/// Event invoked when the high coverage value of the model is changed
		/// </summary>
		public event Action<float> ModelHighCoverageChanged;

		/// <summary>
		/// Event invoked when the high coverage value of the model is changed
		/// </summary>
		public event Action<Units.Metres> ModelCloudLayerHeightChanged;

		#endregion


		/// <summary>
		/// Gets/sets the flag that enables model changes
		/// </summary>
		public bool EnableModelChanges
		{
			set
			{
				highCoverRangeSlider.SliderEnabled = value;
				lowCoverageRangeSlider.SliderEnabled = value;
				cloudLayerHeightRangeSlider.SliderEnabled = value;
			}
		}

		#endregion

	}
}
