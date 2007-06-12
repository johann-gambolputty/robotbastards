using System;

namespace Rb.Core.Interaction
{
	/// <summary>
	/// A command input interpreter is used to generate CommandMessage objects from a CommandInputBinding
	/// </summary>
	public class CommandInputInterpreter
	{
		/// <summary>
		/// Creates a CommandMessage from 
		/// </summary>
		public virtual CommandMessage CreateMessage( CommandInputBinding binding )
		{
			return new CommandMessage( binding.Command );
		}
	}
}
