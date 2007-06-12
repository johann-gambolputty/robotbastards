using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// The hugely named CommandEnumInputInterpreterAttribute associates a CommandInputInterpeter class with a Command. Used when
	/// generating the command with an enum value
	/// </summary>
	/// <example>
	/// public enum MyCommands
	/// {
	///		[ Interaction.CommandEnumDescription( "Self Destructs" ) ]
	///		[ Interaction.CommandEnumInputInterpreter( typeof( MyCommandInputInterpreter ) ) ]
	///		SelfDestruct
	/// }
	/// </example>
	public class CommandEnumInputInterpreterAttribute : Attribute
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public CommandEnumInputInterpreterAttribute( Type interpreterType )
		{
			m_Type = interpreterType;
		}

		/// <summary>
		/// Gets the stored interpreter type
		/// </summary>
		public Type InterpreterType
		{
			get
			{
				return m_Type;
			}
		}

		private Type m_Type;
	}
}
