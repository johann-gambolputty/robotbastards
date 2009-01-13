
using Rb.Interaction.Classes;

namespace Poc1.Bob.Core.Classes.Commands
{
	/// <summary>
	/// Planet commands
	/// </summary>
	public class PlanetCommands
	{
		public PlanetCommands( )
		{
		}

		#region Private Members

		private readonly static CommandGroup s_ViewTemplatesGroup = new CommandGroup( DefaultCommands.ViewCommands, "templates", "&Templates" );

		private readonly static Command s_ViewPlanetTemplate = s_ViewTemplatesGroup.NewCommand( "planetTemplate", "&Planet Template", "Show the planet template view" );
		private readonly static Command s_ViewAtmosphereTemplate = s_ViewTemplatesGroup.NewCommand( "atmosphereTemplate", "&Atmosphere Template", "Show the atmosphere template view" );
		private readonly static Command s_ViewBiomeTemplate = s_ViewTemplatesGroup.NewCommand( "biomeTemplate", "&Biome Template", "Show the biome template view" );

		#endregion
	}
}
