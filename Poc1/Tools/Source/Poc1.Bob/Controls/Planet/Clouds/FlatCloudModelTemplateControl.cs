using System;
using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Planets.Clouds;
using Poc1.Core.Interfaces;
using Rb.Core.Maths;

namespace Poc1.Bob.Controls.Planet.Clouds
{
	public partial class FlatCloudModelTemplateControl : UserControl, IFlatCloudModelTemplateView
	{
		public FlatCloudModelTemplateControl( )
		{
			InitializeComponent( );
		}

		#region ICloudModelTemplateView Members

		#region Template Events

		/// <summary>
		/// Event invoked when the low coverage range is changed
		/// </summary>
		public event Action<Range<float>> TemplateLowCoverageChanged
		{
			add { cloudModelTemplateControl1.TemplateLowCoverageChanged += value; }
			remove { cloudModelTemplateControl1.TemplateLowCoverageChanged -= value; }
		}

		/// <summary>
		/// Event invoked when the high coverage range is changed
		/// </summary>
		public event Action<Range<float>> TemplateHighCoverageChanged
		{
			add { cloudModelTemplateControl1.TemplateHighCoverageChanged += value; }
			remove { cloudModelTemplateControl1.TemplateHighCoverageChanged -= value; }
		}

		/// <summary>
		/// Event invoked when the template cloud layer height range is changed
		/// </summary>
		public event Action<Range<Units.Metres>> TemplateCloudLayerHeightChanged
		{
			add { cloudModelTemplateControl1.TemplateCloudLayerHeightChanged += value; }
			remove { cloudModelTemplateControl1.TemplateCloudLayerHeightChanged -= value; }
		}

		#endregion

		#region Model events

		/// <summary>
		/// Event invoked when the low coverage value of the model is changed
		/// </summary>
		public event Action<float> ModelLowCoverageChanged
		{
			add { cloudModelTemplateControl1.ModelLowCoverageChanged += value; }
			remove { cloudModelTemplateControl1.ModelLowCoverageChanged -= value; }
		}

		/// <summary>
		/// Event invoked when the high coverage value of the model is changed
		/// </summary>
		public event Action<float> ModelHighCoverageChanged
		{
			add { cloudModelTemplateControl1.ModelHighCoverageChanged += value; }
			remove { cloudModelTemplateControl1.ModelHighCoverageChanged -= value; }
		}

		/// <summary>
		/// Event invoked when the high coverage value of the model is changed
		/// </summary>
		public event Action<Units.Metres> ModelCloudLayerHeightChanged
		{
			add { cloudModelTemplateControl1.ModelCloudLayerHeightChanged += value; }
			remove { cloudModelTemplateControl1.ModelCloudLayerHeightChanged -= value; }
		}

		#endregion


		/// <summary>
		/// Gets/sets the flag that enables model changes
		/// </summary>
		public bool EnableModelChanges
		{
			set { cloudModelTemplateControl1.EnableModelChanges = value; }
		}

		#endregion

	}
}
