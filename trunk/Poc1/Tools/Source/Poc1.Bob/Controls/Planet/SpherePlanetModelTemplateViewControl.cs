using System.Windows.Forms;
using Poc1.Bob.Core.Interfaces.Planets;
using Poc1.Core.Classes.Astronomical.Planets.Spherical.Models.Templates;
using Poc1.Core.Interfaces;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Spherical.Models;
using Rb.Core.Utils;

namespace Poc1.Bob.Controls.Planet
{
	public partial class SpherePlanetModelTemplateViewControl : UserControl, ISpherePlanetModelTemplateView
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public SpherePlanetModelTemplateViewControl( )
		{
			InitializeComponent( );
		}

		#region ISpherePlanetModelTemplateView Members

		/// <summary>
		/// Event raised when the model radius slider is moved
		/// </summary>
		public event System.Action<Units.Metres> ModelRadiusChanged;

		#endregion

		#region IPlanetModelTemplateView Members

		/// <summary>
		/// Gets/sets the planet model displayed by this view
		/// </summary>
		public IPlanetModel PlanetModel
		{
			get { return m_PlanetModel; }
			set
			{
				m_PlanetModel = ( ISpherePlanetModel )value;
				if ( m_PlanetModel == null )
				{
					radiusRangeSlider.SliderEnabled = false;
				}
				else
				{
					radiusRangeSlider.SliderEnabled = true;
					radiusRangeSlider.Value = ( decimal )m_PlanetModel.Radius.Value / 1000.0m;
				}
			}
		}

		/// <summary>
		/// Gets/sets the planet model template displayed by this view
		/// </summary>
		public IPlanetModelTemplate PlanetModelTemplate
		{
			get { return m_PlanetTemplate; }
			set
			{
				Arguments.CheckNotNull( value, "value" );
				m_PlanetTemplate = ( SpherePlanetModelTemplate )value;
				radiusRangeSlider.MinValue = ( decimal )m_PlanetTemplate.Radius.Minimum.Value / 1000.0m;
				radiusRangeSlider.MaxValue = ( decimal )m_PlanetTemplate.Radius.Maximum.Value / 1000.0m;
			}
		}

		#endregion

		#region Private Members

		private ISpherePlanetModel m_PlanetModel;
		private ISpherePlanetModelTemplate m_PlanetTemplate;

		#region Event Handlers

		private void radiusRangeSlider_ValueChanged( object sender, System.EventArgs e )
		{
			if ( ModelRadiusChanged != null )
			{
				Units.Metres radius = new Units.Metres( ( double )radiusRangeSlider.Value * 1000.0 );
				ModelRadiusChanged( radius );
			}
		}


		#endregion

		#endregion

	}
}
