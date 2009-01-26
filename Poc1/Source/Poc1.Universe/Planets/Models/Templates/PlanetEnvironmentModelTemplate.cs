using System;
using Poc1.Universe.Interfaces.Planets.Models;
using Poc1.Universe.Interfaces.Planets.Models.Templates;

namespace Poc1.Universe.Planets.Models.Templates
{
	/// <summary>
	/// Base class implementing <see cref="IPlanetEnvironmentModelTemplate"/>
	/// </summary>
	public abstract class PlanetEnvironmentModelTemplate : IPlanetEnvironmentModelTemplate
	{
		#region IPlanetModelTemplateBase Members

		/// <summary>
		/// Event raised when the template changes
		/// </summary>
		public event EventHandler TemplateChanged;

		#endregion

		#region IPlanetEnvironmentModelTemplate Members

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		public abstract void CreateInstance( IPlanetModel planetModel, ModelTemplateInstanceContext context );

		#endregion

		#region Protected Members

		/// <summary>
		/// Raises the <see cref="TemplateChanged"/> event
		/// </summary>
		protected void OnTemplateChanged( )
		{
			OnTemplateChanged( EventArgs.Empty );
		}

		/// <summary>
		/// Raises the <see cref="TemplateChanged"/> event
		/// </summary>
		protected void OnTemplateChanged( EventArgs args )
		{
			if ( TemplateChanged != null )
			{
				TemplateChanged( this, args );
			}
		}

		#endregion

	}
}
