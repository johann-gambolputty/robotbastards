using Poc1.Universe.Interfaces;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;
using Rb.Core.Maths;

namespace Poc1.Universe.Planets.Models.Templates
{
	/// <summary>
	/// Base template for planetary rings
	/// </summary>
	public abstract class PlanetRingModelTemplate : PlanetEnvironmentModelTemplate, IPlanetRingModelTemplate
	{
		/// <summary>
		/// Gets/sets the range of width values that the planetary rings can take
		/// </summary>
		public Range<Units.Metres> RingWidth
		{
			get { return m_RingWidth; }
			set { m_RingWidth = value; }
		}
		
		/// <summary>
		/// Sets up a ring model instance
		/// </summary>
		public override void SetupInstance( IPlanetEnvironmentModel model, ModelTemplateInstanceContext context )
		{
			IPlanetRingModel ringModel = ( IPlanetRingModel )model;
			double width = RingWidth.Minimum + ( RingWidth.Maximum - RingWidth.Minimum ) * context.Random.NextDouble( );
			ringModel.Width = new Units.Metres( width );
		}

		#region Private Members

		private Range<Units.Metres> m_RingWidth;

		#endregion
	}
}
