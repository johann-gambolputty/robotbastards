
using Goo.Core.Commands;
using Goo.Core.Host;

namespace Goo.Core.Environment
{
	/// <summary>
	/// Environment interface
	/// </summary>
	public interface IEnvironment : IServiceProvider, ICommandHost
	{
		/// <summary>
		/// Gets the application host
		/// </summary>
		IHost Host
		{
			get;
		}

		/// <summary>
		/// Gets the command executor registry
		/// </summary>
		ICommandExecutorRegistry CommandExecutors
		{
			get;
		}

		/// <summary>
		/// Gets the controller factory registry
		/// </summary>
		IControllerFactoryRegistry ControllerFactories
		{
			get;
		}
	}
}
