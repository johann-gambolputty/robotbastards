
namespace Rb.Interaction.Interfaces
{
	/// <summary>
	/// Command user factory interface
	/// </summary>
	public interface ICommandUserFactory
	{
		/// <summary>
		/// Creates a new command user
		/// </summary>
		/// <param name="name">Command user name</param>
		/// <returns>Returns the new command user</returns>
		ICommandUser Create( string name );
	}

}
