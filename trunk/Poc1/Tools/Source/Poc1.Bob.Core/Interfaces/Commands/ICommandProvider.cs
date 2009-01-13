using Rb.Interaction.Classes;

namespace Poc1.Bob.Core.Interfaces.Commands
{
	/// <summary>
	/// Command group provider
	/// </summary>
	public interface ICommandProvider
	{
		/// <summary>
		/// Gets the commands supported by this provider
		/// </summary>
		Command[] Commands
		{
			get;
		}
	}
}
