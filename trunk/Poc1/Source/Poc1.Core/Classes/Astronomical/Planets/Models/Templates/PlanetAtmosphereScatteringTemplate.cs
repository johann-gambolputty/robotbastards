using Poc1.Core.Classes.Astronomical.Planets.Spherical;
using Poc1.Core.Interfaces.Astronomical.Planets.Models;
using Poc1.Core.Interfaces.Astronomical.Planets.Models.Templates;
using Poc1.Tools.Atmosphere;

namespace Poc1.Core.Classes.Astronomical.Planets.Models.Templates
{
	/// <summary>
	/// Template for planetary atmosphere model instances
	/// </summary>
	/// <remarks>
	/// An atmosphere template consists of a list of possible atmospheric paramaterizations. When the
	/// template is instanced, it creates an atmosphere model using a randomly selected parameterization.
	/// </remarks>
	public class PlanetAtmosphereScatteringTemplate : PlanetAtmosphereTemplate, IPlanetAtmosphereScatteringTemplate
	{
		/// <summary>
		/// Gets the atmosphere build parameters. This defines the fidelity of the atmosphere calculations
		/// </summary>
		public AtmosphereBuildParameters AtmosphereParameters
		{
			get { return m_AtmosphereParameters; }
		}

		/// <summary>
		/// Gets the atmosphere build model. This defines the composition of the atmosphere
		/// </summary>
		public AtmosphereBuildModel AtmosphereModel
		{
			get { return m_AtmosphereModel; }
		}

		/// <summary>
		/// Gets the atmosphere build outputs
		/// </summary>
		public AtmosphereBuildOutputs AtmosphereOutputs
		{
			get { return m_AtmosphereOutputs; }
		}

		/// <summary>
		/// Gets the location at which the atmosphere build outputs are stored
		/// </summary>
		public string AtmosphereOutputLocation
		{
			get { return m_AtmosphereOutputLocation; }
		}

		/// <summary>
		/// Builds the atmosphere outputs
		/// </summary>
		public void Build( AtmosphereBuildProgress progress )
		{
			m_AtmosphereOutputs = new AtmosphereBuilder( ).Build( AtmosphereModel, AtmosphereParameters, progress );
			OnTemplateChanged( );
		}

		#region IPlanetAtmosphereScatteringTemplate Members

		/// <summary>
		/// Atmosphere model creation
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			IPlanetAtmosphereScatteringModel atmosphereModel = ( IPlanetAtmosphereScatteringModel )model;
			double thicknessRatio = AtmosphereModel.AtmosphereThicknessMetres / AtmosphereModel.InnerRadiusMetres;
			atmosphereModel.Thickness = ( ( SpherePlanetModel )model.PlanetModel ).Radius * thicknessRatio;
		}

		#endregion

		#region Private Members

		private readonly AtmosphereBuildParameters m_AtmosphereParameters = new AtmosphereBuildParameters( );
		private readonly AtmosphereBuildModel m_AtmosphereModel = new AtmosphereBuildModel( );
		private AtmosphereBuildOutputs m_AtmosphereOutputs;
		private readonly string m_AtmosphereOutputLocation;

		#endregion

	}
}
