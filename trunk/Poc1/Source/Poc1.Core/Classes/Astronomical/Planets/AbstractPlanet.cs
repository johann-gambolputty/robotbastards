using Poc1.Core.Interfaces.Astronomical;
using Poc1.Core.Interfaces.Astronomical.Planets;
using Rb.Core.Utils;

namespace Poc1.Core.Classes.Astronomical.Planets
{
	/// <summary>
	/// Abstract planet implementation
	/// </summary>
	public class AbstractPlanet : IPlanet
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="modelFactory">Factory used to create this planet's model</param>
		/// <param name="rendererFactory">Factory used to create this planet's renderer</param>
		public AbstractPlanet( IPlanetModelFactory modelFactory, IPlanetRendererFactory rendererFactory )
		{
			Arguments.CheckNotNull( modelFactory, "modelFactory" );
			Arguments.CheckNotNull( rendererFactory, "rendererFactory" );

			m_Model = modelFactory.Create( this );
			m_Renderer = rendererFactory.Create( this );
		}

		#region IPlanet Members

		/// <summary>
		/// Gets/sets the orbit of this planet
		/// </summary>
		public IOrbit Orbit
		{
			get { return m_Orbit; }
			set { m_Orbit = value; }
		}

		/// <summary>
		/// Gets the planet model
		/// </summary>
		public IPlanetModel Model
		{
			get { return m_Model; }
		}

		/// <summary>
		/// Gets the planet renderer
		/// </summary>
		public IPlanetRenderer Renderer
		{
			get { return m_Renderer; }
		}

		#endregion

		#region Private Members

		private readonly IPlanetModel m_Model;
		private readonly IPlanetRenderer m_Renderer;
		private IOrbit m_Orbit;

		#endregion
	}
}
